using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace PathFindingAlgo
{
    public class Board
    {
        public int Rows; public int Cols; public Panel BoardPanel; 
        public List<List<BoardCell>> BoardCells; public bool Grid;
        public Dijkstra DijkstraAlgo;
        public Astar AstarAlgo;

        public BoardCell BeginCell;
        public Point BeginPoint;
        public BoardCell EndCell;
        public Point EndPoint;
        public bool VisualizeAlgo;
        public bool UseAstarAlgo;

        private const int EnlargeShrinkScale = 2;
        private const int RandomRemoveInt = 50;

        /// <summary>
        /// Initialize the class
        /// (Not the constructor, because in load board this class is initialized again,
        ///     with a new board in string format)
        /// </summary>
        /// <param name="rows">N of rows</param>
        /// <param name="cols">N of Columns</param>
        /// <param name="boardSize">The size of the board (Usually the size of the panel)</param>
        /// <param name="boardPanel">The panel which the board is displayed on</param>
        /// <param name="randomizeBoard">true if sender = constructor, false if sender = load_board</param>
        /// <param name="grid">Visualize the grid default to false</param>
        private void Init(int rows, int cols, Rectangle boardSize, Panel boardPanel, bool randomizeBoard, bool grid)
        {
            BoardPanel = boardPanel;
            BoardPanel.Location = boardSize.Location;
            BoardPanel.Size = boardSize.Size;
            Rows = rows;
            Cols = cols;
            Grid = grid;


            if (randomizeBoard)
            {
                RandomizeBoard();
                BoardPanel.Paint += Draw_Board;
                BoardPanel.MouseClick += Board_MC;
            }
        }

        /// <summary>
        /// Constructor goes to Init()
        /// </summary>
        public Board(int rows, int cols, Rectangle boardSize, Panel boardPanel)
        {
            Init(rows, cols, boardSize, boardPanel, true, false);
        }

        /// <summary>
        /// 1 First gives every cell in BoardCells celltype 1 (solid cell).
        /// 2 Then changes the lowest row and the most left col to celltype 0 (empty cell).
        /// 3 At last changes random cells to celltype 0 (empty cell)
        ///     Percentage held is based on RandomRemoveInt.
        /// </summary>
        public void RandomizeBoard()
        {
            BoardCells = new List<List<BoardCell>>();

            Random rand = new Random();
            BeginPoint = new Point(rand.Next(0, Cols - 1), rand.Next(0, Rows - 1));
            EndPoint = new Point(rand.Next(0, Cols - 1), rand.Next(0, Rows - 1));
            //BeginPoint = new Point(2, 2);
            //EndPoint = new Point(Cols - 2, Rows - 2);

            // 1
            for (int r = 0; r < Rows; r++)
            {
                List<BoardCell> SubList = new List<BoardCell>();
                for (int c = 0; c < Cols; c++)
                {
                    int cellType;
                    if (c == BeginPoint.X && r == BeginPoint.Y)     cellType = 2;
                    else if (c == EndPoint.X && r == EndPoint.Y)    cellType = 3;
                    else cellType = 1;

                    BoardCell Cell = new BoardCell(c, r,
                                                   new Rectangle(
                                                   new Point(c * BoardPanel.Width / Cols, r * BoardPanel.Height / Rows),
                                                   new Size(BoardPanel.Width / Cols, BoardPanel.Height / Rows)),
                                                   cellType, this);
                    if (c == BeginPoint.X && r == BeginPoint.Y)     BeginCell = Cell;
                    else if (c == EndPoint.X && r == EndPoint.Y)    EndCell = Cell;
                    SubList.Add(Cell);
                }
                BoardCells.Add(SubList);
            }

            /*
            // 2
            // Bottom row
            for(int i = BeginCell.X; i < EndCell.X; i++)
            {
                BoardCells[EndCell.Y][i].ChangeTypeTo(0);
            }
            // Left col
            for(int i = BeginCell.Y + 1; i <= EndCell.Y; i++)
            {
                BoardCells[i][BeginCell.X].ChangeTypeTo(0);
            }
            */

            // 3
            foreach(List<BoardCell> Sublist in BoardCells)
            {
                foreach(BoardCell Cell in Sublist)
                {
                    if (rand.Next(1, 100) >= RandomRemoveInt && Cell.CellType != 2 && Cell.CellType != 3)
                    {
                        Cell.ChangeTypeTo(0);
                    }
                }
            }
            BoardPanel.Invalidate();
        }

        /// <summary>
        /// Restors the begin cell and the endcell.
        /// Clears the board from all Celltypes above 3 (Mainly used by the algorithms).
        /// </summary>
        public void ClearBoard()
        {
            foreach(List<BoardCell> Sublist in BoardCells)
            {
                foreach(BoardCell Cell in Sublist)
                {
                    if (Cell == BeginCell)      Cell.ChangeTypeTo(2);
                    else if (Cell == EndCell)   Cell.ChangeTypeTo(3);
                    else if (Cell.CellType > 3) Cell.ChangeTypeTo(0);
                }
            }
            BoardPanel.Invalidate();
        }

        /// <summary>
        /// Toggles the grid and invalidates the board.
        /// </summary>
        public void ToggleGrid()
        {
            Grid = !Grid;
            BoardPanel.Invalidate();
        }

        /// <summary>
        /// Enlarges the board by 2 and randomizes the new cells
        /// </summary>
        public void Enlarge()
        {
            Cols += EnlargeShrinkScale;
            Rows += EnlargeShrinkScale;
            RandomizeBoard();
        }
        /// <summary>
        /// Shrinks the board by 2 and randomizes the new cells
        /// Can't go under 2x2
        /// </summary>
        public void Shrink()
        {
            Cols = Cols - EnlargeShrinkScale > 1 ? Cols - EnlargeShrinkScale : Cols;
            Rows = Rows - EnlargeShrinkScale > 1 ? Rows - EnlargeShrinkScale : Rows;
            RandomizeBoard();
        }

        /// <summary>
        /// A PaintEvent added to BoardPanel (The panel this board is drawn in)
        /// 1 Paints the grid on the panel
        /// 2 Calls the cells to draw themselves
        /// </summary>
        /// <param name="obj">The BoardPanel</param>
        private void Draw_Board(object obj, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //1
            if (Grid)
            {
                for (int r = 0; r < Rows; r++)
                {
                    g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, r * BoardPanel.Height / Rows), new Size(BoardPanel.Width, 1)));

                }
                for (int c = 0; c < Cols; c++)
                {
                    g.FillRectangle(Brushes.Black, new Rectangle(new Point(c * BoardPanel.Width / Cols), new Size(1, BoardPanel.Height)));

                }
                g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, BoardPanel.Height - 1), new Size(BoardPanel.Width, 1)));
                g.FillRectangle(Brushes.Black, new Rectangle(new Point(BoardPanel.Width - 1, 0), new Size(1, BoardPanel.Height)));
            }

            //2
            foreach(List<BoardCell> Sublist in BoardCells)
            {
                foreach(BoardCell Cell in Sublist)
                {
                    Cell.Draw_cell(g);
                }
            }
        }

        /// <summary>
        /// Calls every cell to check if the cell was clicked
        /// </summary>
        /// <param name="obj">The BoardPanel</param>
        private void Board_MC(object obj, MouseEventArgs mea)
        {
            foreach(List<BoardCell> Sublist in BoardCells)
            {
                foreach(BoardCell Cell in Sublist)
                {
                    Cell.HitCell(mea);
                }
            }
            BoardPanel.Invalidate();
        }


        /// <summary>
        /// Converts all the board components to a csv file.
        /// First line will be:
        ///     rows, cols, panel x, panel y, panel width, panel height
        /// The rest of the lines will be the celltype of that cell per row
        /// </summary>
        /// <returns>The csv string</returns>
        public override string ToString()
        {
            string csv = "";
            csv += Rows.ToString() + "," + Cols.ToString() + "," +
                   BoardPanel.Location.X.ToString() + "," + BoardPanel.Location.Y.ToString() + "," +
                   BoardPanel.Width.ToString() + "," + BoardPanel.Height.ToString() + "\n"; 
            foreach(List<BoardCell> Sublist in BoardCells)
            {
                foreach(BoardCell Cell in Sublist)
                {
                    csv += Cell.CellType.ToString() + ",";
                }
                csv = csv.Remove(csv.Length - 1);
                csv += "\n";
            }
            return csv;
        }

        /// <summary>
        /// Takes a streamreader containing a csv file (made by the Board.ToString) 
        ///     and converts it to the board object.
        /// </summary>
        /// <param name="sr">csv file made by ToString</param>
        public void Load_board(StreamReader sr)
        {
            int x = 0; int y = 0;
            string line = sr.ReadLine();
            string[] parts = line.Split(',');

            BoardCells = new List<List<BoardCell>>(); 
            while (!sr.EndOfStream)
            {
                List<BoardCell> sublist = new List<BoardCell>();
                foreach (string celltype in sr.ReadLine().Split(','))
                {
                    BoardCell Cell = new BoardCell(x, y,
                                                   new Rectangle(
                                                   new Point(x * int.Parse(parts[4]) / int.Parse(parts[1]), y * int.Parse(parts[5]) / int.Parse(parts[0])),
                                                   new Size(int.Parse(parts[4]) / int.Parse(parts[1]), int.Parse(parts[5]) / int.Parse(parts[0]))),
                                                   int.Parse(celltype), this);
                    if      (Cell.CellType == 2) BeginCell = Cell;
                    else if (Cell.CellType == 3) EndCell = Cell;
                    sublist.Add(Cell);
                    x++;
                }
                BoardCells.Add(sublist);
                x = 0;
                y++;
            }

            Rectangle boardsize = new Rectangle(new Point(int.Parse(parts[2]), int.Parse(parts[3])),
                                                new Size(int.Parse(parts[4]), int.Parse(parts[5])));
            Init(int.Parse(parts[0]), int.Parse(parts[1]), boardsize, BoardPanel, false, Grid);
            BoardPanel.Invalidate();
        }

        /// <summary>
        /// Creates a DijkstraAlgo instance and then runs the Dijkstra algorithm
        /// </summary>
        public void RunDijkstraAlgo()
        {
            DijkstraAlgo = new Dijkstra(this);
            DijkstraAlgo.Run_algorithm();
        } 

        /// <summary>
        /// Creates a AstarAlgo instance and then runs the Astar algorithm
        /// </summary>
        public void RunAstarAlgo()
        {
            AstarAlgo = new Astar(this);
            AstarAlgo.Run_algorithm();
        }
    }
}
