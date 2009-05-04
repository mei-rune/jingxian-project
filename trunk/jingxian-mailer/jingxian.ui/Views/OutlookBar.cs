using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace jingxian.ui.views
{
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Xml.Serialization;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    using jingxian.ui.controls;

    class OutlookBar : SwitchBar
    {
        private readonly IPageService _pageSvc;
        private readonly IIconResourceService _iconService;
        private readonly IPartRegistry _partRegsistry;

        public OutlookBar(IPageService pageSvc, IIconResourceService iconService,IPartRegistry partRegsistry, IViewPart viewPart)
        {
            _pageSvc = pageSvc;
            _iconService = iconService;
            _partRegsistry = partRegsistry;
            List<string> collection = new List<string>();

            string selected = null;
			System.IO.TextReader textReader = new System.IO.StringReader(viewPart.ConfigurationXml);
            using (XmlReader reader = XmlReader.Create(textReader, XmlUtils.CreateFragmentReaderSettings(), XmlUtils.CreateParserContext()))
            {
                reader.ReadStartElement("viewSet"); //NON-NLS-1

                while (reader.IsStartElement("viewBar")) //NON-NLS-1
                {
                    string id = XmlUtils.ReadAttribute(reader, "ref");
                    collection.Add( id );

                    if (string.IsNullOrEmpty(selected))
                        selected = id;
                    else if (0 == string.Compare("true", XmlUtils.ReadOptionalAttributeString(reader, "selected"), true))
                        selected = id;

                    reader.ReadStartElement();
                }
                reader.ReadEndElement();

                //while (reader.Read())
                //{
                //    if( reader.IsStartElement("viewBar") )
                //        collection.Add( XmlUtils.ReadAttribute(reader, "ref") );
                //}
            }


            foreach (string config in collection)
            {
                IViewPart viewBar = _partRegsistry.Get<IViewPart>(config);
                Add(viewBar.Id, viewBar.Name, _iconService.GetBitmap(viewBar.IconId), viewBar.Widget);

                if (viewBar.Id == selected)
                    Switch(selected);
            }
        }



    }
}
