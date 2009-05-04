
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
    /// OLVListItems are specialized ListViewItems that know which row object they came from,
    /// and the row index at which they are displayed, even when in group view mode. They
    /// also know the image they should draw against themselves
    /// </summary>
    public class OLVListItem : ListViewItem
    {
        /// <summary>
        /// Create a OLVListItem for the given row object
        /// </summary>
        public OLVListItem(object rowObject)
            : base()
        {
            this.rowObject = rowObject;
        }

        /// <summary>
        /// Create a OLVListItem for the given row object, represented by the given string and image
        /// </summary>
        public OLVListItem(object rowObject, string text, Object image)
            : base(text, -1)
        {
            this.rowObject = rowObject;
            this.imageSelector = image;
        }

        /// <summary>
        /// RowObject is the model object that is source of the data for this list item.
        /// </summary>
        public object RowObject
        {
            get { return rowObject; }
            set { rowObject = value; }
        }
        private object rowObject;

        /// <summary>
        /// DisplayIndex is the index of the row where this item is displayed. For flat lists,
        /// this is the same as ListViewItem.Index, but for grouped views, it is different.
        /// </summary>
        public int DisplayIndex
        {
            get { return displayIndex; }
            set { displayIndex = value; }
        }
        private int displayIndex;

        /// <summary>
        /// Get or set the image that should be shown against this item
        /// </summary>
        /// <remarks><para>This can be an Image, a string or an int. A string or an int will
        /// be used as an index into the small image list.</para></remarks>
        public Object ImageSelector
        {
            get { return imageSelector; }
            set
            {
                imageSelector = value;
                if (value is Int32)
                    this.ImageIndex = (Int32)value;
                else if (value is String)
                    this.ImageKey = (String)value;
                else
                    this.ImageIndex = -1;
            }
        }
        private Object imageSelector;
    }

    /// <summary>
    /// A ListViewSubItem that knows which image should be drawn against it.
    /// </summary>
    [Browsable(false)]
    public class OLVListSubItem : ListViewItem.ListViewSubItem
    {
        /// <summary>
        /// Create a OLVListSubItem
        /// </summary>
        public OLVListSubItem()
            : base()
        {
        }

        /// <summary>
        /// Create a OLVListSubItem that shows the given string and image
        /// </summary>
        public OLVListSubItem(string text, Object image)
            : base()
        {
            this.Text = text;
            this.ImageSelector = image;
        }

        /// <summary>
        /// Get or set the image that should be shown against this item
        /// </summary>
        /// <remarks><para>This can be an Image, a string or an int. A string or an int will
        /// be used as an index into the small image list.</para></remarks>
        public Object ImageSelector
        {
            get { return imageSelector; }
            set { imageSelector = value; }
        }
        private Object imageSelector;


        /// <summary>
        /// Return the state of the animatation of the image on this subitem.
        /// Null means there is either no image, or it is not an animation
        /// </summary>
        public ImageRenderer.AnimationState AnimationState
        {
            get { return animationState; }
            set { animationState = value; }
        }
        private ImageRenderer.AnimationState animationState;

    }
}
