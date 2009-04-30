

using System;
using System.Collections.Generic;
using System.Globalization;


namespace jingxian.core.runtime
{
    public class Descriptor : IComponentDescriptor
    {
        string _id;
        IEnumerable<Type> _services;
        IProperties _extendedProperties;
        Type _implementationType;

        public Descriptor( string id,
            IEnumerable<Type> services,
            Type bestKnownImplementationType)
        : this(id, services, bestKnownImplementationType,  null )
        {
        }

        public Descriptor( string id,
            IEnumerable<Type> services, 
            Type implementationType,
            IProperties extendedProperties)
        {
            _id = Enforce.ArgumentNotNullOrEmpty(id, "id");
            _services = Enforce.ArgumentNotNull(services, "services");


            if (null == extendedProperties)
                _extendedProperties = new MapProperties();
            else
                _extendedProperties = extendedProperties;

            _implementationType = Enforce.ArgumentNotNull(implementationType, "implementationType");
        }

        public IEnumerable<Type > Services
        {
            get { return _services; }
        }

        public string Id
        {
            get { return _id; }
        }

        public  IProperties ExtendedProperties
        {
            get { return _extendedProperties; }
        }

        public Type ImplementationType
        {
            get { return _implementationType; }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("Id=");
            sb.Append(_id);
            sb.Append(";Services=");
            Utils.Join<Type>(sb, ",", _services);

            sb.Append(";ImplementationType=");
            sb.Append(_implementationType.ToString());
            return sb.ToString();
        }
    }
}
