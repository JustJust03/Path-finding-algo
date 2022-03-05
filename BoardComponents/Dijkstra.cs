using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace PathFindingAlgo
{
    /// <summary>
    /// An algorithm used to find the fastest path between nodes.
    /// Finds the shortest path from the source node to every other node, until the EndCell is hit.
    /// </summary>
    public class Dijkstra
    {
        protected readonly Board DBoard;
        /// <summary>
        /// A list with all the Relevant cells to the algorithm.
        /// ie the cells that should be looked around next.
        /// </summary>
        protected readonly List<BoardCell> CellList;
        readonly List<BoardCell> FinishedCellList;
        protected bool RunningAlgo;
        protected bool FoundEnd;

        private int searches = 0;

        /// <summary>
        /// Initializes the components.
        /// </summary>
        /// <param name="dBoard">The board in which the algorithm is run</param>
        public Dijkstra(Board dBoard)
        {
            DBoard = dBoard;
            CellList = new List<BoardCell>() {DBoard.BeginCell};
            FinishedCellList = new List<BoardCell>();
        }

        /// <summary>
        /// 1 First resets all the variables from the cells (incase this isn't the first time the algo is ran)
        /// 2 Then assigns TravelCosts to all the nodes untill the EndCell is found,
        ///     or the CellList got exhausted and there was no path to the EndCell.
        /// 3 Then Draws the shortest path from the BeginCell to the EndCell.
        /// </summary>
        public void Run_algorithm()
        {
            DBoard.ClearBoard();
            RunningAlgo = true;

            //1
            foreach(List<BoardCell> Sublist in DBoard.BoardCells)
            {
                foreach(BoardCell Cell in Sublist)
                {
                    Cell.ClearCellVariables();
                }
            }

            //2
            while (RunningAlgo)
            {

                Assign_TraveCosts(Lowest_travel_cost());
            }

            //3
            foreach(BoardCell cell in DBoard.EndCell.Path)
            {
                cell.ChangeTypeTo(4);
            }
            if (!FoundEnd)  DBoard.ClearBoard();

            DBoard.BoardPanel.Invalidate();
            double pathlength = CalculatePathLenght(DBoard.EndCell.Path);
            int s = searches;
        }

        /// <summary>
        /// Goes through every move possible from the OriginalCell,
        ///     and calculates the new travel cost to this cell.
        /// If this new travelcost is lower than the travelcost the cell had, 
        ///     the travelcost and the fastest path to this cell will be updated.
        /// </summary>
        /// <param name="OriginalCell">Cell to search travelcosts around</param>
        private void Assign_TraveCosts(BoardCell OriginalCell)
        {
            Move_cell(OriginalCell);
            
            List<BoardCell> directions = Find_Neighbours(OriginalCell);
            foreach(BoardCell cell in directions)
            {
                double NewDistance = OriginalCell.TravelCost + OriginalCell.DistTo(cell);
                if(NewDistance < cell.TravelCost)
                {
                    cell.TravelCost = NewDistance;

                    List<BoardCell> NewPath = new List<BoardCell>(OriginalCell.Path);
                    NewPath.Add(OriginalCell);
                    cell.Path = NewPath;
                    if (!FinishedCellList.Contains(cell)) CellList.Add(cell);
                    if (DBoard.VisualizeAlgo)  Visualize_Algo(NewPath);
                    if (cell == DBoard.EndCell) FinishAlgo();
                }
            }
        }

        /// <summary>
        /// Adds the EndCell to it's own path, to ensure it's drawn too.
        /// Clears the board in case the pink trail is still on the screen.
        /// </summary>
        private void FinishAlgo()
        {
            List<BoardCell> NewPath = new List<BoardCell>(DBoard.EndCell.Path);
            NewPath.Add(DBoard.EndCell);
            DBoard.EndCell.Path = NewPath;
            if (DBoard.VisualizeAlgo)  DBoard.ClearBoard();
            RunningAlgo = false;
            FoundEnd = true;
        }

        /// <summary>
        /// Looks to the 8 cells around the cell and returns them if the cell cell is empty.
        /// </summary>
        /// <param name="OriginCell">Cell to look around</param>
        /// <returns>A list with cells that the original cell can move to</returns>
        private List<BoardCell> Find_Neighbours(BoardCell OriginCell)
        {
            List<BoardCell> Neighbours = new List<BoardCell>();
            // Looks in an area of 3x3. Corrects for edges -> 2x3, 3x2, 2x2.
            for (int dx = (OriginCell.X > 0 ? -1 : 0); dx <= (OriginCell.X < DBoard.Cols - 1 ? 1 : 0); dx++)
            {
                for (int dy = (OriginCell.Y > 0 ? -1 : 0); dy <= (OriginCell.Y < DBoard.Rows - 1 ? 1 : 0); dy++)
                {
                    BoardCell NeighbourCell = DBoard.BoardCells[OriginCell.Y + dy][OriginCell.X + dx];
                    if (NeighbourCell != OriginCell && NeighbourCell.CellType != 1) 
                        Neighbours.Add(NeighbourCell);
                }
            }
            return Neighbours;
        }

        /// <summary>
        /// Finds the BoardCell with the lowest travel cost.
        /// Defaults to the first cell in the list.
        /// </summary>
        /// <returns>The BoardCell with the lowest travel cost</returns>
        protected virtual BoardCell Lowest_travel_cost()
        {
            BoardCell LowestCell = CellList[0];
            foreach(BoardCell cell in CellList)
            {
                if (cell.CellType != 1 && cell.TravelCost < LowestCell.TravelCost)
                {
                    LowestCell = cell;
                }
            }
            return LowestCell;
        }

        /// <summary>
        /// Moves a cell from The CellList to the FinishedCellList
        /// </summary>
        /// <param name="Cell">The desired cell to be moved</param>
        private void Move_cell(BoardCell Cell)
        {
            searches += 1;
            CellList.Remove(Cell);
            FinishedCellList.Add(Cell);
        }

        /// <summary>
        /// Creates a pink path along the way the algorithm is looking right now.
        /// </summary>
        /// <param name="Path"></param>
        private void Visualize_Algo(List<BoardCell> Path)
        {
            DBoard.ClearBoard();

            foreach (BoardCell cell in Path)
                cell.ChangeTypeTo(5);

            DBoard.BoardPanel.Invalidate();
            Application.DoEvents();
        }

        /// <summary>
        /// Calculates the path length of the path.
        /// Only used for debugging purposes.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private int CalculatePathLenght(List<BoardCell> Path)
        {
            int length = 0;
            for(int i = 0; i < Path.Count - 1; i++)
                length += Path[i].DistTo(Path[i + 1]);
            return length;
        }
    }
}
