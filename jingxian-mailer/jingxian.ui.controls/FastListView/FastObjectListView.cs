

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
    /// A FastObjectListView trades function for speed.
    /// </summary>
    /// <remarks>
    /// <para>On my mid-range laptop, this view builds a list of 10,000 objects in 0.1 seconds,
    /// as opposed to a normal ObjectListView which takes 10-15 seconds. Lists of up to 50,000 items should be
    /// able to be handled with sub-second response times even on low end machines.</para>
    /// <para>
    /// A FastObjectListView is implemented as a virtual list with some of the virtual modes limits (e.g. no sorting)
    /// fixed through coding. There are some functions that simply cannot be provided. Specifically, a FastObjectListView cannot:
    /// <list>
    /// <item>shows groups</item>
    /// <item>use Tile view</item>
    /// <item>display images on subitems</item>
    /// </list>
    /// </para>
    /// <para>You can circumvent the limit on subitem images by making the list owner drawn, and giving the column
    /// a Renderer of BaseRenderer, e.g. <code>myColumnWithImage.Renderer = new BaseRenderer();</code> </para>
    /// </remarks>
    public class FastObjectListView : VirtualObjectListView
    {
        public FastObjectListView()
        {
            this.SearchForVirtualItem += new SearchForVirtualItemEventHandler(FastObjectListView_SearchForVirtualItem);

            this.CustomSorter = delegate(OLVColumn column, SortOrder sortOrder)
            {
                this.lastRetrieveVirtualItemIndex = -1;
                if (sortOrder != SortOrder.None)
                    this.objectList.Sort(new ModelObjectComparer(column, sortOrder, this.SecondarySortColumn, this.SecondarySortOrder));
                this.objectsToIndexMap.Clear();
                for (int i = 0; i < this.objectList.Count; i++)
                    this.objectsToIndexMap[this.objectList[i]] = i;
            };
        }
        Hashtable objectsToIndexMap = new Hashtable();

        #region Public properties

        /// <summary>
        /// Get/set the list of objects that are shown by the control.
        /// </summary>
        /// <remarks>
        /// <para>This method preserves selection, if possible. Use SetObjects() if
        /// you do not want to preserve the selection. Preserving selection is the slowest part of this
        /// code and performance is O(n) where n is the number of selected rows.</para>
        /// <para>This method is not thread safe.</para>
        /// </remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public ArrayList Objects
        {
            get { return this.objectList; }
            set
            {
                ArrayList previousSelection = this.SelectedObjects;
                this.SetObjects(value);
                this.SelectedObjects = previousSelection;
            }
        }
        private ArrayList objectList = new ArrayList();

        /// <summary>
        /// When the user types into a list, should the values in the current sort column be searched to find a match?
        /// If this is false, the primary column will always be used regardless of the sort column.
        /// </summary>
        /// <remarks>When this is true, the behavior is like that of ITunes.</remarks>
        [Category("Behavior"),
        Description("When the user types into a list, should the values in the current sort column be searched to find a match?"),
        DefaultValue(false)]
        public bool IsSearchOnSortColumn
        {
            get { return isSearchOnSortColumn; }
            set { isSearchOnSortColumn = value; }
        }
        private bool isSearchOnSortColumn = false;

        #endregion

        #region Commands

        /// <summary>
        /// Set the collection of objects that this control will show.
        /// </summary>
        /// <param name="collection"></param>
        override public void SetObjects(IEnumerable collection)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetObjectsInvoker(this.SetObjects), new object[] { collection });
                return;
            }

            if (collection == null)
                this.objectList = new ArrayList();
            else if (collection is ICollection)
                this.objectList = new ArrayList((ICollection)collection);
            else
            {
                this.objectList = new ArrayList();
                foreach (object x in collection)
                    this.objectList.Add(x);
            }
            this.VirtualListSize = this.objectList.Count;
            this.Sort();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event handler for the column click event
        /// </summary>
        /// <remarks>
        /// This differs from its base version by trying to preserve selection. The base class,
        /// being a pure virtual list, cannot maintain selection since it cannot map a
        /// model objects to the row that is responsible for displaying it. This class can do that.
        /// </remarks>
        override protected void HandleColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Toggle the sorting direction on successive clicks on the same column
            if (this.lastSortColumn != null && e.Column == this.lastSortColumn.Index)
                this.lastSortOrder = (this.lastSortOrder == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending);
            else
                this.lastSortOrder = SortOrder.Ascending;

            this.BeginUpdate();
            ArrayList previousSelection = this.SelectedObjects;
            this.Sort(e.Column);
            this.SelectedObjects = previousSelection;
            this.EndUpdate();
        }

        void FastObjectListView_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            // The event has e.IsPrefixSearch, but as far as I can tell, this is always false (maybe that's different under Vista)
            // So we ignore IsPrefixSearch and IsTextSearch and always to a case insensitve prefix match

            int increment = (e.Direction == SearchDirectionHint.Up ? -1 : 1);
            OLVColumn column = this.GetColumn(0);
            if (this.IsSearchOnSortColumn && this.View == View.Details && this.lastSortColumn != null)
                column = this.lastSortColumn;

            int i;
            for (i = e.StartIndex; i >= 0 && i < this.objectList.Count; i += increment)
            {
                string data = column.GetStringValue(this.objectList[i]);
                if (data.StartsWith(e.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    e.Index = i;
                    return;
                }
            }

            // Also the LVFINDINFO has a LV_WRAP flag, but the SearchForVirtualItemEventArgs does not. Why?
            // We always wrap
            i = (increment < 0 ? this.objectList.Count : 0);
            while ((increment < 0 && i > e.StartIndex) || (increment > 0 && i < e.StartIndex))
            {
                string data = column.GetStringValue(this.objectList[i]);
                if (data.StartsWith(e.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    e.Index = i;
                    return;
                }
                i += increment;
            }
        }

        #endregion

        #region Object manipulation

        /// <summary>
        /// Select the row that is displaying the given model object.
        /// </summary>
        /// <param name="modelObject">The object that gave data</param>
        override public void SelectObject(object modelObject)
        {
            if (!this.objectsToIndexMap.ContainsKey(modelObject))
                return;

            int index = (int)this.objectsToIndexMap[modelObject];

            // If this object is already selected, we don't need to do anything
            if (this.SelectedIndices.Count == 1 && this.SelectedIndices[0] == index)
                return;

            this.SelectedIndices.Clear();
            if (index >= 0)
                this.SelectedIndices.Add(index);
        }

        /// <summary>
        /// Select the rows that is displaying any of the given model object.
        /// </summary>
        /// <param name="modelObjects">A collection of model objects</param>
        override public void SelectObjects(IList modelObjects)
        {
            this.SelectedIndices.Clear();

            foreach (object model in modelObjects)
            {
                if (this.objectsToIndexMap.ContainsKey(model))
                {
                    int index = (int)this.objectsToIndexMap[model];
                    if (index >= 0)
                        this.SelectedIndices.Add(index);
                }
            }
        }

        /// <summary>
        /// Update the rows that are showing the given objects
        /// </summary>
        override public void RefreshObjects(IList modelObjects)
        {
            this.Invalidate();
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Return the row object for the given row index
        /// </summary>
        /// <param name="index">index of the row whose object is to be fetched</param>
        /// <returns>A model object or null if no delegate is installed</returns>
        override protected object GetRowObjectAt(int index)
        {
            return this.objectList[index];
        }


        internal class ModelObjectComparer : IComparer
        {
            public ModelObjectComparer(OLVColumn col, SortOrder order)
            {
                this.column = col;
                this.sortOrder = order;
                this.secondComparer = null;
            }

            public ModelObjectComparer(OLVColumn col, SortOrder order, OLVColumn col2, SortOrder order2)
                : this(col, order)
            {
                // There is no point in secondary sorting on the same column
                if (col != col2)
                    this.secondComparer = new ModelObjectComparer(col2, order2);
            }

            public int Compare(object x, object y)
            {
                int result = 0;
                object x1 = this.column.GetValue(x);
                object y1 = this.column.GetValue(y);

                if (this.sortOrder == SortOrder.None)
                    return 0;

                // Handle nulls. Null values come last
                bool xIsNull = (x1 == null || x1 == System.DBNull.Value);
                bool yIsNull = (y1 == null || y1 == System.DBNull.Value);
                if (xIsNull || yIsNull)
                {
                    if (xIsNull && yIsNull)
                        result = 0;
                    else
                        result = (xIsNull ? -1 : 1);
                }
                else
                {
                    result = this.CompareValues(x1, y1);
                }

                if (this.sortOrder == SortOrder.Descending)
                    result = 0 - result;

                // If the result was equality, use the secondary comparer to resolve it
                if (result == 0 && this.secondComparer != null)
                    result = this.secondComparer.Compare(x, y);

                return result;
            }

            public int CompareValues(object x, object y)
            {
                // Force case insensitive compares on strings
                if (x is String)
                    return String.Compare((String)x, (String)y, true);
                else
                {
                    IComparable comparable = x as IComparable;
                    if (comparable != null)
                        return comparable.CompareTo(y);
                    else
                        return 0;
                }
            }

            private OLVColumn column;
            private SortOrder sortOrder;
            private ModelObjectComparer secondComparer;
        }

        #endregion
    }
}
