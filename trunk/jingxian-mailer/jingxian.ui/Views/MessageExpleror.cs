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

    public class MessageExplorer :  IView
    {
        private readonly IBundleService m_BundleService;
        private readonly IIconResourceService m_IconResourceService;
        private readonly IDbSessionFactory m_sessionFactory;
        private readonly IPartRegistry m_PartRegistry;
        private readonly IToolbarFactory m_ToolStripFactory;
        private readonly IPageService m_PageService;
        private readonly IVirtualFileSystem m_VirtualFileSystem;


        IViewPart m_viewPart;
        Control _control;

        public MessageExplorer(IBundleService bundleService
            , IIconResourceService iconResourceService
            , IDbSessionFactory sessionFactory
            , IPartRegistry partRegistry
            , IPageService pageService
            , IVirtualFileSystem virtualFileSystem)
        {
            m_BundleService = bundleService;
            m_IconResourceService = iconResourceService;
            m_sessionFactory = sessionFactory;
            m_PartRegistry = partRegistry;
            m_PageService = pageService;
            m_ToolStripFactory = pageService.PageLayoutProvider.ToolbarFactory;
            m_VirtualFileSystem = virtualFileSystem;
        }        
        
        #region IView 成员

        public void Configure(IViewPart owningPart)
        {
            m_viewPart = owningPart;
        }

        public IViewPart OwningViewPart
        {
            get { return m_viewPart; }
        }

        public Control Widget
        {
            get 
            {
                if (null == _control)
                    _control = new MessageExplorerWidget(m_BundleService, m_IconResourceService, m_sessionFactory, m_PartRegistry, m_PageService, m_VirtualFileSystem, m_viewPart);
                return _control; 
            }
        }

        #endregion
    }
}
