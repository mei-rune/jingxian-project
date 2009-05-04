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
    public partial class ObjectListView
    {
        private String emptyListMsg = "";
        private Font emptyListMsgFont;

        /// <summary>
        /// If there are no items in this list view, what message should be drawn onto the control?
        /// </summary>
        [Category("Appearance"),
         Description("When the list has no items, show this message in the control"),
         DefaultValue("")]
        public String EmptyListMsg
        {
            get { return emptyListMsg; }
            set
            {
                if (emptyListMsg != value)
                {
                    emptyListMsg = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// What font should the 'list empty' message be drawn in?
        /// </summary>
        [Category("Appearance"),
        Description("What font should the 'list empty' message be drawn in?"),
        DefaultValue(null)]
        public Font EmptyListMsgFont
        {
            get { return emptyListMsgFont; }
            set { emptyListMsgFont = value; }
        }

        /// <summary>
        /// Return the font for the 'list empty' message or a default
        /// </summary>
        [Browsable(false)]
        public Font EmptyListMsgFontOrDefault
        {
            get
            {
                if (this.EmptyListMsgFont == null)
                    return new Font("Tahoma", 14);
                else
                    return this.EmptyListMsgFont;
            }
        }

        /// <summary>
        /// Does this listview have a message that should be drawn when the list is empty?
        /// </summary>
        [Browsable(false)]
        public bool HasEmptyListMsg
        {
            get { return !string.IsNullOrEmpty(this.EmptyListMsg); }
        }

        #region Empty List Msg handling

        /// <summary>
        /// Perform any steps needed before painting the control
        /// </summary>
        protected void HandlePrePaint()
        {
            // When we get a WM_PAINT msg, remember the rectangle that is being updated.
            // We can't get this information later, since the BeginPaint call wipes it out.
            this.lastUpdateRectangle = NativeMethods.GetUpdateRect(this);

            // When the list is empty, we want to handle the drawing of the control by ourselves.
            // Unfortunately, there is no easy way to tell our superclass that we want to do this.
            // So we resort to guile and deception. We validate the list area of the control, which
            // effectively tells our superclass that this area does not need to be painted.
            // Our superclass will then not paint the control, leaving us free to do so ourselves.
            // Without doing this trickery, the superclass will draw the
            // list as empty, and then moments later, we will draw the empty msg, giving a nasty flicker
            if (this.GetItemCount() == 0 && this.HasEmptyListMsg)
                NativeMethods.ValidateRect(this, this.ClientRectangle);
        }

        /// <summary>
        /// Perform any steps needed after painting the control
        /// </summary>
        protected void HandlePostPaint()
        {
            // If the list isn't empty or there isn't an emptyList msg, do nothing
            if (this.GetItemCount() != 0 || !this.HasEmptyListMsg)
                return;

            // Draw the empty list msg centered in the client area of the control
            using (BufferedGraphics buffered = BufferedGraphicsManager.Current.Allocate(this.CreateGraphics(), this.ClientRectangle)) {
                Graphics g = buffered.Graphics;
                g.Clear(this.BackColor);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                g.DrawString(this.EmptyListMsg, this.EmptyListMsgFontOrDefault, SystemBrushes.ControlDark, this.ClientRectangle, sf);
                buffered.Render();
            }
        }

		#endregion

    }
}
