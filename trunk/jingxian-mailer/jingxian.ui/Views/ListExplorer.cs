using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace jingxian.ui.views
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Filters;
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime.Xml.Serialization;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    using jingxian.ui.controls.XPTable.Models;


    public partial class ListExplorer : UserControl
    {

        protected readonly IBundleService m_BundleService;
        protected readonly IIconResourceService m_IconResourceService;
        protected readonly IPartRegistry m_PartRegistry;
        protected readonly IToolbarFactory m_ToolStripFactory;
        protected readonly IPageService m_PageService;
        protected readonly IVirtualFileSystem m_VirtualFileSystem;
        protected readonly IViewPart m_viewPart;


        List<IFieldDescriptor> _fieldDescriptors = new List<IFieldDescriptor>();


        public List<IFieldDescriptor> FieldDescriptors
        {
            get { return _fieldDescriptors; }
        }

        public ListExplorer(IBundleService bundleService
            , IIconResourceService iconResourceService
            , IPartRegistry partRegistry
            , IPageService pageService
            , IVirtualFileSystem virtualFileSystem
            , IViewPart viewPart )
        {
            m_BundleService = bundleService;
            m_IconResourceService = iconResourceService;
            m_PartRegistry = partRegistry;
            m_PageService = pageService;
            m_ToolStripFactory = pageService.PageLayoutProvider.ToolbarFactory;
            m_VirtualFileSystem = virtualFileSystem;
            m_viewPart = viewPart;

            InitializeComponent();

            DataExplorerConfiguration config = null;
            System.IO.TextReader textReader = new System.IO.StringReader(m_viewPart.ConfigurationXml);
            using (XmlReader reader = XmlReader.Create(textReader, XmlUtils.CreateFragmentReaderSettings(), XmlUtils.CreateParserContext()))
            {
                config = XmlUtils.ReadElement<DataExplorerConfiguration>(reader, "dataExplorer");
            }

            foreach (ColumnModelConfiguration columnConfiguration in config.Table.Columns)
            {
                IFieldDescriptor descriptor = CreateDescriptor(columnConfiguration.Type); 
                if (!string.IsNullOrEmpty(columnConfiguration.Text))
                    descriptor.Column.Text = columnConfiguration.Text;
                if (!string.IsNullOrEmpty(columnConfiguration.Format))
                    descriptor.Column.Format = columnConfiguration.Format;
                if (!string.IsNullOrEmpty(columnConfiguration.Alignment))
                    descriptor.Column.Alignment = (ColumnAlignment)Enum.Parse(typeof(ColumnAlignment), columnConfiguration.Alignment);
                if (0 > columnConfiguration.Width)
                    descriptor.Column.Width = columnConfiguration.Width;
                if (0 > columnConfiguration.ContentWidth)
                    descriptor.Column.ContentWidth = columnConfiguration.ContentWidth;
                if (!string.IsNullOrEmpty(columnConfiguration.Image))
                    descriptor.Column.Image = iconResourceService.GetImage( columnConfiguration.Image );
                if (!columnConfiguration.ImageOnRight)
                    descriptor.Column.ImageOnRight = columnConfiguration.ImageOnRight;
                if (!columnConfiguration.Visible)
                    descriptor.Column.Visible = columnConfiguration.Visible;
                if (!columnConfiguration.Sortable)
                    descriptor.Column.Sortable = columnConfiguration.Sortable;
                if (!columnConfiguration.Resizable)
                    descriptor.Column.Resizable = columnConfiguration.Resizable;
                //if (!string.IsNullOrEmpty(columnConfiguration.DefaultComparerType))
                //    descriptor.Column.DefaultComparerType = Type.GetType( columnConfiguration.DefaultComparerType );
                //if (!string.IsNullOrEmpty(columnConfiguration.SortOrder))
                //    descriptor.Column.SortOrder =(SortOrder)Enum.Parse(typeof(SortOrder), columnConfiguration.SortOrder );
                if (!columnConfiguration.Editable)
                    descriptor.Column.Editable = columnConfiguration.Editable;
                if (!columnConfiguration.Enabled)
                    descriptor.Column.Enabled = columnConfiguration.Enabled;
                if (!columnConfiguration.Selectable)
                    descriptor.Column.Selectable = columnConfiguration.Selectable;
                if (!string.IsNullOrEmpty(columnConfiguration.ToolTipText))
                    descriptor.Column.ToolTipText = columnConfiguration.ToolTipText;


                _fieldDescriptors.Add(descriptor);
            }
        }

        public IFieldDescriptor CreateDescriptor(string type)
        { 
            switch( type.ToLower() )
            {
                case "image":
                    return new ImageFieldDescriptor();
                case "text":
                    return new TextFieldDescriptor();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
