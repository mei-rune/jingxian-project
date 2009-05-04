using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian
{

    public interface IConverter
    {
        object ConvertTo(object parameter);
    }

    public interface IConvertSystem
    {
        IConverter Find(object parameter, Type to_type);

        void AddConverter(Type type, IConverter converter);

        object ConvertTo(object parameter, Type to_type);
    }

    public class Converter : IConvertSystem
    {
        Dictionary<Type, IConverter> _converters = new Dictionary<Type, IConverter>();
        //Dictionary<Type, IConverter> _converterByconvertTypes = new Dictionary<Type, IConverter>();

        public static Converter Instance = new Converter();

        public Converter()
        {
            _converters[typeof(bool)] = new DefaultConverter(typeof(bool));
            _converters[typeof(byte)] = new DefaultConverter(typeof(byte));
            _converters[typeof(sbyte)] = new DefaultConverter(typeof(sbyte));
            _converters[typeof(short)] = new DefaultConverter(typeof(short));
            _converters[typeof(ushort)] = new DefaultConverter(typeof(ushort));
            _converters[typeof(int)] = new DefaultConverter(typeof(int));
            _converters[typeof(uint)] = new DefaultConverter(typeof(uint));
            _converters[typeof(long)] = new DefaultConverter(typeof(long));
            _converters[typeof(ulong)] = new DefaultConverter(typeof(ulong));
            _converters[typeof(char)] = new DefaultConverter(typeof(char));
            _converters[typeof(double)] = new DefaultConverter(typeof(double));
            _converters[typeof(float)] = new DefaultConverter(typeof(float));
            _converters[typeof(TimeSpan)] = new DefaultConverter(typeof(TimeSpan));
            _converters[typeof(DateTime)] = new DefaultConverter(typeof(DateTime));
            _converters[typeof(System.Net.IPAddress)] = new DefaultConverter(typeof(System.Net.IPAddress));
            _converters[typeof(string)] = new StringConverter();
        }

        public object ConvertTo(object parameter, Type type)
        {
            if (null == parameter)
                return null;
            if (type.IsAssignableFrom(parameter.GetType()))
                return parameter;
            IConverter converter = null;
            if (_converters.TryGetValue(type, out converter) && null != converter)
                return converter.ConvertTo(parameter);

            converter = GetConverterFrom(parameter);
            if (null != converter)
                converter.ConvertTo(parameter);

            throw new NotSupportedException("不支持的数据类型[" + type.Name + "]!");
        }

        public IConverter Find(object parameter, Type type)
        {
            if (null == parameter)
                return null;

            IConverter converter = null;
            if (_converters.TryGetValue(type, out converter) && null != converter)
                return converter;

            return GetConverterFrom(parameter);
        }

        IConverter GetConverterFrom(object parameter)
        {
            return null;
        //    string name = KernelUtils.GetAsString(properties, "converter");
        //    if (string.IsNullOrEmpty(name))
        //        return null;
        //    Type type = Type.GetType(name);
        //    if (null == type)
        //        return null;

        //    return makeConverter(type);
        //    // 如果用实现的IConverter不是线程安全的,有可能会有问题
        //    //IConverter converter = null;
        //    //if (_converterByconvertTypes.TryGetValue(type, out converter))
        //    //    return converter;

        //    //converter = makeConverter(type);
        //    //_converterByconvertTypes[type] = converter;
        //    //return converter;
        }

        object createInstance(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(IConvertSystem) });
            if (null != constructorInfo)
                return Activator.CreateInstance(type, this);

            constructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (null != constructorInfo)
                return Activator.CreateInstance(type);

            return null;
        }

        IConverter makeConverter(Type type)
        {
            if (typeof(IConverter).IsAssignableFrom(type))
            {
                ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(IConvertSystem) });
                if (null != constructorInfo)
                    return (IConverter)Activator.CreateInstance(type, this);

                constructorInfo = type.GetConstructor(Type.EmptyTypes);
                if (null != constructorInfo)
                    return (IConverter)Activator.CreateInstance(type);

                return null;
            }

            MethodInfo methodInfo = type.GetMethod("ConvertTo");
            if (null == methodInfo)
                return null;

            object instance = null;
            if (!methodInfo.IsStatic)
                instance = createInstance(type);

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            switch (parameterInfos.Length)
            {
                case 1:
                    return new NotContextMethodConverter(instance, methodInfo);
                case 2:
                    return new MethodConverter(instance, methodInfo);
                default:
                    return null;
            }
        }

        public void AddConverter(Type type, IConverter converter)
        {
            _converters[type] = converter;
        }

        public class DefaultConverter : IConverter
        {
            Type _type;
            MethodInfo _methodInfo;

            public DefaultConverter(Type type)
            {
                _type = type;

                _methodInfo = _type.GetMethod("Parse");
                if (null == _methodInfo)
                    throw new NotSupportedException();
                if (_methodInfo.IsStatic)
                    throw new NotSupportedException();
            }

            public object ConvertTo(object parameter)
            {
                return _methodInfo.Invoke(null, new object[] { parameter.ToString() });
            }
        }

        public class StringConverter : IConverter
        {
            public object ConvertTo(object parameter)
            {
                return parameter.ToString();
            }
        }

        public class MethodConverter : IConverter
        {
            object _instance;
            MethodInfo _methodInfo;

            public MethodConverter(object instance, MethodInfo methodInfo)
            {
                _instance = instance;
                _methodInfo = methodInfo;
            }
            public object ConvertTo(object parameter)
            {
                return _methodInfo.Invoke(Instance, new object[] { parameter });
            }
        }

        public class NotContextMethodConverter : IConverter
        {
            object _instance;
            MethodInfo _methodInfo;

            public NotContextMethodConverter(object instance, MethodInfo methodInfo)
            {
                _instance = instance;
                _methodInfo = methodInfo;
            }
            public object ConvertTo(object parameter)
            {
                return _methodInfo.Invoke(Instance, new object[] { parameter });
            }
        }
    }
}
