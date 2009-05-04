using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace jingxian.ui.controls.renderer
{
    public class MyRenderer : Office2007Renderer
    {
        public MyRenderer()
        {
            ColorTable.UseSystemColors = true;
        }

        public static ProfessionalColorTable ColorsTable
        {
            get
            {
                return Instance.ColorTable;
            }
        }

        private static MyRenderer _instance = new MyRenderer();
        public static MyRenderer Instance
        {
            get
            {
                return _instance;
            }
        }

        public void DoRenderToolStripContentPanelBackground(Graphics g, Rectangle r)
        {


            using (LinearGradientBrush backBrush = new LinearGradientBrush(r,
                                                                               ColorTable.ToolStripContentPanelGradientEnd,
                                                                               ColorTable.ToolStripContentPanelGradientBegin,
                                                                               90f))
            {
                g.FillRectangle(backBrush, r);
            }
        }

        static private void FillWithDoubleGradient(Graphics g, Color beginColor, Color middleColor, Color endColor,
            Rectangle r, LinearGradientMode mode)
        {
            if ((r.Width != 0) && (r.Height != 0))
            {
                Rectangle rectangle1 = r;
                Rectangle rectangle2 = r;

                if (mode == LinearGradientMode.Horizontal)
                {
                    rectangle2.Width = 12;
                    rectangle1.Width = 13;
                    rectangle1.X = r.Right - rectangle1.Width;
                }
                else
                {
                    rectangle2.Height = 12;
                    rectangle1.Height = 13;
                    rectangle1.Y = r.Bottom - rectangle1.Height;
                }

                // need to render three pieces
                if (r.Width > 24)
                {
                    using (Brush brush1 = new SolidBrush(middleColor))
                    {
                        g.FillRectangle(brush1, r);
                    }
                    using (Brush brush2 = new LinearGradientBrush(rectangle2, beginColor, middleColor, mode))
                    {
                        g.FillRectangle(brush2, rectangle2);
                    }
                    using (LinearGradientBrush brush3 = new LinearGradientBrush(rectangle1, middleColor, endColor, mode))
                    {
                        if (mode == LinearGradientMode.Horizontal)
                        {
                            rectangle1.X++;
                            rectangle1.Width--;
                        }
                        else
                        {
                            rectangle1.Y++;
                            rectangle1.Height--;
                        }
                        g.FillRectangle(brush3, rectangle1);
                    }
                }
                else
                {
                    using (Brush b = new LinearGradientBrush(r, beginColor, endColor, mode))
                    {
                        g.FillRectangle(b, r);
                    }
                }
            }
        }

        public static void DrawHeader(Graphics graphics, Rectangle r)
        {

            FillWithDoubleGradient(graphics, ColorsTable.ToolStripGradientBegin, ColorsTable.ToolStripGradientMiddle, ColorsTable.ToolStripGradientEnd, r, LinearGradientMode.Vertical);
        }

        public static void DrawLeftHeader(Graphics graphics, Rectangle r)
        {
            FillWithDoubleGradient(graphics, ColorsTable.ToolStripGradientBegin, ColorsTable.ToolStripGradientMiddle, ColorsTable.ToolStripGradientEnd, r, LinearGradientMode.Horizontal);
        }

        public static void DrawBackground(Graphics graphics, Rectangle r)
        {
            Instance.DoRenderToolStripContentPanelBackground(graphics, r);
        }

        public static void DrawHighlightedItem(Graphics g, Rectangle r)
        {
            Instance.DrawGradientItem(g, r, Office2007Renderer._itemToolItemSelectedColors);
        }

        public static void DrawHighlightedSelectedItem(Graphics g, Rectangle r)
        {
            Instance.DrawGradientItem(g, r, Office2007Renderer._itemToolItemCheckPressColors);
        }

        public static void DrawSelectedItem(Graphics g, Rectangle r)
        {
            Instance.DrawGradientItem(g, r, Office2007Renderer._itemToolItemCheckedColors);
        }

        public static void DrawGradientBackSelected(Graphics g, Rectangle r)
        {
            Instance.DrawGradientBack(g, r, Office2007Renderer._itemToolItemCheckedColors);
        }

        public static void DrawGradientBackHighlightSelected(Graphics g, Rectangle r)
        {
            Instance.DrawGradientBack(g, r, Office2007Renderer._itemToolItemCheckPressColors);
        }

        public static void DrawGradientBackHighlighted(Graphics g, Rectangle r)
        {
            Instance.DrawGradientBack(g, r, Office2007Renderer._itemToolItemSelectedColors);
        }


        public static void DrawGradientBorderSelected(Graphics g, Rectangle r)
        {
            Instance.DrawGradientBorder(g, r, Office2007Renderer._itemToolItemCheckedColors);
        }

        public static void DrawGradientBorderHighlightSelected(Graphics g, Rectangle r)
        {
            Instance.DrawGradientBorder(g, r, Office2007Renderer._itemToolItemCheckPressColors);
        }

        public static void DrawGradientBorderHighlighted(Graphics g, Rectangle r)
        {
            Instance.DrawGradientBorder(g, r, Office2007Renderer._itemToolItemSelectedColors);
        }

        public static void DrawGradientBack(Graphics g, Rectangle outline, Color color)
        {
            GradientItemColors colors = Office2007Renderer._itemToolItemSelectedColors.Clone();

            colors.FillTop1 = AdobeColors.ModifySaturation(color, 0.2);
            colors.FillTop2 = AdobeColors.ModifySaturation(color, 0.4);

            colors.FillTop1 = AdobeColors.ModifyBrightness(colors.FillTop1, 1.1);
            colors.FillTop2 = AdobeColors.ModifyBrightness(colors.FillTop2, 1.1);

            colors.FillBottom1 = color;
            colors.FillBottom2 = AdobeColors.ModifyBrightness(colors.FillTop2, 1.2);

            colors.InsideBottom1 = AdobeColors.ModifyBrightness(colors.FillBottom1, 1.6);
            colors.InsideBottom2 = AdobeColors.ModifyBrightness(colors.FillBottom2, 1.6);
            colors.InsideBottom1 = AdobeColors.ModifySaturation(colors.InsideBottom1, 0.8);
            colors.InsideBottom2 = AdobeColors.ModifySaturation(colors.InsideBottom2, 0.8);

            colors.InsideTop1 = AdobeColors.ModifyBrightness(colors.InsideTop1, 1.3);
            colors.InsideTop2 = AdobeColors.ModifyBrightness(colors.InsideTop2, 1.3);
         
            Instance.DrawGradientBack(g, outline, colors);
        }

        public static void DrawGradientBorder(Graphics g, Rectangle outline, Color color)
        {
            GradientItemColors colors = Office2007Renderer._itemToolItemSelectedColors.Clone();

            colors.FillTop1 = AdobeColors.ModifySaturation(color, 0.2);
            colors.FillTop2 = AdobeColors.ModifySaturation(color, 0.4);
            colors.FillBottom1 = color;
            colors.FillBottom2 = AdobeColors.ModifyBrightness(colors.FillTop2, 1.2);

            Instance.DrawGradientBorder(g, outline, colors); 
        }
    }
}
