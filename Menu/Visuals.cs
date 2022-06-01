using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace PathFindingAlgo
{
    internal class Visuals
    {
        public static void FillRoundedBox(Rectangle box, Brush b, Graphics g, int CornerOffset)
        {
            g.FillRectangle(b, new Rectangle(box.X + CornerOffset, box.Y, box.Width - 2 * CornerOffset, box.Height));
            g.FillRectangle(b, new Rectangle(box.X, box.Y + CornerOffset, box.Width, box.Height - 2 * CornerOffset));

            Size CornerSize = new Size(CornerOffset * 2, CornerOffset * 2);
            g.FillEllipse(b, new Rectangle(new Point(box.X, box.Y), CornerSize));
            g.FillEllipse(b, new Rectangle(new Point(box.X + box.Width - CornerOffset * 2, box.Y), CornerSize));
            g.FillEllipse(b, new Rectangle(new Point(box.X + box.Width - CornerOffset * 2 , box.Y + box.Height - CornerOffset * 2), CornerSize));
            g.FillEllipse(b, new Rectangle(new Point(box.X, box.Y + box.Height - CornerOffset * 2), CornerSize));
        }
        public static void DrawRoundedBox(Rectangle box, Brush FrontColor, Brush BackColor, Graphics g, int CornerOffset, int LineWidth)
        {
            FillRoundedBox(box, FrontColor, g, CornerOffset);
            box.Inflate(-LineWidth, -LineWidth);
            FillRoundedBox(box, BackColor, g, CornerOffset -LineWidth);
        }
    }
}
