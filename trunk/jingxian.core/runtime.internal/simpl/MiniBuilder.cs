

using System;
using System.Diagnostics;
using jingxian.core.runtime.utilities;
using System.Collections;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
    [Service(
        typeof(IObjectBuilder), typeof(MiniBuilder),
        RuntimeConstants.ObjectBuilderServiceId,
        Constants.Bundles.Internal,
        Name = MiniBuilder.OriginalName)]
    internal sealed class MiniBuilder : Service, IObjectBuilder
    {
        public const string OriginalName = "Object Builder Service";

        private readonly MiniKernel _container;

        public MiniBuilder( IKernel container )
        {
            _container = Enforce.ArgumentNotNull( container as MiniKernel, "container" );
        }

        #region IObjectBuilder 成员

        public bool TryGetType(string typeName, out Type type)
        {
            Enforce.ArgumentNotNullOrEmpty(typeName, "typeName");


            type = Type.GetType(typeName, false, false);
            return type != null;
        }

        public Type GetType(string typeName)
        {
            Enforce.ArgumentNotNullOrEmpty(typeName, "typeName");

            Type type = Type.GetType(typeName, true, false);
            return type;
        }

        public T BuildTransient<T>()
        {
            return (T)BuildTransient(typeof(T));
        }

        public object BuildTransient(Type classType)
        {
            Enforce.ArgumentNotNull(classType, "classType");

            return _container.Build(classType, null, null);
        }


        public object BuildTransient(string classType)
        {
            Enforce.ArgumentNotNullOrEmpty(classType, "classType");

            Type type = null;
            if (TryGetType(classType, out type))
                return BuildTransient(type);
            return null;
        }

        #endregion
    }
}