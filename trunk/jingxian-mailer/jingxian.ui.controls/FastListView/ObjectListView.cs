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
    public partial class ObjectListView : ListView, ISupportInitialize
	{
		public ObjectListView()
			: base()
		{
			this.ColumnClick += new ColumnClickEventHandler(this.HandleColumnClick);
            this.ItemCheck += new ItemCheckEventHandler(this.HandleItemCheck);
            this.Layout += new LayoutEventHandler(this.HandleLayout);
            this.ColumnWidthChanging += new ColumnWidthChangingEventHandler(this.HandleColumnWidthChanging);
            this.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.HandleColumnWidthChanged);

			base.View = View.Details;
			this.DoubleBuffered = true; // kill nasty flickers. hiss... me hates 'em
		    this.AlternateRowBackColor = Color.Empty;
            this.ShowSortIndicators = true;
		}

		#region Public properties

        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable Objects
        {
            get { return this.objects; }
            set { this.SetObjects(value); }
        }

        /// <summary>
        /// Get or set all the columns that this control knows about.
        /// Only those columns where IsVisible is true will be seen by the user.
        /// </summary>
        /// <remarks>If you want to add new columns programmatically, add them to
        /// AllColumns and then call RebuildColumns(). Normally, you do not have to
        /// deal with this property directly. Just use the IDE.</remarks>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<OLVColumn> AllColumns
        {
            get
            {
                // If someone has wiped out the columns, put the list back
                if (allColumns == null)
                    allColumns = new List<OLVColumn>();

                // If we don't know the columns, use the columns from the control.
                // This handles legacy cases
                if (allColumns.Count == 0 && this.Columns.Count > 0) {
                    for (int i = 0; i < this.Columns.Count; i++) {
                        this.allColumns.Add(this.GetColumn(i));
                    }
                }
                return allColumns;
            }
            set { allColumns = value; }
        }
        private List<OLVColumn> allColumns = new List<OLVColumn>();

        /// <summary>
        /// Return the visible columns in the order they are displayed to the user
        /// </summary>
        [Browsable(false)]
        public List<OLVColumn> ColumnsInDisplayOrder
        {
            get
            {
                List<OLVColumn> columnsInDisplayOrder = new List<OLVColumn>(this.Columns.Count);
                for (int i = 0; i < this.Columns.Count; i++)
                    columnsInDisplayOrder.Add(null);
                for (int i = 0; i < this.Columns.Count; i++) {
                    OLVColumn col = this.GetColumn(i);
                    columnsInDisplayOrder[col.DisplayIndex] = col;
                }
                return columnsInDisplayOrder;
            }
        }


        /// <summary>
        /// Should the list view show images on subitems?
        /// </summary>
        /// <remarks>
        /// <para>Under Windows, this works by sending messages to the underlying
        /// Windows control. To make this work under Mono, we would have to owner drawing the items :-(</para></remarks>
        [Category("Behavior"),
         Description("Should the list view show images on subitems?"),
         DefaultValue(false)]
        public bool ShowImagesOnSubItems
        {
            get { return showImagesOnSubItems; }
            set { showImagesOnSubItems = value; }
        }

		/// <summary>
		/// This property controls whether group labels will be suffixed with a count of items.
		/// </summary>
		/// <remarks>
        /// The format of the suffix is controlled by GroupWithItemCountFormat/GroupWithItemCountSingularFormat properties
		/// </remarks>
		[Category("Behavior"),
		 Description("Will group titles be suffixed with a count of the items in the group?"),
		 DefaultValue(false)]
		public bool ShowItemCountOnGroups {
			get { return showItemCountOnGroups; }
			set { showItemCountOnGroups = value; }
		}

        /// <summary>
        /// When a group title has an item count, how should the lable be formatted?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group</item>
        /// </list>
        /// </remarks>
        /// <example>"{0} [{1} items]"</example>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountFormat
        {
            get { return groupWithItemCountFormat; }
            set { groupWithItemCountFormat = value; }
        }

        /// <summary>
        /// Return this.GroupWithItemCountFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountFormatOrDefault
        {
            get {
                if (String.IsNullOrEmpty(this.GroupWithItemCountFormat))
                    return "{0} [{1} items]";
                else
                    return this.GroupWithItemCountFormat;
            }
        }

        /// <summary>
        /// When a group title has an item count, how should the lable be formatted if
        /// there is only one item in the group?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group (always 1)</item>
        /// </list>
        /// </remarks>
        /// <example>"{0} [{1} item]"</example>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountSingularFormat
        {
            get { return groupWithItemCountSingularFormat; }
            set { groupWithItemCountSingularFormat = value; }
        }

        /// <summary>
        /// Return this.GroupWithItemCountSingularFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountSingularFormatOrDefault
        {
            get {
                if (String.IsNullOrEmpty(this.GroupWithItemCountSingularFormat))
                    return "{0} [{1} item]";
                else
                    return this.GroupWithItemCountSingularFormat;
            }
        }

		/// <summary>
		/// Should the list give a different background color to every second row?
		/// </summary>
        /// <remarks><para>The color of the alternate rows is given by AlternateRowBackColor.</para>
        /// <para>There is a "feature" in .NET for listviews in non-full-row-select mode, where
        /// selected rows are not drawn with their correct background color.</para></remarks>
		[Category("Appearance"),
		 Description("Should the list view use a different backcolor to alternate rows?"),
		 DefaultValue(false)]
		public bool UseAlternatingBackColors {
			get { return useAlternatingBackColors; }
			set { useAlternatingBackColors = value; }
		}

        /// <summary>
        /// Should the list view show a bitmap in the column header to show the sort direction?
        /// </summary>
        [Category("Behavior"),
         Description("Should the list view show sort indicators in the column headers?"),
         DefaultValue(true)]
        public bool ShowSortIndicators
        {
            get { return showSortIndicators; }
            set { showSortIndicators = value; }
        }

		/// <summary>
		/// If every second row has a background different to the control, what color should it be?
		/// </summary>
		[Category("Appearance"),
		 Description("If using alternate colors, what color should alterate rows be?"),
		 DefaultValue(typeof(Color), "Empty")]
		public Color AlternateRowBackColor {
			get { return alternateRowBackColor; }
			set { alternateRowBackColor = value; }
		}

		/// <summary>
		/// Return the alternate row background color that has been set, or the default color
		/// </summary>
		[Browsable(false)]
		public Color AlternateRowBackColorOrDefault {
			get {
				if (alternateRowBackColor == Color.Empty)
					return Color.LemonChiffon;
				else
					return alternateRowBackColor;
			}
		}

        /// <summary>
        /// Get or set whether or not the listview is frozen. When the listview is
        /// frozen, it will not update itself.
        /// </summary>
        /// <remarks><para>The Frozen property is similar to the methods Freeze()/Unfreeze()
        /// except that changes to the Frozen property do not nest.</para></remarks>
        /// <example>objectListView1.Frozen = false; // unfreeze the control regardless of the number of Freeze() calls
        /// </example>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Frozen
        {
            get { return freezeCount > 0; }
            set {
                if (value)
                    Freeze();
                else if (freezeCount > 0) {
                    freezeCount = 1;
                    Unfreeze();
                }
            }
        }
        private int freezeCount = 0;

        /// <summary>
        /// Get/set the list of columns that should be used when the list switches to tile view.
        /// </summary>
        /// <remarks>If no list of columns has been installed, this value will default to the
        /// first column plus any column where IsTileViewColumn is true.</remarks>
        [Browsable(false),
        Obsolete("Use GetFilteredColumns() and OLVColumn.IsTileViewColumn instead"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OLVColumn> ColumnsForTileView
        {
            get { return this.GetFilteredColumns(View.Tile); }
        }

        /// <summary>
        /// Get the ListViewItem that is currently selected . If no row is selected, or more than one is selected, return null.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListViewItem SelectedItem
        {
            get {
                if (this.SelectedIndices.Count == 1)
                    return this.GetItem(this.SelectedIndices[0]);
                else
                    return null;
            }
            set {
                this.SelectedIndices.Clear();
                if (value != null)
                    this.SelectedIndices.Add(value.Index);
            }
        }

        /// <summary>
        /// Get the model object from the currently selected row. If no row is selected, or more than one is selected, return null.
        /// Select the row that is displaying the given model object. All other rows are deselected.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Object SelectedObject
        {
            get { return this.GetSelectedObject(); }
            set { this.SelectObject(value); }
        }

        /// <summary>
        /// Get the model objects from the currently selected rows. If no row is selected, the returned List will be empty.
        /// When setting this value, select the rows that is displaying the given model objects. All other rows are deselected.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrayList SelectedObjects
        {
            get { return this.GetSelectedObjects(); }
            set { this.SelectObjects(value); }
        }

        /// <summary>
        /// Get/set the style of view that this listview is using
        /// </summary>
        /// <remarks>Switching to tile or details view installs the columns appropriate to that view.
        /// Confusingly, in tile view, every column is shown as a row of information.</remarks>
        new public View View
        {
            get { return base.View; }
            set {
                if (base.View == value)
                    return;

                if (this.Frozen) {
                    base.View = value;
                    return;
                }

                this.Freeze();

                // If we are switching to a Detail or Tile view, setup the columns needed for that view
                if (value == View.Details || value == View.Tile) {
                    this.ChangeToFilteredColumns(value);

                    if (value == View.Tile)
                        this.CalculateReasonableTileSize();
                }

                base.View = value;
                this.Unfreeze();
            }
        }

        /// <summary>
        /// Specify the height of each row in the control in pixels.
        /// </summary>
        /// <remarks><para>The row height in a listview is normally determined by the font size and the small image list size.
        /// This setting allows that calculation to be overridden (within reason: you still cannot set the line height to be
        /// less than the line height of the font used in the control). </para>
        /// <para>Setting it to -1 means use the normal calculation method.</para>
        /// <para><bold>This feature is experiemental!</bold> Strange things may happen to your program, 
        /// your spouse or your pet if you use it.</para>
        /// </remarks>
        [Category("Appearance"),
         DefaultValue(-1)]
        public int RowHeight
        {
            get { return rowHeight; }
            set {
                if (value < 1)
                    rowHeight = -1;
                else
                    rowHeight = value;
                this.SetupExternalImageList();
            }
        }
        private int rowHeight = -1;

        /// <summary>
        /// Override the SmallImageList property so we can correctly shadow its operations.
        /// </summary>
        /// <remarks><para>If you use the RowHeight property to specify the row height, the SmallImageList
        /// must be fully initialised before setting/changing the RowHeight. If you add new images to the image
        /// list after setting the RowHeight, you must assign the imagelist to the control again. Something as simple
        /// as this will work:
        /// <code>listView1.SmallImageList = listView1.SmallImageList;</code></para>
        /// </remarks>
        new public ImageList SmallImageList
        {
            get { return this.shadowedImageList; }
            set {
                this.shadowedImageList = value;
                this.SetupExternalImageList();
            }
        }
        private ImageList shadowedImageList = null;

        /// <summary>
        /// Give access to the image list that is actually being used by the control
        /// </summary>
        [Browsable(false)]
        public ImageList BaseSmallImageList
        {
            get { return base.SmallImageList; }
        }

        /// <summary>
        /// Get/set the column that will be used to resolve comparisons that are equal when sorting.
        /// </summary>
        /// <remarks>There is no user interface for this setting. It must be set programmatically.
        /// The default is the first column.</remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OLVColumn SecondarySortColumn
        {
            get {
        		if (this.secondarySortColumn == null) {
        			if (this.Columns.Count > 0)
        				return this.GetColumn(0);
        			else
        				return null;
        		} else
        			return this.secondarySortColumn;
        	}
            set {
        		this.secondarySortColumn = value;
        	}
        }
        private OLVColumn secondarySortColumn;

        /// <summary>
        /// When the SecondarySortColumn is used, in what order will it compare results?
        /// </summary>
         [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SortOrder SecondarySortOrder
        {
        	get { return this.secondarySortOrder; }
        	set { this.secondarySortOrder = value; }
        }
        private SortOrder secondarySortOrder = SortOrder.Ascending;

        /// <summary>
        /// When the listview is grouped, should the items be sorted by the primary column?
        /// If this is false, the items will be sorted by the same column as they are grouped.
        /// </summary>
        [Category("Behavior"),
         Description("When the listview is grouped, should the items be sorted by the primary column? If this is false, the items will be sorted by the same column as they are grouped."),
         DefaultValue(true)]
        public bool SortGroupItemsByPrimaryColumn
        {
        	get { return this.sortGroupItemsByPrimaryColumn; }
        	set { this.sortGroupItemsByPrimaryColumn = value; }
        }
        private bool sortGroupItemsByPrimaryColumn = true;

        /// <summary>
        /// How does a user indicate that they want to edit cells?
        /// </summary>
        public enum CellEditActivateMode
        {
            /// <summary>
            /// This list cannot be edited. F2 does nothing.
            /// </summary>
            None = 0,

            /// <summary>
            /// A single click on  a <strong>subitem</strong> will edit the value. Single clicking the primary column,
            /// selects the row just like normal. The user must press F2 to edit the primary column.
            /// </summary>
            SingleClick = 1,

            /// <summary>
            /// Double clicking a subitem or the primary column will edit that cell.
            /// F2 will edit the primary column.
            /// </summary>
            DoubleClick = 2,

            /// <summary>
            /// Pressing F2 is the only way to edit the cells. Once the primary column is being edited,
            /// the other cells in the row can be edited by pressing Tab.
            /// </summary>
            F2Only = 3
        }

        /// <summary>
        /// How does the user indicate that they want to edit a cell?
        /// None means that the listview cannot be edited.
        /// </summary>
        /// <remarks>Columns can also be marked as editable.</remarks>
        [Category("Behavior"),
        Description("How does the user indicate that they want to edit a cell?"),
        DefaultValue(CellEditActivateMode.None)]
        public CellEditActivateMode CellEditActivation
        {
            get { return cellEditActivation; }
            set { cellEditActivation = value; }
        }
        private CellEditActivateMode cellEditActivation = CellEditActivateMode.None;

        /// <summary>
        /// Return true if a cell edit operation is currently happening
        /// </summary>
        [Browsable(false)]
        public bool IsCellEditing
        {
            get { return this.cellEditor != null; }
        }

        /// <summary>
        /// When the user right clicks on the column headers, should a menu be presented which will allow
        /// them to choose which columns will be shown in the view?
        /// </summary>
        [Category("Behavior"),
        Description("When the user right clicks on the column headers, should a menu be presented which will allow them to choose which columns will be shown in the view?"),
        DefaultValue(true)]
        public bool SelectColumnsOnRightClick
        {
            get { return selectColumnsOnRightClick; }
            set { selectColumnsOnRightClick = value; }
        }
        private bool selectColumnsOnRightClick = true;

        /// <summary>
        /// When the column select menu is open, should it stay open after an item is selected?
        /// Staying open allows the user to turn more than one column on or off at a time.
        /// </summary>
        [Category("Behavior"),
        Description("When the column select menu is open, should it stay open after an item is selected?"),
        DefaultValue(true)]
        public bool SelectColumnsMenuStaysOpen
        {
            get { return selectColumnsMenuStaysOpen; }
            set { selectColumnsMenuStaysOpen = value; }
        }
        private bool selectColumnsMenuStaysOpen = true;

        /// <summary>
        /// When resizing a column by dragging its divider, should any space filling columns be
        /// resized at each mouse move? If this is false, the filling columns will be
        /// updated when the mouse is released.
        /// </summary>
        /// <remarks>
        /// I think that this looks very ugly, but it does give more immediate feedback. It looks ugly because every
        /// column the divider being dragged gets updated twice: once when the column be resized changes size (this moves
        /// all the columns slightly to the right); then again when the filling columns are updated, but they will be shrunk
        /// so that the combined width is not more than the control, so everything jumps slightly back to the left again.
        /// </remarks>
        [Category("Misc"),
        Description("When resizing a column by dragging its divider, should any space filling columns be resized at each mouse move?"),
        DefaultValue(false)]
        public bool UpdateSpaceFillingColumnsWhenDraggingColumnDivider
        {
            get { return updateSpaceFillingColumnsWhenDraggingColumnDivider; }
            set { updateSpaceFillingColumnsWhenDraggingColumnDivider = value; }
        }
        private bool updateSpaceFillingColumnsWhenDraggingColumnDivider = false;

        #endregion

        #region Callbacks

        /// <summary>
        /// This delegate can be used to sort the table in a custom fasion.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SortDelegate CustomSorter
        {
            get { return customSorter; }
            set { customSorter = value; }
        }

        /// <summary>
        /// This delegate can be used to format a OLVListItem before it is added to the control.
        /// </summary>
        /// <remarks>
        /// <para>The model object for the row can be found through the RowObject property of the OLVListItem object.</para>
        /// <para>All subitems normally have the same style as list item, so setting the forecolor on one
        /// subitem changes the forecolor of all subitems.
        /// To allow subitems to have different attributes, do this:<code>myListViewItem.UseItemStyleForSubItems = false;</code>.
        /// </para>
        /// <para>If UseAlternatingBackColors is true, the backcolor of the listitem will be calculated
        /// by the control and cannot be controlled by the RowFormatter delegate. In general, trying to use a RowFormatter
        /// when UseAlternatingBackColors is true does not work well.</para></remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RowFormatterDelegate RowFormatter
        {
            get { return rowFormatter; }
            set { rowFormatter = value; }
        }
        private RowFormatterDelegate rowFormatter;

        /// <summary>
        /// This delegate will be called whenever the ObjectListView needs to know the check state
        /// of the row associated with a given model object
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckStateGetterDelegate CheckStateGetter
        {
            get { return checkStateGetter; }
            set { checkStateGetter = value; }
        }
        private CheckStateGetterDelegate checkStateGetter;

        /// <summary>
        /// This delegate will be called whenever the user tries to change the check state
        /// of a row. The delegate should return the value that the listview should actuall
        /// use, which may be different to the one given to the delegate.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CheckStatePutterDelegate CheckStatePutter
        {
            get { return checkStatePutter; }
            set { checkStatePutter = value; }
        }
        private CheckStatePutterDelegate checkStatePutter;

		#endregion

        #region List commands

        /// <summary>
        /// Allow the SetObjects method to be called in a thread safe manner
        /// </summary>
        /// <param name="collection"></param>
        protected delegate void SetObjectsInvoker(IEnumerable collection);

		/// <summary>
		/// Set the collection of objects that will be shown in this list view.
		/// </summary>
		/// <remarks>The list is updated immediately</remarks>
		/// <param name="collection">The objects to be displayed</param>
		virtual public void SetObjects (IEnumerable collection)
		{
			if (this.InvokeRequired) {
                //this.Invoke(new SetObjectsInvoker(this.SetObjects), new object[] { collection });
                this.Invoke((MethodInvoker)delegate { this.SetObjects(collection); }); // this seems neater
				return;
			}
			this.objects = collection;
			this.BuildList(false);
        }

        /// <summary>
        /// Update the list to reflect the contents of the given collection, without affecting
        /// the scrolling position, selection or sort order.
        /// </summary>
        /// <param name="collection">The objects to be displayed</param>
        /// <remarks>
        /// <para>This method is about twice as slow as SetObjects().</para>
        /// <para>This method is experimental -- it may disappear in later versions of the code.</para>
        /// <para>There has to be a better way to do this! JPP 15/1/2008</para>
        /// <para>In most situations, if you need this functionality, use a FastObjectListView instead. JPP 2/2/2008</para>
        /// </remarks>
        [Obsolete("Use a FastObjectListView instead of this method.", false)]
        virtual public void IncrementalUpdate(IEnumerable collection)
        {
            if (this.InvokeRequired) {
                this.Invoke(new SetObjectsInvoker(this.IncrementalUpdate), new object[] { collection });
                return;
            }

            this.BeginUpdate();

            this.ListViewItemSorter = null;
            ArrayList previousSelection = this.SelectedObjects;

            // Replace existing rows, creating new listviewitems if we get to the end of the list
            List<OLVListItem> newItems = new List<OLVListItem>();
            int rowIndex = 0;
            int itemCount = this.Items.Count;
            foreach (object model in collection) {
                if (rowIndex < itemCount) {
                    OLVListItem lvi = this.GetItem(rowIndex);
                    lvi.RowObject = model;
                    this.RefreshItem(lvi);
                } else {
                    OLVListItem lvi = new OLVListItem(model);
                    this.FillInValues(lvi, model);
                    newItems.Add(lvi);
                }
                rowIndex++;
            }

            // Delete any excess rows
            int numRowsToDelete = itemCount - rowIndex;
            for (int i = 0; i < numRowsToDelete; i++)
                this.Items.RemoveAt(rowIndex);

            this.Items.AddRange(newItems.ToArray());
            this.Sort(this.lastSortColumn);

            SetAllSubItemImages();

            this.SelectedObjects = previousSelection;

            this.EndUpdate();

            this.objects = collection;
        }

        /// <summary>
        /// Remove all items from this list
        /// </summary>
        virtual public void ClearObjects()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(ClearObjects));
            else
                this.Items.Clear();
        }

        /// <summary>
        /// Build/rebuild all the list view items in the list
        /// </summary>
        virtual public void BuildList()
        {
            this.BuildList(true);
        }

        /// <summary>
        /// Build/rebuild all the list view items in the list
        /// </summary>
        /// <param name="shouldPreserveSelection">If this is true, the control will try to preserve the selection,
        /// i.e. all objects that were selected before the call will be selected after the rebuild</param>
        /// <remarks>Use this method in situations were the contents of the list is basically the same
        /// as previously.</remarks>
		virtual public void BuildList(bool shouldPreserveSelection)
        {
            if (this.Frozen)
                return;

            ArrayList previousSelection = new ArrayList();
            if (shouldPreserveSelection && this.objects != null) {
                previousSelection = this.SelectedObjects;
            }

			this.BeginUpdate();
			this.Items.Clear();
			this.ListViewItemSorter = null;

            if (this.objects != null)
            {
                // Build a list of all our items and then display them. (Building
                // a list and then doing one AddRange is about 10-15% faster than individual adds)
                List<OLVListItem> l = new List<OLVListItem>();
                foreach (object rowObject in this.objects)
                {
                    OLVListItem lvi = new OLVListItem(rowObject);
                    this.FillInValues(lvi, rowObject);
                    l.Add(lvi);
                }
                this.Items.AddRange(l.ToArray());
                this.SetAllSubItemImages();
                this.Sort(this.lastSortColumn);

                if (shouldPreserveSelection) {
                    this.SelectedObjects = previousSelection;
                }
            }

			this.EndUpdate();
        }

        /// <summary>
        /// Sort the items by the last sort column
        /// </summary>
        new public void Sort()
        {
            this.Sort(this.lastSortColumn);
        }

        /// <summary>
        /// Organise the view items into groups, based on the last sort column or the first column
        /// if there is no last sort column
        /// </summary>
        public void BuildGroups()
        {
            this.BuildGroups(this.lastSortColumn);
        }

        /// <summary>
        /// Organise the view items into groups, based on the given column
        /// </summary>
        /// <param name="column">The column whose values should be used for sorting.</param>
        virtual public void BuildGroups(OLVColumn column)
        {
            if (column == null)
                column = this.GetColumn(0);

            this.Groups.Clear();

            // Getting the Count forces any internal cache of the ListView to be flushed. Without
            // this, iterating over the Items will not work correctly if the ListView handle
            // has not yet been created.
            int dummy = this.Items.Count;

            // Separate the list view items into groups, using the group key as the descrimanent
            Dictionary<object, List<OLVListItem>> map = new Dictionary<object, List<OLVListItem>>();
            foreach (OLVListItem olvi in this.Items) {
                object key = column.GetGroupKey(olvi.RowObject);
                if (key == null)
                    key = "{null}"; // null can't be used as the key for a dictionary
                if (!map.ContainsKey(key))
                    map[key] = new List<OLVListItem>();
                map[key].Add(olvi);
            }

            // Make a list of the required groups
            List<ListViewGroup> groups = new List<ListViewGroup>();
            foreach (object key in map.Keys) {
                ListViewGroup lvg = new ListViewGroup(column.ConvertGroupKeyToTitle(key));
                lvg.Tag = key;
                groups.Add(lvg);
            }

            // Sort the groups
            groups.Sort(new ListViewGroupComparer(this.lastSortOrder));

            // Put each group into the list view, and give each group its member items.
            // The order of statements is important here:
            // - the header must be calculate before the group is added to the list view,
            //   otherwise changing the header causes a nasty redraw (even in the middle of a BeginUpdate...EndUpdate pair)
            // - the group must be added before it is given items, otherwise an exception is thrown (is this documented?)
            string fmt = column.GroupWithItemCountFormatOrDefault;
            string singularFmt = column.GroupWithItemCountSingularFormatOrDefault;
            ColumnComparer itemSorter = new ColumnComparer((this.SortGroupItemsByPrimaryColumn ? this.GetColumn(0) : column),
                                                           this.lastSortOrder, this.SecondarySortColumn, this.SecondarySortOrder);
            foreach (ListViewGroup group in groups) {
                if (this.ShowItemCountOnGroups) {
                    int count = map[group.Tag].Count;
                    group.Header = String.Format((count == 1 ? singularFmt : fmt), group.Header, count);
                }
                this.Groups.Add(group);
                // If there is no sort order, don't sort since the sort isn't stable
                if (this.lastSortOrder != SortOrder.None)
                    map[group.Tag].Sort(itemSorter);
                group.Items.AddRange(map[group.Tag].ToArray());
            }
        }

        /// <summary>
        /// Pause (or unpause) all animations in the list
        /// </summary>
        /// <param name="isPause">true to pause, false to unpause</param>
        public void PauseAnimations(bool isPause)
        {
            for (int i = 0; i < this.Columns.Count; i++) {
                OLVColumn col = this.GetColumn(i);
                if (col.Renderer is ImageRenderer)
                    ((ImageRenderer)col.Renderer).Paused = isPause;
            }
        }

        /// <summary>
        /// Give the listview a reasonable size of its tiles, based on the number of lines of
        /// information that each tile is going to display.
        /// </summary>
        public void CalculateReasonableTileSize()
        {
            if (this.Columns.Count <= 0)
                return;

            int imageHeight = (this.LargeImageList == null ? 16 : this.LargeImageList.ImageSize.Height);
            int dataHeight = (this.Font.Height + 1) * this.Columns.Count;
            int tileWidth = (this.TileSize.Width == 0 ? 200 : this.TileSize.Width);
            int tileHeight = Math.Max(this.TileSize.Height, Math.Max(imageHeight, dataHeight));
            this.TileSize = new Size(tileWidth, tileHeight);
        }

        /// <summary>
        /// Rebuild this list for the given view
        /// </summary>
        /// <param name="view"></param>
        internal void ChangeToFilteredColumns(View view)
        {
            this.Freeze();
            this.Clear();
            List<OLVColumn> cols = this.GetFilteredColumns(view);
            this.Columns.AddRange(cols.ToArray());
            if (view == View.Details) {
                foreach (OLVColumn x in cols) {
                    if (x.LastDisplayIndex == -1 || x.LastDisplayIndex > cols.Count - 1)
                        x.DisplayIndex = cols.Count - 1;
                    else
                        x.DisplayIndex = x.LastDisplayIndex;
                }
                this.ShowSortIndicator();
            }
            this.BuildList();
            this.Unfreeze();
        }

        /// <summary>
        /// Rebuild the columns based upon its current view and column visibility settings
        /// </summary>
        public void RebuildColumns()
        {
            this.ChangeToFilteredColumns(this.View);
        }

        /// <summary>
        /// Copy a text and html representation of the selected rows onto the clipboard.
        /// </summary>
        /// <remarks>Be careful when using this with virtual lists. If the user has selected
        /// 10,000,000 rows, this method will faithfully try to copy all of them to the clipboard.
        /// From the user's point of view, your program will appear to have hung.</remarks>
        public void CopySelectionToClipboard()
        {
            if (this.SelectedIndices.Count == 0)
                return;

            //THINK: Do we want to include something like this?
            //if (this.SelectedIndices.Count > 10000)
            //    return;

            // Build text and html versions of the selection
            StringBuilder sbText = new StringBuilder();
            StringBuilder sbHtml = new StringBuilder("<table>");

            foreach (int i in this.SelectedIndices) {
                OLVListItem row = this.GetItem(i);
                sbHtml.Append("<tr><td>");
                for (int j = 0; j < row.SubItems.Count; j++) {
                    if (j > 0) {
                        sbText.Append("\t");
                        sbHtml.Append("</td><td>");
                    }
                    sbText.Append(row.SubItems[j].Text);
                    sbHtml.Append(row.SubItems[j].Text);
                }
                sbText.AppendLine();
                sbHtml.AppendLine("</td></tr>");
            }
            sbHtml.AppendLine("</table>");

            // Put both the text and html versions onto the clipboard
            DataObject dataObject = new DataObject();
            dataObject.SetText(sbText.ToString(), TextDataFormat.UnicodeText);
            dataObject.SetText(ConvertToHtmlFragment(sbHtml.ToString()), TextDataFormat.Html);
            Clipboard.SetDataObject(dataObject);
        }

        /// <summary>
        /// Convert the fragment of HTML into the Clipboards HTML format.
        /// </summary>
        /// <remarks>The HTML format is found here http://msdn2.microsoft.com/en-us/library/aa767917.aspx
        /// </remarks>
        /// <param name="fragment">The HTML to put onto the clipboard. It must be valid HTML!</param>
        /// <returns>A string that can be put onto the clipboard and will be recognized as HTML</returns>
        private string ConvertToHtmlFragment(string fragment)
        {
            // Minimal implementation of HTML clipboard format
            string source = "http://www.codeproject.com/KB/list/ObjectListView.aspx";

            const String MARKER_BLOCK =
                "Version:1.0\r\n" +
                "StartHTML:{0,8}\r\n" +
                "EndHTML:{1,8}\r\n" +
                "StartFragment:{2,8}\r\n" +
                "EndFragment:{3,8}\r\n" +
                "StartSelection:{2,8}\r\n" +
                "EndSelection:{3,8}\r\n" +
                "SourceURL:{4}\r\n" +
                "{5}";

            int prefixLength = String.Format(MARKER_BLOCK, 0, 0, 0, 0, source, "").Length;

            const String DEFAULT_HTML_BODY =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML><HEAD></HEAD><BODY><!--StartFragment-->{0}<!--EndFragment--></BODY></HTML>";

            string html = String.Format(DEFAULT_HTML_BODY, fragment);
            int startFragment = prefixLength + html.IndexOf(fragment);
            int endFragment = startFragment + fragment.Length;

            return String.Format(MARKER_BLOCK, prefixLength, prefixLength + html.Length, startFragment, endFragment, source, html);
        }

        /// <summary>
        /// Return a string that represents the current state of the ObjectListView, such
        /// that the state can be restored by RestoreState()
        /// </summary>
        /// <remarks>
        /// <para>The state of an ObjectListView includes the attributes that the user can modify:
        /// <list>
        /// <item>current view (i.e. Details, Tile, Large Icon...)</item>
        /// <item>sort column and direction</item>
        /// <item>column order</item>
        /// <item>column widths</item>
        /// <item>column visibility</item>
        /// </list>
        /// </para>
        /// <para>
        /// It does not include selection or the scroll position.
        /// </para>
        /// </remarks>
        /// <returns>A string representing the state of the ObjectListView</returns>
        public byte[] SaveState()
        {
            ObjectListViewState olvState = new ObjectListViewState();
            olvState.VersionNumber = 1;
            olvState.NumberOfColumns = this.AllColumns.Count;
            olvState.CurrentView = this.View;

            // If we have a sort column, it is possible that it is not currently being shown, in which
            // case, it's Index will be -1. So we calculate its index directly. Technically, the sort
            // column does not even have to a member of AllColumns, in which case IndexOf will return -1,
            // which is works fine since we have no way of restoring such a column anyway.
            if (this.lastSortColumn != null)
                olvState.SortColumn = this.AllColumns.IndexOf(this.lastSortColumn);
            olvState.LastSortOrder = this.lastSortOrder;
            olvState.IsShowingGroups = this.ShowGroups;

            if (this.AllColumns.Count > 0 && this.AllColumns[0].LastDisplayIndex == -1)
                this.RememberDisplayIndicies();

            foreach (OLVColumn column in this.AllColumns) {
                olvState.ColumnIsVisible.Add(column.IsVisible);
                olvState.ColumnDisplayIndicies.Add(column.LastDisplayIndex);
                olvState.ColumnWidths.Add(column.Width);
            }

            // Now that we have stored our state, convert it to a string
            MemoryStream ms = new MemoryStream();
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(ms, olvState);

            //return Encoding.ASCII.GetString(ms.ToArray());
            return ms.ToArray();
        }

        /// <summary>
        /// Restore the state of the control from the given string, which must have been
        /// produced by SaveState()
        /// </summary>
        /// <param name="state">A string returned from SaveState()</param>
        /// <returns>Returns true if the state was restored</returns>
        public bool RestoreState(byte[] state)
        {
            //MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(state));
            MemoryStream ms = new MemoryStream(state);
            BinaryFormatter deserializer = new BinaryFormatter();
            ObjectListViewState olvState;
            try {
                olvState = deserializer.Deserialize(ms) as ObjectListViewState;
            } catch (System.Runtime.Serialization.SerializationException e) {
                return false;
            }

            // The number of columns has changed. We have no way to match old
            // columns to the new ones, so we just give up.
            if (olvState.NumberOfColumns != this.AllColumns.Count)
                return false;

            if (olvState.SortColumn == -1) {
                this.lastSortColumn = null;
                this.lastSortOrder = SortOrder.None;
            } else {
                this.lastSortColumn = this.AllColumns[olvState.SortColumn];
                this.lastSortOrder = olvState.LastSortOrder;
            }

            for (int i = 0; i < olvState.NumberOfColumns; i++) {
                OLVColumn column = this.AllColumns[i];
                column.Width = (int)olvState.ColumnWidths[i];
                column.IsVisible = (bool)olvState.ColumnIsVisible[i];
                column.LastDisplayIndex = (int)olvState.ColumnDisplayIndicies[i];
            }

            if (olvState.IsShowingGroups != this.ShowGroups)
                this.ShowGroups = olvState.IsShowingGroups;

            if (this.View == olvState.CurrentView)
                this.RebuildColumns();
            else
                this.View = olvState.CurrentView;

            return true;
        }

        /// <summary>
        /// Instances of this class are used to store the state of an ObjectListView.
        /// </summary>
        /// <remarks>
        /// This is an internal class. It is only public because XmlSerializer will only operate on public classes.
        /// </remarks>
        [Serializable]
        public class ObjectListViewState
        {
            public int VersionNumber = 1;
            public int NumberOfColumns = 1;
            public View CurrentView;
            public int SortColumn = -1;
            public bool IsShowingGroups;
            public SortOrder LastSortOrder = SortOrder.None;
            public ArrayList ColumnIsVisible = new ArrayList();
            public ArrayList ColumnDisplayIndicies = new ArrayList();
            public ArrayList ColumnWidths = new ArrayList();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for the column click event
        /// </summary>
        virtual protected void HandleColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Toggle the sorting direction on successive clicks on the same column
            if (this.lastSortColumn != null && e.Column == this.lastSortColumn.Index)
                this.lastSortOrder = (this.lastSortOrder == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending);
            else
                this.lastSortOrder = SortOrder.Ascending;

            this.BeginUpdate();
            this.Sort(e.Column);
            this.EndUpdate();
        }

        /// <summary>
        /// Handle when a user checks/unchecks a row
        /// </summary>
        protected void HandleItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.CheckStatePutter != null)
                e.NewValue = this.CheckStatePutter(this.GetModelObject(e.Index), e.NewValue);
        }

        #endregion

        #region Low level Windows message handling

        /// <summary>
        /// Override the basic message pump for this control
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg) {
                case 0x0F: // WM_PAINT
                    this.HandlePrePaint();
                    base.WndProc(ref m);
                    this.HandlePostPaint();
                    break;
                case 0x4E: // WM_NOTIFY
                    if (!this.HandleNotify(ref m))
                        base.WndProc(ref m);
                    break;
                case 0x05:  // WM_SIZE
                case 0x114: // WM_HSCROLL:
                case 0x115: // WM_VSCROLL:
                case 0x20A: // WM_MOUSEWHEEL:
                case 0x20E: // WM_MOUSEHWHEEL:
                    this.PossibleFinishCellEditing();
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion


        #region Column header clicking, column hiding and resizing

        /// <summary>
        /// When the control is created capture the messages for the header.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
#if !MONO
            hdrCtrl = new HeaderControl(this);
#endif
        }
