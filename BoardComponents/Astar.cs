using System;

namespace PathFindingAlgo
{
    /// <summary>
    /// The best path search algorithm with optimal efficiency.
    /// </summary>
    public class Astar : Dijkstra
    {
        public Astar(Board aboard) : base(aboard)
        {

        }

        /// <summary>
        /// Incorporates The distance to the target to optimize the order in which the cells are searched.
        /// Keeps The output the same as Dijkstra but faster.
        /// When the CellList is empty and the EndCell still hasn't been found, ends the algo.
        /// </summary>
        /// <returns>The cell with the lowest current path lenght + distance to EndCell</returns>
        protected override BoardCell Lowest_travel_cost()
        {
            try
            {
                BoardCell LowestCell = CellList[0];
                foreach(BoardCell cell in CellList)
                {
                    if (cell.CellType != 1 && cell.TravelCost + cell.DistTo(DBoard.EndCell) < LowestCell.TravelCost + LowestCell.DistTo(DBoard.EndCell))
                    {
                        LowestCell = cell;
                    }
                }
                return LowestCell;
            }
            catch (ArgumentOutOfRangeException)
            {
                RunningAlgo = false;
                FoundEnd = false;
                return DBoard.BeginCell;
            }
        }
    }
}
