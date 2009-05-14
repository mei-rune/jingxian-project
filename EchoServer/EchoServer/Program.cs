using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Networks
{
    class Request
    {
        public Socket _socket;                 // io对象
        public SelectMode _mode;               // io请求类型
        public IEnumerator<Request> _callback; // 包含了堆栈的迭代器

        public Request(Socket socket, SelectMode mode, IEnumerator<Request> handler)
        {
            _socket = socket;
            _mode = mode;
            _callback = handler;
        }
    }

    /// <summary>
    /// 作者：runner.mei@gmail.com
    /// 日期：2008-07-04
    /// </summary>
    class EchoServer
    {
        /// IO请求对象队列
        List<Request> _requests = new List<Request>();

        /// IO请求可以执行的运行堆栈队列，存放IO请求可以继续执行不会产生
        /// 阻塞的包含运行堆栈的迭代器
        LinkedList<IEnumerator<Request>> _currents = new LinkedList<IEnumerator<Request>>();


        public EchoServer(IPAddress ip, int port)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(ip, port));
            server.Listen(9);
            _currents.AddLast(RunServer(server));
        }

        void processRequest(Socket socket)
        {
            // 根据IO对象找到IO请求对象
            Request r = _requests.Find(delegate(Request request)
             { return socket == request._socket; });
            if (null != r)
            {
                // 将它添加到IO请求可以执行的运行堆栈队列，并在稍后执行
                _currents.AddLast(r._callback);
                // 从IO请求对象队列中删除它
                _requests.Remove(r);
            }
        }

        /// <summary>
        /// 作者：runner.mei@gmail.com
        /// 日期：2008-07-04
        /// </summary>
        public void RunForever()
        {
            while (true)
            {
                while (0 != _currents.Count)
                {
                    // 处理IO请求可以继续执行不会产生阻塞的IO请求对象
                    IEnumerator<Request> enumerator = _currents.First.Value;
                    _currents.RemoveFirst();

                    //将运行堆栈恢复，并继续运行
                    if (enumerator.MoveNext())
                    {
                        // 取本次运行后返回的IO请求对象，将包含运行堆栈的迭代器
                        // 保存在它的_callback中请见CreateRequest函数
                        enumerator.Current._callback = enumerator;
                    }
                }

                List<Socket> readset = new List<Socket>();
                List<Socket> writeset = new List<Socket>();

                foreach (Request r in _requests)
                {
                    if (r._mode == SelectMode.SelectWrite)
                        writeset.Add(r._socket);
                    else
                        readset.Add(r._socket);
                }

                /// 检测是不是有IO对象可以继续
                Socket.Select(readset, writeset, null, 10000);
                foreach (Socket socket in readset)
                {
                    //该IO对象可以继续io操作
                    processRequest(socket);
                }

                foreach (Socket socket in writeset)
                {
                    //该IO对象可以继续io操作
                    processRequest(socket);
                }
            }
        }

        Request CreateRequest(Socket socket, SelectMode mode)
        {
            Request request = new Request(socket, mode, null);　// 注意在创建时并没有放入包含了堆栈的迭代器
            _requests.Add(request);// 将 IO请求对象放在全局的IO请求对象队列
            return request;
        }

        IEnumerator<Request> RunServer(Socket server)
        {
            for (; ; )
            {
                ///在这里我们创建了一个IO读请求,并保存堆栈后返回
                yield return CreateRequest(server, SelectMode.SelectRead);
                _currents.AddLast(OnClient(server.Accept()));
            }
        }

        IEnumerator<Request> OnClient(Socket socket)
        {
            Console.WriteLine("接受来自{0}的连接", socket.RemoteEndPoint);
            byte[] buffer = new byte[1024];
            bool state = true;
            while (state)
            {
                ///在这里我们创建了一个IO读请求,并保存堆栈后返回
                yield return CreateRequest(socket, SelectMode.SelectRead);
                int i = socket.Receive(buffer);
                if (0 == i)
                {
                    state = false;
                    continue;
                }

                ///在这里我们创建了一个IO写请求,并保存堆栈后返回
                yield return CreateRequest(socket, SelectMode.SelectWrite);
                state = (0 != socket.Send(buffer, i, SocketFlags.None));
            }

            Console.WriteLine("来自{0}的连接断开", socket.RemoteEndPoint);
            socket.Close();
        }
        static void Main(string[] args)
        {
            new EchoServer(IPAddress.Any, 30013).RunForever();
        }
    }
}
