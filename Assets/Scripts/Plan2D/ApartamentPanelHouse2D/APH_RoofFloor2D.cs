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
        public APH_RoofFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buildingForm, List<RoomRequisite> requisite, int floor, int floorNumber, Vector2d exit) : base(outerPolygon, buildingForm, requisite, floor, floorNumber, exit)
        {

        }
        public APH_RoofFloor2D()
        {
            
        }
        public APH_RoofFloor2D(APH_RoofFloor2D floor2D)
        {
            ExitPosition = floor2D.ExitPosition;
            MainPolygon = floor2D.MainPolygon;
            BuildingForm = floor2D.BuildingForm;
            roomRequisites = floor2D.roomRequisites;
            Floor = floor2D.Floor;
            flats = floor2D.flats;
            PlanProcessor2D = floor2D.PlanProcessor2D;

            Create2DSpaceInternal();
        }
        public override List<Room2D> GetRooms2D()
        {
            List<Room2D> rooms = new List<Room2D>();

            rooms.Add(Stairs);
            rooms.AddRange(PlanProcessor2D.Rooms);

            return rooms;
        }

        protected override void Create2DSpaceInternal()
        {
            
            var entraceProcessor2D = new RoofFloorPlanProcessor2D(MainPolygon, BuildingForm, ExitPosition);
            entraceProcessor2D.AddStairsToExit();
            entraceProcessor2D.AddFlatsToFloor();

            PlanProcessor2D = entraceProcessor2D;
            var room = entraceProcessor2D.Rooms.FindAll(r => r.RoomType == RoomType.Flat);

            
            
            FindStairs();
            AddExitToStairs();

            room.ForEach(r => r.AddDoorBetweenRooms(Stairs));


        }
      
        private void FindStairs()
        {
            Stairs = PlanProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Stairs);
        }

    }
}
