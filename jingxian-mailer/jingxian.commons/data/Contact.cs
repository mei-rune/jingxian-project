
namespace jingxian.data
{
    public class Contact
    {

        private int _id;
        private string _name;
        private string _nickname;
        private string _mail;
        private string _mobilephone;
        private string _phone;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string NickName
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        public string Mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        public string MobilePhone
        {
            get { return _mobilephone; }
            set { _mobilephone = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
    }
}
