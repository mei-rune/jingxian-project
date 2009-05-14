using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Networks
{
    public class BackEndServer : IDisposable
    {
        public int Id;
        Socket _socket;
        NetworkStream _stream;

        byte[] _buf = new byte[4];

        public BackEndServer(Socket socket)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);

            _buf[0] = (byte)_stream.ReadByte();
            _buf[1] = (byte)_stream.ReadByte();
            _buf[2] = (byte)_stream.ReadByte();
            _buf[3] = (byte)_stream.ReadByte();

            Id = ReadInt32(_buf, 0);


            Console.WriteLine("后端服务器{0}连接到服务器上", Id);
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

        public void Send(Socket client)
        {
            try
            {
                SocketInformation si = client.DuplicateAndClose(Id);
                Console.WriteLine("将socket[{0}]发送到后端服务器{1}上", client.Handle, Id);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(_stream, si);
            }
            catch
            { }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _stream.Dispose();

            Console.WriteLine("后端服务器{0}断开", Id);
        }

        #endregion

        public override string ToString()
        {
            return Id.ToString();
        }
    }

    /// <summary>
    /// 作者：runner.mei@gmail.com
    /// 日期：2008-07-04
    /// </summary>
    class BalanceServer
    {
        IPAddress _ip;
        int _echoPort;
        int _balancePort;
        Socket _balanceSocket;
        IAsyncResult _acceptResult;

        List<BackEndServer> _backEnds = new List<BackEndServer>();
        int _current = 0;

        public BalanceServer(IPAddress ip, int echoPort, int balancePort)
        {
            _ip = ip;
            _echoPort = echoPort;
            _balancePort = balancePort;
        }

        public void RunForever()
        {
            _balanceSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _balanceSocket.Bind(new IPEndPoint(_ip, _balancePort));
            _balanceSocket.Listen(9);
            _acceptResult = _balanceSocket.BeginAccept(this.OnClient, null);

            using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                server.Bind(new IPEndPoint(_ip, _echoPort));
                server.Listen(9);
                while (true)
                {
                    Send( server.Accept() );
                }
            }
        }

        public void Send(Socket socket)
        {
            BackEndServer backend = GetNext();
            if (null == backend)
            {
                Console.WriteLine("没有可用的后端服务器!");
                socket.Close();
                return;
            }
            try
            {
                backend.Send(socket);
            }
            catch //(  Exception e)
            {
                Console.WriteLine("后端服务器{0}不可用,可能退出!", backend);
                _backEnds.Remove(backend);

            }
        }

        BackEndServer GetNext()
        {
            if (0 == _backEnds.Count)
                return null;

            return _backEnds[(_current ++ ) % _backEnds.Count];
        }

        void OnClient(object obj)
        {
            Socket socket = null;
            try
            {
                socket = _balanceSocket.EndAccept(_acceptResult);
            }
            catch
            {
                return;
            }
            finally
            {
                _acceptResult = _balanceSocket.BeginAccept(this.OnClient, null);
            }

            BackEndServer backEnd = null;
            try
            {
                using (backEnd = new BackEndServer(socket))
                {
                    _backEnds.Add(backEnd);

                    byte[] buf = new byte[4];
                    socket.Receive(buf, SocketFlags.None);
                }
            }
            catch
            { }

            if (null != backEnd)
                _backEnds.Remove(backEnd);
        }

        static void Main(string[] args)
        {
            new BalanceServer(IPAddress.Any, 30001, 30002).RunForever(); 
        }
    }
}
    
