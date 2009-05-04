using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.views
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Filters;
    using Empinia.UI;
    using Empinia.UI.Workbench;
    using Empinia.UI.Navigator;

    using jingxian.data;
    using jingxian.domainModel;

    public class ContactPage : TreePage
    {
        private readonly IDbSessionFactory m_sessionFactory;

        public ContactPage(IBundleService bundleService
            , IIconResourceService iconResourceService
            , IPageService pageService
            , IVirtualFileSystem virtualFileSystem
            , IDbSessionFactory sessionFactory)
            : base(bundleService, iconResourceService, pageService, virtualFileSystem)
        {
            m_sessionFactory = sessionFactory;
        }

        protected override System.Collections.IList GetStore()
        {
            using (IDbSession session = m_sessionFactory.NewSession())
            {
                return session.QueryForList("GetAllContacts", null);
            }
        }

        protected override void UpdateParent(object content)
        {
            using (IDbSession session = m_sessionFactory.NewSession())
            using (TransactionScope transactionScope = new TransactionScope(session))
            {
                session.Update("UpdateContactForParent", content);
                transactionScope.VoteCommit();
            }
        }

        protected override void UpdateName(object content)
        {
            using (IDbSession session = m_sessionFactory.NewSession())
            using (TransactionScope transactionScope = new TransactionScope(session))
            {
                session.Update("UpdateContactForName", content);
                transactionScope.VoteCommit();
            }
        }
    }
}
