using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;

namespace jingxian.install
{
    using jingxian.collections;

    public class DbInsteller
    {
        string _binPath;
        DbForm _form;
        public DbInsteller( )
            : this( null )
        {
        }

        public DbInsteller( string binPath )
        {
            _binPath = binPath;
            if (!string.IsNullOrEmpty(_binPath))
                return;
            _binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public bool Instell( bool ignoreExists )
        {
            string databaseFile = Path.Combine( _binPath, "Database.Config" );


            Properties dict = new Properties();
            if (File.Exists(databaseFile))
            {
                if (ignoreExists)
                    return true;

                using (System.Xml.XmlReader reader = new System.Xml.XmlTextReader(databaseFile))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "add")
                        {
                            string key = reader.GetAttribute("key");
                            if (string.IsNullOrEmpty(key))
                                continue;

                            dict[key] = reader.GetAttribute("value");
                        }
                    }
                }
            }

            _form = new DbForm(dict, this.OnTestDB, this.OnGenerateTable, this.OnSaveDB);
            return _form.ShowDialog( ) == System.Windows.Forms.DialogResult.OK;
        }

        private void EnsureValue(Properties properties, string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            if (string.IsNullOrEmpty(properties.Get(key)))
                properties.Put(key, value);
        }

        private void AppendXmlElement(XmlElement node, string key, string value)
        {
            XmlElement newChild = node.OwnerDocument.CreateElement("add");
            node.AppendChild(newChild);
            newChild.Attributes.Append(node.OwnerDocument.CreateAttribute("key")).Value = key;
            newChild.Attributes.Append(node.OwnerDocument.CreateAttribute("value")).Value = value;
        }

        public XmlDocument GetResult()
        {
            Properties properties = _form.GetResult();

            EnsureValue(properties, "useridHibernate", "NHibernate");
            EnsureValue(properties, "databaseHibernate", "NHibernate");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><settings></settings>");
            XmlElement root = doc.DocumentElement;

            foreach (KeyValuePair<string, string> kp in properties)
            {
                AppendXmlElement(root, kp.Key, kp.Value); 
            }
            return doc;
        }

        private IBatisNet.DataMapper.ISqlMapper CreateMapper(string propertiesFile)
        {
            string databaseFile = Path.Combine(_binPath, "db.Config");
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load(databaseFile);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlConfig.NameTable);
            nsmgr.AddNamespace("ab", "http://ibatis.apache.org/dataMapper");
            xmlConfig.SelectSingleNode("/ab:sqlMapConfig/ab:properties/@resource", nsmgr).Value = propertiesFile;

            IBatisNet.DataMapper.Configuration.DomSqlMapBuilder builder = new IBatisNet.DataMapper.Configuration.DomSqlMapBuilder();
            return builder.Configure(xmlConfig);
        }

        public void OnSaveDB(object sender, EventArgs e)
        {
            string propertiesFile = Path.Combine(_binPath, "Database.Config");
            GetResult().Save(propertiesFile);
        }

        public void OnTestDB(object sender, EventArgs e)
        {
            string propertiesFile = Path.Combine(_binPath, "tmpdb.Config");
            GetResult().Save(propertiesFile);
            IBatisNet.DataMapper.ISqlMapper mapper =  CreateMapper( propertiesFile);
           
            mapper.OpenConnection().OpenConnection();
            mapper.CloseConnection();
        }

        public void OnGenerateTable(object sender, EventArgs e)
        {
            string propertiesFile = Path.Combine(_binPath, "Database.Config");
            //GetResult().Save(propertiesFile);
            IBatisNet.DataMapper.ISqlMapper mapper = CreateMapper(propertiesFile);

            mapper.Insert("generateTable", 0);
        }
    }
}
