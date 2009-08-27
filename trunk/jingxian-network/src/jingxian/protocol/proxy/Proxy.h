
#ifndef _Proxy_H_
#define _Proxy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <list>
# include "jingxian/protocol/proxy/ICredentialPolicy.h"
# include "jingxian/protocol/proxy/config/Configuration.h"
# include "jingxian/networks/IOCPServer.h"

_jingxian_begin

namespace proxy
{
    struct BindPort
    {
        bool IsUsed;
        int Port;

		BindPort()
		{
			IsUsed = false;
			Port = 0;
		}
    };

    class BindPorts
    {

	public:
		BindPorts(int begin, int end)
			: _begin(begin)
			, _end(end)
			, _position(-1)
        {
            if (begin > end)
                throw new ArgumentException("begin 不能比 end 还要大!");

            _ports = new BindPort[ Math.Min( 1000,_end - _begin  ) ];

			BindPort bindPort;
            for (int i = _begin; i <= _end; i++)
            {
                bindPort.IsUsed = false;
                bindPort.Port = i;

				_ports.push_back(bindPort);
            }
        }

        public int GetPort()
        {

            for (size_t i = 0; i < _ports.size(); i++)
            {
                _position++;
                if (!_ports[_position % _ports.size()].IsUsed)
                    return _ports[_position % _ports.size()].Port;
            }
            return 0;
        }

        public void ReleasePort( int port)
        {
            if( _begin > port || _end < port )
                return;

            _ports[ port - _begin ].IsUsed = false;
        }

	private:
		
        int _begin;
        int _end;

		std::vector<BindPort> _ports;
        int _position;
    };

	class Proxy  : public AbstractServer
	{
	public:
		Proxy(IOCPServer& core, const tstring& addr)
			: AbstractServer( &core )
		{
			if(!this->initialize(addr))
			{
				FATAL(log(), _T("初始代理服务失败"));
				return;
			}
			acceptor_.accept(this, &EchoServer::OnComplete, &EchoServer::OnError, &core);
		}

        static ILog _logger = LogUtils.Factory.GetLogger( typeof(Proxy) );
        Config.Configuration _configuration;
        //IServiceProvider _serviceProvider;
        Credentials _credentials = new Credentials();

        BindPorts _bindPorts;
        IPAddress _bindIP;

        IPSeg[] _allowedIPs;
        IPSeg[] _blockingIPs;

        public Proxy(IInitializeContext context)
            : base(null)
        {
            if (null == _logger)
                _logger = context.LogFactory.GetLogger(typeof(Proxy));

            IVirtualFileSystem vfs = (IVirtualFileSystem)context.GetService(typeof(IVirtualFileSystem));
            string path = vfs.GetRunPath("proxy.config");
            if (!File.Exists(path))
            {
                string message = string.Format("载入配置失败 - 文件[{0}]不存在!", path);
                throw new RuntimeError(message);
            }

            Initialize(Helper.DeserializeObject<Config.Configuration>(path));
        }

        void Initialize(Config.Configuration config)
        {
            _configuration = config;

            string action = "载入授权模块";

            foreach (Config.Credential credential in _configuration.Credentials)
            {
                try
                {
                    action = "载入授权模块[" + credential.Name + "]";

                    Type type = Helper.GetType(credential.Implement);
                    if (null == type)
                    {
                        _logger.ErrorFormat("在{0}时，发生错误 - 不能载入类型[{1}]!", action, credential.Implement);
                        continue;
                    }

                    if (!typeof(ICredentialPolicy).IsAssignableFrom(type))
                    {
                        _logger.ErrorFormat("在{0}时，发生错误 - 类型[{1}]没有实现接口[ICredentialPolicy]!", action, credential.Implement);
                        continue;
                    }

                    _credentials.Add(new CredentialPolicyFactory(type, credential, this));

                    _logger.InfoFormat(action + "成功!");
                }
                catch (Exception e)
                {
                    _logger.Error(string.Format("在{0}时，发生异常!", action), e);
                }
            }

            foreach (Item item in config.Misc)
            {
                Misc[item.Key] = item.Value;
            }

            string bindPort = Helper.Read(Misc, "BindPort", "30010-30020");
            string[] ar = bindPort.Split('-');
            int begin = 0;
            int end = 0;

            if (!int.TryParse(ar[0].Trim(), out begin))
                begin = 30010;

            if ( 1 == ar.Length || !int.TryParse(ar[1].Trim(), out end))
                end = 30020;

            _bindPorts = new BindPorts(begin, end);
            _bindIP = Helper.Read(Misc, "BindIP", IPAddress.Any);


            _allowedIPs = ParseIPSeg( config.AllowedIPs );
            _blockingIPs = ParseIPSeg(config.BlockingIPs );
        }

        public IPSeg[] ParseIPSeg( string[] ipStr )
        {
            if ( null == ipStr)
                return null;

            List<IPSeg> ipSegs = new List<IPSeg>();

            foreach (string allowedIP in ipStr)
            {
                try
                {
                    ipSegs.Add(new IPSeg(allowedIP));
                }
                catch
                { }
            }

            return 0 == ipSegs.Count ? null : ipSegs.ToArray();
        }

        public Config.Configuration Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        public ICredentials Credentials
        {
            get { return _credentials; }
        }

        public bool IsBlockingIP(IPAddress ip)
        {
            if (null == _blockingIPs)
                return false;

            foreach (IPSeg seg in _blockingIPs)
            {
                if (seg.In(ip)) return true;
            }
            return false;
        }


        public bool IsAllowedIP(IPAddress ip)
        {
            if (IsBlockingIP(ip))
                return false;

            if (null == _allowedIPs)
                return true;

            foreach (IPSeg seg in _allowedIPs)
            {
                if (seg.In(ip)) return true;
            }
            return false;
        }

        public IPAddress BindIP
        {
            get{return _bindIP;}
        }

        public int GetBindPort()
        {
            return _bindPorts.GetPort();
        }

        public void ReleaseBindPort( int port)
        {
             _bindPorts.ReleasePort(port);
        }

        #region IProtocolFactory 成员

        public override IProtocol BuildProtocol(ITransport transport)
        {
            return new SOCKSProtocol(this);
        }
        #endregion
	};
}

_jingxian_end

#endif // _Proxy_H_