using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Networks
{
    class Request
    {
        public Socket _socket;                 // io����
        public SelectMode _mode;               // io��������
        public IEnumerator<Request> _callback; // �����˶�ջ�ĵ�����

        public Request(Socket socket, SelectMode mode, IEnumerator<Request> handler)
        {
            _socket = socket;
            _mode = mode;
            _callback = handler;
        }
    }

    /// <summary>
    /// ���ߣ�runner.mei@gmail.com
    /// ���ڣ�2008-07-04
    /// </summary>
    class EchoServer
    {
        /// IO����������
        List<Request> _requests = new List<Request>();

        /// IO�������ִ�е����ж�ջ���У����IO������Լ���ִ�в������
        /// �����İ������ж�ջ�ĵ�����
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
            // ����IO�����ҵ�IO�������
            Request r = _requests.Find(delegate(Request request)
             { return socket == request._socket; });
            if (null != r)
            {
                // ������ӵ�IO�������ִ�е����ж�ջ���У������Ժ�ִ��
                _currents.AddLast(r._callback);
                // ��IO������������ɾ����
                _requests.Remove(r);
            }
        }

        /// <summary>
        /// ���ߣ�runner.mei@gmail.com
        /// ���ڣ�2008-07-04
        /// </summary>
        public void RunForever()
        {
            while (true)
            {
                while (0 != _currents.Count)
                {
                    // ����IO������Լ���ִ�в������������IO�������
                    IEnumerator<Request> enumerator = _currents.First.Value;
                    _currents.RemoveFirst();

                    //�����ж�ջ�ָ�������������
                    if (enumerator.MoveNext())
                    {
                        // ȡ�������к󷵻ص�IO������󣬽��������ж�ջ�ĵ�����
                        // ����������_callback�����CreateRequest����
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

                /// ����ǲ�����IO������Լ���
                Socket.Select(readset, writeset, null, 10000);
                foreach (Socket socket in readset)
                {
                    //��IO������Լ���io����
                    processRequest(socket);
                }

                foreach (Socket socket in writeset)
                {
                    //��IO������Լ���io����
                    processRequest(socket);
                }
            }
        }

        Request CreateRequest(Socket socket, SelectMode mode)
        {
            Request request = new Request(socket, mode, null);��// ע���ڴ���ʱ��û�з�������˶�ջ�ĵ�����
            _requests.Add(request);// �� IO����������ȫ�ֵ�IO����������
            return request;
        }

        IEnumerator<Request> RunServer(Socket server)
        {
            for (; ; )
            {
                ///���������Ǵ�����һ��IO������,�������ջ�󷵻�
                yield return CreateRequest(server, SelectMode.SelectRead);
                _currents.AddLast(OnClient(server.Accept()));
            }
        }

        IEnumerator<Request> OnClient(Socket socket)
        {
            Console.WriteLine("��������{0}������", socket.RemoteEndPoint);
            byte[] buffer = new byte[1024];
            bool state = true;
            while (state)
            {
                ///���������Ǵ�����һ��IO������,�������ջ�󷵻�
                yield return CreateRequest(socket, SelectMode.SelectRead);
                int i = socket.Receive(buffer);
                if (0 == i)
                {
                    state = false;
                    continue;
                }

                ///���������Ǵ�����һ��IOд����,�������ջ�󷵻�
                yield return CreateRequest(socket, SelectMode.SelectWrite);
                state = (0 != socket.Send(buffer, i, SocketFlags.None));
            }

            Console.WriteLine("����{0}�����ӶϿ�", socket.RemoteEndPoint);
            socket.Close();
        }
        static void Main(string[] args)
        {
            new EchoServer(IPAddress.Any, 30013).RunForever();
        }
    }
}
