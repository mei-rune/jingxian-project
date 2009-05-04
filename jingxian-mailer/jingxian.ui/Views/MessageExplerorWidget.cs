using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace jingxian.ui.views
{
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.Xml;
    using Empinia.Core.Runtime.Filters;
    using Empinia.UI;
    using Empinia.UI.Workbench;
    using Empinia.UI.Navigator;

    using jingxian.data;
    using jingxian.domainModel;

    using jingxian.ui.controls.XPTable.Models;
    using jingxian.ui.controls.XPTable.Events;


    public partial class MessageExplorerWidget : UserControl
    {
        #region Cells

        public class MessageRow : Row
        {
            Message _message;
            public MessageRow(Message message)
            {
                _message = message;
            }

            public Message Instance
            {
                get { return _message; }
            }
        }

        public abstract class MessageCell : Cell
        {
            private readonly IDbSessionFactory m_sessionFactory;
            protected readonly Image[] _images;
            protected readonly Message _message;

            public MessageCell(IDbSessionFactory sessionFactory, Message message, Image[] images)
                : base("")
            {
                m_sessionFactory = sessionFactory;
                _images = images;
                _message = message;
                Refresh();
            }



            public void UpdateDB(string statement)
            {
                using (IDbSession session = m_sessionFactory.NewSession())
                using (TransactionScope transaction = new TransactionScope(session))
                {
                    session.Update(statement, _message);
                    transaction.VoteCommit();
                }
            }

            public abstract void Click();
            public abstract void Refresh();
        }

        public class MailCell : MessageCell
        {
            public MailCell(IDbSessionFactory sessionFactory, Message message, Image[] images)
                : base(sessionFactory, message, images)
            {
            }

            public override void Refresh()
            {
                base.Data = _message.InfoRead > 0 ? 1 : 0;
                base.Image = _images[_message.InfoRead > 0 ? 1 : 0];
            }

            public override void Click()
            {
            }
        }

        public class ReadStateCell : MessageCell
        {
            List<MessageCell> _cells = new List<MessageCell>();

            public ReadStateCell(IDbSessionFactory sessionFactory, Message message, Image[] images, params MessageCell[] cells)
                : base(sessionFactory, message, images)
            {
                _cells.AddRange(cells);
            }

            public IList<MessageCell> Cells
            {
                get { return _cells; }
            }

            public override void Refresh()
            {
                if (null != Row)
                    Row.Font = new Font(Row.Font, (_message.InfoRead <= 0) ? FontStyle.Bold : FontStyle.Regular);
                base.Image = _images[_message.InfoRead > 0 ? 1 : 0];
                foreach (MessageCell cell in _cells)
                {
                    cell.Refresh();
                }
            }

            public override void Click()
            {
                switch (_message.InfoRead)
                {
                    case 0:
                        _message.InfoRead = 1;
                        break;
                    default:
                        _message.InfoRead = 0;
                        break;
                }
                UpdateDB("UpdateMessageForReadState");
                Refresh();
            }
        }

        public class LevelCell : MessageCell
        {
            public LevelCell(IDbSessionFactory sessionFactory, Message message, Image[] images)
                : base(sessionFactory, message, images)
            {
            }

            public override void Refresh()
            {
                switch (_message.Importance)
                {
                    case 0:
                        base.Image = _images[0];
                        break;
                    case 1:
                        base.Image = _images[1];
                        break;
                    default:
                        if (null != Row)
                            Row.ForeColor = Color.Red;
                        base.Image = _images[2];
                        break;
                }
            }

            public override void Click()
            {
                switch (_message.Importance)
                {
                    case 0:
                        _message.Importance++;
                        break;
                    case 1:
                        _message.Importance++;
                        break;
                    default:
                        _message.Importance = 0;
                        break;
                }
                UpdateDB("UpdateMessageForImportance");
                Refresh();
            }
        }

        public class SpamCell : MessageCell
        {
            public SpamCell(IDbSessionFactory sessionFactory, Message message, Image[] images)
                : base(sessionFactory, message, images)
            {
            }

            public override void Refresh()
            {
                switch (_message.IsSpam)
                {
                    case 0:
                        base.Image = _images[0];
                        break;
                    default:
                        base.Image = _images[1];
                        break;
                }
            }

            public override void Click()
            {
                switch (_message.IsSpam)
                {
                    case 0:
                        _message.IsSpam = 1;
                        break;
                    default:
                        _message.IsSpam = 0;
                        break;
                }
                UpdateDB("UpdateMessageForIsSpam");
                Refresh();
            }
        }

        #endregion

        private readonly IBundleService m_BundleService;
        private readonly IIconResourceService m_IconResourceService;
        private readonly IDbSessionFactory m_sessionFactory;
        private readonly IPartRegistry m_PartRegistry;
        private readonly IToolbarFactory m_ToolStripFactory;
        private readonly IPageService m_PageService;
        private readonly IVirtualFileSystem m_VirtualFileSystem;


        IViewPart m_viewPart;
        Control _control;

        Image m_image_mail;
        Image m_image_open_mail;
        Image m_image_close_mail;
        Image m_image_litter_point;
        Image m_image_big_point;
        Image m_image_spam;
        Image m_image_red_flag;
        Image m_image_gray_flag;
        Image m_image_right;

        public MessageExplorerWidget(IBundleService bundleService
            , IIconResourceService iconResourceService
            , IDbSessionFactory sessionFactory
            , IPartRegistry partRegistry
            , IPageService pageService
            , IVirtualFileSystem virtualFileSystem
            , IViewPart viewPart)
        {
            m_BundleService = bundleService;
            m_IconResourceService = iconResourceService;
            m_sessionFactory = sessionFactory;
            m_PartRegistry = partRegistry;
            m_PageService = pageService;
            m_ToolStripFactory = pageService.PageLayoutProvider.ToolbarFactory;
            m_VirtualFileSystem = virtualFileSystem;
            m_viewPart = viewPart;

            InitializeComponent();



            m_image_mail = iconResourceService.GetImage("jingxian.ui.icons.mail.png");
            m_image_open_mail = iconResourceService.GetIcon("jingxian.ui.icons.maillist.open_mail.ico").ToBitmap();
            m_image_close_mail = iconResourceService.GetIcon("jingxian.ui.icons.maillist.close_mail.ico").ToBitmap();
            m_image_litter_point = iconResourceService.GetImage("jingxian.ui.icons.maillist.litter_point.png");
            m_image_big_point = iconResourceService.GetImage("jingxian.ui.icons.maillist.big_point.png");
            m_image_spam = iconResourceService.GetImage("jingxian.ui.icons.maillist.spam.png");
            m_image_red_flag = iconResourceService.GetImage("jingxian.ui.icons.maillist.red_flag.png");
            m_image_gray_flag = iconResourceService.GetImage("jingxian.ui.icons.maillist.gray_flag.png");
            m_image_right = iconResourceService.GetImage("jingxian.ui.icons.maillist.right.png");


            m_image_open_mail = iconResourceService.GetIcon("jingxian.ui.icons.maillist.open_mail.ico").ToBitmap();
            m_image_close_mail = iconResourceService.GetIcon("jingxian.ui.icons.maillist.close_mail.ico").ToBitmap();
            m_image_litter_point = iconResourceService.GetBitmap("jingxian.ui.icons.maillist.litter_point.png");
            m_image_big_point = iconResourceService.GetBitmap("jingxian.ui.icons.maillist.big_point.png");
            m_image_spam = iconResourceService.GetBitmap("jingxian.ui.icons.maillist.spam.png");
            m_image_red_flag = iconResourceService.GetBitmap("jingxian.ui.icons.maillist.red_flag.png");
            m_image_gray_flag = iconResourceService.GetBitmap("jingxian.ui.icons.maillist.gray_flag.png");
            m_image_right = iconResourceService.GetBitmap("jingxian.ui.icons.maillist.right.png");

            LoadAllMessages();

            string startView = null;

            System.IO.TextReader textReader = new System.IO.StringReader(m_viewPart.ConfigurationXml);
            using (XmlReader reader = XmlReader.Create(textReader, XmlUtils.CreateFragmentReaderSettings(), XmlUtils.CreateParserContext()))
            {
                if( reader.IsStartElement("view") )
                    startView = XmlUtils.ReadRequiredAttributeString(reader, "ref");
            }

            if (string.IsNullOrEmpty(startView))
                return;

            IWorkbenchPart welcomePart;
            m_PartRegistry.TryGet( jingxian.ui.Constants.WelcomeId, out welcomePart );
            if (null == welcomePart)
                return;
            UserControl control = new UserControl();
            control.BorderStyle = BorderStyle.Fixed3D;
            control.Dock = DockStyle.Fill;
            this.m_splitter.Panel2.Controls.Add(control);

            Control welcomePage = welcomePart.Widget;
            welcomePage.Dock = DockStyle.Fill;
            control.Controls.Add(welcomePage);
        }

        public void LoadAllMessages()
        {
            IList messages = null;
            using (IDbSession session = m_sessionFactory.NewSession())
            {
                messages = session.QueryForList("GetAllMessages", null);
            }

            List<Row> rows = new List<Row>();
            foreach (Message message in messages)
            {
                MessageRow row = new MessageRow( message );
                row.Tag = message;
                MailCell mailCell = new MailCell(m_sessionFactory, message, new Image[] { m_image_close_mail, m_image_open_mail });
                row.Cells.Add(mailCell);
                row.Cells.Add(new LevelCell(m_sessionFactory, message, new Image[] { m_image_right, m_image_gray_flag, m_image_red_flag }));
                row.Cells.Add(new Cell(message.HeaderFrom));
                row.Cells.Add(new ReadStateCell(m_sessionFactory, message, new Image[] { m_image_litter_point, m_image_big_point }, mailCell));
                row.Cells.Add(new Cell(message.HeaderSubject));
                row.Cells.Add(new SpamCell(m_sessionFactory, message, new Image[] { m_image_litter_point, m_image_spam }));
                row.Cells.Add(new Cell(message.ReceivedDate.ToShortTimeString()));
                row.Cells.Add(new Cell(message.HeaderSize));

                rows.Add(row);

            }
            tableModel.Rows.Clear();
            tableModel.Rows.AddRange(rows.ToArray());

            foreach (Row row in tableModel.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    MessageCell messageCell = cell as MessageCell;
                    if (null != messageCell)
                        messageCell.Refresh();
                }
            }
        }




        private void table_SelectionChanged(object sender, SelectionEventArgs e)
        {
            if( 1 != e.NewSelectedIndicies.Length )
                return;

            MessageRow row = e.TableModel.Rows[e.NewSelectedIndicies[0]] as MessageRow;
            if (null == row)
                return;

            m_viewPart.Select(row.Instance);
        }

        private void table_CellClick(object sender, CellMouseEventArgs e)
        {
            MessageCell message = e.Cell as MessageCell;
            if (null != message)
                message.Click();
        }

    }
}
