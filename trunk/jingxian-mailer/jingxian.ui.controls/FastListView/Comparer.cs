
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
    /// This comparer sort list view groups.
    /// It does this on the basis of the values in the Tags, if we can figure out how to compare
    /// objects of that type. Failing that, it uses a case insensitive compare on the group header.
    /// </summary>
    internal class ListViewGroupComparer : IComparer<ListViewGroup>
    {
        public ListViewGroupComparer(SortOrder order)
        {
            this.sortOrder = order;
        }

        public int Compare(ListViewGroup x, ListViewGroup y)
        {
            // If we know how to compare the tags, do that.
            // Otherwise do a case insensitive compare on the group header.
            // We have explicitly catch the "almost-null" value of DBNull.Value,
            // since comparing to that value always produces a type exception.
            int result;
            IComparable comparable = x.Tag as IComparable;
            if (comparable != null && y.Tag != System.DBNull.Value)
                result = comparable.CompareTo(y.Tag);
            else
                result = String.Compare(x.Header, y.Header, true);

            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;

            return result;
        }

        private SortOrder sortOrder;
    }

    /// <summary>
    /// ColumnComparer is the workhorse for all comparison between two values of a particular column.
    /// If the column has a specific comparer, use that to compare the values. Otherwise, do
    /// a case insensitive string compare of the string representations of the values.
    /// </summary>
    /// <remarks><para>This class inherits from both IComparer and its generic counterpart
    /// so that it can be used on untyped and typed collections.</para></remarks>
    internal class ColumnComparer : IComparer, IComparer<OLVListItem>
    {
        public ColumnComparer(OLVColumn col, SortOrder order)
        {
            this.column = col;
            this.sortOrder = order;
            this.secondComparer = null;
        }

        public ColumnComparer(OLVColumn col, SortOrder order, OLVColumn col2, SortOrder order2)
            : this(col, order)
        {
            // There is no point in secondary sorting on the same column
            if (col != col2)
                this.secondComparer = new ColumnComparer(col2, order2);
        }

        public int Compare(object x, object y)
        {
            return this.Compare((OLVListItem)x, (OLVListItem)y);
        }

        public int Compare(OLVListItem x, OLVListItem y)
        {
            int result = 0;
            object x1 = this.column.GetValue(x.RowObject);
            object y1 = this.column.GetValue(y.RowObject);

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
        private ColumnComparer secondComparer;
    }
}
