using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace jingxian.ui.controls
{
    public class ObjectSelector : ComboBox
    {

        public class ObjectItem
        {
            object _image;
            string _text;

            public ObjectItem(object img)
                : this( null, img )
            {
                
            }

            public ObjectItem(string text, object img)
            {
                _image = img;

                if (string.IsNullOrEmpty(text))
                    _text = img.ToString();
                else 
                    _text = text;
            }

            public object Instance
            {
                get { return _image; }
                set { _image = value; }
            }

            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            public override int GetHashCode()
            {
                return Text.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                ObjectItem item = obj as ObjectItem;
                if (null == item)
                    return false;

                return _text == item._text;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        public ObjectSelector()
            : base()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 17;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                base.OnDrawItem(e);
                return;
            }

            ObjectItem item = this[e.Index];
            if (item == null)
            {
                base.OnDrawItem(e);
                return;
            }
            e.Graphics.DrawString(item.Text, e.Font, new SolidBrush( Color.Black ), e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        public object Selected
        {
            get 
            {
                ObjectItem item = this.SelectedItem as ObjectItem;
                return (null == item) ? null : item.Instance;
            }
            set
            {
                SelectOjbect(value);
            }
        }

        public void Add( object instance )
        {
            Items.Add(new ObjectItem( instance));
        }

        public void Add(string text, object instance)
        {
            Items.Add(new ObjectItem(text, instance));
        }

        public void Remove(string text)
        {
            Items.Remove(new ObjectItem(text, null));
        }

        public object[] GetObjects()
        {
            List<object> items = new List<object>();

            foreach (ObjectItem item in this.Items)
            {
                if (null != item)
                    items.Add(item.Instance );
            }
            return items.ToArray();
        }

        public ObjectItem this[int index]
        {
            get { return Items[index] as ObjectItem; }
        }

        public bool SelectText(string text)
        {
            foreach (ObjectItem item in this.Items)
            {
                if (null != item && item.Text == text)
                {
                    this.SelectedItem = item;
                    return true;
                }
            }
            return false;
        }
        public bool SelectOjbect(object value)
        {
            if (null == value)
                return false;

            for (int i = 0; i < this.Items.Count; i++)
            {
                ObjectItem item = this.Items[i] as ObjectItem;
                if (null != item && value.Equals(item.Instance))
                {
                    this.SelectedItem = item;
                    return true;
                }
            }
            return false;
        }
    }
}
