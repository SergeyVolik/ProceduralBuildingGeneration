using ArchitectureGrid;

using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Floor
{ 
    public abstract class APH_BaseFloor2D : Floor2D
    {
        public APH_BaseFloor2D() { }
        public APH_BaseFloor2D(APH_BaseFloor2D floor) : base(floor) { Create2DSpaceInternal(); }
        public APH_BaseFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buildingPolygon,  List<RoomRequisite> requisite, int floor, int floornumber, Vector2d exit) : base(outerPolygon, buildingPolygon, exit, requisite,  floor, floornumber)
        {

        }

        public const int MinLenght = 20;


        //protected override void Create2DSpaceInternal()
        //{

        //    PlanProcessor2D = new DefaultFloorPlanProcessor2D(MainPolygon, BuildingForm, ExitPosition);

        //    PlanProcessor2D.CreatePlan();


        //    FindCorrdior();

        //    FindLift();
        //    FindStairs();
        //    AddExitToStairs();
        //    AddDoorForRooms();


        //}

        
      


        public override List<Room2D> GetRooms2D()
        {
            var room = new List<Room2D>();

            flats.ForEach(f => room.AddRange(f.GetRooms2D()));

            room.Add(Corridor);
            room.Add(Stairs);
            room.Add(Lift);

            return room;

        }
       


        protected void AddExitToStairs()
        {
            Stairs.Walls.ForEach(w => {
                if (PlanProcessor2D.ExitCell.PartsOfOutsideWalls != null && PlanProcessor2D.ExitCell.PartsOfOutsideWalls.Exists(ww => ww.Equals(w)))
                    w.WallType = WallType.WallWithDoor;
            });
        }
        

       

    }
}
