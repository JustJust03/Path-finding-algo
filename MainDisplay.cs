using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace PathFindingAlgo
{
    /// <summary>
    /// The main Class of the Path finding program.
    /// The main Display to paint on.
    /// </summary>
    public partial class MainDisplay : Form
    {
        public static readonly string rootfolder = @"Z:\justh\C#\PathFindingAlgo\";
        List<MenuButton> ButtonList;
        Panel BoardPanel;
        Board ABoard;

        /// <summary>
        /// Constructor method of the program, and the main function.
        ///
        /// Sets styling for the form paint method, to make it smoother.
        /// Initializes the board, MenuBar and the events for the Form.
        /// </summary>
        public MainDisplay()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            InitBoard();
            InitMenuBar();

            this.Size = new Size(820, 950);
            this.MouseMove += MMove;
            BoardPanel.MouseMove += MMove;
            this.MouseClick += MClick;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        /// <summary>
        /// Initializes the Menu bar with all the buttons and adds them to the paint method.
        /// The buttons are stored in ButtonList.
        /// 
        /// Button Layout:
        ///     (1) Save (2) Load (3) Grid (4) Dijkstra (5) Reset (6) Randomize (7) Enlarge (8) Shrink.
        /// </summary>
        private void InitMenuBar()
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

            this.Paint += PaintMenuBarButtons;
        }

        /// <summary>
        /// Initializes the Board.
        /// Creates the BoardPanel and Gives it to the Board class.
        /// </summary>
        private void InitBoard()
        {
            BoardPanel = new SmoothPanel();
            ABoard = new Board(32, 32, new Rectangle(2, 102, 800, 800), BoardPanel);

            //StreamReader sr = new StreamReader("C:\\Users\\justh\\Documents\\FixAstarBoard.csv");
            //ABoard.Load_board(sr);
            //sr.Close();

            this.Controls.Add(BoardPanel);
        }

        /// <summary>
        /// Used to determine if the Mouse is hovered over the Form or over a Panel.
        /// </summary>
        /// <returns>"Main Dispay"</returns>
        public override string ToString()
        {
            return "Main Display";
        }

        /// <summary>
        /// Paints the buttons in AntiAlias mode.
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
        private void MMove(object obj, MouseEventArgs mea)
        {
            foreach (MenuButton mb in ButtonList)
            {
                mb.BHover(new Point(mea.X, mea.Y), obj.ToString());
            }
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
    }
}


