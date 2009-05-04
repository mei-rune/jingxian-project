using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace jingxian.ui.controls
{
    public partial class MessageViewer : UserControl
    {
        bool ShowAllAddress = false;

        public MessageViewer()
        {
            InitializeComponent();
        }


        //private string GenerateHeaderHtml(string caption
        //    , string captionurl
        //    , MailAddress[] addresses
        //    , string linkIdIfNotPresent
        //    , int limit)
        //{
        //    StringBuilder htmlHeader = new StringBuilder();

        //    if (captionurl != null && captionurl != "")
        //    {
        //        htmlHeader.Append("<br/><a href='" + captionurl + "'><b>" + QuickHtml.HTMLParser.XMLize(caption) + "</b></a>");
        //    }
        //    else
        //    {
        //        htmlHeader.Append("<br/><b>" + QuickHtml.HTMLParser.XMLize(caption) + "</b>");
        //    }

        //    bool bFirst = true;
        //    int addresscount = limit;
        //    if (ShowAllAddress)
        //        addresscount = 1000;



        //    for(int i = 0; i < Math.Min(addresscount, addresses.Length); i++)
        //    {
        //        if (addresses[i] == null)
        //            continue;

        //        MailAddress address = addresses[i];
        //        if (bFirst == false)
        //        {
        //            htmlHeader.Append(", ");
        //            bFirst = false;
        //        }

        //        htmlHeader.Append("<tooltip text='" + QuickHtml.HTMLParser.XMLize(address.DisplayName) + "'>");

        //        if (linkIdIfNotPresent != "" && linkIdIfNotPresent != null)
        //        {
        //            htmlHeader.Append("<a href='" + QuickHtml.HTMLParser.XMLize(linkIdIfNotPresent) 
        //                + ":" + QuickHtml.HTMLParser.XMLize(address.DisplayName) + "'>");
        //        }
        //        else
        //        {
        //            htmlHeader.Append("<a href='" + QuickHtml.HTMLParser.XMLize(address.DisplayName) + "'>");
        //        }

        //        htmlHeader.Append(QuickHtml.HTMLParser.XMLize(address.DisplayName) + "</a>");
        //        htmlHeader.Append("</tooltip>");
        //    }

        //    if (addresses.Length > addresscount)
        //    {
        //        htmlHeader.Append(", <a href='more'>...</a>");
        //    }

        //    return htmlHeader.ToString();
        //}

        //private string GenerateHeaderHtml()
        //{
        //    StringBuilder htmlHeader = new StringBuilder();

        //    htmlHeader.Append("<font size='13'>" + QuickHtml.HTMLParser.GenerateHtmlStringWithTooltip(Message.Subject, 82) + "</font>");

        //    htmlHeader.Append("<br/><font size='8' color='#444444'>" + Message.HeaderDate.ToString() + "</font>");


        //    if (Message.From != null)
        //    {
        //        htmlHeader.Append(GenerateHeaderHtml("From:", "from", new JingXian.Utils.MailAddress[] { Message.From }, "", "", 8));
        //    }

        //    htmlHeader.Append(GenerateHeaderHtmlAddresses(8));

        //    htmlHeader.Append(Message.CustomHeader);

        //    return htmlHeader.ToString();
        //}

        //private string GenerateHeaderHtmlAddresses(int limit)
        //{
        //    StringBuilder htmlHeader = new StringBuilder();
        //    if (Message.To.Length > 0)
        //    {
        //        htmlHeader.Append(GenerateHeaderHtml("To:", "to", Message.To, "", "", limit));
        //    }

        //    if (Message.CC.Length > 0)
        //    {
        //        htmlHeader.Append(GenerateHeaderHtml("CC:", "cc", Message.CC, "", "", limit));
        //    }

        //    if (Message.BCC.Length > 0)
        //    {
        //        htmlHeader.Append(GenerateHeaderHtml("BCC:", "bcc", Message.BCC, "", "", limit));
        //    }

        //    return htmlHeader.ToString();
        //}

        void OnLinkClicked(Control owner, string url, Rectangle bounds)
        {
            //if (url == "to")
            //{
            //    ContextMenuStrip mnu = ContactAdapter.CreateMenuForMailAddresses(FindForm(), Message.To);

            //    mnu.Show(owner, new Point(bounds.Left, bounds.Bottom));
            //}
            //else if (url == "from")
            //{
            //    ContextMenuStrip mnu = ContactAdapter.CreateMenuForMailAddresses(FindForm(), new MailAddress[] { Message.From });

            //    mnu.Show(owner, new Point(bounds.Left, bounds.Bottom));
            //}
            //else if (url == "cc")
            //{
            //    ContextMenuStrip mnu = ContactAdapter.CreateMenuForMailAddresses(FindForm(), Message.CC);

            //    mnu.Show(owner, new Point(bounds.Left, bounds.Bottom));
            //}
            //else if (url == "bcc")
            //{
            //    ContextMenuStrip mnu = ContactAdapter.CreateMenuForMailAddresses(FindForm(), Message.BCC);

            //    mnu.Show(owner, new Point(bounds.Left, bounds.Bottom));
            //}
            //else if (url == "more")
            //{
            //    ShowMoreBallon(Message.To);

            //}
            //else if (url.StartsWith("http"))
            //{
            //    Process.Start(url);
            //}
            //else
            //{ // clicked on a contact
            //    MailAddress ma = new MailAddress(url);

            //    ContextMenuStrip mnu = ContactAdapter.CreateMenuForMailAddresses(FindForm(), new MailAddress[] { ma });

            //    mnu.Show(owner, new Point(bounds.Left, bounds.Bottom));
            //}
        }
		
    }
}
