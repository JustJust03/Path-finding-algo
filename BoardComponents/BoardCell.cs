using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace PathFindingAlgo
{
    public class BoardCell
    {
        //0 Empty cell, 1 Black solid cell, 2 Begin cell, 3 End cell, 4 Path Trail, 5 Temporary path.
        public int X; public int Y; private bool Circle;
        Rectangle HitBox; int VisualCircleShrink;
        public int CellType; Brush B;
        readonly Board ABoard;

        public double TravelCost; public List<BoardCell> Path;

        /// <summary>
        /// The Cell in a board
        /// </summary>
        /// <param name="x">X coordinate in the board</param>
        /// <param name="y">Y coordinate in the board</param>
        /// <param name="hitBox">A Rectangle with the pixels of the cell within the panel</param>
        /// <param name="cellType">0 Empty cell, 1 Black solid cell, 2 Begin cell, 3 End cell, 4 Path Trail, 5 Temporary path.</param>
        /// <param name="aboard">The board this cell is in</param>
        public BoardCell(int x, int y, Rectangle hitBox, int cellType, Board aboard)
        {
            ABoard = aboard;
            X = x;
            Y = y;
            HitBox = hitBox;
            ChangeTypeTo(cellType);
            VisualCircleShrink = HitBox.Width / 8;
            
            ClearCellVariables();
        }

        /// <summary>
        /// Draws the cell.
        /// </summary>
        /// <param name="g"></param>
        public void Draw_cell(Graphics g)
        {
            if (CellType != 0)
                if (Circle)
                    g.FillEllipse(B, Rectangle.Inflate(HitBox, -VisualCircleShrink, -VisualCircleShrink));
                else
                    g.FillEllipse(B, Rectangle.Inflate(HitBox, -VisualCircleShrink, -VisualCircleShrink));
        }

        /// <summary>
        /// Used to create and remove walls
        /// </summary>
        /// <param name="mea"></param>
        /// <returns>Whether or not a wall was changed</returns>
        public bool HitCell(MouseEventArgs mea)
        {
            if (HitBox.Contains(mea.X, mea.Y) && CellType == 1)
            {
                ChangeTypeTo(0);
                return true;
            }
            else if (HitBox.Contains(mea.X, mea.Y) && CellType == 0)
            {
                ChangeTypeTo(1);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Calculates the distance to another cell.
        /// Horizontal and Vertical moves add 10 to the distance,
        ///     Slants add 14 (almost sqrt(2)) to the distance.
        /// Takes the virtually fastest route to a node, not the absolute distance
        /// </summary>
        /// <param name="ToCell">The target cell</param>
        /// <returns>The distance for a cell with width 10</returns>
        public int DistTo(BoardCell ToCell)
        {
            int DeltaX = Math.Abs(ToCell.X - X);
            int DeltaY = Math.Abs(ToCell.Y - Y);
            int Distance = 0;
            while(DeltaX > 0 && DeltaY > 0)
            {
                Distance += 14;
                DeltaX--;
                DeltaY--;
            }
            Distance += 10 * DeltaX + 10 * DeltaY;
            return Distance;
        }

        /// <summary>
        /// Changes the celltype and the brushtype
        /// </summary>
        /// <param name="type">0 Empty cell, 1 Black solid cell, 2 Begin cell, 3 End cell, 4 Path Trail, 5 Temporary path.</param>
        public void ChangeTypeTo(int type)
        {
            this.CellType = type;
            if (CellType == 1)  Circle = false;
            else                Circle = true;
            this.B = GetBrushType();
        }

        /// <summary>
        /// Clears the variables used for the algorithms.
        /// </summary>
        public void ClearCellVariables()
        {
            if (this == ABoard.BeginCell) TravelCost = 0;
            else TravelCost = double.PositiveInfinity;
            Path = new List<BoardCell>();   
        }

        /// <summary>
        /// Changes brushtype according to their celltype
        /// </summary>
        /// <returns>the Brush with the right color</returns>
        private Brush GetBrushType()
        {
                 if (CellType == 1) return Brushes.DarkSlateGray;   //Solid cell
            else if (CellType == 2) return Brushes.DarkOrange;            //Begin cell
            else if (CellType == 3) return Brushes.DarkOrange;             //End cell
            else if (CellType == 4) return Brushes.DeepPink;          //Path trail
            else if (CellType == 5) return Brushes.DeepPink;            //Temporary path
            else                    return Brushes.LightSkyBlue;    //Error color (Should never be used)
        }
    }
}
