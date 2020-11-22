using ArchitectureGrid;
using Assets.Scripts.Premies.Buildings.Floors;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor
{
    public class Entrance2D : Premises2D, IBuildingPremises2D
    {
        public Floor2D basementFloor2d;
        public List<Floor2D> floors;
        public Floor2D roofFloor2d;

        public int FloorNumber;
        public List<RoomRequisite> roomsRequisite;
        public Vector2d _exit;
        public RoofType _roofType;

        public RoofType RoofType => _roofType;
        public bool NeedFirstFloorPassage;
        public Entrance2D(Entrance2D entrance) {
            _roofType = entrance.RoofType;
            BuildingForm = entrance.BuildingForm;
            FloorNumber = entrance.FloorNumber;
            MainPolygon = entrance.MainPolygon;
            roomsRequisite = entrance.roomsRequisite;
            _exit = entrance._exit;

            basementFloor2d = entrance.basementFloor2d;
            floors = entrance.floors;
            roofFloor2d = entrance.roofFloor2d;
            NeedFirstFloorPassage = entrance.NeedFirstFloorPassage;
        }
        public Entrance2D(List<Vector2d> outerPolygon, List<Vector2d> buildingPolygon, int floorsNumber, Vector2d exit , List<RoomRequisite> _roomsRequisite, RoofType roofType, bool passage)
        {
            _roofType = roofType;
            BuildingForm = buildingPolygon;
            FloorNumber = floorsNumber;
            MainPolygon = outerPolygon;
            floors = new List<Floor2D>();
            roomsRequisite = _roomsRequisite;
            _exit = exit;
            NeedFirstFloorPassage = passage;
        }

        public override List<Room2D> GetRooms2D()
        {
            var rooms = new List<Room2D>();

            floors.ForEach(e => rooms.AddRange(e.GetRooms2D()));

            return rooms;
        }


        protected override void Create2DSpaceInternal()
        {
            bool passage = false;

            if (NeedFirstFloorPassage)
            {                            
                    passage = true;               
            }
            else passage = false;

            basementFloor2d = new APH_BasementFloor2D(MainPolygon, BuildingForm, roomsRequisite, floors.Count, _exit, passage);
            basementFloor2d.Create2DSpace();
            floors.Add(basementFloor2d);

           
            for (var i = 0; i < FloorNumber-2; i++)
            {
                if (NeedFirstFloorPassage)
                {
                    if (i == 0)
                        passage = true;
                    else passage = false;
                }
                

                var floor = new APH_DefaulFloor2D(MainPolygon, BuildingForm, roomsRequisite, i + 1, floors.Count, _exit, passage);
                floor.Create2DSpace();
                floors.Add(floor);
            }

            if (_roofType == RoofType.FLAT)
            {
                roofFloor2d = new APH_RoofFloor2D(MainPolygon, BuildingForm, roomsRequisite, floors.Count, floors.Count, _exit);
                roofFloor2d.Create2DSpace();
                floors.Add(roofFloor2d);
            }
        }
    }
}
