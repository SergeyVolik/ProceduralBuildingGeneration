using Rooms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
  

    public class RoomGrowthProcessor2D : GrowthProcessor
    {



        public RoomGrowthProcessor2D(PlanCell[,] _Grid, List<PlanCell> _gridVector, List<Room2D> _Rooms, PlanCell exitcell) : base(_Grid, _gridVector,_Rooms, exitcell)
        {
            
        }

        protected override void FirstExpansionStep()
        {
            AddCellsToCorrectCells();
            PlacingRoomsInCells();

            int growthedRooms = 0;
            int breakNumber = 40;
            int countder = 0;

            RoomsToGrowth = Rooms;
            do
            {
                if (countder > breakNumber)
                    break;
                countder++;
                growthedRooms = 0;

                foreach (var room in RoomsToGrowth)
                {
                    var NextLeftCells = new List<PlanCell>();
                    var NextRightCells = new List<PlanCell>();
                    var NextTopCells = new List<PlanCell>();
                    var NextBottomCells = new List<PlanCell>();

                    GrowthScore leftScore = null, rightScore = null, topScore = null, bottomScore = null;
                    var scores = new List<GrowthScore>();
                    //left growth

                    if (room.Area > room.RoomRequisite.AreaMax)
                        continue;




                    if (NextCellsForGrowthByWay(room, out NextLeftCells, CellIsFree, FindRoomCellsForExpantionLeft, CheckLeftCellByCondition))
                    {

                        leftScore = new GrowthScore(room, NextLeftCells);
                        scores.Add(leftScore);
                    }
                    //right growth

                    if (NextCellsForGrowthByWay(room, out NextRightCells, CellIsFree, FindRoomCellsForExpantionRight, CheckRightCellByCondition))
                    {
                        rightScore = new GrowthScore(room, NextRightCells);
                        scores.Add(rightScore);
                    }
                    //top growth

                    if (NextCellsForGrowthByWay(room, out NextTopCells, CellIsFree, FindRoomCellsForExpantionTop, CheckTopCellByCondition))
                    {
                        topScore = new GrowthScore(room, NextTopCells);
                        scores.Add(topScore);
                    }
                    //bottom growth

                    if (NextCellsForGrowthByWay(room, out NextBottomCells, CellIsFree, FindRoomCellsForExpantionBottom, CheckBottomCellByCondition))
                    {

                        bottomScore = new GrowthScore(room, NextBottomCells);
                        scores.Add(bottomScore);
                    }

                    if (scores.Count == 0)
                        continue;

                    //выбор лучего решения
                    //GrowthScore.NormalizeScores(scores);
                    var max = scores.Max(s => s.TotalScore);

                    if (max == 0)
                        continue;

                    var bestScore = scores.Find(s => max == s.TotalScore);

                    List<PlanCell> bestCells = null;
                    if (leftScore != null && bestScore == leftScore)
                    {
                        bestCells = NextLeftCells;
                    }
                    else if (rightScore != null && bestScore == rightScore)
                    {
                        bestCells = NextRightCells;
                    }
                    else if (topScore != null && bestScore == topScore)
                    {
                        bestCells = NextTopCells;
                    }
                    else if (bottomScore != null && bestScore == bottomScore)
                    {
                        bestCells = NextBottomCells;
                    }

                    if (bestCells != null)
                    {
                        bestCells.ForEach(c => c.room = room);
                        growthedRooms++;
                        room.Cells.AddRange(bestCells);
                    }
                }
            } while (growthedRooms != 0);
        }




        //Цель шага в топ чтобы помочь комнатам которые (заросли) и 
        //не смошли расшириться до своего минимального размера
        protected override void FixingStep()
        {

        }


        public enum GrowthDiraction { Left, Right, Top, Bottom }
        //в данном шаге комнаты расшряються чтобы заполнить все пустое пространство.
        protected override void SecondExpansionStep()
        {
            var growthMore = RoomsToGrowth.FindAll(r => r.RoomRequisite.GrowthIfMaxSize);

            var FreeCells = GridVector.FindAll(cell => cell.IsFree);

            int breakCOunt = 0;
            while (FreeCells.Count != 0)
            {
                var toRemove = new List<PlanCell>();

                foreach (var cell in FreeCells)
                {
                    var neighbours = cell.NotFreeNeighbours;

                    neighbours.RemoveAll(c => c.room.RoomRequisite != null && !c.room.RoomRequisite.GrowthIfMaxSize);

                    PlanCell randomCell;

                    if (neighbours.Count != 0)
                    {
                        randomCell = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
                        randomCell.room.AddCell(cell);
                        toRemove.Add(cell);

                    }


                }


                toRemove.ForEach(c => FreeCells.Remove(c));
                breakCOunt++;

                if (breakCOunt > 100)
                {
                    UnityEngine.Debug.Log("Error [SecondExpansionStep] ");
                    break;
                }

            }



        }






    }
}
