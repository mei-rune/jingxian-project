using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace jingxian.ui.controls.models
{

    public interface ICell
    {
        string Text { get; set; }

        string Format { get; set; }

        string Alignment { get; set; }

        int Width { get; set; }

        int ContentWidth { get; set; }

        Image Image { get; set; }

        bool ImageOnRight { get; set; }

        bool Visible { get; set; }

        bool Sortable { get; set; }

        bool Resizable { get; set; }

        bool Editable { get; set; }

        bool Enabled { get; set; }

        bool Selectable { get; set; }

        string ToolTipText { get; set; }
    }
}
