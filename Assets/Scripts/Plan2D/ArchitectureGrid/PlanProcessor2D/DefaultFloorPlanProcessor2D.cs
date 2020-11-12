using DataStructures.PriorityQueue;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Tools;
using UnityEngine;

using Assets.Scripts.Premies.Buildings.Building2D;

namespace ArchitectureGrid
{
    /// <summary>
    /// Основной класс ответсвенный за построение плана помещения 
    /// </summary>
    /// 
    public class DefaultFloorPlanProcessor2D : PlanProcessor2D
    {


        #region Constructors
        public DefaultFloorPlanProcessor2D(List<Vector2d> polygon, List<Vector2d> buildingForm, Vector2d exitPoint) : base(polygon, buildingForm, exitPoint)
        {
            neighbourCells = new List<PlanCell>();
            Rooms = new List<Room2D>();

            growthProcessor = new FlatGrowthProcessor2D(Grid, GridVector, Rooms, ExitCell);

        }

        public override void CreatePlan()
        {

            AddStairsToExit();
            AddCorridorToStairs();
            AddFlatsToFloor();
            AddLift();

            growthProcessor.GrowthOfRooms();
        }


        #endregion

        protected override List<PlanCell> GetCorridorCells(PlanCell door)
        {
            var list = new List<PlanCell>();
            var indexes = Grid.FindIndex(door);



            list.Add(Grid[indexes[0] - 3, indexes[1]]);
            list.Add(Grid[indexes[0] - 3, indexes[1] + 1]);
            list.Add(Grid[indexes[0] - 3, indexes[1] - 1]);



            return list;
        }
        protected override List<PlanCell> GetStairsCells(PlanCell door)
        {
            var list = new List<PlanCell>();
            var indexes = Grid.FindIndex(door);



            list.Add(Grid[indexes[0] - 1, indexes[1]]);
            list.Add(Grid[indexes[0] - 2, indexes[1]]);
            list.Add(door);

            return list;
        }
        protected override List<PlanCell> GetLiftCells(PlanCell door)
        {
            var list = new List<PlanCell>();
            var indexes = Grid.FindIndex(door);



            list.Add(Grid[indexes[0] - 2, indexes[1] + 1]);


            return list;
        }
        public void AddStairsToExit()
        {
            Room2D room = new Room2D(RoomType.Stairs, "Stairs") { Cells = GetStairsCells(ExitCell) };

          
            room.CenterOfRoom = room.FindCenterOfRoomForRectangle();

            Rooms.Add(room);
        }
        public void AddCorridorToStairs()
        {
            Room2D room = new Room2D(RoomType.Corridor, "Corridor") { Cells = GetCorridorCells(ExitCell) };


            Rooms.Add(room);
        }
        public void AddLift()
        {
            Room2D room = new Room2D(RoomType.Lift, "Lift") { Cells = GetLiftCells(ExitCell) };


            Rooms.Add(room);
        }
        public void AddFlatsToFloor()
        {

            Rooms.Add(
                new Room2D(RoomType.Flat, "Flat1")
                {
                    Cells = new List<PlanCell>() { Grid[Grid.GetLength(0) - 1, Grid.GetLength(1) - 1] }
                }
            );
            growthProcessor.RoomsToGrowth.Add(Rooms[Rooms.Count - 1]);

            Rooms.Add(new Room2D(RoomType.Flat, "Flat2")
            {
                Cells = new List<PlanCell>() { Grid[0, 0] }
            });

            growthProcessor.RoomsToGrowth.Add(Rooms[Rooms.Count - 1]);
            Rooms.Add(new Room2D(RoomType.Flat, "Flat3")
            {
                Cells = new List<PlanCell>() { Grid[0, Grid.GetLength(1) - 1] }
            }
            );
            growthProcessor.RoomsToGrowth.Add(Rooms[Rooms.Count - 1]);
            Rooms.Add(new Room2D(RoomType.Flat, "Flat4")
            {
                Cells = new List<PlanCell>() { Grid[Grid.GetLength(0) - 1, 0] }
            }
            );
            growthProcessor.RoomsToGrowth.Add(Rooms[Rooms.Count - 1]);




        }

    }
    public class RoofFloorPlanProcessor2D : PlanProcessor2D
    {


        #region Constructors
        public RoofFloorPlanProcessor2D(List<Vector2d> polygon, List<Vector2d> buildingForm, Vector2d exitPoint) : base(polygon, buildingForm, exitPoint)
        {
            neighbourCells = new List<PlanCell>();
            Rooms = new List<Room2D>();

           // growthProcessor = new EntrancesGrowthProcessor(Grid, Rooms, ExitCell);

        }

        public override void CreatePlan()
        {

            AddStairsToExit();
            
            AddFlatsToFloor();         

            growthProcessor.GrowthOfRooms();
        }


        #endregion

       
        protected override List<PlanCell> GetStairsCells(PlanCell door)
        {
            var list = new List<PlanCell>();
            var indexes = Grid.FindIndex(door);



            list.Add(Grid[indexes[0] - 1, indexes[1]]);
            list.Add(Grid[indexes[0] - 2, indexes[1]]);
            list.Add(door);

            return list;
        }
        
        public void AddStairsToExit()
        {
            Room2D room = new Room2D(RoomType.Stairs, "Stairs") { Cells = GetStairsCells(ExitCell) };


            room.CenterOfRoom = room.FindCenterOfRoomForRectangle();

            Rooms.Add(room);
        }       
        public void AddFlatsToFloor()
        {
            var freeSpace = new Room2D(RoomType.Flat, "FreeSpace")
            {
                Cells = new List<PlanCell>() { }
            };

            for (var i = 0; i < Grid.GetLength(0); i++)
                for (var j = 0; j < Grid.GetLength(1); j++)
                    if (Grid[i, j].IsFree)
                        freeSpace.AddCell(Grid[i, j]);

            Rooms.Add(
                freeSpace
            );
            //growthProcessor.RoomsToGrowth.Add(Rooms[Rooms.Count - 1]);

        }

        protected override List<PlanCell> GetCorridorCells(PlanCell door)
        {
            throw new NotImplementedException();
        }

        protected override List<PlanCell> GetLiftCells(PlanCell door)
        {
            throw new NotImplementedException();
        }
    }
}

