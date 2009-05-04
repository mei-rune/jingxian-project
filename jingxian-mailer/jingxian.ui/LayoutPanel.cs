using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui
{
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Xml.Serialization;
    using Empinia.UI;
    using Empinia.UI.Workbench;


    using jingxian.ui.Serialization;

    public partial class LayoutPanel : ToolStripContainer, IPage
	{
        private ICollection<ToolStripPanel> m_ToolbarContainers;

        public LayoutPanel()
		{
			m_ToolbarContainers = new List<ToolStripPanel>();

			Initialize();
		}

		private void Initialize()
        {
            InitializeComponent();

            this.ContentPanel.Controls.Add(this.m_splitter);

			m_Empty = false; //TODO: still needed?

			//here the 4 toolbar areas (ToolStripPanels of this ToolStripContainer)
			//are named. The names are hardcoded ids, don't change them, otherwise
			//loaded toolbars won't show up atm.
			TopToolStripPanel.Name = "top"; //NON-NLS-1
			RightToolStripPanel.Name = "right"; //NON-NLS-1
			BottomToolStripPanel.Name = "bottom"; //NON-NLS-1
			LeftToolStripPanel.Name = "left"; //NON-NLS-1

			m_ToolbarContainers.Add(TopToolStripPanel);
			m_ToolbarContainers.Add(RightToolStripPanel);
			m_ToolbarContainers.Add(BottomToolStripPanel);
			m_ToolbarContainers.Add(LeftToolStripPanel);

			Dock = DockStyle.Fill;

			SetStyle(
				ControlStyles.DoubleBuffer
				| ControlStyles.UserPaint
				| ControlStyles.AllPaintingInWmPaint,
				true);

			SetStyle(
				ControlStyles.ResizeRedraw,
				false);

			UpdateStyles();

			//Settings.Default.PropertyChanged += OnSettingsPropertyChanged;

			Show();
            ContentPanel.Show();
			TopToolStripPanel.Show();
			RightToolStripPanel.Show();
		}


		private void OnParentFormResizeBegin(object sender, EventArgs e)
		{
			if (Settings.Default.ShowPageContentOnResize)
			{
				return;
			}
			SuspendLayout();
			Visible = false;
		}

		private void OnParentFormResizeEnd(object sender, EventArgs e)
		{
			if (Settings.Default.ShowPageContentOnResize)
			{
				return;
			}
			ResumeLayout();
			Visible = true;
		}

        //private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case "EnableDocking":
        //            m_DockSpace.AllowEndUserDocking = Settings.Default.EnableDocking;
        //            break;
        //        case "EnableNestedDocking":
        //            m_DockSpace.AllowEndUserNestedDocking = Settings.Default.EnableNestedDocking;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        [HierarchyDecorator("jingxian.ui.hierarchy.splitter")]
		public SplitContainer Splitter
		{
			get
			{
                return m_splitter;
			}
		}

        [HierarchyDecorator("jingxian.ui.hierarchy.toolbar")]
		public ICollection<ToolStripPanel> ToolbarContainers
		{
			get
			{
				return m_ToolbarContainers;
			}
		}

        public void Show( Control widget , bool isSouth )
        {
            Panel panel = isSouth ? Splitter.Panel1 : Splitter.Panel2;


            Control[] controls = new Control[panel.Controls.Count];
            panel.Controls.CopyTo(controls, 0);
            panel.Controls.Clear();

            foreach (Control control in controls)
            {
                control.Dispose();
            }

            panel.SuspendLayout();
            panel.Controls.Add(widget);

            widget.Dock = System.Windows.Forms.DockStyle.Fill;

            panel.ResumeLayout(true );
            panel.PerformLayout();
        }

		#region IPage Members

		private bool m_Empty = true;

		bool IPage.Empty
		{
			get
			{
				return m_Empty;
			}
		}

		/// <exception cref="NotSupportedException">Only workbench windows are supported which are derived from Form.</exception>
		public void Configure(IWorkbenchWindow cfg)
		{
			Form parent = cfg as Form;
			if (parent == null)
			{
				throw new NotSupportedException(
					"Only workbench windows are supported which are derived from Form."); //NON-NLS-1
			}
			else
			{
				if (Parent != null && ParentForm != null)
				{
					// detach old event handlers
					ParentForm.ResizeBegin -= OnParentFormResizeBegin;
					ParentForm.ResizeEnd -= OnParentFormResizeEnd;
				}
				Parent = parent;
				parent.ResizeBegin += OnParentFormResizeBegin;
				parent.ResizeEnd += OnParentFormResizeEnd;
			}
		}

		#endregion

	}
}
