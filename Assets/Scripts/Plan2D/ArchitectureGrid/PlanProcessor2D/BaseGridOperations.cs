using Assets.Scripts.Premies.Buildings.Building2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
    public abstract class BaseGridOperations : Polygon
    {
        public double MaxDistanceBetweenExitAndCells(PlanCell[,] grid, PlanCell exit)
        {
            var distances = new List<double>();
            for (var i = 0; i < grid.GetLength(0); i++)
            {

                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].Tag == PlanCellTag.Inside)
                    {
                        distances.Add(DistanceBetweenCells(grid[i, j], exit));
                    }
                }
            }
            return distances.Max();
        }
        public double DistanceBetweenCells(PlanCell cell1, PlanCell cell2)
        {
            return  cell1.Center.DistanceTo(cell2.Center);
        }
        public bool CheckTopCellByCondition(PlanCell[,] Grid, (int i, int j) currentCellIdexes,  Func<PlanCell, bool> Condition)
        {
            var iPlus = currentCellIdexes.i + 1;

            if (IndexIInRange(iPlus, Grid))
                if (/*Grid[iPlus, indexes.j].IsFree*/Condition(Grid[iPlus, currentCellIdexes.j]))                                 
                    return true;      
            return false;

        }
        public bool CheckBottomCellByCondition(PlanCell[,] Grid,(int i, int j) indexes,  Func<PlanCell, bool> Condition)
        {
            var iMinus = indexes.i - 1;

            if (IndexIInRange(iMinus,Grid))
                if (Condition(Grid[iMinus, indexes.j]))                                  
                    return true;                
            return false;

        }
        public bool IndexIInRange(int i, PlanCell[,] Grid)
        {
            return Grid.GetLength(0) > i && i >= 0 ? true : false;
        }
        public static bool IndexJInRange(int j, PlanCell[,] Grid)
        {
            return Grid.GetLength(1) > j && j >= 0 ? true : false;
        }

        public  bool CheckRightCellByCondition(PlanCell[,] Grid,(int i, int j) indexes,  Func<PlanCell, bool> Condition)
        {
            var jPlus = indexes.j + 1;

            if (IndexJInRange(jPlus, Grid))
                if (Condition(Grid[indexes.i, jPlus]))
                   
                    return true;

         
            return false;

        }
        public bool CheckLeftCellByCondition(PlanCell[,] Grid, (int i, int j) indexes, Func<PlanCell, bool> Condition)
        {
            var jMinus = indexes.j - 1;

            if (IndexJInRange(jMinus,Grid))
                if ( Condition(Grid[indexes.i, jMinus]))                              
                    return true;
               
            return false;

        }
        public bool CellIsFree(PlanCell cell)
        {
            return cell.IsFree;
        }
        public bool CellIsOuter(PlanCell cell)
        {
            return cell.IsOuterWallCell();
        }
    }
}

