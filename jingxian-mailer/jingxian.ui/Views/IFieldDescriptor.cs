using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace jingxian.ui.views
{
    using jingxian.ui.controls.XPTable.Models;

    public class DescriptorEventArgs : EventArgs
    {
        ColumnModelConfiguration _config;
        Column _column;
        IFieldDescriptor _descriptor;

        public Column Column
        {
            get { return _column; }
        }

        public ColumnModelConfiguration Configuration
        {
            get { return _config; }
        }

        public IFieldDescriptor Descriptor
        {
            get { return _descriptor; }
            set { _descriptor = value; }
        }
    }

    public interface IFieldDescriptor
    {
        Column Column { get; }

        Cell Generate(object instance);

        IAccessor Accessor { get; set; }
    }


    public abstract class FieldDescriptor : IFieldDescriptor
    {
        protected Column _column;
        protected IAccessor _propertyInfo;

        public FieldDescriptor(Column column)
        {
            _column = column;
        }

        public Column Column
        {
            get { return _column; }
        }

        public IAccessor Accessor
        {
            get { return _propertyInfo; }
            set { _propertyInfo = value; }
        }

        public Type Type
        {
            get { return _propertyInfo.InterfaceType; }
        }

        public Cell Generate(object instance)
        {
            Cell cell = new Cell("");
            cell.PropertyChanged += this.onChanged;
            cell.Data = _propertyInfo.GetValue(instance);
            return cell;
        }

        public abstract void onChanged(object sender, jingxian.ui.controls.XPTable.Events.CellEventArgs e);
    }

    public class TextFieldDescriptor : FieldDescriptor
    {
        public TextFieldDescriptor()
            : base(new TextColumn())
        {
        }

        public override void onChanged(object sender, jingxian.ui.controls.XPTable.Events.CellEventArgs e)
        {
            if (null == e.Cell.Data)
            {
                e.Cell.Text = "";
                return;
            }
            e.Cell.Text = e.Cell.Data.ToString();
        }
    }

    public class ImageFieldDescriptor : FieldDescriptor
    {
        Image[] _images;
        public ImageFieldDescriptor()
            : base(new ImageColumn())
        {
        }

        public Image[] Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public override void onChanged(object sender, jingxian.ui.controls.XPTable.Events.CellEventArgs e)
        {
            if (null == e.Cell.Data)
            {
                e.Cell.Image = _images[0];
                return;
            }


            if (base.Type == typeof(bool))
            {
                e.Cell.Image = ((bool)e.Cell.Data) ? _images[0] : _images[1];
                return;
            }

            if (base.Type == typeof(int))
            {
                e.Cell.Image = _images[(int)e.Cell.Data];
                return;
            }

            e.Cell.Image = _images[0];
        }
    }
}
