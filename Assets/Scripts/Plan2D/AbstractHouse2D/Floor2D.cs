using ArchitectureGrid;
using Rooms;
using StraightSkeleton.Primitives;
using System.Collections.Generic;
using DataStructures.PriorityQueue;
using UnityEngine;
using System;
using Assets.Scripts.Premies.Buildings.Floors;

namespace Floor
{

    public abstract class Floor2D : Premises2D, IBuildingPremises2D
    {



        public List<Flat2D> flats;
        public Room2D Corridor;
        public Room2D Lift;
        public Room2D Stairs;
        public int Floor;
        public int FloorsNumber;

        public Floor2D(Floor2D floor) {
            ExitPosition = floor.ExitPosition;
            MainPolygon = floor.MainPolygon;
            BuildingForm = floor.BuildingForm;
            roomRequisites = floor.roomRequisites;
            Floor = floor.Floor;
        }
        public Floor2D(List<Vector2d> _MainPolygon, List<Vector2d> _BuildingForm, Vector2d exitPoint, List<RoomRequisite> requisite, int floor, int floorNumber)
        {
            ExitPosition = exitPoint;
            MainPolygon = _MainPolygon;
            BuildingForm = _BuildingForm;
            roomRequisites = requisite;
            Floor = floor;
        }

        
    }

}
