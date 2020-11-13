using ArchitectureGrid;

using Rooms;
using StraightSkeleton.Primitives;
using System.Collections.Generic;

namespace Floor
{
   
    public class APH_BasementFloor2D : APH_RoofFloor2D
    {
        public APH_BasementFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buildingForm, List<RoomRequisite> requisite, int floorsNumber,Vector2d exit) : base(outerPolygon, buildingForm, requisite, 0, floorsNumber, exit)
        {

        }

       

    }
}
