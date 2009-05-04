
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace jingxian.ui.controls.fastlistview
{

    /// <summary>
    /// A virtual object list view operates in virtual mode, that is, it only gets model objects for
    /// a row when it is needed. This gives it the ability to handle very large numbers of rows with
    /// minimal resources.
    /// </summary>
    /// <remarks><para>A listview is not a great user interface for a large number of items. But if you've
    /// ever wanted to have a list with 10 million items, go ahead, knock yourself out.</para>
    /// <para>Virtual lists can never iterate their contents. That would defeat the whole purpose.</para>
    /// <para>Given the above, grouping and sorting are not possible on virtual lists. But if the backing data store has
    /// a sorting mechanism, a CustomSorter can be installed which will be called when the sorting is required.</para>
    /// <para>For the same reason, animate GIFs should not be used in virtual lists. Animated GIFs require some state
    /// information to be stored for each animation, but virtual lists specifically do not keep any state information.
    /// You really do not want to keep state information for 10 million animations!</para>
    /// </remarks>
    public class VirtualObjectListView : ObjectListView
    {
        /// <summary>
        /// Create a VirtualObjectListView
        /// </summary>
        public VirtualObjectListView()
            : base()
        {
            this.ShowGroups = false; // virtual lists can never show groups
            this.VirtualMode = true;
            this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.HandleRetrieveVirtualItem);

            // Install a null custom sorter to turn off sorting. Who wants to fetch and sort 10 million items?
            this.CustomSorter = delegate(OLVColumn column, SortOrder sortOrder) { };
        }

        #region Public Properties

        /// <summary>
        /// This delegate is used to fetch a rowObject, given it's index within the list
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RowGetterDelegate RowGetter
        {
            get { return rowGetter; }
            set { rowGetter = value; }
        }

        #endregion

        #region OLV accessing

        /// <summary>
        /// Return the number of items in the list
        /// </summary>
        /// <returns>the number of items in the list</returns>
        override public int GetItemCount()
        {
            return this.VirtualListSize;
        }

        /// <summary>
        /// Return the item at the given index
        /// </summary>
        /// <param name="name">Index of the item to be returned</param>
        /// <returns>An OLVListItem</returns>
        override public OLVListItem GetItem(int index)
        {
            return this.MakeListViewItem(index);
        }

        /// <summary>
        /// Return the model object at the given index
        /// </summary>
        /// <param name="name">Index of the model object to be returned</param>
        /// <returns>A model object</returns>
        override public object GetModelObject(int index)
        {
            return this.GetRowObjectAt(index);
        }

        #endregion

        #region Object manipulation

        /// <summary>
        /// Remove all items from this list
        /// </summary>
        override public void ClearObjects()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(ClearObjects));
            else
                this.VirtualListSize = 0;
        }

        /// <summary>
        /// Select the row that is displaying the given model object.
        /// This does nothing in virtual lists.
        /// </summary>
        /// <remarks>This is a no-op for virtual lists, since there is no way to map the model
        /// object back to the ListViewItem that represents it.</remarks>
        /// <param name="modelObject">The object that gave data</param>
        override public void SelectObject(object modelObject)
        {
            // do nothing
        }

        /// <summary>
        /// Select the rows that is displaying any of the given model object.
        /// This does nothing in virtual lists.
        /// </summary>
        /// <remarks>This is a no-op for virtual lists, since there is no way to map the model
        /// objects back to the ListViewItem that represents them.</remarks>
        /// <param name="modelObjects">A collection of model objects</param>
        override public void SelectObjects(IList modelObjects)
        {
            // do nothing
        }

        /// <summary>
        /// Update the rows that are showing the given objects
        /// </summary>
        /// <remarks>This is a no-op for virtual lists, since there is no way to map the model
        /// objects back to the ListViewItem that represents them.</remarks>
        override public void RefreshObjects(IList modelObjects)
        {
            // do nothing
        }

        #endregion

        /// <summary>
        /// Invalidate any cached information when we rebuild the list.
        /// </summary>
        public override void BuildList(bool shouldPreserveSelection)
        {
            this.lastRetrieveVirtualItemIndex = -1;

            // This call was in the older code, but I can't think why since virtual lists
            // can't be build in that fashion.
            //base.BuildList(false);
        }

        /// <summary>
        /// Prepare the listview to show alternate row backcolors
        /// </summary>
        /// <remarks>Alternate colored backrows can't be handle in the same way as our base class.
        /// With virtual lists, they are handled at RetrieveVirtualItem time.</remarks>
        protected override void PrepareAlternateBackColors()
        {
            // do nothing
        }

        #region Event handlers

        /// <summary>
        /// Handle a RetrieveVirtualItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HandleRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            // .NET 2.0 seems to generate a lot of these events. Before drawing *each* sub-item,
            // this event is triggered 4-8 times for the same index. So we save lots of CPU time
            // by caching the last result.
            if (this.lastRetrieveVirtualItemIndex != e.ItemIndex)
            {
                this.lastRetrieveVirtualItemIndex = e.ItemIndex;
                this.lastRetrieveVirtualItem = this.MakeListViewItem(e.ItemIndex);
            }
            e.Item = this.lastRetrieveVirtualItem;
        }

        /// <summary>
        /// Create a OLVListItem for given row index
        /// </summary>
        /// <param name="itemIndex">The index of the row that is needed</param>
        /// <returns>An OLVListItem</returns>
        public OLVListItem MakeListViewItem(int itemIndex)
        {
            OLVListItem olvi = new OLVListItem(this.GetRowObjectAt(itemIndex));
            this.FillInValues(olvi, olvi.RowObject);
            if (this.UseAlternatingBackColors)
            {
                if (this.View == View.Details && itemIndex % 2 == 1)
                    olvi.BackColor = this.AlternateRowBackColorOrDefault;
                else
                    olvi.BackColor = this.BackColor;

                this.CorrectSubItemColors(olvi);
            }
            this.SetSubItemImages(itemIndex, olvi);
            return olvi;
        }

        /// <summary>
        /// Return the row object for the given row index
        /// </summary>
        /// <param name="index">index of the row whose object is to be fetched</param>
        /// <returns>A model object or null if no delegate is installed</returns>
        virtual protected object GetRowObjectAt(int index)
        {
            if (this.RowGetter == null)
                return null;
            else
                return this.RowGetter(index);
        }

        #endregion

        #region Variable declaractions

        private RowGetterDelegate rowGetter;
        protected int lastRetrieveVirtualItemIndex = -1;
        private OLVListItem lastRetrieveVirtualItem;

        #endregion
    }
}
