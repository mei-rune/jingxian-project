using System;

namespace jingxian.data
{
    using jingxian.collections;
    
    public enum Behaviour { LeaveMessageOnServer, RemoveFromServer, Disabled };

    public class BaseServer
    {
        private int _id = -1;
        private string _caption;
        private string _address;
        private int _port;
        private string _username;
        private string _password;
        protected string _type;
        private Properties _misc;

        public BaseServer()
        {
        }

        public BaseServer(string nm, string address, int port, string username, string password)
        {
            _caption = nm;
            _address = address;
            _port = port;
            _username = username;
            _password = password;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Type
        {
            get { return _type; }
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public Properties Misc
        {
            get 
            {
                if (null == _misc)
                    _misc = new Properties();
                return _misc; 
            }

            set { _misc = value; }
        }

        public override string ToString()
        {
            if (0 < this._id )
                return this.Caption;
            return this.Caption + " ( Î´±£´æ )";
        }
    }

    public class SmtpServer : BaseServer
    {

        public SmtpServer()
        {
            _type = "smtp";
        }

        public SmtpServer(string nm, string address, int port, string username, string password)
        : base( nm, address, port, username, password )
        {
            _type = "smtp";
        }

        public int Proxy
        {
            get
            {
                int id = 0;
                if( int.TryParse( Misc["proxy"], out id ) )
                    return id;
                return 0;
            }
            set
            {
                if (value <= 0)
                    Misc.Remove("proxy");
                else
                    Misc["proxy"] = value.ToString();
            }
        }


        public string Display
        {
            get { return Misc["display"]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Misc.Remove("display");
                else
                    Misc["display"] = value;
            }
        }

        public string Signature
        {
            get { return Misc["signature"]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Misc.Remove("signature");
                else
                    Misc["signature"] = value;
            }
        }
    }

    public class PopServer : BaseServer
    {

        public PopServer()
        {
            _type = "pop";
        }

        public PopServer(string nm, string address, int port, string username, string password)
            : base(nm, address, port, username, password)
        {
            _type = "pop";
        }

        public int Proxy
        {
            get
            {
                int id = 0;
                if (int.TryParse(Misc["proxy"], out id))
                    return id;
                return 0;
            }
            set
            {
                if (value <= 0)
                    Misc.Remove("proxy");
                else
                    Misc["proxy"] = value.ToString();
            }
        }

        public int SSL
        {
            get
            {
                int id = 0;
                if (int.TryParse(Misc["ssl"], out id))
                    return id;
                return 0;
            }
            set
            {
                if (value <= 0)
                    Misc.Remove("ssl");
                else
                    Misc["ssl"] = value.ToString();
            }
        }

        public string Behaviour
        {
            get { return Misc["behaviour"]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Misc.Remove("behaviour");
                else
                    Misc["behaviour"] = value;
            }
        }

        public bool HeaderOnly
        {
            get { return 0 == string.Compare("True", Misc["headeronly"], true ); }
            set
            {
                if (value)
                    Misc["headeronly"] = value.ToString();
                else
                    Misc.Remove("headeronly");
            }
        }

        public bool Disabled
        {
            get { return 0 == string.Compare("True", Misc["disabled"], true); }
            set
            {
                if (value)
                    Misc["disabled"] = value.ToString();
                else
                    Misc.Remove("disabled");
            }
        }
        
    }

    public class ProxyServer : BaseServer
    {

        public ProxyServer()
        {
            _type = "proxy";
        }

        public ProxyServer(string nm, string address, int port, string username, string password)
            : base(nm, address, port, username, password)
        {
            _type = "proxy";
        }

        public int Version
        {
            get
            {
                int version = 0;
                if (int.TryParse(Misc["version"], out version))
                    return version;
                return 0;
            }
            set { Misc["version"] = value.ToString(); }
        }

        public bool EnableDNS
        {
            get { return 0 == string.Compare("True", Misc["EnableDNS"], true); }
            set
            {
                if (value)
                    Misc["EnableDNS"] = value.ToString();
                else
                    Misc.Remove("EnableDNS");
            }
        }
    }

    public class IMAPServer : BaseServer
    { 

        public IMAPServer()
        {
            _type = "imap";
        }

        public IMAPServer(string nm, string address, int port, string username, string password)
            : base(nm, address, port, username, password)
        {
            _type = "imap";
        }

        public int Proxy
        {
            get
            {
                int id = 0;
                if (int.TryParse(Misc["proxy"], out id))
                    return id;
                return 0;
            }
            set
            {
                if (value <= 0)
                    Misc.Remove("proxy");
                else
                    Misc["proxy"] = value.ToString();
            }
        }

        public int SSL
        {
            get
            {
                int id = 0;
                if (int.TryParse(Misc["ssl"], out id))
                    return id;
                return 0;
            }
            set
            {
                if (value <= 0)
                    Misc.Remove("ssl");
                else
                    Misc["ssl"] = value.ToString();
            }
        }

        public string Behaviour
        {
            get { return Misc["behaviour"]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Misc.Remove("behaviour");
                else
                    Misc["behaviour"] = value;
            }
        }

        public bool HeaderOnly
        {
            get { return 0 == string.Compare("True", Misc["headeronly"], true); }
            set
            {
                if (value)
                    Misc["headeronly"] = value.ToString();
                else
                    Misc.Remove("headeronly");
            }
        }

        public bool Disabled
        {
            get { return 0 == string.Compare("True", Misc["disabled"], true); }
            set
            {
                if (value)
                    Misc["disabled"] = value.ToString();
                else
                    Misc.Remove("disabled");
            }
        }
    }
}