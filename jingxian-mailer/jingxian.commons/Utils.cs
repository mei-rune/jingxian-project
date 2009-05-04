using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace jingxian
{
    public class Utils
    {

        public static int ReadInt(string txt, int defaultValue)
        {
            if (string.IsNullOrEmpty(txt))
                return defaultValue;
            int value = 0;
            if (int.TryParse(txt, out value))
                return value;
            return defaultValue;
        }



        public static bool ReadBoolean(string txt)
        {
            return 0 == string.Compare( txt, "true", true );
        }

       public static void ShowMsgBox(Exception e)
        {
            MessageBox.Show(e.Message, "错误");
        }

       public static void ShowMsgBox(string txt)
       {
           MessageBox.Show( txt, "警告");
       }

       public static object invokeMethod( object instance, string methodName, object[] parameters )
       {
           if (null == instance)
               throw new ArgumentNullException("instance");

           Type implementationType = instance.GetType();
           MethodInfo methodInfo = implementationType.GetMethod(methodName);
           if (null == methodInfo)
               throw new NotImplementedException( string.Format( "类型\"{0}\"没有\"{1}\" 方法!", implementationType.Name, methodName ) );

           return methodInfo.Invoke(instance, parameters);
       }

       public static object invokeGetter(object instance, string key)
       {
           Type instanceType = instance.GetType();
           PropertyInfo propertyInfo = instanceType.GetProperty(key);
           if (null != propertyInfo)
           {
               if (!propertyInfo.CanRead)
                   return null;

               return propertyInfo.GetValue(instance, null);
           }
           else
           {
               FieldInfo fieldInfo = instanceType.GetField(key);
               if (null != fieldInfo)
                   return fieldInfo.GetValue(instance);

               return null;
           }
       }


       public static object invokeSetter(object instance, string key, object value)
       {
           return invokeSetter(instance, key, value, null);
       }

       public static object invokeSetter(object instance, string key, object value, IConverter converter )
       {
           Type instanceType = instance.GetType();

           PropertyInfo propertyInfo = instanceType.GetProperty(key);
           if (null != propertyInfo)
           {
               if (!propertyInfo.CanWrite)
                   return null;

               object oldValue = (propertyInfo.CanRead) ? (propertyInfo.GetValue(instance, null) ?? value) : value;

               if (null == converter)
                   propertyInfo.SetValue(instance, Converter.Instance.ConvertTo(value, propertyInfo.PropertyType), null);
               else
                   propertyInfo.SetValue(instance, converter.ConvertTo(value), null);

               return oldValue;
           }
           else
           {

               FieldInfo fieldInfo = instanceType.GetField(key);
               if (null == fieldInfo)
                   return null;

               object oldValue = fieldInfo.GetValue(instance) ?? value;


               if (null == converter)
                   fieldInfo.SetValue(instance, Converter.Instance.ConvertTo(value, propertyInfo.PropertyType));
               else
                   fieldInfo.SetValue(instance, converter.ConvertTo(value));

               return oldValue;
           }
       }
    }
}
