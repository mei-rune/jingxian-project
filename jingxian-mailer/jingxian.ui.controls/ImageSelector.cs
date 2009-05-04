using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace jingxian.ui.controls
{
    public class ImageSelector : ComboBox
    {

        public class ImageItem
        {
            Image _image;
            string _text;

            public ImageItem(string text, Image img)
            {
                _image = img;
                _text = text;
            }

            public Image Image
            {
                get { return _image; }
                set { _image = value; }
            }

            public string Id
            {
                get { return _text; }
                set { _text = value; }
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                ImageItem item = obj as ImageItem;
                if (null == item)
                    return false;

                return _text == item._text;
            }

            public override string ToString()
            {
                return Id;
            }
        }

        public ImageSelector()
            : base()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 17;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                base.OnDrawItem( e );
                return;
            }

            ImageItem item = this[e.Index];
            if (item == null)
            {
                base.OnDrawItem(e);
                return;
            }

            e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            e.Graphics.DrawImage(item.Image, new Rectangle(e.Bounds.X, e.Bounds.Y, item.Image.Width, item.Image.Height));

            //e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor), text_rect);
        }

        public string SelectedImagePath
        {
            get 
            {
                ImageItem item = this.SelectedItem as ImageItem;
                return (null == item) ? "" : item.Id;

            }
        }

        public void Add(string id, Image image)
        {
            Items.Add( new ImageItem( id, image ) );
        }

        public void Remove(string id)
        {
            Items.Remove(new ImageItem(id, null));
        }

        public ImageItem this[int index]
        {
            get { return Items[index] as ImageItem; }
        }

        public bool Select(string id)
        {
            foreach (ImageItem item in this.Items)
            {
                if (item.Id == id)
                {
                    this.SelectedItem = item;
                    return true;
                }
            }
            return false;
        }
    }
}
