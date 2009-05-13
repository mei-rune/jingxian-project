using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

//namespace Hazel.MicroKernel
//{
//    public class KernelUtils
//    {
//        [ThreadStatic]
//        public static CreationContext _creationContext;

//        public static CreationContext getCreationContext()
//        {
//            return _creationContext;
//        }

//        public static void setCreationContext(CreationContext context)
//        {
//            _creationContext = context;
//        }

//        public static void Start(IKernel kernel, IServiceRef serviceRef )
//        {
//            invokeMethod("Start", kernel, serviceRef);
//        }

//        public static void Stop(IKernel kernel, IServiceRef serviceRef)
//        {
//            invokeMethod("Stop", kernel, serviceRef);
//        }

//        static void invokeMethod(string methodName, IKernel kernel, IServiceRef serviceRef)
//        {
//            MethodInfo methodInfo = _creationContext.ImplementationType.GetMethod(methodName);
//            if (null == methodInfo)
//                return;

//            ParameterInfo[] parameters = methodInfo.GetParameters();
//            switch (parameters.Length)
//            {
//                case 0:
//                    {
//                        object instance = serviceRef.Get();
//                        methodInfo.Invoke(instance, new object[] { });
//                        break;
//                    }
//                case 1:
//                    if (parameters[0].ParameterType == typeof(IKernel))
//                    {
//                        object instance = serviceRef.Get();
//                        methodInfo.Invoke(instance, new object[] { kernel });
//                    }
//                    break;
//            }
//        }



//        public static object invokeGetter(object instance, string key)
//        {
//            Type instanceType = instance.GetType();
//            PropertyInfo propertyInfo = instanceType.GetProperty(key);
//            if (null != propertyInfo)
//            {
//                if (!propertyInfo.CanRead)
//                    return null;

//                return propertyInfo.GetValue(instance, null);
//            }
//            else
//            {
//                FieldInfo fieldInfo = instanceType.GetField(key);
//                if (null != fieldInfo)
//                    return propertyInfo.GetValue(instance, null);

//                return null;
//            }
//        }

//        public static object invokeSetter(object instance, string key, object value, IConvertSystem converter)
//        {
//            Type instanceType = instance.GetType();

//            PropertyInfo propertyInfo = instanceType.GetProperty(key);
//            if (null != propertyInfo)
//            {
//                if (!propertyInfo.CanWrite)
//                    return null;

//                object oldValue = (propertyInfo.CanRead) ? (propertyInfo.GetValue(instance, null) ?? value) : value;

//                propertyInfo.SetValue(instance, (null == converter) ? value : converter.ConvertTo(value, propertyInfo.PropertyType, null), null);

//                return oldValue;
//            }
//            else
//            {

//                FieldInfo fieldInfo = instanceType.GetField(key);
//                if (null == fieldInfo)
//                    return null;

//                object oldValue = propertyInfo.GetValue(instance, null) ?? value;

//                fieldInfo.SetValue(instance, (null == converter) ? value : converter.ConvertTo(value, propertyInfo.PropertyType, null));
//                return oldValue;
//            }
//        }

//        public static string GetAsString(IProperties properties, string key)
//        {
//            object r = properties[key];
//            return (null == r) ? string.Empty : r.ToString();
//        }

//        public static int GetAsInteger(IProperties properties, string key, int defaultValue)
//        {
//            object r = properties[key];
//            if (null == r)
//                return defaultValue;

//            if (r.GetType() == typeof(int))
//                return (int)r;

//            int value = 0;
//            if (int.TryParse(r.ToString(), out value))
//                return value;
//            return defaultValue;
//        }



//       public static object readFromProperites(IProperties properties, Type type, IConvertSystem convertSystem)
//        {
//            if (null == properties)
//                return null;

//            if (!type.IsClass && !type.IsValueType)
//                return null;

//            if (type.IsAbstract)
//                return null;

//            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
//            if (null == constructorInfo)
//                return null;

//            object instance = constructorInfo.Invoke(new object[] { });
//            foreach (KeyValuePair<string, object> kp in properties)
//            {
//                invokeSetter(instance, kp.Key, kp.Value, convertSystem);
//            }

//            return instance;
//        }
//    }
//}
