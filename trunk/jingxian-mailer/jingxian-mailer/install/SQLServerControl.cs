using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace jingxian.install
{
    using jingxian.collections;

    public partial class SQLServerControl : UserControl, IDbEditor
    {
        Properties _properties;
        public SQLServerControl()
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
                _properties.Put("database", this.DBInitialCatalog_textBox.Text);
                _properties.Put("datasource", this.DBHost_textBox.Text);
                _properties.Put("driverOdbc", string.Concat("{", this.ToString(), "}"));
                return _properties;
            }
            set
            {
                _properties = value;

                this.DBUsername_textBox.Text = _properties.Get("userid") ?? "";
                this.DBPassword_textBox.Text = _properties.Get("password") ?? "";
                this.DBInitialCatalog_textBox.Text = _properties.Get("database") ?? "";
                this.DBHost_textBox.Text = _properties.Get("datasource") ?? "";
            }
        }

        public Control Widget
        {
            get { return this; }
        }

        #endregion

        public override string ToString()
        {
            return "SQL Server";
        }
    }
}
