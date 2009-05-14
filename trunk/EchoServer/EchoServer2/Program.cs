using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Networks
{
    /// <summary>
    /// 作者：runner.mei@gmail.com
    /// 日期：2008-07-04
    /// </summary>
    class EchoServerByThread
    {
        IPAddress _ip;
        int _port;
        public EchoServerByThread(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public void RunForever()
        {
            using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                server.Bind(new IPEndPoint(_ip, _port));
                server.Listen(9);
                while( true )
                {
                    // 从线程池中取出一个线程来处理接受的连接
                    ThreadPool.QueueUserWorkItem(this.OnClient, server.Accept());
                }
            }
        }


        void OnClient(object obj)
        {
            try
            {
                using (Socket socket = (Socket)obj)
                {
                    Console.WriteLine("接受来自{0}的连接", socket.RemoteEndPoint);
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int i = socket.Receive(buffer);
                        if (0 == i)
                            break;

                        i = socket.Send(buffer, i, SocketFlags.None);
                        if (0 == i)
                            break;
                    }
                    Console.WriteLine("来自{0}的连接断开", socket.RemoteEndPoint);
                    socket.Close();
                }
            }
            catch
            {
                //TODO: what?
            }
        }

        static void Main(string[] args)
        {
            new EchoServerByThread(IPAddress.Any, 30013).RunForever();
        }
    }
}
