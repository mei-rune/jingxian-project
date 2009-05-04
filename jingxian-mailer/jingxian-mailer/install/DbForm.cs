using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace jingxian.install
{
    using jingxian.collections;

    public interface IDbEditor
    {
        Properties Driver { get;set; }
        Control Widget{get;}
    }

    public partial class DbForm : Form
    {
        EventHandler _onTestDB;
        EventHandler _onSaveDB;
        EventHandler _onGenerateTable;

        private IDbEditor _connectionEditor;
        string _dbPath = "Database.config";

        public DbForm(Properties properties, EventHandler onTestDB, EventHandler onGenerateTable, EventHandler onSaveDB)
        {
            if (null == properties)
                throw new ArgumentNullException("properties");
            if (null == onSaveDB)
                throw new ArgumentNullException("onSaveDB");
            if (null == onTestDB)
                throw new ArgumentNullException("onTestDB");
            if (null == onGenerateTable)
                throw new ArgumentNullException("onGenerateTable");
 
            _onTestDB = onTestDB;
            _onSaveDB = onSaveDB;
            _onGenerateTable = onGenerateTable;
            
            InitializeComponent();

            this.DBType_comboBox.Items.Clear();
            this.DBType_comboBox.Add( new SQLServerControl());
#if ORACLE
            this.DBType_comboBox.Add( new OracleControl()  );
#endif

            string action = "载入配置";
            try
            {
                foreach (IDbEditor dbEditor in this.DBType_comboBox.GetObjects() )
                {
                    dbEditor.Driver = properties;
                }

                action = "分析连接串";
                string typeKey = properties.Get("driverOdbc");

                if (null != typeKey)
                {
                    this.DBType_comboBox.SelectText(typeKey.Trim().Trim('{', '}'));
                    DBType_ComboBox_SelectedIndexChanged(this, EventArgs.Empty);
                    return;
                }

                this.DBType_comboBox.SelectedIndex = 0;
                DBType_ComboBox_SelectedIndexChanged(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("{0}时,发生异常 - {1}!", action, e.Message));
            }
        }

        private void DBType_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            if (null != _connectionEditor)
                this.tableLayoutPanel1.Controls.Remove(_connectionEditor.Widget );

            _connectionEditor = this.DBType_comboBox.Selected as IDbEditor;
            _connectionEditor.Widget.Dock = System.Windows.Forms.DockStyle.Fill;

            this.tableLayoutPanel1.SetColumnSpan(_connectionEditor.Widget, 2);
            this.tableLayoutPanel1.Controls.Add(_connectionEditor.Widget, 1, 3);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        public Properties GetResult()
        {
            return _connectionEditor.Driver;
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Test_Button_Click(object sender, EventArgs e)
        {
            try
            {
                _onTestDB(sender, e);
                MessageBox.Show(this, "数据库连接测试成功", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, GetMessageString( exception ), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        string GetMessageString(Exception e)
        {
            while (null != e.InnerException)
            {
                e = e.InnerException;
                if (e is System.Data.Common.DbException)
                    break;
            }
            return e.Message;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            string action = "测试数据库";
            try
            {
                _onTestDB(sender, e);
                action = "保存数据库配置";
                _onSaveDB(sender, e);
                action = "更新数据库";
                _onGenerateTable(sender, e);

                MessageBox.Show(this, "数据库配置保存成功", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Concat("当", action, "时发生错误，原因如下：\n", GetMessageString(exception) ), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}