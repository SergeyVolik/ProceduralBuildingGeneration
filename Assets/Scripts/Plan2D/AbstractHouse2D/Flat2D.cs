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
    public class Flat2D : Premises2D, IFloorPremises2D
    {               

       

        public Flat2D(List<Vector2d> mainPolygon, List<Vector2d> buildinPolygon,  List<RoomRequisite> _roomsRequisite, Vector2d exit)
        {

            BuildingForm = buildinPolygon;
            MainPolygon = mainPolygon;
            Rooms = new List<Room2D>();
            _roomsRequisite.ForEach(req => Rooms.Add(new Room2D(req)));

            roomRequisites = _roomsRequisite;
            ExitPosition = exit;
            

        }
        
        public override List<Room2D> GetRooms()
        {
            return Rooms;
        }

        protected override void Create2DSpaceInternal()
        {

            PlanProcessor2D = new FlatPlanProcessor2D(MainPolygon, BuildingForm, ExitPosition);
            PlanProcessor2D.Rooms.AddRange(Rooms);
            PlanProcessor2D.CreatePlan();
            UpdateWallsType();


        }

        protected void UpdateWallsType()
        {
            Rooms.ForEach(r =>
            {
                r.Walls.ForEach(w =>
                {

                    var ww = PlanProcessor2D.Windows.FirstOrDefault(windows => windows.Equals(w));

                    if (ww?.Equals(w) == true)
                        w.WallType = ww.WallType;
                });
            });
        }
    }
}
