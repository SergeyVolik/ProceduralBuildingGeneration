
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor
{
    public class APH_DefaulFloor2D : APH_BaseFloor2D
    {

        public APH_DefaulFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buoildingPolygon, List<RoomRequisite> requisite, int floor, int floorsNumber, Vector2d exit) : base(outerPolygon, buoildingPolygon, requisite, floor, floorsNumber,exit)
        {

        }

        //public override List<Room2D> GetRooms()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
