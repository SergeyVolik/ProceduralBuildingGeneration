
using Assets.Scripts.Premies.Buildings.Building2D;
using Floor;
using Rooms;
using StraightSkeleton;
using StraightSkeleton.Corridor;
using StraightSkeleton.Polygon.RandomRectangularPolygon;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buldings
{
    public class PrivateHouse2D : Building2D
    {       
        public new int Angles
        {
            get => _angles;
            set
            {
                if (value % 2 != 0 || value < 4)
                    throw new ArgumentException("Invalid value: _angels can't set " +
                        "incorect number! need if(value >= 4 and value % 2 == 0)");
                else _angles = value;
            }
        }

        public new float Area
        {
            get => _area;
            set
            {
                if (value < 1000 || value > 2000)
                    throw new ArgumentException("Invalid value: input correct area: area >= 1000 and area <= 2000");
                else _area = value;
            }
        }
        public PrivateHouse2D(int numberOfFloors, int angles, float area, List<Vector2d> polygon, RoofType roof) : base(numberOfFloors, area, polygon, roof)
        {
            NumberOfFloors = numberOfFloors;
            Area = area;
            Angles = angles;
            MainPolygon = polygon;
        }
       

        protected override void Create2DSpaceInternal()
        {
            throw new NotImplementedException();
        }

        public override List<Room2D> GetRooms()
        {
            throw new NotImplementedException();
        }
    }
}