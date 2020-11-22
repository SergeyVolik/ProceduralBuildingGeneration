
using Assets.Scripts.Tools;
using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
    public class FlatGrowthProcessor2D : GrowthProcessor
    {
      

     
        public FlatGrowthProcessor2D(PlanCell[,] _Grid, List<PlanCell> _gridVector,  List<Room2D> _Rooms, PlanCell exitcell) : base(_Grid, _gridVector, _Rooms, exitcell)
        {          
            
        }
       
        protected override void FirstExpansionStep()
        {
            int growthedRooms = 0;
            int breakNumber = 40;
            int countder = 0;
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
                        NextLeftCells.RemoveAll(c => !c.IsFree);
                        bestCells = NextLeftCells;
                    }
                    else if (rightScore != null && bestScore == rightScore)
                    {
                        NextRightCells.RemoveAll(c => !c.IsFree);
                        bestCells = NextRightCells;
                    }
                    else if (topScore != null && bestScore == topScore)
                    {
                        NextTopCells.RemoveAll(c => !c.IsFree);
                        bestCells = NextTopCells;
                    }
                    else if (bottomScore != null && bestScore == bottomScore)
                    {
                        NextBottomCells.RemoveAll(c => !c.IsFree);
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

        //в данном шаге комнаты расшряються чтобы заполнить все пустое пространство.
        protected override void SecondExpansionStep()
        {
            
        }

        
       
       

        
    }
}
