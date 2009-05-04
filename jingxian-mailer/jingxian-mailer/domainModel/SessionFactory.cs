using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace jingxian.domainModel
{
    using IBatisNet.DataMapper;

    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.CodeAnalysis;
    using Empinia.Core.Runtime.Xml.Expressions;
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime.Commands;

    [LabelDecorator("SessionFactory.labeler", IconId = "defaulticons.service.png")]
	/// <summary>
	/// Implementation of an <see cref="Empinia.UI.IActionService"/>.
	/// </summary>
	[Service(
        typeof(IDbSessionFactory),
        typeof(SessionFactory),
        SessionFactory.Id,
        jingxian.Constants.BundleId,
        Name = SessionFactory.OriginalName)]
    public class SessionFactory : Service, IDbSessionFactory
    {
        public const string Id = "jingxian.domainModel.SessionFactory";
        public const string OriginalName = "jingxian.domainModel.SessionFactory";

        static ISqlMapper _mapper;

        public static ISqlMapper Mapper
        {
            get { return _mapper; }
            set { _mapper = value; }
        }

        public IDbSession NewSession()
        {
            return new DbSession(_mapper);
        }
    }
}
