
#ifndef _Proxy_H_
#define _Proxy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files

class RequestPacket
{
public:
    //+----+----+----+----+----+----+----+----+----+----+...+----+
    //| VN | CD | DSTPORT |      DSTIP        | USERID      |NULL|
    //+----+----+----+----+----+----+----+----+----+----+...+----+
    //   1    1      2              4           variable       1
    //VN      SOCKS协议版本号，应该是0x04
    //CD      SOCKS命令，可取如下值:
    //        0x01    CONNECT
    //        0x02    BIND
    //DSTPORT CD相关的端口信息
    //DSTIP   CD相关的地址信息
    //USERID  客户方的USERID
    //NULL    0x00

    Encoding _encoding;
    byte[] _IOBuffer;
    int _length = 8 + 1;

    public RequestPacket(Encoding encoding)
            : this(encoding, new byte[1024], 8 + 1)
    {
    }

    public RequestPacket(Encoding encoding, byte[] IOBuffer, int length)
    {
        _encoding = encoding;
        _IOBuffer = IOBuffer;
        _length = length;
    }


    public RequestPacket(Encoding encoding, IOBuffer IOBuffer)
    {
        _encoding = encoding;
        _length = IOBuffer.Count;
        if (0 == IOBuffer.Begin)
        {
            _IOBuffer = IOBuffer.Array;
        }
        else
        {
            _IOBuffer = new byte[_length];
            IOBuffer.CopyTo(_IOBuffer);
        }
    }

    public int Version
    {
        get { return (int)_IOBuffer[0];
            }
        set { _IOBuffer[0] = (byte)value;
            }
    }

    public int Command
    {
        get { return (int)_IOBuffer[1];
            }
        set { _IOBuffer[1] = (byte)value;
            }
    }

    public short Port
    {
        get { return Helper.ReadInt16(_IOBuffer, 2, true);
            }
        set { Helper.Write(_IOBuffer, 2, value, true);
            }
    }

    public bool IsValidIP
    {
        get
        {
            return (_IOBuffer[4] != 0);
        }
    }

    public byte[] IP
    {
        get
        {
            byte[] ip = new byte[4];
            Array.Copy(_IOBuffer, 4, ip, 0, 4);
            return ip;
        }
        set
        {
            if (4 != value.Length)
                throw new ArgumentException("长度大于4!", "IP");
            Array.Copy(value, 0, _IOBuffer, 4, 4);
        }
    }

    public IPEndPoint EndPoint
    {
        get { return new IPEndPoint(new IPAddress(IP), Port);
            }
        set
        {
            Port = (short)value.Port;
            IP = value.Address.GetAddressBytes();
        }
    }

    /// <summary>
    /// 设置User后，Host会变成null
    /// </summary>
    public string Host
    {
        get
        {
            if (IsValidIP)
                return null;

            for (int i = 8; i < _length; i++)
            {
                if (_IOBuffer[i] != 0)
                    continue;
                return _encoding.GetString(_IOBuffer, i + 1, _length - i - 2);

            }
            return null;
        }
        set
        {
            byte[] ip = new byte[0];
            ip[0] = 0;
            ip[1] = 0;
            ip[2] = 0;
            ip[3] = 1;

            byte[] u = _encoding.GetBytes(value);
            Array.Copy(u, 0, _IOBuffer, _length, u.Length);
            _IOBuffer[_length + u.Length + 1] = 0;
            _length = _length + u.Length + 1;
        }
    }

    /// <summary>
    /// 设置User后，Host会变成null
    /// </summary>
    public string User
    {
        get
        {

            int length = _length - 8 - 1;
            if (0 >= length)
                return null;

            byte[] b = new byte[length];
            Array.Copy(_IOBuffer, 8, b, 0, length);
            if (!IsValidIP)
            {
                for (int i = 0; i < b.Length; i++)
                {
                    if (_IOBuffer[i] != 0)
                        continue;

                    if (0 == i)
                        return null;

                    return _encoding.GetString(b);
                }
            }
            return _encoding.GetString(b);
        }
        set
        {
            byte[] u = _encoding.GetBytes(value);
            Array.Copy(u, 0, _IOBuffer, 8, u.Length);
            _IOBuffer[8 + u.Length] = 0;
            _length = 8 + u.Length + 1;
        }
    }

    public IOBuffer ToBuffer()
    {
        return new IOBuffer(_IOBuffer, 0, _length);
    }
};

class ResponsePacket
{
    //+----+----+----+----+----+----+----+----+
    //| VN | CD | DSTPORT |      DSTIP        |
    //+----+----+----+----+----+----+----+----+
    //   1    1      2              4
    //VN      应该为0x00而不是0x04
    //CD      可取如下值:
    //        0x5A    允许转发
    //        0x5B    拒绝转发，一般性失败
    //        0x5C    拒绝转发，SOCKS 4 Server无法连接到SOCS 4 Client所在主机的
    //                IDENT服务
    //        0x5D    拒绝转发，请求报文中的USERID与IDENT服务返回值不相符
    //DSTPORT CD相关的端口信息
    //DSTIP   CD相关的地址信息

    byte[] _IOBuffer = new byte[8];

    public ResponsePacket()
    {
    }

    public ResponsePacket(byte[] IOBuffer)
    {
        _IOBuffer = IOBuffer;
    }


    public ResponsePacket(IOBuffer IOBuffer)
    {
        if (0 == IOBuffer.Begin)
        {
            _IOBuffer = IOBuffer.Array;
        }
        else
        {
            _IOBuffer = new byte[8];
            IOBuffer.CopyTo(_IOBuffer);
        }
    }

    public int Version
    {
        get { return (int)_IOBuffer[0];
            }
        set { _IOBuffer[0] = (byte)value;
            }
    }

    public int Error
    {
        get { return (int)_IOBuffer[1];
            }
        set { _IOBuffer[1] = (byte)value;
            }
    }

    public short Port
    {
        get { return Helper.ReadInt16(_IOBuffer, 2, true);
            }
        set { Helper.Write(_IOBuffer, 2, value, false);
            }
    }

    public byte[] IP
    {
        get
        {
            byte[] ip = new byte[4];
            Array.Copy(_IOBuffer, 4, ip, 0, 4);
            return ip;
        }
        set
        {
            if (4 != value.Length)
                throw new ArgumentException("长度大于4!", "IP");
            Array.Copy(value, 0, _IOBuffer, 4, 4);
        }
    }

    public IPEndPoint EndPoint
    {
        get { return new IPEndPoint(new IPAddress(IP), Port);
            }
        set
        {
            Port = (short)value.Port;
            IP = value.Address.GetAddressBytes();
        }
    }

    public string GetErrorMessage()
    {
        switch (Error)
        {
        case 0x5A:
            return "允许转发";
        case 0x5B:
            return "拒绝转发，一般性失败";
        case 0x5C:
            return "拒绝转发，SOCKS 4 Server无法连接到SOCS 4 Client所在主机的IDENT服务";
        case 0x5D:
            return "拒绝转发，请求报文中的USERID与IDENT服务返回值不相符";
        }
        return "未知错误";
    }

    public IOBuffer ToBuffer()
    {
        return new IOBuffer(_IOBuffer, 0, 8);
    }
};

_jingxian_end

#endif // _Proxy_H_