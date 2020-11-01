using ArchitectureGrid;

using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Floor
{
    public class APH_RoofFloor2D : APH_BaseFloor2D
    {
        public APH_RoofFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buildingForm, List<RoomRequisite> requisite, int floor, Vector2d exit) : base(outerPolygon, buildingForm, requisite, floor, exit)
        {

        }

        public override List<Room2D> GetRooms()
        {
            List<Room2D> rooms = new List<Room2D>();

            rooms.Add(Stairs);
            rooms.AddRange(planProcessor2D.Rooms);

            return rooms;
        }

        protected override void Create2DSpaceInternal()
        {
            
            var entraceProcessor2D = new RoofFloorPlanProcessor2D(MainPolygon, BuildingForm, _exitPoint);
            entraceProcessor2D.AddStairsToExit();
            entraceProcessor2D.AddFlatsToFloor();

            planProcessor2D = entraceProcessor2D;
            var room = entraceProcessor2D.Rooms.FindAll(r => r.RoomType == RoomType.Flat);

            FindStairs();
            AddExitToStairs();



        }
      
        private void FindStairs()
        {
            Stairs = planProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Stairs);
        }

    }
}
