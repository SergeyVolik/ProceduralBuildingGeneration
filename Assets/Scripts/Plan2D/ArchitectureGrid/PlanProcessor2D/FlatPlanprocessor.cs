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





    public class FlatPlanProcessor2D : PlanProcessor2D
    {


        List<PlanCell> OutsideCells;

        #region Constructors
        public FlatPlanProcessor2D(List<Vector2d> polygon, List<Vector2d> buildingForm, Vector2d exitPoint, List<PlanCell> outsideCells) : base(polygon, buildingForm, exitPoint)
        {
            OutsideCells = outsideCells;

             BuildingForm = buildingForm;
            neighbourCells = new List<PlanCell>();
            Rooms = new List<Room2D>();
           
            growthProcessor = new RoomGrowthProcessor2D(Grid, GridVector, Rooms, ExitCell);

        }

        public override void CreatePlan()
        {
            FindWindows();

            if(OutsideCells != null)
                GridVector.ForEach(cell =>
                {
                    if (OutsideCells.Exists(c => c.Center.Equals(cell.Center)))
                        cell.Tag = PlanCellTag.Outside;
                });

            growthProcessor.GrowthOfRooms();
            AddDoorsToRooms();
        }

        void AddDoorsToRooms()
        {

            int roomsWithDoors = 0;

            int breakCound = 0;
            while (roomsWithDoors != Rooms.Count)
            {
                roomsWithDoors = 0;

                var roomWithourDoor = Rooms.FirstOrDefault(r => !r.HaveDoor);

                for (var i = 0; i < Rooms.Count; i++)
                {
                    if (Rooms[i] == roomWithourDoor)
                        continue;

                    if (Rooms[i].RoomRequisite.MaxDoorConections > Rooms[i].DoorCount)
                    {
                        if (roomWithourDoor.AddDoorBetweenRooms(Rooms[i]))
                            break;
                    }
                }
                breakCound++;

                roomsWithDoors = Rooms.Count(r => r.HaveDoor);

                if (breakCound > 100)
                    break;
            }

        }
        void FindWindows()
        {
            var windows = Windows;
        }

        #endregion

        protected override List<PlanCell> GetCorridorCells(PlanCell door)
        {
            var list = new List<PlanCell>();
          

            return list;
        }
        protected override List<PlanCell> GetStairsCells(PlanCell door)
        {
            var list = new List<PlanCell>();
          

            return list;
        }
        protected override List<PlanCell> GetLiftCells(PlanCell door)
        {
            var list = new List<PlanCell>();
            

            return list;
        }
        








    }
}

