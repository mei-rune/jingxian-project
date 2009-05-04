using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;



namespace jingxian.install
{
    using jingxian.collections;

    public partial class OracleControl : UserControl, IDbEditor
    {
        Properties _properties;

        public OracleControl( )
        {
            InitializeComponent();
        }

        #region IDbEditor ≥…‘±

        public Properties Driver
        {
            get
            {
                if (null == _properties)
                    _properties = new Properties();

                _properties.Put("userid", this.DBUsername_textBox.Text);
                _properties.Put("password", this.DBPassword_textBox.Text);
                _properties.Put("datasource", this.DBTNS_textBox.Text);
                _properties.Put("driverOdbc", string.Concat( "{", this.ToString(), "}" ) );
                return _properties;
            }
            set
            {
                _properties = value;

                this.DBUsername_textBox.Text = _properties.Get("userid") ?? "";
                this.DBPassword_textBox.Text = _properties.Get("password") ?? "";
                this.DBTNS_textBox.Text = _properties.Get("datasource") ?? "";

            }
        }

        public Control Widget
        {
            get { return this; }
        }

        #endregion

        public override string ToString()
        {
            return "Oracle";
        }
    }
}