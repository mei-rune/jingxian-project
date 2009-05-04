using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace jingxian.ui.controls.renderer
{
    public class SimpleColorItem //: IImageComboBoxItem
    {
        private Color _color;
        private string _name;

        public SimpleColorItem(Color clr, string name)
        {
            _color = clr;
            _name = name;
        }

        public override int GetHashCode()
        {
            return _color.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SimpleColorItem)
            {
                SimpleColorItem item = (SimpleColorItem)obj;

                if (item.Color == Color) return true;                   
            }
            return base.Equals(obj);
        }

	    public Color Color
	    {
		    get { return _color;}
		    set { _color = value;}
	    }
	
        public Image Image
        {
            get
            {
                Bitmap bmp = new Bitmap(16, 16);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    MyRenderer.DrawGradientBack(g, new Rectangle(2, 2, 12, 12), Color);
                    MyRenderer.DrawGradientBorder(g, new Rectangle(2, 2, 12, 12), Color);                   
                }

                return bmp;
            }
        }

        public string Text
        {
            get {
                return _name;          
            }
        } 
    }

    public class DefaultColorHelper
    {
        static Color[] _colorTable = new Color[]
        {
            Color.FromArgb(0xE88651),
            Color.FromArgb(0x738AC8),
            Color.FromArgb(0x6BB76D),
            Color.FromArgb(0xC64847),
            Color.FromArgb(0xF0AD00),
            Color.FromArgb(0xAC66BB),
            Color.FromArgb(0x0F6FC6),
            Color.FromArgb(0x7CCA62)
        };

        static string[] _colorNames = new string[]{
            "Orange", 
            "Light blue", 
            "Light green", 
            "Red", 
            "Gold",
            "Mauve", 
            "Blue", 
            "Green"
        };

        static DefaultColorHelper(){
            for (int i = 0; i < _colorTable.Length; i++)
            {
                _colorTable[i] = Color.FromArgb(255, _colorTable[i]);
            }
        }

        public static List<SimpleColorItem> DefaultColorsItems
        {
            get{
                List<SimpleColorItem> lst = new List<SimpleColorItem>();

                for(int i=0; i<_colorTable.Length; i++)
                {
                    SimpleColorItem item = new SimpleColorItem(_colorTable[i], _colorNames[i]);

                    lst.Add(item);
                }

                return lst;
            }
        }

        public static Color[] DefaultColors
        {
            get{
                return _colorTable;
            }
        }

    }
}
