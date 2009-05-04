using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.Serialization
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Utilities;
    using Empinia.UI;
    using Empinia.UI.Workbench;

	internal class WidgetConfiguration: PageConfiguration
	{
        private readonly IStatusService m_StatusService; 
        private Dictionary<string, ToolbarAreaConfiguration> m_ToolbarAreas;

		public WidgetConfiguration()
		{
			m_StatusService = ServiceRegistrySingleton.Instance.GetService<IStatusService>();
		}

        public WidgetConfiguration(LayoutPanel page)
		{
			m_StatusService = ServiceRegistrySingleton.Instance.GetService<IStatusService>();

            //Contents.Add(new ContentConfiguration( page.Splitter.Panel1 as ContentWidget ));
            //Contents.Add(new ContentConfiguration( page.Splitter.Panel2 as ContentWidget));


			foreach (ToolStripPanel panel in page.ToolbarContainers)
			{
				ToolbarAreas.Add(panel.Name, new ToolbarAreaConfiguration(panel));
			}
        }

        public IDictionary<string, ToolbarAreaConfiguration> ToolbarAreas
        {
            get
            {
                if (m_ToolbarAreas == null)
                {
                    m_ToolbarAreas = new Dictionary<string, ToolbarAreaConfiguration>();
                }
                return m_ToolbarAreas;
            }
        }

        private List<ContentConfiguration> m_Contents;

        public ICollection<ContentConfiguration> Contents
        {
            get
            {
                if (m_Contents == null)
                {
                    m_Contents = new List<ContentConfiguration>();
                }
                return m_Contents;
            }
        }

		#region Log

		private log4net.ILog m_Log;

		protected log4net.ILog Log
		{
			get
			{
				if (m_Log == null)
				{
					m_Log = log4net.LogManager.GetLogger(GetType());
				}
				return m_Log;
			}
		}

		#endregion
	}
}
