
using ArchitectureGrid;
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
        protected bool needPassage;

        public APH_DefaulFloor2D()
        { }
        public APH_DefaulFloor2D(APH_DefaulFloor2D floor2D) {

            ExitPosition = floor2D.ExitPosition;
            MainPolygon = floor2D.MainPolygon;
            BuildingForm = floor2D.BuildingForm;
            roomRequisites = floor2D.roomRequisites;
            Floor = floor2D.Floor;
            flats = floor2D.flats;
            PlanProcessor2D = floor2D.PlanProcessor2D;

            needPassage = floor2D.needPassage;

            Create2DSpaceInternal();
        }
        public APH_DefaulFloor2D(List<Vector2d> outerPolygon, List<Vector2d> buoildingPolygon, List<RoomRequisite> requisite, int floor, int floorsNumber, Vector2d exit, bool passage) : base(outerPolygon, buoildingPolygon, requisite, floor, floorsNumber,exit)
        {
            needPassage = passage;
        }

        protected override void Create2DSpaceInternal()
        {

            PlanProcessor2D = new DefaultFloorPlanProcessor2D(MainPolygon, BuildingForm, ExitPosition, needPassage);

            PlanProcessor2D.CreatePlan();


            FindCorrdior();

            FindLift();
            FindStairs();
            AddExitToStairs();
            AddDoorForRooms();


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
        void AddDoorForRooms()
        {
            AddDoorBetweenTwoRooms(Corridor, Stairs, WallType.NoWall);
            AddDoorBetweenTwoRooms(Lift, Corridor, WallType.NoWall);

            var localFlatsRooms = PlanProcessor2D.Rooms.FindAll(r => r.RoomType == RoomType.Flat);




            flats = new List<Flat2D>();
            localFlatsRooms.ForEach(r =>
            {
                var walls = r.Walls;
                flats.Add(new Flat2D(r.MainPolygon, BuildingForm, roomRequisites, FindExit(Corridor, r),((DefaultFloorPlanProcessor2D)PlanProcessor2D).outsideCells));
            });

            int j = 0;
            flats.ForEach(f => {

                f.Create2DSpace();

                for (var i = 0; i < f.Rooms.Count; i++)
                    f.Rooms[i].FlatId = j;

                f.Rooms.ForEach(r =>
                {

                    if (r.AddDoorBetweenRooms(Corridor))
                        return;
                });
                j++;
            });



        }
        private void FindCorrdior()
        {
            Corridor = PlanProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Corridor);
        }
        private void FindLift()
        {
            Lift = PlanProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Lift);
        }
        private void FindStairs()
        {
            Stairs = PlanProcessor2D.Rooms.Find(r => r.RoomType == RoomType.Stairs);
        }

    }
}
