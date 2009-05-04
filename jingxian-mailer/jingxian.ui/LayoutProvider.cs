using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Globalization;

namespace jingxian.ui
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    using jingxian.ui.Serialization;

    [Extension(LayoutProvider.PageLayoutProviderPointId,
            jingxian.ui.Constants.BundleId,
            WorkbenchConstants.PageLayoutProviderPointId,
            typeof(LayoutProvider),
            Name = LayoutProvider.OriginalName,
            Description = "Provides a Jingxian GUI implementation based on DockPanel Suite by Weifen Luo.")] //NON-NLS-1
    public class LayoutProvider : IPageLayoutProvider
    {
        public const string PageLayoutProviderPointId = "jingxianIntegrationLayoutProvider";
        public const string OriginalName = "jingxian mailer suite gui"; //NON-NLS-1

        private readonly IExtensionRegistry m_ExtensionRegistry;
        private readonly IIconResourceService m_IconResourceService;
        private readonly IPartRegistry m_PartRegistry;
        private readonly IStatusService m_StatusService;
        private readonly IObjectBuilderService m_ObjectBuilder;
        private readonly IKeyBindingService m_KeyBindingService;
        private readonly Dictionary<IWorkbenchPart, ToolStrip> m_ActiveToolStrips;


        private IWorkbenchConfiguration m_WorkbenchConfiguration;
        private IPerspective m_CurrentPerspective;
        private IToolbarFactory m_ToolbarFactory;
        private LayoutPanel m_Page;
        private IWorkbenchPart m_SelectedPart;

        IViewPart m_viewPart1;
        ContentWidget m_contentWidget1;


        IViewPart m_viewPart2;
        ContentWidget m_contentWidget2;

        public event EventHandler<WorkbenchPartEventArgs> SelectedPartChanged;

        public LayoutProvider(
            IExtensionRegistry extensionRegistry,
            IIconResourceService iconResourceService,
            IPartRegistry partRegistry,
            IStatusService statusService,
            IObjectBuilderService objectBuilder,
            IKeyBindingService keyBindingService)
        {
            m_ExtensionRegistry = extensionRegistry;
            m_IconResourceService = iconResourceService;
            m_PartRegistry = partRegistry;
            m_StatusService = statusService;
            m_ObjectBuilder = objectBuilder;
            m_KeyBindingService = keyBindingService;

            m_ActiveToolStrips = new Dictionary<IWorkbenchPart, ToolStrip>();
        }


        #region Log
        private log4net.ILog m_Log;
        private log4net.ILog Log
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


        #region IPageLayoutProvider 成员

        public string Id
        {
            get { return PageLayoutProviderPointId; }
        }
        #region ApplyPerspective
        private static WidgetConfiguration GetPageConfiguration(IPerspective perspective)
        {
            WidgetConfiguration pageConfig = new WidgetConfiguration();
            if (perspective.Configuration.PageConfiguration != null)
                return perspective.Configuration.PageConfiguration as WidgetConfiguration;

            if (string.IsNullOrEmpty(perspective.Configuration.PageConfigurationXml))
                throw new InvalidOperationException(string.Format("The Perspective ({0}) wasn't created correctly. Its PageConfiguration and PageConfigurationXml are both null or empty", perspective.Id));

            using (StringReader input = new StringReader(perspective.Configuration.PageConfigurationXml))
            using (XmlReader reader = XmlReader.Create(input))
            {
                pageConfig.ReadXml(reader);
            }
            return pageConfig;
        }

        private void DetachToolbars()
        {
            foreach (ToolStripPanel toolbarContainer in Page.ToolbarContainers)
            {
                Control[] tmpList = new Control[toolbarContainer.Controls.Count];
                toolbarContainer.Controls.CopyTo(tmpList, 0);
                foreach (Control control in tmpList)
                {
                    control.Visible = false;
                    toolbarContainer.Controls.Remove(control);
                    control.Dispose();
                }
            }
        }

        private ToolStripPanel GetToolStripPanel(string areaId)
        {
            switch (areaId)
            {
                case "top":
                    return Page.TopToolStripPanel;
                case "right":
                    return Page.RightToolStripPanel;
                case "bottom":
                    return Page.BottomToolStripPanel;
                case "left":
                    return Page.LeftToolStripPanel;
                default:
                    //TODO: insert handlung of unknown/floating TSPs here
                    return Page.TopToolStripPanel;
            }
        }

        private void CreateToolbars(WidgetConfiguration pageConfiguration)
        {
            foreach (ToolbarAreaConfiguration areaConfiguration in pageConfiguration.ToolbarAreas.Values)
            {
                ToolStripPanel area = GetToolStripPanel(areaConfiguration.Name);
                for (int row = 0; row < areaConfiguration.RowConfigurations.Length; row++)
                {
                    for (int position = areaConfiguration.RowConfigurations[row].ToolbarConfigurations.Length - 1; position >= 0; position--)
                    {
                        ToolStrip strip =
                            GetToolbarWidget(areaConfiguration
                                                    .RowConfigurations[row]
                                                     .ToolbarConfigurations[position]
                                                        .ToolbarTypeId);

                        //@todo Not implemented in Mono (find alternative?). They are just added.
                        area.Join(strip, row);
                        if (area.Orientation == Orientation.Horizontal)
                        {
                            strip.Left = areaConfiguration
                                                .RowConfigurations[row]
                                                .ToolbarConfigurations[position]
                                                 .Offset;
                        }
                        else
                        {
                            strip.Top = areaConfiguration
                                                .RowConfigurations[row]
                                                .ToolbarConfigurations[position]
                                                 .Offset;
                        }
                        area.PerformLayout();
                    }
                }
            }
        }

        private void ConfigurePage(WidgetConfiguration pageConfig)
        {

        }

        private void CreatePanes(WidgetConfiguration pageConfig)
        {
        }

        public void ApplyPerspective(IPerspective perspective)
        {
            m_StatusService.HandleStatus(Status.Debug(string.Format("Loading perspective: {0}", perspective.Name))); /// @todo: remove
            m_CurrentPerspective = perspective; /// @todo: check if this should be placed at the end of the loading process

            WidgetConfiguration pageConfig = GetPageConfiguration(perspective);
            DetachToolbars();
            CreateToolbars(pageConfig);
            ConfigurePage(pageConfig);
            CreatePanes(pageConfig);
            m_Page.PerformLayout();
        }
        #endregion

        public ICollection<Area> Areas
        {
            get
            {
                if (m_CurrentPerspective != null)
                {
                    return m_CurrentPerspective.Areas;
                }
                else
                {
                    return new List<Area>();
                }
            }
        }

        public ICollection<IViewPart> AvailableViews
        {
            get { return new IViewPart[] { m_viewPart1, m_viewPart2 }; }
        }

        public void Close(IWorkbenchPart part)
        {
            throw new NotImplementedException();
        }

        public void Focus(IWorkbenchPart part)
        {
            if (part == null)
                throw new ArgumentNullException();

            IViewPart viewPart = part as IViewPart;

            if (null != viewPart)
                return;

            //ContentWidget content;
            //@todo Refactor: this is ugly...
            //Trace.Assert(viewPart.Widget.Parent.Parent.Parent is ContentWidget);
            //content = viewPart.Widget.Parent.Parent.Parent as ContentWidget;

            SelectedPart = viewPart;
        }

        public void Hide(IWorkbenchPart part)
        {
        }

        LayoutPanel CreatePage()
        {
            m_Page = new LayoutPanel();
            return m_Page;
        }

        public LayoutPanel Page
        {
            get { return m_Page??CreatePage(); }
        }

        IPage IPageLayoutProvider.Page
        {
            get { return Page; }
        }

        public PageConfiguration PageConfiguration
        {
            get { return PageConfig; }
		}

        private WidgetConfiguration PageConfig
		{
            get { return new WidgetConfiguration(Page); }
		}

        public IWorkbenchPart SelectedPart
        {
            get { return m_viewPart2; }
            private set
            {
                if (m_SelectedPart == value)
                    return;

                m_SelectedPart = value;
                if (value == null)
                    return;

                RaiseSelectedPartChanged(new WorkbenchPartEventArgs(value));
                m_SelectedPart.Widget.Focus();
            }
        }

        private void RaiseSelectedPartChanged(WorkbenchPartEventArgs e)
        {
            if (null != SelectedPartChanged)
                SelectedPartChanged(this, e);
        }

        private bool TryGetExistingToolStrip(IToolbarPart toolbarPart, out ToolStrip toolStrip)
        {
            foreach (Control control in Page.TopToolStripPanel.Controls)
            {
                if ((control is ToolStripEx) && ((control as ToolStripEx).ToolbarPart.Equals(toolbarPart)))
                {
                    toolStrip = control as ToolStrip;
                    return true;
                }
            }
            toolStrip = null;
            return false;
        }


        private void AttachViewsToolbars(ContentWidget content)
        {
            if (content.ViewPart.Toolbars.Count > 0)
            {
                foreach (IToolbarPart toolbarPart in content.ViewPart.Toolbars)
                {
                    if (toolbarPart.Widget == null)
                    {
                        ToolStrip strip = new ToolStrip();// = CreateStrip(toolbarPart, false);// <--------------------------------------------
                        if (strip.Items.Count == 0)
                        {
                            continue;
                        }
                        strip.GripStyle = ToolStripGripStyle.Hidden;
                        strip.AllowMerge = false;
                        content.ToolStripContainer.TopToolStripPanel.Controls.Add(strip);
                    }
                }
            }

        }

        private ContentWidget CreateViewPart(IViewPart view)
        {
            ContentWidget content = new ContentWidget(view, m_IconResourceService, m_KeyBindingService, false);
            AttachViewsToolbars(content);
            return content;
        }

        private bool ExistsContentConfiguration(ContentWidget content, out ContentConfiguration contentConfig)
        {
            //foreach (ContentConfiguration contentConfiguration in PageConfig.Contents)
            //{
            //    if (contentConfiguration.Id != content.ViewPart.TypeId)
            //    {
            //        continue;
            //    }
            //    contentConfig = contentConfiguration;
            //    return true;
            //}
            contentConfig = null;
            return false;
        }

        private bool GetPreferred(IWorkbenchPartLayout layout)
        {
            foreach (string areaId in layout.PreferredAreas)
            {
                if (0 == string.Compare("south", areaId, true))
                
                    return true;
                
            }
            return false;
        }

        private void CreateAndShowView(IViewPart view)
        {
            ContentWidget content = CreateViewPart(view);

            if (GetPreferred(view.Layout))
            {
                Page.Show(content, true);
                m_viewPart1 = view;
                m_contentWidget1 = content;
            }
            else
            {
                Page.Show(content, false );
                m_viewPart2 = view;
                m_contentWidget2 = content;
            }
        }

        public void Show(IWorkbenchPart part)
        {
            if (part is IViewPart)
            {
                IViewPart view = part as IViewPart;
                //ContentConfiguration content;

                if ( part != m_viewPart1 
                    || part != m_viewPart2 )
                {
                    CreateAndShowView(view);
                }
                else
                {
                    Focus(view);
                }
            }
            else if (part is IToolbarPart)
            {
                IToolbarPart toolbar = part as IToolbarPart;
                ToolStrip toolStrip;

                if (!TryGetExistingToolStrip(toolbar, out toolStrip))
                {
                    toolStrip = GetToolbarWidget(toolbar);
                    Page.TopToolStripPanel.Controls.Add(toolStrip);
                }
                else
                {
                    toolStrip.Show();
                }
            }
        }

        public IToolbarFactory ToolbarFactory
        {
            get
            {
                if (m_ToolbarFactory == null)
                {
                    #region find & build factory
                    IExtension toolbarFactoryExtension;
                    if (m_ExtensionRegistry.TryGetExtension(jingxian.ui.ToolbarFactory.ToolbarFactoryId, out toolbarFactoryExtension))
                    {
                        m_ToolbarFactory = m_ObjectBuilder.BuildTransient<IToolbarFactory>(toolbarFactoryExtension.Implementation);
                        #region debug msg
                        if (Log.IsDebugEnabled)
                        {
                            Log.DebugFormat(CultureInfo.InvariantCulture, " * ToolbarFactory '{0}' is used.", WorkbenchConfiguration.ToolbarProviderId); //NON-NLS-1
                        }
                        #endregion
                    }
                    else
                    {
                        throw new PlatformConfigurationException(
                                string.Format("The given ToolbarFactory ('{0}') is not available in the ExtensionRegistry.",
                                                    WorkbenchConfiguration.ToolbarProviderId));
                    }
                    #endregion
                }

                return m_ToolbarFactory;
            }
            set
            {
                #region checks
                if (m_ToolbarFactory != null)
                {
                    throw new InvalidOperationException(
                        "Toolbar factory can only be set once. Changing the factory after the initial setup is not supported.");
                }
                if (value == null)
                {
                    throw new ArgumentNullException("Toolbar factory can't be null. Check the calling code.");
                }
                #endregion
                m_ToolbarFactory = value;
            }
        }

        public ICollection<IWorkbenchPart> VisibleParts
        {
            get { throw new NotImplementedException(); }
        }

        [OptionalDependency]
        public IWorkbenchConfiguration WorkbenchConfiguration
        {
            get
            {
                return m_WorkbenchConfiguration;
            }
            set
            {
                m_WorkbenchConfiguration = value;
            }
        }

        #endregion

        private ToolStrip GetToolbarWidget(string toolbarPartId)
        {
            IWorkbenchPart part;
            if (!m_PartRegistry.TryGet(toolbarPartId, out part))
            {
                throw new InvalidOperationException(string.Format("'{0}' is no valid toolbar part type id.", toolbarPartId));
            }
            return GetToolbarWidget((IToolbarPart)part);
        }


        private ToolStrip GetToolbarWidget(IToolbarPart toolbarPart)
        {
            ToolbarFactory.InitializeWidget(toolbarPart);
            return toolbarPart.Widget as ToolStrip;
        }
    }
}
