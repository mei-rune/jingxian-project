using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace jingxian.ui.views
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Filters;
    using Empinia.UI;
    using Empinia.UI.Workbench;
    using Empinia.UI.Navigator;

    using jingxian.data;
    using jingxian.domainModel;

    public class NodeWapper
    {
        object _instance;

        PropertyInfo _idAccessor;
        PropertyInfo _parentAccessor;
        PropertyInfo _iconAccessor;
        PropertyInfo _orderingAccessor;
        PropertyInfo _nameAccessor;
        
        

        public NodeWapper( object instance )
        {
            if (null == instance)
                throw new ArgumentNullException("instance");

            _instance = instance;
            Type instanceType = instance.GetType();
            _idAccessor = instanceType.GetProperty("Id");
            _parentAccessor = instanceType.GetProperty("Parent");
            _iconAccessor = instanceType.GetProperty("Icon");
            _orderingAccessor = instanceType.GetProperty("Ordering");
            _nameAccessor = instanceType.GetProperty("Name");

        }

        public int Id
        {
            get { return (int)_idAccessor.GetValue( _instance, null ); }
            set { _idAccessor.SetValue(_instance, value, null); }
        }

        public int Parent
        {
            get { return (int)_parentAccessor.GetValue(_instance, null); }
            set { _parentAccessor.SetValue(_instance, value, null); }
        }

        public string Icon
        {
            get { return (string)_iconAccessor.GetValue(_instance, null); }
            set { _iconAccessor.SetValue(_instance, value, null); }
        }

        public int Ordering
        {
            get { return (int)_orderingAccessor.GetValue(_instance, null); }
            set { _orderingAccessor.SetValue(_instance, value, null); }
        }

        public string Name
        {
            get { return (string)_nameAccessor.GetValue(_instance, null); }
            set { _nameAccessor.SetValue(_instance, value, null); }
        }

        public object GetInstance()
        {
            return _instance;
        }

        public override string ToString()
        {
            return _instance.ToString();
        }
    }

    public abstract class TreePage : TreeView, IView
    {
        public class ContentNode : TreeNode, IContentProvider
        {
            NodeWapper m_Content;
            public ContentNode(NodeWapper content)
                : base(content.ToString())
            {
                m_Content = content;
            }

            public int Id
            {
                get { return m_Content.Id; }
            }

            public NodeWapper Content
            {
                get { return m_Content; }
            }

            public NodeWapper ParentContent
            {
                get
                {
                    ContentNode parent = this.Parent as ContentNode;
                    if (null == parent)
                        return null;
                    return parent.Content;
                }
            }

            #region IContentProvider 成员

            object IContentProvider.Content
            {
                get { return this.Content; }
            }

            object IContentProvider.ParentContent
            {
                get { return this.ParentContent; }
            }

            #endregion
        }

		private readonly IBundleService m_BundleService;
		private readonly IIconResourceService m_IconResourceService;
        private readonly IToolbarFactory m_ToolStripFactory;
        private readonly IVirtualFileSystem m_VirtualFileSystem;
        private IViewPart _viewPart;
        private ContextMenuStrip m_ContextMenu;
        private ImageList m_Icons;

        public TreePage(IBundleService bundleService
            , IIconResourceService iconResourceService
            , IPageService pageService
            , IVirtualFileSystem virtualFileSystem)
        {
            m_BundleService = bundleService;
            m_IconResourceService = iconResourceService;
            
            m_ToolStripFactory = pageService.PageLayoutProvider.ToolbarFactory;
            m_VirtualFileSystem = virtualFileSystem;
        }

        protected void Initialize()
        {

            InitializeVisualElements();

            this.AfterLabelEdit += new NodeLabelEditEventHandler(MailPage_AfterLabelEdit);
            this.ItemDrag += new ItemDragEventHandler(MailPage_ItemDrag);

            // Add event handlers for the required drag events.
            this.DragEnter += new DragEventHandler(MailPage_DragEnter);
            this.DragOver += new DragEventHandler(MailPage_DragOver);
            this.DragDrop += new DragEventHandler(MailPage_DragDrop);
            this.NodeMouseClick += new TreeNodeMouseClickEventHandler(MailPage_NodeMouseClick);
            this.MouseClick += new MouseEventHandler(MailPage_MouseClick);
            this.AfterSelect += new TreeViewEventHandler(MailPage_AfterSelect);
        }

        protected abstract IList GetStore();
        protected abstract void UpdateParent(object content);
        protected abstract void UpdateName(object content);

        protected void InitializeVisualElements()
        {
            SuspendLayout();

            try
            {

                this.LabelEdit = true;
                this.AllowDrop = true;


                m_Icons = new ImageList();
                m_Icons.ColorDepth = ColorDepth.Depth32Bit;
                m_Icons.ImageSize = new Size(16, 16);
                m_Icons.TransparentColor = Color.White;

                this.ImageList = m_Icons;

                m_ContextMenu = m_ToolStripFactory.CreateToolbar<ContextMenuStrip>(this);
                this.ContextMenuStrip = m_ContextMenu;



                Dictionary<int, NodeWapper> folders = new Dictionary<int, NodeWapper>();
                Dictionary<int, ContentNode> nodes = new Dictionary<int, ContentNode>();



                foreach ( object instance in GetStore() )
                {
                    NodeWapper nodeWappper = new NodeWapper(instance);
                    folders.Add(nodeWappper.Id, nodeWappper);
                }

                while (folders.Count > 0)
                {
                    NodeWapper[] tmpList = new NodeWapper[folders.Count];
                    folders.Values.CopyTo(tmpList, 0);

                    foreach (NodeWapper folder in tmpList)
                    {
                        if (folders.ContainsKey(folder.Parent))
                            continue;

                        ContentNode parent = null;
                        nodes.TryGetValue(folder.Parent, out parent);
                        ContentNode node = CreateNode(folder);
                        InsertNode((null == parent) ? this.Nodes : parent.Nodes, node);
                        nodes.Add(node.Id, node);
                        folders.Remove(folder.Id);
                    }
                }
            }
            finally
            {
                ResumeLayout();
            }
        }

        #region EventHandler

        void MailPage_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _viewPart.Select(e.Node);
        }

        void MailPage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            TreeNode node = this.GetNodeAt(e.Location);
            if (node != null)
            {
                this.SelectedNode = node;
            }
            else
            {
                m_ContextMenu.Items.Clear();
                m_ContextMenu.Items.AddRange(m_ToolStripFactory.GetItems(this));
            }
        }

        private void MailPage_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.SelectedNode = e.Node;

            if(e.Button != MouseButtons.Right)
                return;

            m_ContextMenu.Items.Clear();
            NodeWapper nodeWapper = ((IContentProvider)e.Node).Content as NodeWapper;
            m_ContextMenu.Items.AddRange(m_ToolStripFactory.GetItems( nodeWapper.GetInstance() ));
        }

        private void MailPage_DragEnter(object sender, DragEventArgs e)
        {

            ContentNode draggedNode = (ContentNode)e.Data.GetData(typeof(ContentNode));
            if (null != draggedNode )
                e.Effect = e.AllowedEffect;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MailPage_DragOver(object sender, DragEventArgs e)
        {
            System.Drawing.Point targetPoint = this.PointToClient(new System.Drawing.Point(e.X, e.Y));
            this.SelectedNode = this.GetNodeAt(targetPoint);
        }

        private void MailPage_DragDrop(object sender, DragEventArgs e)
        {
            System.Drawing.Point targetPoint = this.PointToClient(new System.Drawing.Point(e.X, e.Y));

            ContentNode targetNode = this.GetNodeAt(targetPoint) as ContentNode;
            if (null == targetNode)
                return;

            ContentNode draggedNode = (ContentNode)e.Data.GetData(typeof(ContentNode));

            if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    int oldParent = draggedNode.Content.Parent;

                    draggedNode.Content.Parent = targetNode.Content.Id;
                    try
                    {
                        UpdateParent(draggedNode.Content);

                        draggedNode.Remove();
                        targetNode.Nodes.Add(draggedNode);

                        targetNode.Expand();
                    }
                    catch (Exception exception)
                    {
                        draggedNode.Content.Parent = oldParent;
                        Utils.ShowMsgBox(exception);
                    }
                }
            }
        }

        void MailPage_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;

            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        void MailPage_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                Utils.ShowMsgBox("名字不能为空");
                return;
            }

            ContentNode node = e.Node as ContentNode;
            string oldName = node.Content.Name;
            try
            {
                node.Content.Name = e.Label;
                UpdateName( node.Content);
            }
            catch (Exception exception)
            {
                node.Content.Name = oldName;
                e.CancelEdit = true;

                Utils.ShowMsgBox(exception);
            }
        }

        #endregion

        public void InsertNode(TreeNodeCollection list, ContentNode node)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (((ContentNode)list[i]).Content.Ordering > node.Content.Ordering)
                {
                    list.Insert(i, node);
                    return;
                }
            }
            list.Add(node);
        }

        public ContentNode CreateNode(NodeWapper content)
        {
            ContentNode node = new ContentNode(content);
            node.ImageKey = content.Icon;
            node.SelectedImageKey = content.Icon;
            EnsureIconIsInImageList(content.Icon);
            return node;
        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            return ContainsNode(node1, node2.Parent);
        }

        private void EnsureIconIsInImageList(string iconId)
        {
            if (!m_Icons.Images.ContainsKey(iconId))
            {
                m_Icons.Images.Add(iconId, Helper.GetImage(iconId, "jingxian.folder.standard.png", m_IconResourceService, m_VirtualFileSystem));
            }
        }

        #region IView 成员

        public void Configure(IViewPart owningPart)
        {
            _viewPart = owningPart;
        }

        public IViewPart OwningViewPart
        {
            get { return _viewPart; }
        }

        public System.Windows.Forms.Control Widget
        {
            get { return this ; }
        }

        #endregion
    }
}