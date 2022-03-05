using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PathFindingAlgo
{
    /// <summary>
    /// A panel containing buttons to interact with the board.
    /// </summary>
    public class MenuBar : SmoothPanel
    {
        List<MenuButton> ButtonList;
        Board ABoard;

        /// <summary>
        /// A panel containing buttons to interact with the board.
        /// </summary>
        /// <param name="BarSize">The upper bar of the form</param>
        /// <param name="aboard">The Board to be interacted with</param>
        public MenuBar(Rectangle BarSize,Board aboard)
        {
            ABoard = aboard;
            this.Location = BarSize.Location;
            this.Size = BarSize.Size;

            InitializeButtons();
            this.MouseMove += MMove;
            this.MouseClick += MClick;
            this.Paint += PaintMenuBarButtons;
        }

        /// <summary>
        /// Initializes the buttons and adds them to the ButtonList
        /// Button Layout:
        ///     (1) Save (2) Load (3) Grid (4) Dijkstra (5) Reset (6) Randomize (7) Enlarge (8) Shrink.
        /// </summary>
        private void InitializeButtons()
        {
            Size button_size = new Size(80, 80);
            string type = "Rounded Box";
            ButtonList = new List<MenuButton>
            {
                new SaveButton(Brushes.LightGray, new Point(10, 10), button_size, type, this, ABoard),
                new LoadButton(Brushes.LightGreen, new Point(110, 10), button_size, type, this, ABoard),
                new GridButton(Brushes.LightBlue, new Point(210, 10), button_size, type, this, ABoard),
                new AstarButton(Brushes.MediumPurple, new Point(310, 10), button_size, type, this, ABoard),
                new ResetButton(Brushes.LightSeaGreen, new Point(410, 10), button_size, type, this, ABoard),
                new RandomizeButton(Brushes.LightSalmon, new Point(510, 10), button_size, type, this, ABoard),
                new EnlargeBoardButton(Brushes.LightSteelBlue, new Point(610, 10), button_size, type, this, ABoard),
                new ShrinkBoardButton(Brushes.LightSlateGray, new Point(710, 10), button_size, type, this, ABoard)
            };
        }

        /// <summary>
        /// Paints the buttons.
        /// </summary>
        /// <param name="obj">Form</param>
        /// <param name="pea">pea from the Form</param>
        private void PaintMenuBarButtons(object obj, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach(MenuButton mb in ButtonList)
            {
                mb.DrawButton(g);
            }
        }

        /// <summary>
        /// Checks if the Menu Bar Buttons are hovered (Runs BHover on them).
        /// </summary>
        /// <param name="obj">Either this, or BoardPanel</param>
        /// <param name="mea">mea from the Form</param>
        public void MMove(object obj, MouseEventArgs mea)
        {
            foreach (MenuButton mb in ButtonList)
            {
                mb.BHover(new Point(mea.X, mea.Y), obj.ToString());
            }
            this.Invalidate();
        }

        /// <summary>
        /// Check if the Menu Buttons are clicked (Runs BClick on them).
        /// </summary>
        /// <param name="obj">this</param>
        /// <param name="mea">mea from the Form</param>
        private void MClick(object obj, MouseEventArgs mea)
        {
            foreach (MenuButton mb in ButtonList)
            {
                mb.BClick(new Point(mea.X, mea.Y));
            }
        }

        /// <summary>
        /// Used to determine if the Mouse is hovered over the MenuBar Panel or over another Panel.
        /// </summary>
        /// <returns>"MenuBar"</returns>
        public override string ToString()
        {
            return "MenuBar";
        }
    }
}
