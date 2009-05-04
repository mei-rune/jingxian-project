using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    public partial class ContentWidget : UserControl
    {

        public event EventHandler<WorkbenchPartStateChangedEventArgs> StateChanged;

        private readonly IIconResourceService m_IconResourceService;
        private readonly IKeyBindingService m_KeyBindingService;
        private readonly IViewPart m_ViewPart;

        ToolStripContainer m_ToolStripContainer;

        public ContentWidget(IViewPart viewPart, IIconResourceService iconResourceService, IKeyBindingService keyBindingService, bool lazy)
        {
            InitializeComponent();

            m_ViewPart = viewPart;
            m_IconResourceService = iconResourceService;
            m_KeyBindingService = keyBindingService;
            Text = viewPart.Name;
            Name = viewPart.Id;

            //if (!string.IsNullOrEmpty(viewPart.IconId))
            //{
            //    Icon = m_IconResourceService.GetIcon(viewPart.IconId);
            //    ShowIcon = true;
            //}

            if (!lazy)
            {
                CreateAndApplyViewControl();
            }
            KeyDown += HandleKeyInput;
            //Closing += ViewPart_Closing;
        }

        void ViewPart_Closing(object sender, CancelEventArgs e)
        {
            KeyDown -= HandleKeyInput;
            //Closing -= ViewPart_Closing;
        }

        private void HandleKeyInput(object sender, KeyEventArgs e)
        {
            m_KeyBindingService.HandleKeyEvent(e);
        }

        public void CreateAndApplyViewControl()
        {
            m_ToolStripContainer = new ToolStripContainer();
            m_ToolStripContainer.Dock = DockStyle.Fill;

            base.Controls.Add(m_ToolStripContainer);

            Control widget = m_ViewPart.Widget;

            if (widget == null)
            {
                throw new InvalidOperationException(string.Format("The ContentWidget's '{0}' Control is null.", m_ViewPart.Id));  //NON-NLS-1)
            }

            if (!m_ToolStripContainer.ContentPanel.Controls.Contains(widget))
            {
                widget.Dock = DockStyle.Fill;

                SuspendLayout();
                m_ToolStripContainer.ContentPanel.Controls.Add(widget);
                ResumeLayout(true);

                if (m_ViewPart is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)m_ViewPart).PropertyChanged += OnViewPartPropertyChanged;
                }
            }
        }

        private void OnViewPartPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                Text = m_ViewPart.Name;
            }
        }

        public IViewPart ViewPart
        {
            get { return m_ViewPart; }
        }

        public ToolStripContainer ToolStripContainer
        {
            get { return m_ToolStripContainer; }
        }
    }
}
