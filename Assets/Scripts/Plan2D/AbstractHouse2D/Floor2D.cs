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
  
    public abstract class Floor2D : Premises2D
    {
        
        

        public List<Flat2D> flats;
        protected Room2D Corridor;
        protected Room2D Lift;
        protected Room2D Stairs;
        public int Floor;
        public int FloorsNumber;
        public Floor2D(List<Vector2d> _MainPolygon, List<Vector2d> _BuildingForm, Vector2d exitPoint, List<RoomRequisite> requisite, int floor, int floorNumber)
        {
            _exitPoint = exitPoint;
            MainPolygon = _MainPolygon;
            BuildingForm = _BuildingForm;
            roomRequisites = requisite;
            Floor = floor;
        }

        
    }

}
