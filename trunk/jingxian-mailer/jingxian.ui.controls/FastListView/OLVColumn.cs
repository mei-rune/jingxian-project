
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
    /// An OLVColumn knows which aspect of an object it should present.
    /// </summary>
    /// <remarks>
    /// The column knows how to:
    /// <list type="bullet">
    ///	<item>extract its aspect from the row object</item>
    ///	<item>convert an aspect to a string</item>
    ///	<item>calculate the image for the row object</item>
    ///	<item>extract a group "key" from the row object</item>
    ///	<item>convert a group "key" into a title for the group</item>
    /// </list>
    /// <para>For sorting to work correctly, aspects from the same column
    /// must be of the same type, that is, the same aspect cannot sometimes
    /// return strings and other times integers.</para>
    /// </remarks>
    [Browsable(false)]
    public partial class OLVColumn : ColumnHeader
    {
        /// <summary>
        /// Create an OLVColumn
        /// </summary>
        public OLVColumn()
            : base()
        {
        }

        /// <summary>
        /// Initialize a column to have the given title, and show the given aspect
        /// </summary>
        /// <param name="title">The title of the column</param>
        /// <param name="aspect">The aspect to be shown in the column</param>
        public OLVColumn(string title, string aspect)
            : this()
        {
            this.Text = title;
            this.AspectName = aspect;
        }

        #region Public Properties

        /// <summary>
        /// The name of the property or method that should be called to get the value to display in this column.
        /// This is only used if a ValueGetterDelegate has not been given.
        /// </summary>
        /// <remarks>This name can be dotted to chain references to properties or methods.</remarks>
        /// <example>"DateOfBirth"</example>
        /// <example>"Owner.HomeAddress.Postcode"</example>
        [Category("Behavior"),
         Description("The name of the property or method that should be called to get the aspect to display in this column")]
        public string AspectName
        {
            get { return aspectName; }
            set { aspectName = value; }
        }

        /// <summary>
        /// This format string will be used to convert an aspect to its string representation.
        /// </summary>
        /// <remarks>
        /// This string is passed as the first parameter to the String.Format() method.
        /// This is only used if ToStringDelegate has not been set.</remarks>
        /// <example>"{0:C}" to convert a number to currency</example>
        [Category("Behavior"),
         Description("The format string that will be used to convert an aspect to its string representation"),
         DefaultValue(null)]
        public string AspectToStringFormat
        {
            get { return aspectToStringFormat; }
            set { aspectToStringFormat = value; }
        }

        /// <summary>
        /// Group objects by the initial letter of the aspect of the column
        /// </summary>
        /// <remarks>
        /// One common pattern is to group column by the initial letter of the value for that group.
        /// The aspect must be a string (obviously).
        /// </remarks>
        [Category("Behavior"),
         Description("The name of the property or method that should be called to get the aspect to display in this column"),
         DefaultValue(false)]
        public bool UseInitialLetterForGroup
        {
            get { return useInitialLetterForGroup; }
            set { useInitialLetterForGroup = value; }
        }

        /// <summary>
        /// Get/set whether this column should be used when the view is switched to tile view.
        /// </summary>
        /// <remarks>Column 0 is always included in tileview regardless of this setting.
        /// Tile views do not work well with many "columns" of information, 2 or 3 works best.</remarks>
        [Category("Behavior"),
        Description("Will this column be used when the view is switched to tile view"),
         DefaultValue(false)]
        public bool IsTileViewColumn
        {
            get { return isTileViewColumn; }
            set { isTileViewColumn = value; }
        }
        private bool isTileViewColumn = false;

        /// <summary>
        /// This delegate will be used to extract a value to be displayed in this column.
        /// </summary>
        /// <remarks>
        /// If this is set, AspectName is ignored.
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectGetterDelegate AspectGetter
        {
            get { return aspectGetter; }
            set
            {
                aspectGetter = value;
                aspectGetterAutoGenerated = false;
            }
        }

        /// <summary>
        /// The delegate that will be used to translate the aspect to display in this column into a string.
        /// </summary>
        /// <remarks>If this value is set, ValueToStringFormat will be ignored.</remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectToStringConverterDelegate AspectToStringConverter
        {
            get { return aspectToStringConverter; }
            set { aspectToStringConverter = value; }
        }

        /// <summary>
        /// This delegate is called to get the image selector of the image that should be shown in this column.
        /// It can return an int, string, Image or null.
        /// </summary>
        /// <remarks><para>This delegate can use these return value to identify the image:</para>
        /// <list>
        /// <item>null or -1 -- indicates no image</item>
        /// <item>an int -- the int value will be used as an index into the image list</item>
        /// <item>a String -- the string value will be used as a key into the image list</item>
        /// <item>an Image -- the Image will be drawn directly (only in OwnerDrawn mode)</item>
        /// </list>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImageGetterDelegate ImageGetter
        {
            get { return imageGetter; }
            set { imageGetter = value; }
        }

        /// <summary>
        /// This delegate is called to get the object that is the key for the group
        /// to which the given row belongs.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GroupKeyGetterDelegate GroupKeyGetter
        {
            get { return groupKeyGetter; }
            set { groupKeyGetter = value; }
        }

        /// <summary>
        /// This delegate is called to convert a group key into a title for that group.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GroupKeyToTitleConverterDelegate GroupKeyToTitleConverter
        {
            get { return groupKeyToTitleConverter; }
            set { groupKeyToTitleConverter = value; }
        }

        /// <summary>
        /// This delegate is called when a cell needs to be drawn in OwnerDrawn mode.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RenderDelegate RendererDelegate
        {
            get { return rendererDelegate; }
            set { rendererDelegate = value; }
        }

        /// <summary>
        /// Get/set the renderer that will be invoked when a cell needs to be redrawn
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseRenderer Renderer
        {
            get { return renderer; }
            set
            {
                renderer = value;
                if (renderer == null)
                    this.RendererDelegate = null;
                else
                {
                    renderer.Column = this;
                    this.RendererDelegate = new RenderDelegate(renderer.HandleRendering);
                }
            }
        }
        private BaseRenderer renderer;

        /// <summary>
        /// Remember if this aspect getter for this column was generated internally, and can therefore
        /// be regenerated at will
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AspectGetterAutoGenerated
        {
            get { return aspectGetterAutoGenerated; }
            set { aspectGetterAutoGenerated = value; }
        }
        private bool aspectGetterAutoGenerated;

        /// <summary>
        /// When the listview is grouped by this column and group title has an item count,
        /// how should the lable be formatted?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group</item>
        /// </list>
        /// <para>If this value is not set, the values from the list view will be used</para>
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
        private string groupWithItemCountFormat;

        /// <summary>
        /// Return this.GroupWithItemCountFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountFormat))
                    // There is one rare but pathelogically possible case where the ListView can
                    // be null, so we have to provide a workable default for that rare case.
                    if (this.ListView == null)
                        return "{0} [{1} items]";
                    else
                        return ((ObjectListView)this.ListView).GroupWithItemCountFormatOrDefault;
                else
                    return this.GroupWithItemCountFormat;
            }
        }

        /// <summary>
        /// When the listview is grouped by this column and a group title has an item count,
        /// how should the lable be formatted if there is only one item in the group?
        /// </summary>
        /// <remarks>
        /// The given format string can/should have two placeholders:
        /// <list type="bullet">
        /// <item>{0} - the original group title</item>
        /// <item>{1} - the number of items in the group (always 1)</item>
        /// </list>
        /// <para>If this value is not set, the values from the list view will be used</para>
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
        private string groupWithItemCountSingularFormat;

        /// <summary>
        /// Return this.GroupWithItemCountSingularFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountSingularFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountSingularFormat))
                    // There is one pathelogically rare but still possible case where the ListView can
                    // be null, so we have to provide a workable default for that rare case.
                    if (this.ListView == null)
                        return "{0} [{1} item]";
                    else
                        return ((ObjectListView)this.ListView).GroupWithItemCountSingularFormatOrDefault;
                else
                    return this.GroupWithItemCountSingularFormat;
            }
        }

        /// <summary>
        /// What is the minimum width that the user can give to this column?
        /// </summary>
        /// <remarks>-1 means there is no minimum width. Give this the same value as MaximumWidth to make a fixed width column.</remarks>
        [Category("Misc"),
         Description("What is the minimum width to which the user can resize this column?"),
         DefaultValue(-1)]
        public int MinimumWidth
        {
            get { return minWidth; }
            set
            {
                minWidth = value;
                if (this.Width < minWidth)
                    this.Width = minWidth;
            }
        }
        private int minWidth = -1;

        /// <summary>
        /// What is the maximum width that the user can give to this column?
        /// </summary>
        /// <remarks>-1 means there is no maximum width. Give this the same value as MinimumWidth to make a fixed width column.</remarks>
        [Category("Misc"),
         Description("What is the maximum width to which the user can resize this column?"),
         DefaultValue(-1)]
        public int MaximumWidth
        {
            get { return maxWidth; }
            set
            {
                maxWidth = value;
                if (maxWidth != -1 && this.Width > maxWidth)
                    this.Width = maxWidth;
            }
        }
        private int maxWidth = -1;

        /// <summary>
        /// Is this column a fixed width column?
        /// </summary>
        [Browsable(false)]
        public bool IsFixedWidth
        {
            get
            {
                return (this.MinimumWidth != -1 && this.MaximumWidth != -1 && this.MinimumWidth >= this.MaximumWidth);
            }
        }

        /// <summary>
        /// What proportion of the unoccupied horizontal space in the control should be given to this column?
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are situations where it would be nice if a column (normally the rightmost one) would expand as
        /// the list view expands, so that as much of the column was visible as possible without having to scroll
        /// horizontally (you should never, ever make your users have to scroll anything horizontally!).
        /// </para>
        /// <para>
        /// A space filling column is resized to occupy a proportion of the unoccupied width of the listview (the
        /// unoccupied width is the width left over once all the the non-filling columns have been given their space).
        /// This property indicates the relative proportion of that unoccupied space that will be given to this column.
        /// The actual value of this property is not important -- only its value relative to the value in other columns.
        /// For example:
        /// <list type="bullet">
        /// <item>
        /// If there is only one space filling column, it will be given all the free space, regardless of the value in FreeSpaceProportion.
        /// </item>
        /// <item>
        /// If there are two or more space filling columns and they all have the same value for FreeSpaceProportion,
        /// they will share the free space equally.
        /// </item>
        /// <item>
        /// If there are three space filling columns with values of 3, 2, and 1
        /// for FreeSpaceProportion, then the first column with occupy half the free space, the second will
        /// occupy one-third of the free space, and the third column one-sixth of the free space.
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FreeSpaceProportion
        {
            get { return freeSpaceProportion; }
            set { freeSpaceProportion = Math.Max(0, value); }
        }
        private int freeSpaceProportion = 0;

        /// <summary>
        /// Should this column resize to fill the free space in the listview?
        /// </summary>
        /// <remarks>
        /// <para>
        /// If you want two (or more) columns to equally share the available free space, set this property to True.
        /// If you want this column to have a larger or smaller share of the free space, you must
        /// set the FreeSpaceProportion property explicitly.
        /// </para>
        /// <para>
        /// Space filling columns are still governed by the MinimumWidth and MaximumWidth properties.
        /// </para>
        /// /// </remarks>
        [Category("Misc"),
         Description("Will this column resize to fill unoccupied horizontal space in the listview?"),
         DefaultValue(false)]
        public bool FillsFreeSpace
        {
            get { return this.FreeSpaceProportion > 0; }
            set
            {
                if (value)
                    this.freeSpaceProportion = 1;
                else
                    this.freeSpaceProportion = 0;
            }
        }

        /// <summary>
        /// This delegate will be used to put an edited value back into the model object.
        /// </summary>
        /// <remarks>
        /// This does nothing if IsEditable == false.
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectPutterDelegate AspectPutter
        {
            get { return aspectPutter; }
            set { aspectPutter = value; }
        }

        /// <summary>
        /// Can the values shown in this column be edited?
        /// </summary>
        /// <remarks>This defaults to true, since the primary means to control the editability of a listview
        /// is on the listview itself. Once a listview is editable, all the columns are too, unless the
        /// programmer explicitly marks them as not editable</remarks>
        [Category("Misc"),
        Description("Can the value in this column be edited?"),
        DefaultValue(true)]
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }
        private bool isEditable = true;

        /// <summary>
        /// Return the control that should be used to edit cells in this column
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control CellEditor
        {
            get { return cellEditor; }
            set { cellEditor = value; }
        }
        private Control cellEditor;

        /// <summary>
        /// Can this column be seen by the user?
        /// </summary>
        /// <remarks>After changing this value, you must call RebuildColumns() before the changes will be effected.</remarks>
        [Category("Misc"),
        Description("Can this column be seen by the user?"),
        DefaultValue(true)]
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        private bool isVisible = true;

        /// <summary>
        /// Where was this column last positioned within the Detail view columns
        /// </summary>
        /// <remarks>DisplayIndex is volatile. Once a column is removed from the control,
        /// there is no way to discover where it was in the display order. This property
        /// guards that information even when the column is not in the listview's active columns.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int LastDisplayIndex = -1;

        #endregion

        /// <summary>
        /// For a given row object, return the object that is to be displayed in this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>An object, which is the aspect to be displayed</returns>
        public object GetValue(object rowObject)
        {
            if (this.aspectGetter == null)
                return this.GetAspectByName(rowObject);
            else
                return this.aspectGetter(rowObject);
        }

        /// <summary>
        /// For a given row object, extract the value indicated by the AspectName property of this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>An object, which is the aspect named by AspectName</returns>
        public object GetAspectByName(object rowObject)
        {
            if (string.IsNullOrEmpty(this.aspectName))
                return null;

            //CONSIDER: Should we include NonPublic in this list?
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField;
            object source = rowObject;
            foreach (string property in this.aspectName.Split('.'))
            {
                try
                {
                    source = source.GetType().InvokeMember(property, flags, null, source, null);
                }
                catch (System.MissingMethodException)
                {
                    return String.Format("Cannot invoke '{0}' on a {1}", property, source.GetType());
                }
            }
            return source;
        }

        public void PutValue(Object rowObject, Object newValue)
        {
            if (this.aspectPutter == null)
                this.PutAspectByName(rowObject, newValue);
            else
                this.aspectPutter(rowObject, newValue);
        }

        public void PutAspectByName(Object rowObject, Object newValue)
        {
            if (string.IsNullOrEmpty(this.aspectName))
                return;

            // Navigated through the dotted path until we reach the target object.
            // We then try to set the last property on the dotted path on that target.
            // So, if rowObject is a Person, then an aspect named "HomeAddress.Postcode"
            // will first fetch the "HomeAddress" property, and then try to set the
            // "Postcode" property on the home address object.

            //CONSIDER: Should we include NonPublic in this list?
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField;
            Object target = rowObject;
            List<String> parentProperties = new List<string>(this.aspectName.Split('.'));
            String lastProperty = parentProperties[parentProperties.Count - 1];
            parentProperties.RemoveAt(parentProperties.Count - 1);
            foreach (string property in parentProperties)
            {
                try
                {
                    target = target.GetType().InvokeMember(property, flags, null, target, null);
                }
                catch (System.MissingMethodException)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("Cannot invoke '{0}' on a {1}", property, target.GetType()));
                    return;
                }
            }

            // Now try to set the value
            try
            {
                BindingFlags flags2 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.SetField;
                target.GetType().InvokeMember(lastProperty, flags2, null, target, new Object[] { newValue });
            }
            catch (System.MissingMethodException ex)
            {
                System.Diagnostics.Debug.WriteLine("Invoke PutAspectByName failed:");
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// For a given row object, return the string representation of the value shown in this column.
        /// </summary>
        /// <remarks>
        /// For aspects that are string (e.g. aPerson.Name), the aspect and its string representation are the same.
        /// For non-strings (e.g. aPerson.DateOfBirth), the string representation is very different.
        /// </remarks>
        /// <param name="rowObject"></param>
        /// <returns></returns>
        public string GetStringValue(object rowObject)
        {
            return this.ValueToString(this.GetValue(rowObject));
        }

        /// <summary>
        /// Convert the aspect object to its string representation.
        /// </summary>
        /// <remarks>
        /// If the column has been given a ToStringDelegate, that will be used to do
        /// the conversion, otherwise just use ToString(). Nulls are always converted
        /// to empty strings.
        /// </remarks>
        /// <param name="value">The value of the aspect that should be displayed</param>
        /// <returns>A string representation of the aspect</returns>
        public string ValueToString(object value)
        {
            // CONSIDER: Should we give aspect-to-string converters a chance to work on a null value?
            if (value == null)
                return "";

            if (this.aspectToStringConverter != null)
                return this.aspectToStringConverter(value);

            string fmt = this.AspectToStringFormat;
            if (String.IsNullOrEmpty(fmt))
                return value.ToString();
            else
                return String.Format(fmt, value);
        }

        /// <summary>
        /// For a given row object, return the image selector of the image that should displayed in this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>int or string or Image. int or string will be used as index into image list. null or -1 means no image</returns>
        public Object GetImage(object rowObject)
        {
            if (this.imageGetter != null)
                return this.imageGetter(rowObject);

            if (!String.IsNullOrEmpty(this.ImageKey))
                return this.ImageKey;

            return this.ImageIndex;
        }

        /// <summary>
        /// For a given row object, return the object that is the key of the group that this row belongs to.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>Group key object</returns>
        public object GetGroupKey(object rowObject)
        {
            if (this.groupKeyGetter == null)
            {
                object key = this.GetValue(rowObject);
                if (key is string && this.UseInitialLetterForGroup)
                {
                    String keyAsString = (String)key;
                    if (keyAsString.Length > 0)
                        key = keyAsString.Substring(0, 1).ToUpper();
                }
                return key;
            }
            else
                return this.groupKeyGetter(rowObject);
        }

        /// <summary>
        /// For a given group value, return the string that should be used as the groups title.
        /// </summary>
        /// <param name="value">The group key that is being converted to a title</param>
        /// <returns>string</returns>
        public string ConvertGroupKeyToTitle(object value)
        {
            if (this.groupKeyToTitleConverter == null)
                return this.ValueToString(value);
            else
                return this.groupKeyToTitleConverter(value);
        }

        #region Utilities

        /// <summary>
        /// Install delegates that will group the columns aspects into progressive partitions.
        /// If an aspect is less than value[n], it will be grouped with description[n].
        /// If an aspect has a value greater than the last element in "values", it will be grouped
        /// with the last element in "descriptions".
        /// </summary>
        /// <param name="values">Array of values. Values must be able to be
        /// compared to the aspect (using IComparable)</param>
        /// <param name="descriptions">The description for the matching value. The last element is the default description.
        /// If there are n values, there must be n+1 descriptions.</param>
        /// <example>
        /// this.salaryColumn.MakeGroupies(
        ///     new UInt32[] { 20000, 100000 },
        ///     new string[] { "Lowly worker",  "Middle management", "Rarified elevation"});
        /// </example>
        public void MakeGroupies<T>(T[] values, string[] descriptions)
        {
            if (values.Length + 1 != descriptions.Length)
                throw new ArgumentException("descriptions must have one more element than values.");

            // Install a delegate that returns the index of the description to be shown
            this.GroupKeyGetter = delegate(object row)
            {
                Object aspect = this.GetValue(row);
                if (aspect == null || aspect == System.DBNull.Value)
                    return -1;
                IComparable comparable = (IComparable)aspect;
                for (int i = 0; i < values.Length; i++)
                {
                    if (comparable.CompareTo(values[i]) < 0)
                        return i;
                }

                // Display the last element in the array
                return descriptions.Length - 1;
            };

            // Install a delegate that simply looks up the given index in the descriptions.
            this.GroupKeyToTitleConverter = delegate(object key)
            {
                if ((int)key < 0)
                    return "";

                return descriptions[(int)key];
            };
        }

        #endregion

        #region Private Variables

        private string aspectName;
        private string aspectToStringFormat;
        private bool useInitialLetterForGroup;
        private AspectGetterDelegate aspectGetter;
        private AspectPutterDelegate aspectPutter;
        private AspectToStringConverterDelegate aspectToStringConverter;
        private ImageGetterDelegate imageGetter;
        private GroupKeyGetterDelegate groupKeyGetter;
        private GroupKeyToTitleConverterDelegate groupKeyToTitleConverter;
        private RenderDelegate rendererDelegate;


        #endregion

    }

}
