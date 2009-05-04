using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace jingxian.ui
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime.Xml.Serialization;

    public class ColumnModelConfiguration : XmlSerializable
    {
        public const string XmlNamespace = RuntimeConstants.CurrentXmlSchemaNamespace;
        public const string XmlSchemaResource = "jingxian.ui.Schemas.DataExplorerConfiguration.xsd";
        public const string XmlTypeName = "ColumnModelConfiguration";
        public const string ElementName = "column";

   
        string _id;
        string _description;
        string _type;
        string  _text;
        string  _format; 
        string  _alignment;
        int  _width;
        int _contentWidth;
        string  _image;
        bool _imageOnRight;
        bool  _visible;
        bool _sortable;
        bool  _resizable;
        string _defaultComparerType;
        string  _sortOrder;
        bool _editable; 
        bool _enabled;
        bool _selectable;
        string _tooltipText;

        public ColumnModelConfiguration()
        { 
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public string Alignment
        {
            get { return _alignment; }
            set { _alignment = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int ContentWidth
        {
            get { return _contentWidth; }
            set { _contentWidth = value; }
        }

        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }


        public bool ImageOnRight
        {
            get { return _imageOnRight; }
            set { _imageOnRight = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool Sortable
        {
            get { return _sortable; }
            set { _sortable = value; }
        }

        public bool Resizable
        {
            get { return _resizable; }
            set { _resizable = value; }
        }


        //public string DefaultComparerType
        //{
        //    get { return _defaultComparerType; }
        //    set { _defaultComparerType = value; }
        //}

        //public string SortOrder
        //{
        //    get { return _sortOrder; }
        //    set { _sortOrder = value; }
        //}

        public bool Editable
        {
            get { return _editable; }
            set { _editable = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public bool Selectable
        {
            get { return _selectable; }
            set { _selectable = value; }
        }

        public string ToolTipText
        {
            get { return _tooltipText; }
            set { _tooltipText = value ?? ""; }
        }

        protected override string GetXmlSchemaNamespace()
        {
            return RuntimeConstants.CurrentXmlSchemaNamespace;
        }

        protected override string GetXmlTypeName()
        {
            return XmlTypeName;
        }

        protected override string GetXmlElementName()
        {
            return ElementName;
        }

        protected override XmlSchema GetXmlSchema()
        {
			throw new NotImplementedException();
        }

        protected override void ReadXmlAttributes(XmlReader reader)
        {
            _id = reader.GetAttribute("id"); //NON-NLS-1
            _description = reader.GetAttribute("description"); //NON-NLS-1
            _type = reader.GetAttribute("type"); //NON-NLS-1


            Text = XmlUtils.ReadOptionalAttributeString( reader, "text" );
            Format = XmlUtils.ReadOptionalAttributeString( reader, "format" );
            Alignment = XmlUtils.ReadOptionalAttributeString( reader, "alignment" );
            int Width = jingxian.Utils.ReadInt( XmlUtils.ReadOptionalAttributeString( reader, "width" ), -1 );
            int ContentWidth = jingxian.Utils.ReadInt(XmlUtils.ReadOptionalAttributeString( reader, "contentWidth" ), -1 );
            Image = XmlUtils.ReadOptionalAttributeString( reader, "image" );
            ImageOnRight = jingxian.Utils.ReadBoolean(XmlUtils.ReadOptionalAttributeString(reader, "imageOnRight"));
            Visible = jingxian.Utils.ReadBoolean( XmlUtils.ReadOptionalAttributeString( reader, "visible" ));
            Sortable =jingxian.Utils.ReadBoolean( XmlUtils.ReadOptionalAttributeString( reader, "sortable" ));
            Resizable =jingxian.Utils.ReadBoolean( XmlUtils.ReadOptionalAttributeString( reader, "resizable" ));
            //DefaultComparerType = XmlUtils.ReadOptionalAttributeString( reader, "DefaultComparerType" );
            //SortOrder = XmlUtils.ReadOptionalAttributeString( reader, "SortOrder" );
            Editable =jingxian.Utils.ReadBoolean( XmlUtils.ReadOptionalAttributeString( reader, "editable" ));
            Enabled = jingxian.Utils.ReadBoolean(XmlUtils.ReadOptionalAttributeString( reader, "enabled" ));
            Selectable = jingxian.Utils.ReadBoolean(XmlUtils.ReadOptionalAttributeString( reader, "selectable" ) );
            ToolTipText = XmlUtils.ReadOptionalAttributeString( reader, "toolTipText" );

            base.ReadXmlAttributes(reader);
        }

        protected override void WriteXmlAttributes(XmlWriter writer)
        {
            XmlUtils.WriteRequiredAttributeString(writer, "id", _id); //NON-NLS-1
            XmlUtils.TryWriteAttributeString(writer, "descriprion", _description); //NON-NLS-1
            XmlUtils.TryWriteAttributeString(writer, "type", _type); //NON-NLS-1

            if (!string.IsNullOrEmpty(Text))
                XmlUtils.WriteRequiredAttributeString(writer, "text", Text);
            if (!string.IsNullOrEmpty(Format))
                XmlUtils.WriteRequiredAttributeString(writer, "format", Format);
            if (!string.IsNullOrEmpty(Alignment))
                XmlUtils.WriteRequiredAttributeString(writer, "alignment", Alignment);
            if (0 > Width)
                XmlUtils.WriteRequiredAttributeString(writer, "width", Width.ToString());
            if (0 > ContentWidth)
                XmlUtils.WriteRequiredAttributeString(writer, "contentWidth", ContentWidth.ToString());
            if (!string.IsNullOrEmpty(Image))
                XmlUtils.WriteRequiredAttributeString(writer, "image", Image);
            if (!ImageOnRight)
                XmlUtils.WriteRequiredAttributeString(writer, "imageOnRight", ImageOnRight.ToString());
            if (!Visible)
                XmlUtils.WriteRequiredAttributeString(writer, "visible", Visible.ToString());
            if (!Sortable)
                XmlUtils.WriteRequiredAttributeString(writer, "sortable", Sortable.ToString());
            if (!Resizable)
                XmlUtils.WriteRequiredAttributeString(writer, "resizable", Resizable.ToString());
            //if (!string.IsNullOrEmpty(DefaultComparerType))
            //    XmlUtils.WriteRequiredAttributeString(writer, "DefaultComparerType", DefaultComparerType);
            //if (!string.IsNullOrEmpty(SortOrder))
            //    XmlUtils.WriteRequiredAttributeString(writer, "SortOrder", SortOrder);
            if (!Editable) 
                XmlUtils.WriteRequiredAttributeString(writer, "editable", Editable.ToString());
            if (!Enabled) 
                XmlUtils.WriteRequiredAttributeString(writer, "enabled", Enabled.ToString());
            if (!Selectable) 
                XmlUtils.WriteRequiredAttributeString(writer, "selectable", Selectable.ToString());
            if (!string.IsNullOrEmpty(ToolTipText))
                XmlUtils.WriteRequiredAttributeString(writer, "toolTipText", ToolTipText);

            base.WriteXmlAttributes(writer);
        }

        public override string ToString()
        {
            return _id;
        }
    }

    public class TableModelConfiguration : XmlSerializable
    {
        public const string XmlNamespace = RuntimeConstants.CurrentXmlSchemaNamespace;
        public const string XmlSchemaResource = "jingxian.ui.Schemas.DataExplorerConfiguration.xsd";
        public const string XmlTypeName = "TableModelConfiguration";
        public const string ElementName = "table";

        IList<ColumnModelConfiguration> _columns = new List<ColumnModelConfiguration>();


        public TableModelConfiguration()
        { 
        }

        public IList<ColumnModelConfiguration> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        protected override string GetXmlSchemaNamespace()
        {
            return RuntimeConstants.CurrentXmlSchemaNamespace;
        }

        protected override string GetXmlTypeName()
        {
            return XmlTypeName;
        }

        protected override string GetXmlElementName()
        {
            return ElementName;
        }

        protected override XmlSchema GetXmlSchema()
        {
            throw new NotImplementedException();
        }

        protected override void ReadXmlElements(XmlReader reader)
        {
            while (reader.IsStartElement("column"))
            {
                _columns.Add( XmlUtils.ReadElement<ColumnModelConfiguration>(reader , "column" ) );
            }
            base.ReadXmlElements(reader);
        }

        protected override void WriteXmlElements(XmlWriter writer)
        {
            foreach( ColumnModelConfiguration column in _columns )
            {
                XmlUtils.WriteElement<ColumnModelConfiguration>(writer, "column", column );
            }
            base.WriteXmlElements(writer);
        }
    }

    public class DataExplorerConfiguration : XmlSerializable
	{
        TableModelConfiguration _table;
        string _viewId;

		public DataExplorerConfiguration()
		{
		}

        public TableModelConfiguration Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public string viewRef
        {
            get { return _viewId; }
            set { _viewId = value; }
        }

		#region Serialization related

		public const string XmlNamespace = RuntimeConstants.CurrentXmlSchemaNamespace;
        public const string XmlSchemaResource = "jingxian.ui.Schemas.DataExplorerConfiguration.xsd";
		/// <summary>
		/// The XML type name.
		/// </summary>
        public const string XmlTypeName = "DataExplorerConfiguration";

        public const string ElementName = "dataExplorer";


        protected override string GetXmlSchemaNamespace()
        {
            return RuntimeConstants.CurrentXmlSchemaNamespace;
        }

        protected override string GetXmlTypeName()
        {
            return XmlTypeName;
        }

        protected override string GetXmlElementName()
        {
            return ElementName;
        }

        protected override XmlSchema GetXmlSchema()
        {
            throw new NotImplementedException();
        }

        protected override void ReadXmlElements(XmlReader reader)
        {
            _table = XmlUtils.ReadElement<TableModelConfiguration>(reader, "table");

            if (reader.IsStartElement("view"))
                _viewId = XmlUtils.ReadAttribute(reader, "ref");
            
            base.ReadXmlElements(reader);
        }

        protected override void WriteXmlElements(XmlWriter writer)
        {
            XmlUtils.WriteElement<TableModelConfiguration>(writer, "table", _table);
            writer.WriteStartElement("view");
            XmlUtils.WriteAttribute(writer, "ref", _viewId);
            writer.WriteEndElement();

            base.WriteXmlElements(writer);
        }

		#endregion

		public override string ToString()
		{
			return "dataExplorer";
		}
	}
}