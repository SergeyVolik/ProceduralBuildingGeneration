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

        public APH_BaseFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buildingPolygon,  List<RoomRequisite> requisite, int floor, Vector2d exit) : base(outerPolygon, buildingPolygon, exit, requisite,  floor)
        {

        }

        public const int MinLenght = 20;


        protected override void Create2DSpaceInternal()
        {

            planProcessor2D = new DefaultFloorPlanProcessor2D(MainPolygon, BuildingForm, _exitPoint);

            planProcessor2D.CreatePlan();


            FindCorrdior();

            FindLift();
            FindStairs();
            AddExitToStairs();
            AddDoorForRooms();


        }

        private void FindCorrdior()
        {
            Corridor = planProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Corridor);
        }
        private void FindLift()
        {
            Lift = planProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Lift);
        }
        private void FindStairs()
        {
            Stairs = planProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Stairs);
        }


        public override List<Room2D> GetRooms()
        {
            var room = new List<Room2D>();

            flats.ForEach(f => room.AddRange(f.GetRooms()));

            room.Add(Corridor);
            room.Add(Stairs);
            room.Add(Lift);

            return room;

        }
        Vector2d AddDoorBetweenTwoRooms(Room2D room1, Room2D room2, WallType type)
        {
            List<PartOfWall> walls1 = new List<PartOfWall>();
            List<PartOfWall> walls2 = new List<PartOfWall>();
            room1.Walls.ForEach(r => {
                room2.Walls.ForEach(r2 => {
                    if (r.Equals(r2))
                    {
                        walls1.Add(r);
                        walls2.Add(r2);
                    }
                });
            });

            var rnd = UnityEngine.Random.Range(0, walls1.Count - 1);

            walls1[rnd].WallType = type;
            walls2[rnd].WallType = type;

            if (walls1.Count == 0)
                return new LineSegment2d(walls2[rnd].V1, walls2[rnd].V2).Center();
            return Vector2d.Empty;
        }


        protected void AddExitToStairs()
        {
            Stairs.Walls.ForEach(w => {
                if (planProcessor2D.ExitCell.PartsOfOutsideWalls != null && planProcessor2D.ExitCell.PartsOfOutsideWalls.Exists(ww => ww.Equals(w)))
                    w.WallType = WallType.WallWithDoor;
            });
        }
        void AddDoorForRooms()
        {
            AddDoorBetweenTwoRooms(Corridor, Stairs, WallType.NoWall);
            AddDoorBetweenTwoRooms(Lift, Corridor, WallType.NoWall);

            var localFlatsRooms = planProcessor2D.Rooms.FindAll(r => r.RoomType == RoomType.Flat);




            flats = new List<Flat2D>();
            localFlatsRooms.ForEach(r =>
            {
                var walls = r.Walls;
                flats.Add(new Flat2D(r.MainPolygon, BuildingForm, roomRequisites, FindExit(Corridor, r)));
            });

            flats.ForEach(f => f.Create2DSpace());
        }

        Vector2d FindExit(Room2D corridor, Room2D flat)
        {
            var possibleExits = new List<Vector2d>();

            corridor.Walls.ForEach(c =>
            {
                flat.Walls.ForEach(c1 =>
                {
                    var center = new LineSegment2d(c1.V1, c1.V2).Center();
                    if (c1.V1 == c.V1 && c1.V2 == c.V2 || c1.V1 == c.V2 && c1.V2 == c.V1 && !possibleExits.Contains(center))
                        possibleExits.Add(new LineSegment2d(c1.V1, c1.V2).Center());
                });
            });

            return possibleExits[UnityEngine.Random.Range(0, possibleExits.Count)];
        }

    }
}
