using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.controls
{
    public partial class SwitchBar : UserControl
    {
        public class View : ToolStripButton
        {
            private string _key;
            private Control _control;

            public View(string text)
                : base(text)
            {
            }

            public string Id
            {
                get { return _key; }
                set { _key = value; }
            }

            public Control RelatedControl
            {
                get { return _control; }
                set { _control = value; }
            }
        }

        static Font _font = new Font(FontFamily.GenericSansSerif, 10);

        public event EventHandler SelectedChanged;

        public SwitchBar()
        {
            InitializeComponent();

            toolStrip.Items.Clear();
        }

        public void Add(string id, string text, Image icon, Control control)
        {
            View view = new View(text);

            view.RelatedControl = control;
            view.Image = icon;
            view.Id = id;
            view.ImageAlign = ContentAlignment.MiddleLeft;

            view.Padding = new Padding(4, 5, 0, 5);
            view.Margin = new Padding(0);

            control.Visible = false;
            control.Dock = DockStyle.Fill;
            this.contentPanel.Controls.Add(control);

            toolStrip.SendToBack();

            view.Click += this.OnClick;

            toolStrip.Items.Add(view );
        }

        public void Switch(string id)
        {
            foreach (View view in toolStrip.Items)
            {
                if (id == view.Id)
                {
                    if (!view.Checked)
                        view.PerformClick();
                }
            }
        }

        View GetCurrentView()
        {
            foreach (View btn in toolStrip.Items)
            {
                if (btn.Checked) return btn;
            }
            return null;
        }

        void OnClick(object sender, EventArgs e)
        {
            View activeView = sender as View;

            foreach (Control c in contentPanel.Controls)
                c.Visible = false;

            foreach (View view in toolStrip.Items)
            {
                if (view == activeView)
                {
                    view.Checked = true;
                    view.RelatedControl.Visible = true;
                }
                else
                {
                    view.Checked = false;
                }
            }

            Rectangle r = ClientRectangle;
            r.Height = 18;

            headerPanel.Invalidate();

            if (SelectedChanged != null)
                SelectedChanged(activeView, new EventArgs());
        }

        void OnLabelPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = headerPanel.ClientRectangle;

            renderer.MyRenderer.DrawGradientBack(e.Graphics, r, renderer.MyRenderer.ColorsTable.ToolStripGradientEnd);
            renderer.MyRenderer.DrawGradientBorder(e.Graphics, r, renderer.MyRenderer.ColorsTable.ToolStripGradientEnd);

            r.Inflate(-3, -1);

            View currentView = GetCurrentView();
            if (currentView != null)
            {
                e.Graphics.DrawImage(currentView.Image, new Rectangle(3, 3, currentView.Image.Width, currentView.Image.Height));

                r.X += 20;
                r.Y += 4;

                e.Graphics.DrawString(currentView.Text, _font, SystemBrushes.WindowText, r);
            }

            base.OnPaint(e);
        }
    }
}