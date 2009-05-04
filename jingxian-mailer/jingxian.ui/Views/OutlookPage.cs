using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.views
{
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Xml.Serialization;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    public class OutlookPage : IView
	{
		private readonly IStatusService m_StatusService;
        private readonly IPageService _pageSvc;
        private readonly IIconResourceService _iconService;
        private readonly IPartRegistry _partRegsistry;

		private IViewPart m_OwningViewPart;
		private Control m_View;


        public OutlookPage(IStatusService statusService, IPageService pageSvc, IIconResourceService iconService, IPartRegistry partRegsistry)
		{
			m_StatusService = statusService;
            _pageSvc = pageSvc;
            _iconService = iconService;
            _partRegsistry = partRegsistry;
		}

		#region IView Members

		IViewPart IView.OwningViewPart
		{
			get { return m_OwningViewPart; }
		}

		void IView.Configure(IViewPart owningPart)
		{
			if (m_OwningViewPart == null)
			{
				m_OwningViewPart = owningPart;
			}
			else
			{
				throw new PlatformConfigurationException("Write Once Configuration Violation");
			}
		}

		Control IView.Widget
		{
			get
			{
				if (m_View == null)
				{
                    m_View = new OutlookBar(_pageSvc,_iconService, _partRegsistry, m_OwningViewPart );
				}

				return m_View;
			}
		}

		#endregion
	}
}
