﻿using ArchitectureGrid;
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
    public class Flat2D : Premises2D
    {               

       

        public Flat2D(List<Vector2d> mainPolygon, List<Vector2d> buildinPolygon,  List<RoomRequisite> _roomsRequisite, Vector2d exit)
        {

            BuildingForm = buildinPolygon;
            MainPolygon = mainPolygon;
            Rooms = new List<Room2D>();
            _roomsRequisite.ForEach(req => Rooms.Add(new Room2D(req)));

            roomRequisites = _roomsRequisite;
            _exitPoint = exit;
            

        }
        
        public override List<Room2D> GetRooms()
        {
            return Rooms;
        }

        protected override void Create2DSpaceInternal()
        {

            planProcessor2D = new FlatPlanProcessor2D(MainPolygon, BuildingForm, _exitPoint);
            planProcessor2D.Rooms.AddRange(Rooms);
            planProcessor2D.CreatePlan();
            UpdateWallsType();


        }

        protected void UpdateWallsType()
        {
            Rooms.ForEach(r =>
            {
                r.Walls.ForEach(w =>
                {

                    var ww = planProcessor2D.Windows.FirstOrDefault(windows => windows.Equals(w));

                    if (ww?.Equals(w) == true)
                        w.WallType = ww.WallType;
                });
            });
        }
    }
}
