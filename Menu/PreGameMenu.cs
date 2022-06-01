using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace PathFindingAlgo
{
    public class PreGameMenu: SmoothPanel
    {
        MainDisplay Display;
        readonly Brush LSBBrush = new SolidBrush(Color.LightSteelBlue);
        readonly Brush WBrush = new SolidBrush(Color.White);
        readonly Brush BBrush = new SolidBrush(Color.Black);
        readonly Brush DSGBrush = new SolidBrush(Color.DarkSlateGray);

        readonly Font TitleFont = new Font("Times New Roman", 48, FontStyle.Bold);
        readonly Font ExplanationFont = new Font("Times New Roman", 24, FontStyle.Bold);

        readonly Rectangle AstarButton = new Rectangle(470, 400, 280, 50);
        readonly Rectangle VisualizeButton = new Rectangle(350, 475, 280, 50);
        readonly Rectangle DijkstraButton = new Rectangle(470, 550, 280, 50);
        readonly Rectangle StartButton = new Rectangle(400, 650, 300, 100);

        bool Astar = true;
        bool Visualize = true;
        bool Dijkstra = false;
        public PreGameMenu(Rectangle PanelSize, MainDisplay di)
        {
            Display = di;

            this.Location = PanelSize.Location;
            this.Size = PanelSize.Size;
            this.BackColor = Color.DarkSlateGray;
            this.MouseClick += MClick;
            this.Paint += Draw;

            this.Invalidate();
        }

        private void MClick(object obj, MouseEventArgs mea)
        {
            if (AstarButton.Contains(new Point(mea.X, mea.Y)))
            {   Astar = true;
                Dijkstra = false;}
            else if (VisualizeButton.Contains(new Point(mea.X, mea.Y))) 
                Visualize = !Visualize;
            else if (DijkstraButton.Contains(new Point(mea.X, mea.Y)))
            {   Astar = false;
                Dijkstra = true;}
            else if (StartButton.Contains(new Point(mea.X, mea.Y)))
            {
                Display.Visualize = Visualize;
                Display.UseAstar = Astar;
                Display.SwitchToStage("InGame");
            }
            this.Invalidate();
        }

        private void Draw(object obj, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //Title
            g.DrawString("Path Finding Algorithm", TitleFont, WBrush, new Point(50, 10));
            g.DrawLine(new Pen(Color.White), new Point(0, 100), new Point(this.Size.Width, 100));

            //Explanation box
            Color LineColor = Color.DarkSlateGray;
            Visuals.FillRoundedBox(new Rectangle(-25, 110, 435, 190), WBrush, g, 25);
            g.DrawString("A program to visualize", ExplanationFont, BBrush, new Point(25, 120));
            g.DrawLine(new Pen(LineColor), new Point(0, 157), new Point(395, 158));
            g.DrawString("how a path searching", ExplanationFont, BBrush, new Point(25, 160));
            g.DrawLine(new Pen(LineColor), new Point(0, 197), new Point(395, 198));
            g.DrawString("algorithm works,", ExplanationFont, BBrush, new Point(25, 200));
            g.DrawLine(new Pen(LineColor), new Point(0, 237), new Point(395, 238));
            g.DrawString("and how it's implemented.", ExplanationFont, BBrush, new Point(25, 240));
            g.DrawLine(new Pen(LineColor), new Point(0, 277), new Point(395, 278));

            //Options and Start
            Visuals.FillRoundedBox(new Rectangle(300, 375, 800, 400), WBrush, g, 25);

            Visuals.DrawRoundedBox(AstarButton, DSGBrush, WBrush, g, 25, 4);
            g.DrawString("A Star", ExplanationFont, BBrush, new Point(480, 407));
            if (Astar) { g.DrawLine(new Pen(Color.DarkSlateGray, 4), new Point(702, 402), new Point(742, 442));
                         g.DrawLine(new Pen(Color.DarkSlateGray, 4), new Point(702, 448), new Point(742, 408));}
            Visuals.DrawRoundedBox(VisualizeButton, DSGBrush, WBrush, g, 25, 4);
            g.DrawString("Visualize", ExplanationFont, BBrush, new Point(480, 482));
            if (Visualize) { g.DrawLine(new Pen(Color.DarkSlateGray, 4), new Point(358, 483), new Point(398, 523));
                             g.DrawLine(new Pen(Color.DarkSlateGray, 4), new Point(358, 518), new Point(398, 477));}
            Visuals.DrawRoundedBox(DijkstraButton, DSGBrush, WBrush, g, 25, 4);
            g.DrawString("Dijkstra", ExplanationFont, BBrush, new Point(480, 555));
            if (Dijkstra) { g.DrawLine(new Pen(Color.DarkSlateGray, 4), new Point(702, 552), new Point(742, 592));
                            g.DrawLine(new Pen(Color.DarkSlateGray, 4), new Point(702, 598), new Point(742, 558));}

            Visuals.FillRoundedBox(StartButton, DSGBrush, g, 25);
            g.DrawString("START", TitleFont, WBrush, new Point(430, 665));

            //Author
            Visuals.FillRoundedBox(new Rectangle(400, 850, 300, 100), WBrush, g, 25);
            g.DrawString("Just Hogenelst", ExplanationFont, BBrush, new Point(440, 865));
        }
    }
}
