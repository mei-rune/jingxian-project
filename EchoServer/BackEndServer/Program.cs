using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Networks
{
    /// <summary>
    /// 作者：runner.mei@gmail.com
    /// 日期：2008-07-04
    /// </summary>
    class BackEndServer
    {
        IPAddress    _ip;
        int    _port;

        public BackEndServer(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public static int ReadInt32(byte[] buf, int startIndex)
        {
            return (int)(buf[startIndex] 
                | buf[startIndex + 1] << 8 
                | buf[startIndex + 2] << 16 
                | buf[startIndex + 3] << 24);
        }

        public static void Write(byte[] buf, int startIndex, int value)
        {
            buf[startIndex] = (byte)value;
            buf[startIndex + 1] = (byte)(value >> 8);
            buf[startIndex + 2] = (byte)(value >> 16);
            buf[startIndex + 3] = (byte)(value >> 24);
        }

        public void RunForever()
        {
            try
            {
                using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    server.Connect(new IPEndPoint(_ip, _port));

                    NetworkStream stream = new NetworkStream(server);

                    byte[] buf = new byte[4];
                    Write(buf, 0, System.Diagnostics.Process.GetCurrentProcess().Id);
                    stream.Write(buf, 0, 4);

                    BinaryFormatter formatter = new BinaryFormatter();

                    while (true)
                    {
                        object socketInformation = formatter.Deserialize(stream);
                        ThreadPool.QueueUserWorkItem(this.OnClient, socketInformation);
                    }

                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("发生错误!");
                System.Console.WriteLine( e );
            }
        }


        void OnClient(object obj)
        {
            try
            {
                SocketInformation si = (SocketInformation)obj;
                using (Socket socket = new Socket(si))
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
            new BackEndServer(IPAddress.Parse(args[0]), int.Parse(args[1])).RunForever();

            //new BackEndServer(IPAddress.Parse(args[0]), int.Parse(args[1])).RunForever();
        }
    }
}