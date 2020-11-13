using ArchitectureGrid;
using Assets.Scripts.Premies.Buildings.Building2D;

using Floor;
using Rooms;
using StraightSkeleton;
using StraightSkeleton.Polygon.RandomRectangularPolygon;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Buildings
{
    class ApartamentPanelHouse2D : Building2D
    {
       
        public new int Angles
        {
            get => _angles;
            set
            {
                if (value != 4)
                    throw new ArgumentException("Invalid value: _angels can't set " +
                        "incorect number! need 4");
                else _angles = value;
            }
        }

        public new float Area
        {
            get => _area;
            set
            {
                if (value < 400 || value > 1200)
                    throw new ArgumentException("Invalid value: input correct area: area >= 400 and area <= 1200");
                if (value / BUILDING_WIDTH % 2 != 0)
                    throw new ArgumentException("Invalid value: input correct area: building lenght not % on 2 -> width is 20 you need area/20-> lenght");
                else _area = value;
            }
        }

        private int entracesNumber;

        public int EntracesNumber
        {
            get => entracesNumber;
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentException("Invalid value: input correct entraces number: entraces < 1 or entraces > 10");
                else entracesNumber = value;
            }
        }

        private const int BUILDING_WIDTH = 20;

      
        public ApartamentPanelHouse2D(int numberOfFloors, float area, int entraces, List<RoomRequisite> requisites, List<Vector2d> buildingPolygon) : base(numberOfFloors, area, buildingPolygon)
        {

            roomRequisites = requisites;
            NumberOfFloors = numberOfFloors;
            Area = area;
            Angles = 4;
            EntracesNumber = entraces;

            Create2DSpaceInternal();
            
        }
     
        public override List<Room2D> GetRooms()
        {
            throw new NotImplementedException();
        }
        List<List<Vector2d>> CreateEntrances()             
        {
            
            BuildingForm = MainPolygon;
            //planProcessor2D = new Floor(MainPolygon, MainPolygon, null);
            Debug.Log("CreateEntrances");

            var bottomEntrances = new List<List<Vector2d>>();

            if (EntracesNumber > 1)
            {
                var lenght = (int)MainPolygon[0].DistanceTo(MainPolygon[1]);
                var step = Area / BUILDING_WIDTH;
                var t = step + 2;
                var currVertexTop = new Vector2d(MainPolygon[3]);
                var currVertexBottom = new Vector2d(MainPolygon[0]);
                var nextTop = new Vector2d(t, Math.Round(MainPolygon[2].Y));
                var nextBottom = new Vector2d(t, Math.Round(MainPolygon[1].Y));

                var list = new List<Vector2d>();

                list.Add(currVertexBottom);
                list.Add(nextBottom);
                list.Add(nextTop);
                list.Add(currVertexTop);

                bottomEntrances.Add(list);

                while ((lenght - t) / step > 1)
                {
                    t += step;

                    list = new List<Vector2d>();
                    currVertexBottom = nextBottom;
                    currVertexTop = nextTop;

                    list.Add(currVertexBottom);
                    nextBottom = new Vector2d(t, currVertexBottom.Y);
                    list.Add(nextBottom);
                    nextTop = new Vector2d(t, currVertexTop.Y);
                    list.Add(nextTop);
                    list.Add(currVertexTop);

                    bottomEntrances.Add(list);

                }

                //bottomEntrances.Remove(list);
                list = new List<Vector2d>();

                list.Add(nextBottom);
                list.Add(MainPolygon[1]);
                list.Add(MainPolygon[2]);
                list.Add(nextTop);


                bottomEntrances.Add(list);
            }
            else bottomEntrances.Add(MainPolygon);

            return bottomEntrances;
        }

        private void CreateFloorPlans(List<List<Vector2d>> entraces)
        {
            entrances = new List<Entrance2D>();

            for (var i = 0; i < entraces.Count; i++)
            {
                var Center = new LineSegment2d(entraces[i][0], entraces[i][1]).Center();
                var entrace = new Entrance2D(entraces[i], BuildingForm, NumberOfFloors, Center, roomRequisites);
                entrace.Create2DSpace();
                entrances.Add(entrace);

            }

        }

        protected override void Create2DSpaceInternal()
        {
            
            CreateFloorPlans(CreateEntrances());
        }

    //    public override List<PartOfWall> GetPartOfBuilding()
    //    {

    //        var parts = new List<PartOfWall>();
    //        entrances.ForEach(e => {

    //            parts.AddRange(e.GetPartOfBuilding());

    //        });

    //        return parts;
    //    }
    }   
}
