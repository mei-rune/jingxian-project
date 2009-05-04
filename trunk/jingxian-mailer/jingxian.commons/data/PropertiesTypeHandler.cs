using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.data
{
    public class PropertiesTypeHandler : IBatisNet.DataMapper.TypeHandlers.ITypeHandlerCallback
    {
        #region ITypeHandlerCallback members

        public object ValueOf(string nullValue)
        {
            throw new NotImplementedException();
        }

        public object GetResult(IBatisNet.DataMapper.TypeHandlers.IResultGetter getter)
        {
            return  jingxian.collections.Properties.BuildFormXMLString( getter.Value.ToString() );
        }

        public void SetParameter(IBatisNet.DataMapper.TypeHandlers.IParameterSetter setter, object parameter)
        {
            setter.Value = jingxian.collections.Properties.GetXMLString( parameter as jingxian.collections.Properties );
        }
        #endregion


        #region ITypeHandlerCallback 成员


        public object NullValue
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
