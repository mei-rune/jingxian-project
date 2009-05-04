using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace jingxian.collections
{
    public class Properties : IEnumerable<KeyValuePair<string,string>>
    {
        Dictionary<string, string> _dict;// = new Dictionary<string, string>();
        public Properties()
        {
            _dict = new Dictionary<string, string>();
        }


        public Properties(Properties properties)
            : this( properties._dict )
        {
        }


        public Properties(IDictionary<string,string> properties)
        {
            _dict = new Dictionary<string, string>(properties);
        }

        public string Put(string key, string value)
        {
            string oldValue = null;
            _dict.TryGetValue(key, out oldValue);
            _dict[key] = value;
            return oldValue;
        }

        public string Get(string key)
        {
            string oldValue = null;
            _dict.TryGetValue(key, out oldValue);
            return oldValue;
        }

        public string Remove(string key)
        {
            string oldValue = null;
            if (_dict.TryGetValue(key, out oldValue))
                _dict.Remove(key);
            return oldValue;
        }

        public string this[string key]
        {
            get { return Get(key); }
            set { Put(key, value); }
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        static XmlAttribute AppendXmlAttribute(XmlElement el, string key, string value)
        {
            XmlAttribute result = el.OwnerDocument.CreateAttribute(key);
            result.Value = value;
            el.Attributes.Append(result);
            return result;
        }

        public static string GetXMLString(Properties properties)
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><configuration></configuration>");
            XmlElement el = doc.DocumentElement.SelectSingleNode("//configuration") as XmlElement;

            foreach (KeyValuePair<string, string> kp in properties)
            {
                XmlElement node = el.OwnerDocument.CreateElement("add");
                AppendXmlAttribute(node, "key", kp.Key);
                AppendXmlAttribute(node, "value", kp.Value);
                el.AppendChild(node);
            }

            return doc.OuterXml;
        }

        public static Properties BuildFormXMLString(string value)
        {
            Properties properties = new Properties();
            if (string.IsNullOrEmpty(value))
                return properties;

            if (value.Substring(0, 5) == "<?xml")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(value);

                foreach (XmlElement element in doc.DocumentElement.SelectNodes("//add"))
                {
                    XmlNode key = element.Attributes["key"];
                    if (null == key || string.IsNullOrEmpty(key.Value))
                        continue;

                    XmlNode valueNode = element.Attributes["value"];
                    properties[key.Value] = (null == valueNode) ? null : valueNode.Value;
                }
                //using (System.IO.StringReader reader = new StringReader(value))
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(XmlConfiguration));
                //    foreach(XmlItem item in (XmlItem[])serializer.Deserialize(reader))
                //    {
                //        _dict[item.Key] = item.Value;
                //    }
                //}
            }
            else
            {
                // 用$号分隔的格式
                string[] values = value.Split(new char[] { '$' });
                for (int i = 0; i < values.Length - 1; i += 2)
                {
                    properties[values[i]] = values[i + 1];
                }
            }
            return properties;
        }
    }
}
