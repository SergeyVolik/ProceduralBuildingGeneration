using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
    class PlanCellScore
    {
        public double ScoreWindow;
        public double ScoreSocial;
        public double ScorePrivate;
        public double ScoreElastic;
        public double ScoreHardConnection;
        public double ScoreDoor;
       
        public double TotalScore;
        public double Normalized;

        public PlanCell cell;

        public PlanCellScore(PlanCell cel, bool NeedWindow, ZoneType zoneType, PlanCell exit, double maxDistance, List<PlanCell> prevThisTypeRoomsCells, PlanCell hardConRoom)
        {
            cell = cel;
            WindowRule(NeedWindow);
            SocialRule(exit, maxDistance);
            PrivateRule(exit, maxDistance);
          
            ElasticRule(prevThisTypeRoomsCells);
            HardConnectionRule(hardConRoom);
            DoorRule(zoneType);

            CalcTotalScore(zoneType);
        }

        
        private void WindowRule(bool NeedWindow)
        {
            bool haveWindow;
            if (cell.PartsOfOutsideWalls!= null)
                haveWindow = cell.PartsOfOutsideWalls.Exists(p => p.WallType == WallType.WallWithWindow);
            else haveWindow = false;

            if (haveWindow)
                ScoreWindow = 1;
            else if (!haveWindow && !NeedWindow)
                ScoreWindow = 0.5;
            else ScoreWindow = 0;

        }
      

        private void SocialRule(PlanCell exit, double distanceMax) {

            var distance = cell.Center.DistanceTo(exit.Center);

            ScoreSocial = 1 - (distance / distanceMax);
        }
       

        private void PrivateRule(PlanCell exit, double distanceMax)
        {
            var distance = cell.Center.DistanceTo(exit.Center);

            ScorePrivate = (distance / distanceMax);
        }
        private void ElasticRule(List<PlanCell> prevThisTypeRoomsCells)
        {
            ScoreElastic = 1;

            for (var i = 0; i < prevThisTypeRoomsCells.Count; i++)
            {
                ScoreElastic *= Math.Pow(0.5, cell.Center.DistanceTo(prevThisTypeRoomsCells[i].Center));
            }
        }
        private void HardConnectionRule(PlanCell hardConnetedRoom)
        {
            if (hardConnetedRoom != null)
            {
                var distance = cell.Center.DistanceTo(hardConnetedRoom.Center);
                if (distance < 4)
                    ScoreHardConnection = Math.Pow(0.5, cell.Center.DistanceTo(hardConnetedRoom.Center));
                else ScoreHardConnection = 0;
            }
            else ScoreHardConnection = 1;
            
        }
        private void DoorRule(ZoneType zoneType)
        {
            if (cell.PartsOfOutsideWalls== null || zoneType == ZoneType.Public && cell.PartsOfOutsideWalls.Exists(w => w.WallType == WallType.WallWithDoor))
                ScoreDoor = 0;
            else ScoreDoor = 1;
        }
        private void CalcTotalScore(ZoneType zoneType)
        {
            if (zoneType == ZoneType.Public)
                TotalScore = ScoreWindow * ScoreSocial * ScoreElastic * ScoreHardConnection * ScoreDoor;
            else TotalScore = ScoreWindow * ScorePrivate * ScoreElastic * ScoreHardConnection * ScoreDoor;
          
        }
        public static void NormalizeScores(List<PlanCellScore> scores)
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
