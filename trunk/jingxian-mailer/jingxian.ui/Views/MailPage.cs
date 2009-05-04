using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace jingxian.ui.views
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Filters;
    using Empinia.UI;
    using Empinia.UI.Workbench;
    using Empinia.UI.Navigator;

    using jingxian.data;
    using jingxian.domainModel;

    public class MailPage : TreePage
    {
        private readonly IDbSessionFactory m_sessionFactory;

        public MailPage(IBundleService bundleService
            , IIconResourceService iconResourceService
            , IPageService pageService
            , IVirtualFileSystem virtualFileSystem
            , IDbSessionFactory sessionFactory)
            : base(bundleService, iconResourceService, pageService, virtualFileSystem)
        {
            m_sessionFactory = sessionFactory;
            Initialize();
        }

        protected override System.Collections.IList GetStore()
        {
            using (IDbSession session = m_sessionFactory.NewSession())
            {
                return session.QueryForList("GetAllFolders", null);
            }
        }

        protected override void UpdateParent( object content )
        {
            using (IDbSession session = m_sessionFactory.NewSession())
            using (TransactionScope transactionScope = new TransactionScope(session))
            {
                session.Update("UpdateFolderForParent", content);
                transactionScope.VoteCommit();
            }
        }

        protected override void UpdateName(object content)
        {
            using (IDbSession session = m_sessionFactory.NewSession())
            using (TransactionScope transactionScope = new TransactionScope(session))
            {
                session.Update("UpdateFolderForName", content);
                transactionScope.VoteCommit();
            }
        }
    }
}
