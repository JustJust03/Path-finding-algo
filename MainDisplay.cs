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
        PreGameMenu PGM;

        //Values set in the PreGameMenu.
        public bool Visualize;
        //true: use AstarAlgo. false: use DijkstraAlgo.
        public bool UseAstar;

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

            this.Size = new Size(820, 950);

            PGM = new PreGameMenu(new Rectangle(0, 0, this.Width, this.Height), this);
            InitBoard();
            InitMenuBar();

            BoardPanel.MouseMove += Bar.MMove;
            this.KeyDown += KDown;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            SwitchToStage("PreGame");
        }

        public void SwitchToStage(string Stage)
        {
            if (Stage == "PreGame")
            {
                Controls.Clear();
                Controls.Add(PGM);
            }
            else if(Stage == "InGame")
            {
                Controls.Clear();
                Controls.Add(Bar);
                Controls.Add(BoardPanel);
                ABoard.VisualizeAlgo = Visualize;
                ABoard.UseAstarAlgo = UseAstar;
            }
        }

        private void InitMenuBar()
        {
            Bar = new MenuBar(new Rectangle(0, 0, 800, 100), ABoard);
        }

        /// <summary>
        /// Initializes the Board.
        /// Creates the BoardPanel and Gives it to the Board class.
        /// </summary>
        private void InitBoard()
        {
            BoardPanel = new SmoothPanel();
            ABoard = new Board(32, 32, new Rectangle(2, 102, 800, 800), BoardPanel);
            ABoard.VisualizeAlgo = Visualize;

            //StreamReader sr = new StreamReader("C:\\Users\\justh\\Documents\\FixAstarBoard.csv");
            //ABoard.Load_board(sr);
            //sr.Close();
        }

        private void KDown(object obj, KeyEventArgs kea)
        {
            if (kea.KeyCode == Keys.Escape) SwitchToStage("PreGame");
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainDisplay
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "MainDisplay";
            this.Load += new System.EventHandler(this.MainDisplay_Load);
            this.ResumeLayout(false);
        }

        private void MainDisplay_Load(object sender, System.EventArgs e)
        {

        }
    }
}


