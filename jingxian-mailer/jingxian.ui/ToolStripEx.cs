using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    public class ToolStripEx: ToolStrip
    {
        private IToolbarPart m_ToolbarPart;

        public ToolStripEx(IToolbarPart toolbarPart)
		{
			m_ToolbarPart = toolbarPart;
		}

		public IToolbarPart ToolbarPart
		{
            get { return m_ToolbarPart; }
            set { Configure( value); }
		}

		public void Configure(IToolbarPart toolbarPart)
		{
			if (toolbarPart == null)
			{
				throw new ArgumentNullException();
			}
			if (m_ToolbarPart != null)
			{
				throw new InvalidOperationException("Write-Once violation: ToolStripEx can't be reconfigured. Please make sure you only call Configure(...) once.");
			}
			m_ToolbarPart = toolbarPart;
		}
	}
}
