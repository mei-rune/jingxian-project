using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.simpl.registrars
{
    using jingxian.core.runtime.registrars;

    public class MiniRegistration : Descriptor
    {
        ComponentLifestyle _lifestyle;
        IEnumerable<IParameter> _parameters;

        public MiniRegistration(string id
            , IEnumerable<Type> services
            , Type implementationType
            , ComponentLifestyle lifestyle
            , IEnumerable<IParameter> parameters
            , IProperties extendedProperties)
            : base( id, services, implementationType, extendedProperties )
        {
            _lifestyle = lifestyle;
            _parameters = parameters;
        }

        public ComponentLifestyle Lifestyle
        {
            get { return _lifestyle; }
            set { _lifestyle = value; }
        }

        public IEnumerable<IParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
    }
}
