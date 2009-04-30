

using System;
using System.Diagnostics;
using jingxian.core.utilities;
using System.Collections;
using System.Globalization;

namespace jingxian.core.runtime.simpl
{
	[Service(
		typeof(IObjectBuilder), typeof(ObjectBuilder),
		RuntimeConstants.ObjectBuilderServiceId,
		Constants.Bundles.Internal,
		Name = ObjectBuilder.OriginalName)]
	internal sealed class ObjectBuilder: Service, IObjectBuilder
	{
		public const string OriginalName = "Object Builder Service";

		private readonly IKernel _container;

        public ObjectBuilder(IKernel container)
		{
			_container = container;
		}

        private IKernel Container
		{
            get { return _container; }
		}

		private string GetUniqueId(Type type)
		{
			string componentTypeId = type.Name.ToUpperInvariant();
			if (_container.Contains(componentTypeId))
			{
				string componentId;
				int i = 0;
				do
				{
					i++;
					componentId = Utils.CreateCompositeId(componentTypeId, i);
				} while (_container.Contains(componentId));
				return componentId;
			}
			else
			{
				return componentTypeId;
			}
		}

		#region IObjectBuilder Members
		public bool TryGetType(string typeName, out Type type)
		{
			if (string.IsNullOrEmpty(typeName))
				throw new StringArgumentException("typeName"); 

		
			type = Type.GetType(typeName, false, false);
			return type != null;
		}

		public Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                throw new StringArgumentException("typeName"); 
		
			Type type = Type.GetType(typeName, true, false);
			return type;
		}

		public T BuildTransient<T>(string id, Type classType, Type contractType)
		{
			if (string.IsNullOrEmpty(id))
				throw new StringArgumentException("id");
			if (classType == null)
				throw new ArgumentNullException("classType");
			if (contractType == null)
				throw new ArgumentNullException("contractType"); 


			Container.Connect(id, classType, contractType, ComponentLifestyle.DependencyInjectionOnly);
			T instance = (T) Container[id];
			Container.Disconnect(id);

			return instance;
		}

		public T BuildTransient<T>(string id, Type classType, Type contractType, IDictionary arguments)
		{
			if (string.IsNullOrEmpty(id))
				throw new StringArgumentException("id");
			if (classType == null)
				throw new ArgumentNullException("classType");
			if (contractType == null)
				throw new ArgumentNullException("contractType");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			Container.Connect(id, classType, contractType, ComponentLifestyle.DependencyInjectionOnly);
			T instance = (T) Container.Get(id, arguments);
			Container.Disconnect(id);

			return instance;
		}

		public T BuildTransient<T>(Type classType)
		{
			if (classType == null)
				throw new ArgumentNullException("classType");


			string id = GetUniqueId(classType);
            Container.Connect(id, classType, typeof(T), ComponentLifestyle.DependencyInjectionOnly);
			T instance = (T) Container[id];
			Container.Disconnect(id);


			return instance;
		}

		public T BuildTransient<T>(Type classType, IDictionary arguments)
		{
			if (classType == null)
				throw new ArgumentNullException("classType");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			string id = GetUniqueId(classType);
            Container.Connect(id, classType, typeof(T), ComponentLifestyle.DependencyInjectionOnly);
            T instance = (T)Container.Get(id, arguments);
			Container.Disconnect(id);


			return instance;
		}

		public T BuildTransient<T>(string classType)
		{
			Type type;
			if (string.IsNullOrEmpty(classType))
				throw new StringArgumentException("classType");
			if (!TryGetType(classType, out type))
                throw new ArgumentException(string.Format(Resources.Messages.ArgumentIsNotValidClassType, classType)); 


			return BuildTransient<T>(type);
		}

        public T BuildTransient<T>(string classType, IDictionary arguments)
        {
            Type type;
            if (string.IsNullOrEmpty(classType))
                throw new StringArgumentException("classType");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            if (!TryGetType(classType, out type))
                throw new ArgumentException(string.Format(Resources.Messages.ArgumentIsNotValidClassType, classType));  

            return BuildTransient<T>(type, arguments);
        }

		public T Build<T>() where T: class
		{
			return Build<T>(GetUniqueId(typeof(T)));
		}

		public T Build<T>(IDictionary arguments) where T: class
        {
            if (arguments == null)
                throw new ArgumentNullException("arguments");

			return Build<T>(GetUniqueId(typeof(T)), arguments);
		}

		public T Build<T>(string id) where T: class
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");

            Container.Connect(id, typeof(T));
            return Container.Get<T>(id);
		}

		public T Build<T>(string id, IDictionary arguments) where T: class
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            Container.Connect(id, typeof(T));
			return Container.Get<T>(id, arguments);
		}

		public T Build<T>(Type classType)
        {
            if (null == classType)
                throw new ArgumentNullException("classType");
		
			return Build<T>(GetUniqueId(classType), classType);
		}

		public T Build<T>(Type classType, IDictionary arguments)
        {
            if (null == classType)
                throw new ArgumentNullException("classType");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
		
			Container.Connect(GetUniqueId(classType), classType);
			return Container.Get<T>(GetUniqueId(classType), arguments);
		}

		public T Build<T>(string id, Type classType)
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if (null == classType)
                throw new ArgumentNullException("classType");

            Container.Connect(id, classType, typeof(T));
			return (T) Container[id];
		}

		public T Build<T>(string id, Type classType, IDictionary arguments)
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if (null == classType)
                throw new ArgumentNullException("classType");
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            Container.Connect(id, classType, typeof(T));
            return Container.Get<T>(id, arguments);
		}

		public T Build<T>(string id, Type classType, Type contractType)
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if (null == classType)
                throw new ArgumentNullException("classType");
            if (null == contractType)
                throw new ArgumentNullException("contractType");

            Container.Connect(id, classType, contractType);
			return (T) Container[id];
		}

		public T Build<T>(string id, Type classType, Type contractType, IDictionary arguments)
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if ( null == classType)
                throw new ArgumentNullException("classType");
            if (null == contractType)
                throw new ArgumentNullException("contractType");
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            Container.Connect(id, classType, contractType);
			return Container.Get<T>(id, arguments);
		}

		public T Build<T>(string id, string classType)
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if (string.IsNullOrEmpty(classType))
                throw new StringArgumentException("classType");

			return Build<T>(id, GetType(classType));
		}

		public T Build<T>(string id, string classType, IDictionary arguments)
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if (string.IsNullOrEmpty(classType))
                throw new StringArgumentException("classType");
            if (arguments == null)
                throw new ArgumentNullException("arguments");

			return Build<T>(id, GetType(classType), arguments);
		}

		public T Build<T>(string id, string classType, string contractType)
        {
            if (string.IsNullOrEmpty(id))
                throw new StringArgumentException("id");
            if (string.IsNullOrEmpty(classType))
                throw new StringArgumentException("classType");
            if (string.IsNullOrEmpty(contractType))
                throw new StringArgumentException("contractType");

			return Build<T>(id, GetType(classType), GetType(contractType));
		}

		public T Build<T>(string id, string classType, string contractType, IDictionary arguments)
		{
			if (string.IsNullOrEmpty(id))
				throw new StringArgumentException("id");
			if (string.IsNullOrEmpty(classType))
				throw new StringArgumentException("classType");
			if (string.IsNullOrEmpty(contractType))
				throw new StringArgumentException("contractType");
			if (arguments == null)
				throw new ArgumentNullException("arguments");


			return Build<T>(id, GetType(classType), GetType(contractType), arguments);
		}

		#endregion

	}
}