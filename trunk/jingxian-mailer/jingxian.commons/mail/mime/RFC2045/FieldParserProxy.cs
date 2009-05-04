
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace jingxian.mail.mime.RFC2045
//{
//    public class FieldParserProxy : IFieldParser,IFieldParserProxy
//    {
//        protected static FieldParserProxy s_Proxy = null;

//        protected FieldParserProxy() 
//        {            
//        }        

//        public static FieldParserProxy Getinstance()
//        {
//            if (s_Proxy == null)
//                s_Proxy = new FieldParserProxy();
//            return s_Proxy;
//        }

//        public void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
//        {         
//        }

//        #region IFieldParserProxy ≥…‘±

//        public void CompilePattern()
//        {
//            throw new NotImplementedException();
//        }

//        public System.Text.RegularExpressions.Regex CompositeType
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public System.Text.RegularExpressions.Regex DescriteType
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public System.Text.RegularExpressions.Regex EndBoundary
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public System.Text.RegularExpressions.Regex MIMEVersion
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public System.Text.RegularExpressions.Regex AddrSpec
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public System.Text.RegularExpressions.Regex StartBoundary
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public string Unfold(string headers)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion
//    }
//}
