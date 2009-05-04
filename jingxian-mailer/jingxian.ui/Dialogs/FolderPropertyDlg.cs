using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace jingxian.ui.dialogs
{
    using jingxian.data;
    using jingxian.ui.controls;
    using Empinia.Core.Runtime;
    using Empinia.UI;

    public partial class FolderPropertyDlg : System.Windows.Forms.Form
    {
        FolderWithBLOBs _folder;
        IExtensionRegistry _extensionRegistry;
        IHierarchyDecoratorService _contentService;
        IIconResourceService _iconResourceService;
        IVirtualFileSystem _virtualFs;

        public FolderPropertyDlg(IExtensionRegistry extensionRegistry
            , IHierarchyDecoratorService contentService
            , IIconResourceService iconResourceService
            , IVirtualFileSystem virtualFs
            , FolderWithBLOBs folder)
        {
            _extensionRegistry = extensionRegistry;
            _contentService = contentService;
            _iconResourceService = iconResourceService;
            _virtualFs = virtualFs;

            InitializeComponent();
            this.archivingSettingPane.Initialize(extensionRegistry);


            foreach (string path in System.IO.Directory.GetFiles(_virtualFs.GetBinPath("icons")))
            {
                try
                {
                    Bitmap img = new Bitmap( Image.FromFile(path) );
                    img.MakeTransparent();
                    string imageFile = "$(file)\\icons\\" + System.IO.Path.GetFileName(path);

                    iconComboBox.Add(imageFile, img);
                }
                catch 
                { }
            }


            string[] icons = new string[]{"jingxian.ui.icons.edit.png",
                                          "jingxian.folder.attachments.png",
                                          "jingxian.folder.calendar.png",
                                          "jingxian.folder.draft.png",
                                          "jingxian.folder.dynamicfolder.png",
                                          "jingxian.folder.inbox.png",
                                          "jingxian.folder.junk.png",
                                          "jingxian.folder.mailinglist.png",
                                          "jingxian.folder.newsgroup.png",
                                          "jingxian.folder.note.png",
                                          "jingxian.folder.outbox.png",
                                          "jingxian.folder.sent.png",
                                          "jingxian.folder.standard.png",
                                          "jingxian.folder.today.png",
                                          "jingxian.folder.trash.png" };

            foreach (string path in icons)
            {
                try
                {
                    Bitmap img = _iconResourceService.GetBitmap (path);
                    img.MakeTransparent();
                    iconComboBox.Add( path , img );
                }
                catch
                { }
            }

            //foreach (object obj in _contentService.GetChildren(_iconResourceService))
            //{
            //    PropertyContentProvider contentProvider = obj as PropertyContentProvider;
            //    if (null == contentProvider || "allIcons" != contentProvider.Name )
            //        continue;                
            //}


            if (null == folder)
                return;

            _folder = folder;
            this.txtFolderCaption.Text = folder.Name;
            this.iconComboBox.Select(folder.Icon);

            this.archivingSettingPane.Time = _folder.Misc["Timeout"];
            this.archivingSettingPane.Action = _folder.Misc["ActionOfTimeout"];
        }

        public FolderWithBLOBs GetResult()
        {
            FolderWithBLOBs folder = _folder ?? new FolderWithBLOBs();
            folder.Name = this.txtFolderCaption.Text;
            folder.Icon = iconComboBox.SelectedImagePath;


            folder.Misc["Timeout"] = this.archivingSettingPane.Time;
            folder.Misc["ActionOfTimeout"] = this.archivingSettingPane.Action;

            return folder;
        }

        private void btnBrowserForIcon_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.gif;*.png|All files|*.*";
            ofd.CheckFileExists = true;
            ofd.DereferenceLinks = true;

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Image img = Image.FromFile(ofd.FileName);
                string path = _virtualFs.GetBinPath("icons");
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                string imageFile = Guid.NewGuid().ToString() + System.IO.Path.GetExtension( ofd.FileName );
                System.IO.File.Copy( ofd.FileName, System.IO.Path.Combine( path, imageFile ) );
                imageFile = "$(file)\\icons\\" + imageFile;
                iconComboBox.Add( imageFile , img);
                iconComboBox.Select(imageFile);
            }
            catch
            {
                MessageBox.Show("无效图片 - " + ofd.FileName);
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (string.IsNullOrEmpty(this.txtFolderCaption.Text))
            {
                Utils.ShowMsgBox("名称不能为空!");
                return;
            }

            Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

