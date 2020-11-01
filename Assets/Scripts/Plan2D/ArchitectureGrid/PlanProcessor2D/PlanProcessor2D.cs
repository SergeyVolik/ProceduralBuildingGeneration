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
    

    


    public abstract class PlanProcessor2D : BasePlanProcessor2D
    {
             

        //debug
        public List<PlanCell> neighbourCells;

        #region Constructors
        public PlanProcessor2D(List<Vector2d> polygon, List<Vector2d> buildingForm, Vector2d  _exitPoint) : base(polygon, buildingForm, _exitPoint)
        {
            BuildingForm = buildingForm;
            neighbourCells = new List<PlanCell>();
            Rooms = new List<Room2D>();
            AddExit(_exitPoint);

        }

        protected void AddExit(Vector2d exitPoint)
        {
            if (exitPoint != Vector2d.Empty)
            {
                var minDist = OuterBoarderCells.Min(c => c.Center.DistanceTo(exitPoint));
                ExitCell = OuterBoarderCells.Find(c => c.Center.DistanceTo(exitPoint) == minDist);

                ExitCell.PartsOfOutsideWalls[0].WallType = WallType.WallWithDoor;
            }
        }


        #endregion


        protected List<PlanCell> FindNeighbourCellsOfRoom(Room2D room)
        {
            var neighbourCells = new List<PlanCell>();

            for (var i = 0; i < room.Cells.Count; i++)
            {
                int w = Grid.GetLength(0); // width
                int h = Grid.GetLength(1); // height

                var indexes = Grid.CoordinatesOf(room.Cells[i]);
                PlanCell cell = null;

               
                if (CheckTopCellByCondition(Grid, indexes, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }) && !neighbourCells.Contains(cell))
                    neighbourCells.Add(cell);


                if (CheckBottomCellByCondition(Grid, indexes, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }) && !neighbourCells.Contains(cell))
                    neighbourCells.Add(cell);


                if (CheckRightCellByCondition(Grid, indexes, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }) && !neighbourCells.Contains(cell))
                    neighbourCells.Add(cell);

                if (CheckLeftCellByCondition(Grid, indexes, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }) && !neighbourCells.Contains(cell))
                    neighbourCells.Add(cell);

            }


            return neighbourCells;
        }




        protected void AddRoomToGrid(Room2D room)
        {
            for (var i = 0; i < Grid.GetLength(0); i++)
            {
                for (var j = 0; j < Grid.GetLength(1); j++)
                {

                    if (PrimitiveUtils.IsPointInsidePolygon(Grid[i, j].Center, room.MainPolygon))
                    {
                        Grid[i, j].room = room;
                        room.Cells.Add(Grid[i, j]);
                    }
                }
            }
        }

        #region Windows

        
        #endregion
        #region Exits
       
        #region ApartamentPanelHause

       


        protected abstract List<PlanCell> GetCorridorCells(PlanCell door);

        protected abstract List<PlanCell> GetStairsCells(PlanCell door);
        protected abstract List<PlanCell> GetLiftCells(PlanCell door);


        #endregion
        #endregion




    }
}

