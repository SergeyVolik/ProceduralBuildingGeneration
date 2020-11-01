using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
    public enum GrowthType { Left, Right, Top, Bottom }

    class GrowthScore
    {
        public double BlockedDirectionScore;
        public double WindowWasteScore;
        public double NecessityScore;
        public double TotalScore;
        public double Normalized;

        Room2D Room;

        public GrowthScore(Room2D room, List<PlanCell> SelectedCells)
        {
            Room = room;
            BlockedDirectionRule(SelectedCells);
            WindowWasteRule();
            NecessityRule(SelectedCells.Count);
            CalculateTotalScore();
        }
        private void BlockedDirectionRule(List<PlanCell> SelectedCells)
        {
            BlockedDirectionScore = 1;
            for (var i = 0; i < SelectedCells.Count; i++)
            {
                if (SelectedCells[i].room != null || SelectedCells[i].Tag == PlanCellTag.Outside)
                {
                    BlockedDirectionScore = 0;
                    break;
                }
            }
           
        }
        private void WindowWasteRule()
        {
            int windowsCounter = 0;
            for (var i = 0; i < Room.Cells.Count; i++)
            {
                if(Room.Cells[i].PartsOfOutsideWalls != null)
                for (var j = 0; j < Room.Cells[i].PartsOfOutsideWalls.Count; j++)
                {

                    if (Room.Cells[i].PartsOfOutsideWalls[j].WallType == WallType.WallWithWindow)
                        windowsCounter++;
                }
            }

            if (windowsCounter > 1)
            {
                WindowWasteScore = 0.2 / (windowsCounter - 1);
            }
            else WindowWasteScore = 1;
            
           
        }
        private void NecessityRule(int numberOfCells)
        {
            // так как 1 клекта это 1 м2 то иъ сума равна площади

            if (Room.RoomRequisite)
             NecessityScore = (numberOfCells + Room.Cells.Count) / Room.RoomRequisite.AreaMax;
            NecessityScore = 1;
        }
        private void CalculateTotalScore()
        {
            TotalScore = BlockedDirectionScore * WindowWasteScore * NecessityScore;
        }

        public static void NormalizeScores(List<GrowthScore> scores)
        {
            double sum = 0;
            for (var i = 0; i < scores.Count; i++)
            {
                sum += scores[i].TotalScore;
            }
            for (var i = 0; i < scores.Count; i++)
            {
                scores[i].Normalized = scores[i].TotalScore / sum;

            }
        }


    }
}
