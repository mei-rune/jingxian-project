
using System;
using System.Text;
using System.Globalization;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.utilities;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ServiceAttribute : Attribute
    {
        private string _name;
        private readonly Type _serviceInterface;

        public ServiceAttribute(string nm, Type serviceInterface)
        {
            _name = nm;
            _serviceInterface = Enforce.ArgumentNotNull<Type>(serviceInterface, "serviceInterface");
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Type ServiceInterface
        {
            get { return _serviceInterface; }
        }
    }
}