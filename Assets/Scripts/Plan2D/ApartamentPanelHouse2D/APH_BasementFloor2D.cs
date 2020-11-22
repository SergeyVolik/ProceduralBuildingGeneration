using ArchitectureGrid;

using Rooms;
using StraightSkeleton.Primitives;
using System.Collections.Generic;

namespace Floor
{
   
    public class APH_BasementFloor2D : APH_DefaulFloor2D{
       
        public APH_BasementFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buildingForm, List<RoomRequisite> requisite, int floorsNumber,Vector2d exit, bool passage) : base(outerPolygon, buildingForm, requisite, 0, floorsNumber, exit, passage)
        {
           
        }

        public APH_BasementFloor2D(APH_BasementFloor2D floor)
        {
            ExitPosition = floor.ExitPosition;
            MainPolygon = floor.MainPolygon;
            BuildingForm = floor.BuildingForm;
            roomRequisites = floor.roomRequisites;
            Floor = floor.Floor;
            flats = floor.flats;
            PlanProcessor2D = floor.PlanProcessor2D;

            needPassage = floor.needPassage;
            Create2DSpaceInternal();
        }

       

    }
}