#if !MONO
        private HeaderControl hdrCtrl = null;

        /// <summary>
        /// Class used to capture window messages for the header of the list view
        /// control.
        /// </summary>
        /// <remarks>We only need this class in order to not change the cursor
        /// when the cursor is over the divider of a fixed width column. It
        /// really is a little too perfectionist even for me.</remarks>
        private class HeaderControl : NativeWindow
        {
            private ObjectListView parentListView = null;

            public HeaderControl(ObjectListView olv)
            {
                parentListView = olv;
                this.AssignHandle(NativeMethods.GetHeaderControl(olv));
            }

            protected override void WndProc(ref Message message)
            {
                const int WM_SETCURSOR = 0x0020;

                switch (message.Msg) {
                    case WM_SETCURSOR:
                        if (IsCursorOverLockedDivider()) {
                            message.Result = (IntPtr)1;	// Don't change the cursor
                            return;
                        }
                        break;
                }

                base.WndProc(ref message);
            }

            private bool IsCursorOverLockedDivider()
            {
                Point pt = this.parentListView.PointToClient(Cursor.Position);
                pt.X += NativeMethods.GetScrollPosition(this.parentListView.Handle, true);
                int dividerIndex = NativeMethods.GetDividerUnderPoint(this.Handle, pt);
                if (dividerIndex >= 0 && dividerIndex < this.parentListView.Columns.Count) {
                    OLVColumn column = this.parentListView.GetColumn(dividerIndex);
                    return column.IsFixedWidth || column.FillsFreeSpace;
                } else
                    return false;
            }

            /// <summary>
            /// Return the index of the column under the current cursor position,
            /// or -1 if the cursor is not over a column
            /// </summary>
            /// <returns>Index of the column under the cursor, or -1</returns>
            public int GetColumnIndexUnderCursor()
            {
                Point pt = this.parentListView.PointToClient(Cursor.Position);
                pt.X += NativeMethods.GetScrollPosition(this.parentListView.Handle, true);
                return NativeMethods.GetColumnUnderPoint(this.Handle, pt);
            }
        }
