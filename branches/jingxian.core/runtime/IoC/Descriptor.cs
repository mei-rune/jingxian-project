

using System;
using System.Collections.Generic;
using System.Globalization;


namespace jingxian.core.runtime
{
    public class Descriptor : IComponentDescriptor
    {
        // 组件的id
        string _id;
        // 组件的接口
        IEnumerable<Type> _services;
        // 组件的实现类型
        Type _implementationType;
        // 生命周期
        ComponentLifestyle _lifestyle;
        // 启动组别
        int _level = int.MaxValue;
        // 参数
        IEnumerable<IParameter> _parameters;
        // 扩展属性
        IProperties _extendedProperties;

        public Descriptor( string id
            , IEnumerable<Type> services
            , Type bestKnownImplementationType)
            : this(id
            , services
            , bestKnownImplementationType
            , ComponentLifestyle.Singleton
            , int.MaxValue
            , null, null )
        {
        }

        public Descriptor( string id
            , IEnumerable<Type> services
            , Type implementationType
            , ComponentLifestyle lifestyle
            , int level
            , IEnumerable<IParameter> parameters
            , IProperties extendedProperties)
        {
            _id = Enforce.ArgumentNotNullOrEmpty(id, "id");
            _implementationType = Enforce.ArgumentNotNull(implementationType, "implementationType");

            _services = services;
            _lifestyle = lifestyle;
            _level = level;
            _parameters = parameters;
            _extendedProperties = extendedProperties;
        }

        public string Id
        {
            get { return _id; }
        }

        public IEnumerable<Type > Services
        {
            get { return _services; }
        }

        public Type ImplementationType
        {
            get { return _implementationType; }
        }

        public ComponentLifestyle Lifestyle
        {
            get { return _lifestyle; }
        }

        public int ProposedLevel
        {
            get { return _level; }
        }

        public IEnumerable<IParameter> Parameters
        {
            get { return _parameters; }
        }

        public IProperties ExtendedProperties
        {
            get { return _extendedProperties; }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("Id=");
            sb.Append(_id);
            
            sb.Append(";Services=");
            Utils.Join<Type>(sb, ",", _services);

            sb.Append(";ImplementationType=");
            sb.Append(_implementationType.ToString());

            sb.Append(";ComponentLifestyle=");
            sb.Append(_lifestyle);
            
            return sb.ToString();
        }
    }
}
