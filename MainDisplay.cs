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
        public const string rootfolder = @"Z:\justh\C#\PathFindingAlgo\";
        Panel BoardPanel;
        MenuBar Bar;
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
            BoardPanel.MouseMove += Bar.MMove;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void InitMenuBar()
        {
            Bar = new MenuBar(new Rectangle(0, 0, 800, 100), ABoard);
            this.Controls.Add(Bar);
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
    }
}