#endif

        /// <summary>
        /// In the notification messages, we handle attempts to change the width of our columns
        /// </summary>
        /// <param name="m">The msg to be processed</param>
        /// <returns>bool to indicate if the msg has been handled</returns>
        protected bool HandleNotify(ref Message m)
        {
            bool isMsgHandled = false;

            const int NM_RCLICK = -5;
            const int HDN_FIRST = (0 - 300);
            const int HDN_DIVIDERDBLCLICKA = (HDN_FIRST - 5);
            const int HDN_DIVIDERDBLCLICKW = (HDN_FIRST - 25);
            const int HDN_BEGINTRACKA = (HDN_FIRST - 6);
            const int HDN_BEGINTRACKW = (HDN_FIRST - 26);
            //const int HDN_ENDTRACKA = (HDN_FIRST - 7);
            //const int HDN_ENDTRACKW = (HDN_FIRST - 27);
            const int HDN_TRACKA = (HDN_FIRST - 8);
            const int HDN_TRACKW = (HDN_FIRST - 28);
            const int HDN_ITEMCHANGINGA = (HDN_FIRST - 0);
            const int HDN_ITEMCHANGINGW = (HDN_FIRST - 20);

            // Handle the notification, remembering to handle both ANSI and Unicode versions
            NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
            //unsafe {
            //    NativeMethods.NMHDR* lParam = (NativeMethods.NMHDR*)m.LParam;
            //    nmhdr.code = lParam->code;
            //}
            //if (nmhdr.code < HDN_FIRST)
            //  System.Diagnostics.Debug.WriteLine(nmhdr.code);

            // In KB Article #183258, MS states that when a header control has the HDS_FULLDRAG style, it will receive
            // ITEMCHANGING events rather than TRACK events. Under XP SP2 (at least) this is not always true, which may be
            // why MS has withdrawn that particular KB article. It is true that the header is always given the HDS_FULLDRAG
            // style. But even while window style set, the control doesn't always received ITEMCHANGING events.
            // The controlling setting seems to be the Explorer option "Show Window Contents While Dragging"!
            // In the category of "truly bizarre side effects", if the this option is turned on, we will receive
            // ITEMCHANGING events instead of TRACK events. But if it is turned off, we receive lots of TRACK events and
            // only one ITEMCHANGING event at the very end of the process.
            // If we receive HDN_TRACK messages, it's harder to control the resizing process. If we return a result of 1, we
            // cancel the whole drag operation, not just that particular track event, which is clearly not what we want.
            // If we are willing to compile with unsafe code enabled, we can modify the size of the column in place, using the
            // commented out code below. But without unsafe code, the best we can do is allow the user to drag the column to
            // any width, and then spring it back to within bounds once they release the mouse button. UI-wise it's very ugly.
            NativeMethods.NMHEADER nmheader;
            switch (nmhdr.code) {

                case NM_RCLICK:
                    this.PossibleFinishCellEditing();
                    int columnIndex = this.hdrCtrl.GetColumnIndexUnderCursor();
                    isMsgHandled = this.HandleHeaderRightClick(columnIndex);
                    break;

                case HDN_DIVIDERDBLCLICKA:
                case HDN_DIVIDERDBLCLICKW:
                case HDN_BEGINTRACKA:
                case HDN_BEGINTRACKW:
                    this.PossibleFinishCellEditing();
                    nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
                    if (nmheader.iItem >= 0 && nmheader.iItem < this.Columns.Count) {
                        OLVColumn column = this.GetColumn(nmheader.iItem);
                        // Space filling columns can't be dragged or double-click resized
                        if (column.FillsFreeSpace) {
                            m.Result = (IntPtr)1; // prevent the change from happening
                            isMsgHandled = true;
                        }
                    }
                    break;

                case HDN_TRACKA:
                case HDN_TRACKW:
                    nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
                    if (nmheader.iItem >= 0 && nmheader.iItem < this.Columns.Count) {
                    //    unsafe {
                    //        NativeMethods.HDITEM *hditem = (NativeMethods.HDITEM*)nmheader.pHDITEM;
                    //        OLVColumn column = this.GetColumn(nmheader.iItem);
                    //        if (hditem->cxy < column.MiniumWidth)
                    //            hditem->cxy = column.MiniumWidth;
                    //        else if (column.MaxiumWidth != -1 && hditem->cxy > column.MaxiumWidth)
                    //            hditem->cxy = column.MaxiumWidth;
                    //    }
                    }
                    break;

                case HDN_ITEMCHANGINGA:
                case HDN_ITEMCHANGINGW:
                    nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
                    if (nmheader.iItem >= 0 && nmheader.iItem < this.Columns.Count) {
                        NativeMethods.HDITEM hditem = (NativeMethods.HDITEM)Marshal.PtrToStructure(nmheader.pHDITEM, typeof(NativeMethods.HDITEM));
                        OLVColumn column = this.GetColumn(nmheader.iItem);
                        // Check the mask to see if the width field is valid, and if it is, make sure it's within range
                        if ((hditem.mask & 1) == 1) {
                            if (hditem.cxy < column.MinimumWidth ||
                                (column.MaximumWidth != -1 && hditem.cxy > column.MaximumWidth)) {
                                m.Result = (IntPtr)1; // prevent the change from happening
                                isMsgHandled = true;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return isMsgHandled;
        }

        /// <summary>
        /// The user has right clicked on the column headers. Do whatever is required
        /// </summary>
        /// <returns>Return true if this event has been handle</returns>
        virtual protected bool HandleHeaderRightClick(int columnIndex)
        {
            ColumnClickEventArgs eventArgs = new ColumnClickEventArgs(columnIndex);
            this.OnColumnRightClick(eventArgs);
            
            if (this.SelectColumnsOnRightClick)
                this.ShowColumnSelectMenu(Cursor.Position);
                
            return this.SelectColumnsOnRightClick;
        }

        /// <summary>
        /// The user has right clicked on the column headers. Do whatever is required
        /// </summary>
        /// <returns>Return true if this event has been handle</returns>
        [Obsolete("Use HandleHeaderRightClick(int) instead")]
        virtual protected bool HandleHeaderRightClick()
        {
            return false;
        }        
        
        /// <summary>
        /// Tell the world when a cell is about to finish being edited.
        /// </summary>
        protected virtual void OnColumnRightClick(ColumnClickEventArgs e)
        {
            if (this.ColumnRightClick != null)
                this.ColumnRightClick(this, e);
        }

        /// <summary>
        /// The callbacks for RightColumnClick events
        /// </summary>
        public delegate void ColumnRightClickEventHandler(object sender, ColumnClickEventArgs e);

        /// <summary>
        /// Triggered when a column header is right clicked.
        /// </summary>
        [Category("Behavior")]
        public event ColumnRightClickEventHandler ColumnRightClick;

        /// <summary>
        /// Show a popup menu at the given point which will allow the user to choose which columns
        /// are visible on this listview
        /// </summary>
        /// <param name="pt">Where should the menu be placed</param>
        protected void ShowColumnSelectMenu(Point pt)
        {
            ContextMenuStrip m = new ContextMenuStrip();

            m.ItemClicked += new ToolStripItemClickedEventHandler(ColumnSelectMenu_ItemClicked);
            m.Closing += new ToolStripDropDownClosingEventHandler(ColumnSelectMenu_Closing);

            List<OLVColumn> columns = new List<OLVColumn>(this.AllColumns);
            columns.Sort(delegate(OLVColumn x, OLVColumn y) { return String.Compare(x.Text, y.Text, true); });
            foreach (OLVColumn col in columns) {
                ToolStripMenuItem mi = new ToolStripMenuItem(col.Text);
                mi.Checked = col.IsVisible;
                mi.Tag = col;

                // The 'Index' property returns -1 when the column is not visible, so if the
                // column isn't visible we have to enable the item. Also the first column can't be turned off
                mi.Enabled = !col.IsVisible || (col.Index > 0);
                m.Items.Add(mi);
            }
            m.Show(pt);
        }

        private void ColumnSelectMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem mi = (ToolStripMenuItem)e.ClickedItem;
            OLVColumn col = (OLVColumn)mi.Tag;
            mi.Checked = !mi.Checked;
            col.IsVisible = mi.Checked;
            this.BeginInvoke(new MethodInvoker(this.RebuildColumns));
        }

        private void ColumnSelectMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            e.Cancel = (this.SelectColumnsMenuStaysOpen &&
                e.CloseReason == ToolStripDropDownCloseReason.ItemClicked);
        }

        /// <summary>
        /// Override the OnColumnReordered method to do what we want
        /// </summary>
        /// <param name="e"></param>
        protected override void OnColumnReordered(ColumnReorderedEventArgs e)
        {
            base.OnColumnReordered(e);

            // The internal logic of the .NET code behind a ENDDRAG event means that,
            // at this point, the DisplayIndex's of the columns are not yet as they are
            // going to be. So we have to invoke a method to run later that will remember
            // what the real DisplayIndex's are.
            this.BeginInvoke(new MethodInvoker(this.RememberDisplayIndicies));
        }

        private void RememberDisplayIndicies()
        {
            // Remember the display indexes so we can put them back at a later date
            foreach (OLVColumn x in this.AllColumns)
                x.LastDisplayIndex = x.DisplayIndex;
        }

        void HandleColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (this.UpdateSpaceFillingColumnsWhenDraggingColumnDivider && !this.GetColumn(e.ColumnIndex).FillsFreeSpace)
                this.ResizeFreeSpaceFillingColumns();
        }

        void HandleColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (!this.GetColumn(e.ColumnIndex).FillsFreeSpace)
                this.ResizeFreeSpaceFillingColumns();
        }

        void HandleLayout(object sender, LayoutEventArgs e)
        {
            this.ResizeFreeSpaceFillingColumns();
        }

        /// <summary>
        /// Resize our space filling columns so they fill any unoccupied width in the control
        /// </summary>
        protected void ResizeFreeSpaceFillingColumns()
        {
            // It's too confusing to dynamically resize columns at design time.
            if (this.DesignMode)
                return;

            // Calculate the free space available
            int freeSpace = this.ClientSize.Width - 2;
            int totalProportion = 0;
            List<OLVColumn> spaceFillingColumns = new List<OLVColumn>();
            for (int i = 0; i < this.Columns.Count; i++) {
                OLVColumn col = this.GetColumn(i);
                if (col.FillsFreeSpace) {
                    spaceFillingColumns.Add(col);
                    totalProportion += col.FreeSpaceProportion;
                } else
                    freeSpace -= col.Width;
            }
			freeSpace = Math.Max(0, freeSpace);

            // Any space filling column that would hit it's Minimum or Maximum
            // width must be treated as a fixed column.
            foreach (OLVColumn col in spaceFillingColumns.ToArray()) {
                int newWidth = (freeSpace * col.FreeSpaceProportion) / totalProportion;

                if (col.MinimumWidth != -1 && newWidth < col.MinimumWidth)
                    newWidth = col.MinimumWidth;
                else if (col.MaximumWidth != -1 && newWidth > col.MaximumWidth)
                    newWidth = col.MaximumWidth;
                else
                    newWidth = 0;

                if (newWidth > 0) {
                    col.Width = newWidth;
                    freeSpace -= newWidth;
                    totalProportion -= col.FreeSpaceProportion;
                    spaceFillingColumns.Remove(col);
                }
            }

            // Distribute the free space between the columns
            foreach (OLVColumn col in spaceFillingColumns) {
                col.Width = (freeSpace * col.FreeSpaceProportion) / totalProportion;
            }
        }

        #endregion

        #region OLV accessing

        /// <summary>
        /// Return the column at the given index
        /// </summary>
        /// <param name="index">Index of the column to be returned</param>
        /// <returns>An OLVColumn</returns>
        public OLVColumn GetColumn(int index)
        {
            return (OLVColumn)this.Columns[index];
        }

        /// <summary>
        /// Return the column at the given title.
        /// </summary>
        /// <param name="name">Name of the column to be returned</param>
        /// <returns>An OLVColumn</returns>
        public OLVColumn GetColumn(string name)
        {
            foreach (ColumnHeader column in this.Columns) {
                if (column.Text == name)
                    return (OLVColumn)column;
            }
            return null;
        }

        /// <summary>
        /// Return the number of items in the list
        /// </summary>
        /// <returns>the number of items in the list</returns>
        virtual public int GetItemCount()
        {
            return this.Items.Count;
        }

        /// <summary>
        /// Return the item at the given index
        /// </summary>
        /// <param name="index">Index of the item to be returned</param>
        /// <returns>An OLVListItem</returns>
        virtual public OLVListItem GetItem(int index)
        {
            return (OLVListItem)this.Items[index];
        }

        /// <summary>
        /// Return the model object at the given index
        /// </summary>
        /// <param name="index">Index of the model object to be returned</param>
        /// <returns>A model object</returns>
        virtual public object GetModelObject(int index)
        {
            return this.GetItem(index).RowObject;
        }

        /// <summary>
        /// Find the item and column that are under the given co-ords
        /// </summary>
        /// <param name="x">X co-ord</param>
        /// <param name="y">Y co-ord</param>
        /// <param name="selectedColumn">The column under the given point</param>
        /// <returns>The item under the given point. Can be null.</returns>
        public OLVListItem GetItemAt(int x, int y, out OLVColumn selectedColumn)
        {
            selectedColumn = null;
            ListViewHitTestInfo info = this.HitTest(x, y);
            if (info.Item == null)
                return null;

            if (info.SubItem != null) {
                int subItemIndex = info.Item.SubItems.IndexOf(info.SubItem);
                selectedColumn = this.GetColumn(subItemIndex);
            }

            return (OLVListItem)info.Item;
        }

        #endregion

        #region Object manipulation

        /// <summary>
        /// Select all rows in the listview
        /// </summary>
        public void SelectAll()
        {
            NativeMethods.SelectAllItems(this);
        }

        /// <summary>
        /// Deselect all rows in the listview
        /// </summary>
        public void DeselectAll()
        {
            NativeMethods.DeselectAllItems(this);
        }

        /// <summary>
        /// Return the model object of the row that is selected or null if there is no selection or more than one selection
        /// </summary>
        /// <returns>Model object or null</returns>
        virtual public object GetSelectedObject()
        {
            if (this.SelectedIndices.Count == 1)
                return this.GetModelObject(this.SelectedIndices[0]);
            else
                return null;
        }

        /// <summary>
        /// Return the model objects of the rows that are selected or an empty collection if there is no selection
        /// </summary>
        /// <returns>ArrayList</returns>
        virtual public ArrayList GetSelectedObjects()
        {
            ArrayList objects = new ArrayList(this.SelectedIndices.Count);
            foreach (int index in this.SelectedIndices)
                objects.Add(this.GetModelObject(index));

            return objects;
        }

        /// <summary>
        /// Return the model object of the row that is checked or null if no row is checked
        /// or more than one row is checked
        /// </summary>
        /// <returns>Model object or null</returns>
        virtual public object GetCheckedObject()
        {
            if (this.CheckedIndices.Count == 1)
                return this.GetModelObject(this.CheckedIndices[0]);
            else
                return null;
        }

        /// <summary>
        /// Return the model objects of the rows that are checked or an empty collection if no row is checked
        /// </summary>
        /// <returns>ArrayList</returns>
        virtual public ArrayList GetCheckedObjects()
        {
            ArrayList objects = new ArrayList(this.CheckedIndices.Count);
            foreach (int index in this.CheckedIndices)
                objects.Add(this.GetModelObject(index));

            return objects;
        }

		/// <summary>
		/// Select the row that is displaying the given model object. All other rows are deselected.
		/// </summary>
		/// <param name="modelObject">The object to be selected or null to deselect all</param>
		virtual public void SelectObject(object modelObject)
		{
            if (this.SelectedItems.Count == 1 && ((OLVListItem)this.SelectedItems[0]).RowObject == modelObject)
                return;

			this.SelectedItems.Clear();

			//TODO: If this is too slow, we could keep a map of model object to ListViewItems
			foreach (ListViewItem lvi in this.Items) {
				if (((OLVListItem)lvi).RowObject == modelObject) {
					lvi.Selected = true;
					break;
				}
			}
		}

		/// <summary>
		/// Select the rows that is displaying any of the given model object. All other rows are deselected.
		/// </summary>
		/// <param name="modelObjects">A collection of model objects</param>
		virtual public void SelectObjects(IList modelObjects)
		{
			this.SelectedItems.Clear();

			//TODO: If this is too slow, we could keep a map of model object to ListViewItems
			foreach (ListViewItem lvi in this.Items) {
				if (modelObjects.Contains(((OLVListItem)lvi).RowObject))
					lvi.Selected = true;
			}
		}

		/// <summary>
		/// Update the ListViewItem with the data from its associated model.
		/// </summary>
		/// <remarks>This method does not resort or regroup the view. It simply updates
		/// the displayed data of the given item</remarks>
		virtual public void RefreshItem(OLVListItem olvi)
		{
			// For some reason, clearing the subitems also wipes out the back color,
			// so we need to store it and then put it back again later
			Color c = olvi.BackColor;
			olvi.SubItems.Clear();
			this.FillInValues(olvi, olvi.RowObject);
			this.SetSubItemImages(olvi.Index, olvi, true);
			olvi.BackColor = c;
		}

		/// <summary>
		/// Update the rows that are showing the given objects
		/// </summary>
		/// <remarks>This method does not resort or regroup the view.</remarks>
		virtual public void RefreshObject(object modelObject)
		{
			this.RefreshObjects(new object[] {modelObject});
		}

        private delegate void RefreshObjectsInvoker(IList modelObjects);

		/// <summary>
		/// Update the rows that are showing the given objects
		/// </summary>
  	    /// <remarks>This method does not resort or regroup the view.</remarks>
		virtual public void RefreshObjects(IList modelObjects)
		{
			if (this.InvokeRequired) {
                this.Invoke(new RefreshObjectsInvoker(this.RefreshObjects), new object[] { modelObjects });
				return;
			}
			foreach (ListViewItem lvi in this.Items) {
				OLVListItem olvi = (OLVListItem)lvi;
				if (modelObjects.Contains(olvi.RowObject))
					this.RefreshItem(olvi);
			}
		}

		/// <summary>
		/// Update the rows that are selected
		/// </summary>
  	    /// <remarks>This method does not resort or regroup the view.</remarks>
		public void RefreshSelectedObjects()
		{
			foreach (ListViewItem lvi in this.SelectedItems)
                this.RefreshItem((OLVListItem)lvi);
		}

        /// <summary>
        /// Find the given model object within the listview and return its index
        /// </summary>
        /// <remarks>Technically, this method will work with virtual lists, but it will
        /// probably be very slow.</remarks>
        /// <param name="modelObject">The model object to be found</param>
        /// <returns>The index of the object. -1 means the object was not present</returns>
        public int IndexOf(Object modelObject)
        {
            for (int i = 0; i < this.GetItemCount(); i++) {
                if (this.GetModelObject(i) == modelObject)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Return the ListViewItem that appears immediately after the given item.
        /// If the given item is null, the first item in the list will be returned.
        /// Return null if the given item is the last item.
        /// </summary>
        /// <param name="itemToFind">The item that is before the item that is returned, or null</param>
        /// <returns>A ListViewItem</returns>
        public ListViewItem GetNextItem(ListViewItem itemToFind)
        {
            if (this.ShowGroups) {
                bool isFound = (itemToFind == null);
                foreach (ListViewGroup group in this.Groups) {
                    foreach (ListViewItem lvi in group.Items) {
                        if (isFound)
                            return lvi;
                        isFound = (lvi == itemToFind);
                    }
                }
                return null;
            } else {
                if (this.GetItemCount() == 0)
                    return null;
                if (itemToFind == null)
                    return this.GetItem(0);
                if (itemToFind.Index == this.GetItemCount() - 1)
                    return null;
                return this.GetItem(itemToFind.Index + 1);
            }
        }

        /// <summary>
        /// Return the ListViewItem that appears immediately before the given item.
        /// If the given item is null, the last item in the list will be returned.
        /// Return null if the given item is the first item.
        /// </summary>
        /// <param name="itemToFind">The item that is before the item that is returned</param>
        /// <returns>A ListViewItem</returns>
        public ListViewItem GetPreviousItem(ListViewItem itemToFind)
        {
            if (this.ShowGroups) {
                ListViewItem previousItem = null;
                foreach (ListViewGroup group in this.Groups) {
                    foreach (ListViewItem lvi in group.Items) {
                        if (lvi == itemToFind)
                            return previousItem;
                        else
                            previousItem = lvi;
                    }
                }
                if (itemToFind == null)
                    return previousItem;
                else
                    return null;
            } else {
                if (this.GetItemCount() == 0)
                    return null;
                if (itemToFind == null)
                    return this.GetItem(this.GetItemCount() - 1);
                if (itemToFind.Index == 0)
                    return null;
                return this.GetItem(itemToFind.Index - 1);
            }
        }

        #endregion

        #region Freezing

        /// <summary>
        /// Freeze the listview so that it no longer updates itself.
        /// </summary>
        /// <remarks>Freeze()/Unfreeze() calls nest correctly</remarks>
        public void Freeze()
        {
            freezeCount++;
        }

        /// <summary>
        /// Unfreeze the listview. If this call is the outermost Unfreeze(),
        /// the contents of the listview will be rebuilt.
        /// </summary>
        /// <remarks>Freeze()/Unfreeze() calls nest correctly</remarks>
        public void Unfreeze()
        {
            if (freezeCount <= 0)
                return;

            freezeCount--;
            if (freezeCount == 0)
                DoUnfreeze();
        }

        /// <summary>
        /// Do the actual work required when the listview is unfrozen
        /// </summary>
        virtual protected void DoUnfreeze()
        {
            BuildList();
        }

		#endregion

		#region Column Sorting

        /// <summary>
        /// Sort the items in the list view by the values in the given column.
        /// If ShowGroups is true, the rows will be grouped by the given column,
        /// otherwise, it will be a straight sort.
        /// </summary>
        /// <param name="columnToSortName">The name of the column whose values will be used for the sorting</param>
        public void Sort(string columnToSortName)
        {
			this.Sort(this.GetColumn(columnToSortName));
        }

        /// <summary>
        /// Sort the items in the list view by the values in the given column.
        /// If ShowGroups is true, the rows will be grouped by the given column,
        /// otherwise, it will be a straight sort.
        /// </summary>
        /// <param name="columnToSortIndex">The index of the column whose values will be used for the sorting</param>
        public void Sort(int columnToSortIndex)
        {
            if (columnToSortIndex >= 0 && columnToSortIndex < this.Columns.Count)
            	this.Sort(this.GetColumn(columnToSortIndex));
        }

        /// <summary>
        /// Allow the Sort method to be called in a thread safe manner
        /// </summary>
        /// <param name="columnToSort">The column whose values will be used for the sorting</param>
        protected delegate void SortInvoker(OLVColumn columnToSort);

        /// <summary>
		/// Sort the items in the list view by the values in the given column.
		/// If ShowGroups is true, the rows will be grouped by the given column,
		/// otherwise, it will be a straight sort.
		/// </summary>
		/// <param name="columnToSort">The column whose values will be used for the sorting</param>
        virtual public void Sort(OLVColumn columnToSort)
		{
            if (this.InvokeRequired) {
				this.Invoke(new SortInvoker(this.Sort), new object [] {columnToSort});
				return;
			}

            if (this.Columns.Count < 1)
                return;

            if (columnToSort == null)
            	columnToSort = this.GetColumn(0);

			if (lastSortOrder == SortOrder.None)
				lastSortOrder = this.Sorting;

            if (this.ShowGroups)
                this.BuildGroups(columnToSort);
            else if (this.CustomSorter != null)
                this.CustomSorter(columnToSort, lastSortOrder);
			else
                this.ListViewItemSorter = new ColumnComparer(columnToSort, lastSortOrder, this.SecondarySortColumn, this.SecondarySortOrder);

            if (this.ShowSortIndicators)
    			this.ShowSortIndicator(columnToSort, lastSortOrder);

			if (this.UseAlternatingBackColors && this.View == View.Details)
				PrepareAlternateBackColors();

            this.lastSortColumn = columnToSort;
		}


        /// <summary>
        /// Put a sort indicator next to the text of the sort column
        /// </summary>
        public void ShowSortIndicator()
        {
            if (this.ShowSortIndicators && this.lastSortOrder != SortOrder.None)
                this.ShowSortIndicator(this.lastSortColumn, this.lastSortOrder);
        }


		/// <summary>
		/// Put a sort indicator next to the text of the given given column
		/// </summary>
		/// <param name="columnToSort">The column to be marked</param>
		/// <param name="sortOrder">The sort order in effect on that column</param>
		protected void ShowSortIndicator(OLVColumn columnToSort, SortOrder sortOrder)
		{
            int imageIndex = -1;

            if (!NativeMethods.HasBuiltinSortIndicators()) {
                // If we can't use builtin image, we have to make and then locate the index of the
                // sort indicator we want to use. SortOrder.None doesn't show an image.
                if (this.SmallImageList == null || !this.SmallImageList.Images.ContainsKey(SORT_INDICATOR_UP_KEY))
                    MakeSortIndicatorImages();

                if (sortOrder == SortOrder.Ascending)
                    imageIndex = this.SmallImageList.Images.IndexOfKey(SORT_INDICATOR_UP_KEY);
                else if (sortOrder == SortOrder.Descending)
                    imageIndex = this.SmallImageList.Images.IndexOfKey(SORT_INDICATOR_DOWN_KEY);
            }

            // Set the image for each column
            for (int i = 0; i < this.Columns.Count; i++) {
                if (i == columnToSort.Index)
                    NativeMethods.SetColumnImage(this, i, sortOrder, imageIndex);
                else
                    NativeMethods.SetColumnImage(this, i, SortOrder.None, -1);
            }
		}

		private const string SORT_INDICATOR_UP_KEY = "sort-indicator-up";
		private const string SORT_INDICATOR_DOWN_KEY = "sort-indicator-down";

		/// <summary>
		/// If the sort indicator images don't already exist, this method will make and install them
		/// </summary>
		protected void MakeSortIndicatorImages()
		{
			ImageList il = this.SmallImageList;
            if (il == null) {
                il = new ImageList();
                il.ImageSize = new Size(16, 16);
            }

			// This arrangement of points works well with (16,16) images, and OK with others
			int midX = il.ImageSize.Width / 2;
			int midY = (il.ImageSize.Height / 2) - 1;
			int deltaX = midX - 2;
			int deltaY = deltaX / 2;

			if (il.Images.IndexOfKey(SORT_INDICATOR_UP_KEY) == -1) {
				Point pt1 = new Point(midX - deltaX, midY + deltaY);
				Point pt2 = new Point(midX,          midY - deltaY - 1);
				Point pt3 = new Point(midX + deltaX, midY + deltaY);
                il.Images.Add(SORT_INDICATOR_UP_KEY, this.MakeTriangleBitmap(il.ImageSize, new Point[] { pt1, pt2, pt3 }));
            }

			if (il.Images.IndexOfKey(SORT_INDICATOR_DOWN_KEY) == -1) {
				Point pt1 = new Point(midX - deltaX, midY - deltaY);
				Point pt2 = new Point(midX,          midY + deltaY);
				Point pt3 = new Point(midX + deltaX, midY - deltaY);
				il.Images.Add(SORT_INDICATOR_DOWN_KEY, this.MakeTriangleBitmap(il.ImageSize, new Point[] { pt1, pt2, pt3 }));
			}

            this.SmallImageList = il;
		}

        private Bitmap MakeTriangleBitmap(Size sz, Point[] pts)
        {
            Bitmap bm = new Bitmap(sz.Width, sz.Height);
            Graphics g = Graphics.FromImage(bm);
            g.FillPolygon(new SolidBrush(Color.Gray), pts);
            return bm;
        }

        #endregion

        #region Utilities

        /// <summary>
		/// Fill in the given OLVListItem with values of the given row
		/// </summary>
        /// <param name="lvi">the OLVListItem that is to be stuff with values</param>
		/// <param name="rowObject">the model object from which values will be taken</param>
		protected void FillInValues(OLVListItem lvi, object rowObject)
		{
			if (this.Columns.Count == 0)
				return;

			OLVColumn column = this.GetColumn(0);
			lvi.Text = column.GetStringValue(rowObject);
            lvi.ImageSelector = column.GetImage(rowObject);

            for (int i = 1; i < this.Columns.Count; i++) {
                column = this.GetColumn(i);
                lvi.SubItems.Add(new OLVListSubItem(column.GetStringValue(rowObject),
                                                    column.GetImage(rowObject)));
            }

            // Give the row formatter a chance to mess with the item
            if (this.RowFormatter != null)
                this.RowFormatter(lvi);

            this.CorrectSubItemColors(lvi);

            // Get the check state of the row, if we are showing check boxes
            if (this.CheckBoxes && this.CheckStateGetter != null)
                lvi.Checked = this.CheckStateGetter(lvi.RowObject);
        }

		/// <summary>
		/// Setup all subitem images on all rows
		/// </summary>
		protected void SetAllSubItemImages()
		{
			if (!this.ShowImagesOnSubItems)
				return;

			this.ForceSubItemImagesExStyle();

            for (int rowIndex = 0; rowIndex < this.GetItemCount(); rowIndex++)
				SetSubItemImages(rowIndex, this.GetItem(rowIndex));
		}

        /// <summary>
        /// Tell the underlying list control which images to show against the subitems
        /// </summary>
        /// <param name="rowIndex">the index at which the item occurs</param>
        /// <param name="item">the item whose subitems are to be set</param>
        protected void SetSubItemImages(int rowIndex, OLVListItem item)
        {
            this.SetSubItemImages(rowIndex, item, false);
        }

		/// <summary>
		/// Tell the underlying list control which images to show against the subitems
		/// </summary>
		/// <param name="rowIndex">the index at which the item occurs</param>
		/// <param name="item">the item whose subitems are to be set</param>
		/// <param name="shouldClearImages">will existing images be cleared if no new image is provided?</param>
		protected void SetSubItemImages(int rowIndex, OLVListItem item, bool shouldClearImages)
		{
            if (!this.ShowImagesOnSubItems)
				return;

			for (int i=1; i<item.SubItems.Count; i++)
			{
                int imageIndex = this.GetActualImageIndex(((OLVListSubItem)item.SubItems[i]).ImageSelector);
                if (shouldClearImages || imageIndex != -1)
					this.SetSubItemImage(rowIndex, i, imageIndex);
			}
		}

        /// <summary>
		/// Prepare the listview to show alternate row backcolors
		/// </summary>
        /// <remarks>We cannot rely on lvi.Index in this method.
		/// In a straight list, lvi.Index is the display index, and can be used to determine
		/// whether the row should be colored. But when organised by groups, lvi.Index is not
		/// useable because it still refers to the position in the overall list, not the display order.
        ///</remarks>
		virtual protected void PrepareAlternateBackColors ()
		{
            Color rowBackColor = this.AlternateRowBackColorOrDefault;
            int i = 0;

			if (this.ShowGroups) {
            	foreach (ListViewGroup group in this.Groups) {
		            foreach (ListViewItem lvi in group.Items) {
		                if (i % 2 == 0)
		                    lvi.BackColor = this.BackColor;
		                else
		                    lvi.BackColor = rowBackColor;
                        CorrectSubItemColors(lvi);
		                i++;
		            }
            	}
            } else {
	            foreach (ListViewItem lvi in this.Items) {
	                if (i % 2 == 0)
	                    lvi.BackColor = this.BackColor;
	                else
	                    lvi.BackColor = rowBackColor;
                    CorrectSubItemColors(lvi);
                    i++;
	            }
            }
     	}

        /// <summary>
        /// For some reason, UseItemStyleForSubItems doesn't work for the colors
        /// when owner drawing the list, so we have to specifically give each subitem
        /// the desired colors
        /// </summary>
        /// <param name="olvi">The item whose subitems are to be corrected</param>
        protected void CorrectSubItemColors(ListViewItem olvi)
        {
            if (this.OwnerDraw && olvi.UseItemStyleForSubItems)
                foreach (ListViewItem.ListViewSubItem si in olvi.SubItems) {
                    si.BackColor = olvi.BackColor;
                    si.ForeColor = olvi.ForeColor;
                }
        }

        /// <summary>
        /// Convert the given image selector to an index into our image list.
        /// Return -1 if that's not possible
        /// </summary>
        /// <param name="imageSelector"></param>
        /// <returns>Index of the image in the imageList, or -1</returns>
        public int GetActualImageIndex(Object imageSelector)
        {
        	if (imageSelector == null)
        		return -1;

        	if (imageSelector is Int32)
        		return (int)imageSelector;

        	if (imageSelector is String && this.SmallImageList != null)
        		return this.SmallImageList.Images.IndexOfKey((String)imageSelector);

        	return -1;
        }

		/// <summary>
		/// Make sure the ListView has the extended style that says to display subitem images.
		/// </summary>
		/// <remarks>This method must be called after any .NET call that update the extended styles
		/// since they seem to erase this setting.</remarks>
		protected void ForceSubItemImagesExStyle ()
		{
			NativeMethods.ForceSubItemImagesExStyle(this);
		}

		/// <summary>
		/// For the given item and subitem, make it display the given image
		/// </summary>
		/// <param name="itemIndex">row number (0 based)</param>
		/// <param name="subItemIndex">subitem (0 is the item itself)</param>
		/// <param name="imageIndex">index into the image list</param>
		protected void SetSubItemImage(int itemIndex, int subItemIndex, int imageIndex)
		{
			NativeMethods.SetSubItemImage(this, itemIndex, subItemIndex, imageIndex);
		}

		#endregion

        #region ISupportInitialize Members

        void ISupportInitialize.BeginInit()
        {
            this.Frozen = true;
        }

        void ISupportInitialize.EndInit()
        {
            this.Frozen = false;
        }

        #endregion

        #region Image list manipulation

        /// <summary>
        /// Update our externally visible image list so it holds the same images as our shadow list, but sized correctly
        /// </summary>
        private void SetupExternalImageList()
        {
            // If a row height hasn't been set, or an image list has been give which is the required size, just assign it
            if (rowHeight == -1 || (this.shadowedImageList != null && this.shadowedImageList.ImageSize.Height == rowHeight))
                base.SmallImageList = this.shadowedImageList;
            else
                base.SmallImageList = this.MakeResizedImageList(rowHeight, shadowedImageList);
        }

        /// <summary>
        /// Return a copy of the given source image list, where each image has been resized to be height x height in size.
        /// If source is null, an empty image list of the given size is returned
        /// </summary>
        /// <param name="height">Height and width of the new images</param>
        /// <param name="source">Source of the images (can be null)</param>
        /// <returns>A new image list</returns>
        private ImageList MakeResizedImageList(int height, ImageList source)
        {
            // Return a copy of the source image list, where each image has been resized to the given width and height
            ImageList il = new ImageList();
            il.ImageSize = new Size(height, height);

            // If there's nothing to copy, just return the new list
            if (source == null)
                return il;

            il.TransparentColor = source.TransparentColor;
            il.ColorDepth = source.ColorDepth;

            // Fill the imagelist with resized copies from the source
            for (int i = 0; i < source.Images.Count; i++) {
                Bitmap bm = this.MakeResizedImage(height, source.Images[i], source.TransparentColor);
                il.Images.Add(bm);
            }

            // Give each image the same key it has in the original
            foreach (String key in source.Images.Keys) {
                il.Images.SetKeyName(source.Images.IndexOfKey(key), key);
            }

            return il;
        }

        /// <summary>
        /// Return a bitmap of the given height x height, which shows the given image, centred.
        /// </summary>
        /// <param name="height">Height and width of new bitmap</param>
        /// <param name="image">Image to be centred</param>
        /// <param name="transparent">The background color</param>
        /// <returns>A new bitmap</returns>
        private Bitmap MakeResizedImage(int height, Image image, Color transparent)
        {
            Bitmap bm = new Bitmap(height, height);
            Graphics g = Graphics.FromImage(bm);
            g.Clear(transparent);
            int x = Math.Max(0, (bm.Size.Width - image.Size.Width) / 2);
            int y = Math.Max(0, (bm.Size.Height - image.Size.Height) / 2);
            g.DrawImage(image, x, y, image.Size.Width, image.Size.Height);
            return bm;
        }

        #endregion

        #region Owner drawing

        /// <summary>
        /// Owner draw the column header
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }

        /// <summary>
        /// Owner draw the item
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = (this.View != View.Details);
            base.OnDrawItem(e);
        }

        int[] columnRightEdge = new int[128]; // will anyone ever want more than 128 columns??

        /// <summary>
        /// Owner draw a single subitem
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
        	// Get the special renderer for this column.
            // If there isn't one, use the default draw mechanism.
            OLVColumn column = this.GetColumn(e.ColumnIndex);
            if (column.RendererDelegate == null) {
                e.DrawDefault = true;
                return;
            }

            // Calculate where the subitem should be drawn
            // It should be as simple as 'e.Bounds', but it isn't :-(

            // There seems to be a bug in .NET where the left edge of the bounds of subitem 0
            // is always 0. This is normally what is required, but it's wrong when
            // someone drags column 0 to somewhere else in the listview. We could
            // drop down into Windows-ville and use LVM_GETSUBITEMRECT, but just to be different
            // we keep track of the right edge of all columns, and when subitem 0
            // isn't being shown at column 0, we make its left edge to be the right
            // edge of the previous column plus a little bit.
            //TODO: Replace with LVM_GETSUBITEMRECT
            Rectangle r = e.Bounds;
            if (e.ColumnIndex == 0 && e.Header.DisplayIndex != 0) {
                r.X = this.columnRightEdge[e.Header.DisplayIndex - 1] + 1;
            } else
                //TODO: Check the size of columnRightEdge and dynamically reallocate?
                this.columnRightEdge[e.Header.DisplayIndex] = e.Bounds.Right;

            // Optimize drawing by only redrawing subitems that touch the area that was damaged
            if (!r.IntersectsWith(this.lastUpdateRectangle)) {
                return;
            }

            // Get a graphics context for the renderer to use.
            // But we have more complications. Virtual lists have a nasty habit of drawing column 0
            // whenever there is any mouse move events over a row, and doing it in an un-double buffered manner,
            // which results in nasty flickers! There are also some unbuffered draw when a mouse is first
            // hovered over column 0 of a normal row. So, to avoid all complications,
            // we always manually double-buffer the drawing.
            Graphics g = e.Graphics;
            BufferedGraphics buffer = null;
            bool avoidFlickerMode = true; // set this to false to see the probems with flicker
            if (avoidFlickerMode) {
                buffer = BufferedGraphicsManager.Current.Allocate(e.Graphics, r);
                g = buffer.Graphics;
            }

            // Finally, give the renderer a chance to draw something
            Object row = ((OLVListItem)e.Item).RowObject;
            column.RendererDelegate(e, g, r, row);

            if (buffer != null) {
                buffer.Render();
                buffer.Dispose();
            }
        }

        #endregion

        #region Selection Event Handling

        /// <summary>
        /// This method is called every time a row is selected or deselected. This can be
        /// a pain if the user shift-clicks 100 rows. We override this method so we can
        /// trigger one event for any number of select/deselects that come from one user action
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            // If we haven't already scheduled an event, schedule it to be triggered
            // By using idle time, we will wait until all select events for the same
            // user action have finished before triggering the event.
            if (!this.hasIdleHandler) {
                this.hasIdleHandler = true;
                Application.Idle += new EventHandler(Application_Idle);
            }
        }
        private bool hasIdleHandler = false;

        /// <summary>
        /// The application is idle. Trigger a SelectionChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Idle(object sender, EventArgs e)
        {
            // Remove the handler before triggering the event
            Application.Idle -= new EventHandler(Application_Idle);
            this.hasIdleHandler = false;

            this.OnSelectionChanged(new EventArgs());
        }

        /// <summary>
        /// This event is triggered once per user action that changes the selection state
        /// of one or more rows.
        /// </summary>
        [Category("Behavior"),
        Description("This event is triggered once per user action that changes the selection state of one or more rows.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Trigger the SelectionChanged event
        /// </summary>
        /// <param name="e"></param>
        virtual protected void OnSelectionChanged(EventArgs e)
        {
            if (this.SelectionChanged != null)
                this.SelectionChanged(this, e);
        }

        #endregion

        #region Cell editing

        // NOTES:
        //
        // - What event should we listen to?
        //
        // We can't use OnMouseClick, OnMouseDoubleClick, OnClick, or OnDoubleClick
        // since they are not triggered for clicks on subitems without Full Row Select.
        //
        // We could use OnMouseDown, but selecting rows is done in OnMouseUp. This means
        // that if we start the editing during OnMouseDown, the editor will automatically
        // lose focus when mouse up happens.
        //

        /// <summary>
        /// We need the click count in the mouse up event, but that is always 1.
        /// So we have to remember the click count from the preceding mouse down event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.lastMouseDownClickCount = e.Clicks;
        }
        private int lastMouseDownClickCount = 0;

        /// <summary>
        /// Check to see if we need to start editing a cell
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            // If it was an unmodified left click, check to see if we should start editing
            if (this.ShouldStartCellEdit(e)) {
                ListViewHitTestInfo info = this.HitTest(e.Location);
                if (info.Item != null && info.SubItem != null) {
                    int subItemIndex = info.Item.SubItems.IndexOf(info.SubItem);

                    // We don't edit the primary column by single clicks -- only subitems.
                    if (this.CellEditActivation != CellEditActivateMode.SingleClick || subItemIndex > 0)
                        this.EditSubItem((OLVListItem)info.Item, subItemIndex);
                }
            }
        }

        /// <summary>
        /// Should we start editing the cell?
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected bool ShouldStartCellEdit(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return false;

            if ((Control.ModifierKeys & (Keys.Shift | Keys.Control | Keys.Alt)) != 0)
                return false;

            if (this.lastMouseDownClickCount == 1 && this.CellEditActivation == CellEditActivateMode.SingleClick)
                return true;

             return (this.lastMouseDownClickCount == 2 && this.CellEditActivation == CellEditActivateMode.DoubleClick);
        }

        /// <summary>
        /// Handle a key press on this control. We specifically look for F2 which edits the primary column,
        /// or a Tab character during an edit operation, which tries to start editing on the next (or previous) cell.
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool isSimpleTabKey = ((keyData & Keys.KeyCode) == Keys.Tab) && ((keyData & (Keys.Alt | Keys.Control)) == Keys.None);

            if (isSimpleTabKey && this.IsCellEditing) { // Tab key while editing
                this.FinishCellEdit();

                OLVListItem olvi = this.cellEditEventArgs.ListViewItem;
                int subItemIndex = this.cellEditEventArgs.SubItemIndex;
                int displayIndex = this.GetColumn(subItemIndex).DisplayIndex;

                // Move to the next or previous editable subitem, depending on Shift key. Wrap at the edges
                List<OLVColumn> columnsInDisplayOrder = this.ColumnsInDisplayOrder;
                do {
                    if ((keyData & Keys.Shift) == Keys.Shift)
                        displayIndex = (olvi.SubItems.Count + displayIndex - 1) % olvi.SubItems.Count;
                    else
                        displayIndex = (displayIndex + 1) % olvi.SubItems.Count;
                } while (!columnsInDisplayOrder[displayIndex].IsEditable);

                // If we found a different editable cell, start editing it
                subItemIndex = columnsInDisplayOrder[displayIndex].Index;
                if (this.cellEditEventArgs.SubItemIndex != subItemIndex) {
                    this.StartCellEdit(olvi, subItemIndex);
                    return true;
                }
            }

            // Treat F2 as a request to edit the primary column
            if (keyData == Keys.F2 && !this.IsCellEditing) {
                this.EditSubItem((OLVListItem)this.FocusedItem, 0);
                return true;
            }

            // We have to catch Return/Enter/Escape here since some types of controls
            // (e.g. ComboBox, UserControl) don't trigger key events that we can listen for.
            // Treat Return or Enter as committing the current edit operation
            if ((keyData == Keys.Return || keyData == Keys.Enter) && this.IsCellEditing) {
                this.FinishCellEdit();
                return true;
            }

            // Treat Escaoe as cancel the current edit operation
            if (keyData == Keys.Escape && this.IsCellEditing) {
                this.CancelCellEdit();
                return true;
            }

            // Treat Ctrl-C as Copy To Clipboard. We still allow the default processing
            if ((keyData & Keys.Control) == Keys.Control && (keyData & Keys.KeyCode) == Keys.C)
                this.CopySelectionToClipboard();

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Begin an edit operation on the given cell.
        /// </summary>
        /// <remarks>This performs various sanity checks and passes off the real work to StartCellEdit().</remarks>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        public void EditSubItem(OLVListItem item, int subItemIndex)
        {
            if (item == null)
                return;

            if (subItemIndex < 0 && subItemIndex >= item.SubItems.Count)
                return;

            if (this.CellEditActivation == CellEditActivateMode.None)
                return;

            if (!this.GetColumn(subItemIndex).IsEditable)
                return;

            this.StartCellEdit(item, subItemIndex);
        }

        /// <summary>
        /// Really start an edit operation on a given cell. The parameters are assumed to be sane.
        /// </summary>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        protected void StartCellEdit(OLVListItem item, int subItemIndex)
        {
            OLVColumn column = this.GetColumn(subItemIndex);
            Rectangle r = this.CalculateCellBounds(item, subItemIndex);
            Control c = this.GetCellEditor(item, subItemIndex);
            c.Bounds = r;

            // Try to align the control as the column is aligned. Not all controls support this property
            PropertyInfo pinfo = c.GetType().GetProperty("TextAlign");
            if (pinfo != null)
                pinfo.SetValue(c, column.TextAlign, null);

            // Give the control the value from the model
            this.SetControlValue(c, column.GetValue(item.RowObject), column.GetStringValue(item.RowObject));

            // Give the outside world the chance to munge with the process
            this.cellEditEventArgs = new CellEditEventArgs(column, c, r, item, subItemIndex);
            this.OnCellEditStarting(this.cellEditEventArgs);
            if (this.cellEditEventArgs.Cancel)
                return;

            // The event handler may have completely changed the control, so we need to remember it
            this.cellEditor = this.cellEditEventArgs.Control;

            // If the control isn't the height of the cell, centre it vertically. We don't
            // need to do this when in Tile view.
            if (this.View != View.Tile && this.cellEditor.Height != r.Height)
                this.cellEditor.Top += (r.Height - this.cellEditor.Height) / 2;

            this.Controls.Add(this.cellEditor);
            this.ConfigureControl();
            this.PauseAnimations(true);
        }
        private Control cellEditor = null;
        private CellEditEventArgs cellEditEventArgs = null;

        /// <summary>
        /// Try to give the given value to the provided control. Fall back to assigning a string
        /// if the value assignment fails.
        /// </summary>
        /// <param name="c">A control</param>
        /// <param name="value">The value to be given to the control</param>
        /// <param name="stringValue">The string to be given if the value doesn't work</param>
        protected void SetControlValue(Control c, Object value, String stringValue)
        {
            // Look for a property called "Value". We have to look twice, the first time might get an ambiguous
            PropertyInfo pinfo = null;
            try {
                pinfo = c.GetType().GetProperty("Value");
            } catch (AmbiguousMatchException) {
                // The lowest level class of the control must have overridden the "Value" property.
                // We now have to specifically  look for only public instance properties declared in the lowest level class.
                pinfo = c.GetType().GetProperty("Value", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            }

            // If we found it, use it to assign a value, otherwise simply set the text
            if (pinfo == null)
                c.Text = stringValue;
            else {
                try {
                    pinfo.SetValue(c, value, null);
                } catch (TargetInvocationException) {
                    // Not a lot we can do about this one. Something went wrong in the bowels
                    // of the method. Let's take the ostrich approach and just ignore it :-)
                } catch (ArgumentException) {
                    c.Text = stringValue;
                }
            }
        }

        /// <summary>
        /// Setup the given control to be a cell editor
        /// </summary>
        protected void ConfigureControl()
        {
            this.cellEditor.Leave += new EventHandler(CellEditor_Leave);
            this.cellEditor.Select();
        }

        /// <summary>
        /// Return the value that the given control is showing
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected Object GetControlValue(Control c)
        {
            try {
                return c.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, c, null);
            } catch (MissingMethodException) { // Microsoft throws this
                return c.Text;
            } catch (MissingFieldException) { // Mono throws this
                return c.Text;
            }
        }

        /// <summary>
        /// Called when the cell editor loses focus. Time to commit the change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellEditor_Leave(object sender, EventArgs e)
        {
            FinishCellEdit();
        }

        /// <summary>
        /// Return the bounds of the given cell
        /// </summary>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        /// <returns>A Rectangle</returns>
        protected Rectangle CalculateCellBounds(OLVListItem item, int subItemIndex)
        {
            // Item 0 is special. Its bounds include all subitems. To get just the bounds
            // of cell for item 0, we have to use GetItemRect().
            if (subItemIndex == 0) {
                return this.GetItemRect(item.Index, ItemBoundsPortion.Label);
            } else
                return item.SubItems[subItemIndex].Bounds;
        }

        /// <summary>
        /// Return a control that can be used to edit the value of the given cell.
        /// </summary>
        /// <param name="item">The row to be edited</param>
        /// <param name="subItemIndex">The index of the cell to be edited</param>
        /// <returns></returns>
        protected Control GetCellEditor(OLVListItem item, int subItemIndex)
        {
            OLVColumn column = this.GetColumn(subItemIndex);
            Object value = column.GetValue(item.RowObject);

            //THINK: Do we want to use a Registry instead of this cascade?
            if (value is Boolean)
                return new BooleanCellEditor();

            if (value is DateTime) {
                DateTimePicker c = new DateTimePicker();
                c.Format = DateTimePickerFormat.Short;
                return c;
            }
            if (value is Int32 || value is Int16 || value is Int64)
                return new IntUpDown();

            if (value is UInt16 || value is UInt32 || value is UInt64)
                return new UintUpDown();

            if (value is Single || value is Double)
                return new FloatCellEditor();

            return this.MakeDefaultCellEditor(column);
        }

        /// <summary>
        /// Return a TextBox that can be used as a default cell editor.
        /// </summary>
        /// <param name="column">What column does the cell belong to?</param>
        /// <returns></returns>
        private Control MakeDefaultCellEditor(OLVColumn column)
        {
            TextBox tb = new TextBox();

            // Build a list of unique values, to be used as autocomplete on the editor
            Dictionary<String, bool> alreadySeen = new Dictionary<string, bool>();
            for (int i = 0; i < Math.Min(this.GetItemCount(), 1000); i++) {
                String str = column.GetStringValue(this.GetModelObject(i));
                if (!alreadySeen.ContainsKey(str)) {
                    tb.AutoCompleteCustomSource.Add(str);
                    alreadySeen[str] = true;
                }
            }

            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tb.AutoCompleteMode = AutoCompleteMode.Append;

            return tb;
        }

        /// <summary>
        /// Stop editing a cell and throw away any changes.
        /// </summary>
        protected void CancelCellEdit()
        {
            // Let the world know that the user has cancelled the edit operation
            this.cellEditEventArgs.Cancel = true;
            this.OnCellEditFinishing(this.cellEditEventArgs);

            // Now cleanup the editing process
            this.CleanupCellEdit();
        }

        /// <summary>
        /// If a cell edit is in progress, finish the edit
        /// </summary>
        protected void PossibleFinishCellEditing()
        {
            if (this.IsCellEditing)
                FinishCellEdit();
        }

        /// <summary>
        /// Finish the cell edit operation, writing changed data back to the model object
        /// </summary>
        protected void FinishCellEdit()
        {
            this.OnCellEditFinishing(this.cellEditEventArgs);

            // If someone doesn't cancel the editing process, write the value back into the model
            if (!this.cellEditEventArgs.Cancel) {
                Object value = this.GetControlValue(this.cellEditor);
                this.cellEditEventArgs.Column.PutValue(this.cellEditEventArgs.RowObject, value);
                this.RefreshItem(this.cellEditEventArgs.ListViewItem);
            }

            this.CleanupCellEdit();
        }

        /// <summary>
        /// Remove all trace of any existing cell edit operation
        /// </summary>
        protected void CleanupCellEdit()
        {
            this.cellEditor.Leave -= new EventHandler(CellEditor_Leave);
            this.Controls.Remove(this.cellEditor);
            this.cellEditor.Dispose(); //THINK: do we need to call this?
            this.cellEditor = null;
            this.Select();
            this.PauseAnimations(false);
        }

        /// <summary>
        /// The callbacks for CellEditing events
        /// </summary>
        public delegate void CellEditEventHandler(object sender, CellEditEventArgs e);

        /// <summary>
        /// Triggered when a cell is about to be edited.
        /// </summary>
        /// <remarks>Set Cancel to true to prevent the cell being edited.
        /// You can change the the Control to be something completely different.</remarks>
        [Category("Behavior")]
        public event CellEditEventHandler CellEditStarting;

        /// <summary>
        /// Triggered when a cell is about to finish being edited.
        /// </summary>
        /// <remarks>If Cancel is already true, the user is cancelling the edit operation.
        /// Set Cancel to true to prevent the value from the cell being written into the model.
        /// You cannot prevent the editing from finishing.</remarks>
        [Category("Behavior")]
        public event CellEditEventHandler CellEditFinishing;
        
        /// <summary>
        /// Tell the world when a cell is about to be edited.
        /// </summary>
        protected virtual void OnCellEditStarting(CellEditEventArgs e)
        {
            if (this.CellEditStarting != null)
                this.CellEditStarting(this, e);
        }

        /// <summary>
        /// Tell the world when a cell is about to finish being edited.
        /// </summary>
        protected virtual void OnCellEditFinishing(CellEditEventArgs e)
        {
            if (this.CellEditFinishing != null)
                this.CellEditFinishing(this, e);
        }

        /// <summary>
        /// Let the world know that a cell edit operation is beginning or ending
        /// </summary>
        public class CellEditEventArgs : EventArgs
        {
            /// <summary>
            /// Create an event args
            /// </summary>
            /// <param name="column"></param>
            /// <param name="c"></param>
            /// <param name="r"></param>
            /// <param name="item"></param>
            /// <param name="subItemIndex"></param>
            public CellEditEventArgs(OLVColumn column, Control c, Rectangle r, OLVListItem item, int subItemIndex)
            {
                this.Cancel = false;
                this.Control = c;
                this.column = column;
                this.cellBounds = r;
                this.listViewItem = item;
                this.rowObject = item.RowObject;
                this.subItemIndex = subItemIndex;
                this.value = column.GetValue(item.RowObject);
            }

            /// <summary>
            /// Change this to true to cancel the cell editing operation.
            /// </summary>
            /// <remarks>
            /// <para>During the CellEditStarting event, setting this to true will prevent the cell from being edited.</para>
            /// <para>During the CellEditFinishing event, if this value is already true, this indicates that the user has
            /// cancelled the edit operation and that the handler should perform cleanup only. Setting this to true,
            /// will prevent the ObjectListView from trying to write the new value into the model object.</para>
            /// </remarks>
            public bool Cancel = false;

            /// <summary>
            /// During the CellEditStarting event, this can be modified to be the control that you want
            /// to edit the value. You must fully configure the control before returning from the event,
            /// including its bounds and the value it is showing.
            /// During the CellEditFinishing event, you can use this to get the value that the user
            /// entered and commit that value to the model. Changing the control during the finishing
            /// event has no effect.
            /// </summary>
            public Control Control = null;

            /// <summary>
            /// The column of the cell that is going to be or has been edited.
            /// </summary>
            public OLVColumn Column
            {
                get { return this.column; }
            }
            private OLVColumn column;

            /// <summary>
            /// The model object of the row of the cell that is going to be or has been edited.
            /// </summary>
            public Object RowObject
            {
                get { return this.rowObject; }
            }
            private Object rowObject;

            /// <summary>
            /// The listview item of the cell that is going to be or has been edited.
            /// </summary>
            public OLVListItem ListViewItem
            {
                get { return this.listViewItem; }
            }
            private OLVListItem listViewItem;

            /// <summary>
            /// The index of the cell that is going to be or has been edited.
            /// </summary>
            public int SubItemIndex
            {
                get { return this.subItemIndex; }
            }
            private int subItemIndex;

            /// <summary>
            /// The data value of the cell before the edit operation began.
            /// </summary>
            public Object Value
            {
                get { return this.value; }
            }
            private Object value;

            /// <summary>
            /// The bounds of the cell that is going to be or has been edited.
            /// </summary>
            public Rectangle CellBounds
            {
                get { return this.cellBounds; }
            }
            private Rectangle cellBounds;
        }

        //-----------------------------------------------------------------------
        // Cell editors
        // These classes are simple cell editors that make it easier to get and set
        // the value that the control is showing.

        /// <summary>
        /// This editor simply shows and edits integer values.
        /// </summary>
        internal class IntUpDown : NumericUpDown
        {
            public IntUpDown()
            {
                this.DecimalPlaces = 0;
                this.Minimum = -9999999;
                this.Maximum = 9999999;
            }

            new public int Value
            {
                get { return Decimal.ToInt32(base.Value); }
                set { base.Value = new Decimal(value); }
            }
        }

        /// <summary>
        /// This editor simply shows and edits unsigned integer values.
        /// </summary>
        internal class UintUpDown : NumericUpDown
        {
            public UintUpDown()
            {
                this.DecimalPlaces = 0;
                this.Minimum = 0;
                this.Maximum = 9999999;
            }

            new public uint Value
            {
                get { return Decimal.ToUInt32(base.Value); }
                set { base.Value = new Decimal(value); }
            }
        }

        /// <summary>
        /// This editor simply shows and edits boolean values.
        /// </summary>
        /// <remarks>You can intercept the CellEditStarting event if you want
        /// to change the characteristics of the editor. For example, by changing
        /// the labels to "No" and "Yes". The false value must come first.</remarks>
        internal class BooleanCellEditor : ComboBox
        {
            public BooleanCellEditor()
            {
                this.DropDownStyle = ComboBoxStyle.DropDownList;
                this.Items.AddRange(new String[] { "False", "True" }); // needs to be localizable
            }

            public bool Value
            {
                get { return this.SelectedIndex == 1; }
                set {
                    if (value)
                        this.SelectedIndex = 1;
                    else
                        this.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// This editor simply shows and edits floating point values.
        /// </summary>
        /// <remarks>You can intercept the CellEditStarting event if you want
        /// to change the characteristics of the editor. For example, by increasing
        /// the number of decimal places.</remarks>
        internal class FloatCellEditor : NumericUpDown
        {
            public FloatCellEditor()
            {
                this.DecimalPlaces = 2;
                this.Minimum = -9999999;
                this.Maximum = 9999999;
            }

            new public double Value
            {
                get { return Convert.ToDouble(base.Value); }
                set { base.Value = Convert.ToDecimal(value); }
            }
        }

        /// <summary>
        /// This editor shows and auto completes values from the given listview column.
        /// </summary>
        internal class AutoCompleteCellEditor : ComboBox
        {
            public AutoCompleteCellEditor(ObjectListView lv, OLVColumn column)
            {
                this.DropDownStyle = ComboBoxStyle.DropDown;

                Dictionary<String, bool> alreadySeen = new Dictionary<string, bool>();
                for (int i = 0; i < Math.Min(lv.GetItemCount(), 1000); i++) {
                    String str = column.GetStringValue(lv.GetModelObject(i));
                    if (!alreadySeen.ContainsKey(str)) {
                        this.Items.Add(str);
                        alreadySeen[str] = true;
                    }
                }

                this.Sorted = true;
                this.AutoCompleteSource = AutoCompleteSource.ListItems;
                this.AutoCompleteMode = AutoCompleteMode.Append;
            }
        }

        /// <summary>
        /// Return a collection of columns that are appropriate to the given view.
        /// Only Tile and Details have columns; all other views have 0 columns.
        /// </summary>
        /// <param name="view">Which view are the columns being calculate for?</param>
        /// <returns>A list of columns</returns>
        public List<OLVColumn> GetFilteredColumns(View view)
        {
            // For both detail and tile view, the first column must be included. Normally, we would
            // use the ColumnHeader.Index property, but if the header is not currently part of a ListView
            // that property returns -1. So, we track the index of
            // the column header, and always include the first header.

            int index = 0;
            switch (view) {
                case View.Details:
                    return this.AllColumns.FindAll(delegate(OLVColumn x) { return (index++ == 0) || x.IsVisible; });
                case View.Tile:
                    return this.AllColumns.FindAll(delegate(OLVColumn x) { return (index++ == 0) || x.IsTileViewColumn; });
                default:
                    return new List<OLVColumn>(); ;
            }
        }

        #endregion

        #region Design Time

        /// <summary>
		/// This class works in conjunction with the OLVColumns property to allow OLVColumns
		/// to be added to the ObjectListView.
		/// </summary>
		internal class OLVColumnCollectionEditor : System.ComponentModel.Design.CollectionEditor
		{
			public OLVColumnCollectionEditor(Type t)
				: base(t)
			{
			}

			protected override Type CreateCollectionItemType()
			{
				return typeof(OLVColumn);
			}

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                ListView.ColumnHeaderCollection cols = (ListView.ColumnHeaderCollection)value;

                // Hack to figure out which ObjectListView we are working on
                ObjectListView olv;
                if (cols.Count == 0) {
                    cols.Add(new OLVColumn());
                    olv = (ObjectListView)cols[0].ListView;
                    cols.Clear();
                    olv.AllColumns.Clear();
                } else
                    olv = (ObjectListView)cols[0].ListView;

                // Edit all the columns, not just the ones that are visible
                base.EditValue(context, provider, olv.AllColumns);

                // Calculate just the visible columns
                List<OLVColumn> newColumns = olv.GetFilteredColumns(View.Details);
                olv.Columns.Clear();
                olv.Columns.AddRange(newColumns.ToArray());

                return olv.Columns;
            }
		}

		/// <summary>
		/// Return Columns for this list. We hide the original so we can associate
		/// a specialised editor with it.
		/// </summary>
		[Editor(typeof(OLVColumnCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		new public ListView.ColumnHeaderCollection Columns {
			get {
				return base.Columns;
			}
		}

		#endregion


        /// <summary>
        /// Which column did we last sort by
        /// </summary>
        protected OLVColumn lastSortColumn;

        /// <summary>
        /// Which direction did we last sort
        /// </summary>
        protected SortOrder lastSortOrder;

        private IEnumerable objects; // the collection of objects on show
        private bool showImagesOnSubItems; // should we try to show images on subitems?
        private bool showSortIndicators; // should we show sort indicators in the column headers?
		private bool showItemCountOnGroups; // should we show items count in group labels?
        private string groupWithItemCountFormat; // when a group title has an item count, how should the label be formatted?
        private string groupWithItemCountSingularFormat; // when a group title has an item count of 1, how should the label be formatted?
		private bool useAlternatingBackColors; // should we use different colors for alternate lines?
		private Color alternateRowBackColor = Color.Empty; // what color background should alternate lines have?
        private SortDelegate customSorter; // callback for handling custom sort by column processing
        private Rectangle lastUpdateRectangle; // remember the update rect from the last WM_PAINT msg
    }


	#region Delegate declarations

	/// <summary>
	/// These delegates are used to extract an aspect from a row object
	/// </summary>
    public delegate Object AspectGetterDelegate(Object rowObject);

    /// <summary>
    /// These delegates are used to put a changed value back into a model object
    /// </summary>
    public delegate void AspectPutterDelegate(Object rowObject, Object newValue);

    /// <summary>
    /// These delegates can be used to convert an aspect value to a display string,
    /// instead of using the default ToString()
    /// </summary>
    public delegate string AspectToStringConverterDelegate(Object value);

    /// <summary>
    /// These delegates are used to the state of the checkbox for a row object.
    /// </summary>
    /// <remarks>For reasons known only to someone in Microsoft, we can only set
    /// a boolean on the ListViewItem to indicate it's "checked-ness", but when
    /// we receive update events, we have to use a tristate CheckState. So we can
    /// be told about an indeterminate state, but we can't set it ourselves.</remarks>
    public delegate bool CheckStateGetterDelegate(Object rowObject);

    /// <summary>
    /// These delegates are used to put a changed check state back into a model object
    /// </summary>
    public delegate CheckState CheckStatePutterDelegate(Object rowObject, CheckState newValue);

	/// <summary>
	/// These delegates are used to retrieve the object that is the key of the group to which the given row belongs.
	/// </summary>
    public delegate Object GroupKeyGetterDelegate(Object rowObject);

	/// <summary>
	/// These delegates are used to convert a group key into a title for the group
	/// </summary>
    public delegate string GroupKeyToTitleConverterDelegate(Object groupKey);

    /// <summary>
    /// These delegates are used to fetch the image selector that should be used
    /// to choose an image for this column.
    /// </summary>
    public delegate Object ImageGetterDelegate(Object rowObject);

    /// <summary>
    /// These delegates are used to draw a cell
    /// </summary>
    public delegate void RenderDelegate(DrawListViewSubItemEventArgs e, Graphics g, Rectangle r, Object rowObject);

	/// <summary>
	/// These delegates are used to fetch a row object for virtual lists
	/// </summary>
    public delegate Object RowGetterDelegate(int rowIndex);

    /// <summary>
    /// These delegates are used to format a listviewitem before it is added to the control.
    /// </summary>
    public delegate void RowFormatterDelegate(OLVListItem olvItem);

    /// <summary>
    /// These delegates are used to sort the listview in some custom fashion
    /// </summary>
    public delegate void SortDelegate(OLVColumn column, SortOrder sortOrder);

	#endregion



}
