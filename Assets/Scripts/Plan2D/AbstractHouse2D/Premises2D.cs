using ArchitectureGrid;
using Assets.Scripts.Premies.Buildings.Building2D;

using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floor
{
    public abstract class Premises2D : Polygon, ISpaceCreator2D
    {
        public Vector2d _exitPoint;
       
        public BasePlanProcessor2D planProcessor2D;
        public List<Room2D> Rooms;
        protected List<RoomRequisite> roomRequisites;
        protected abstract void Create2DSpaceInternal();
        public void Create2DSpace()
        {
            Create2DSpaceInternal();
        }

        public Premises2D(List<Vector2d> _OutsidePolygon, List<Vector2d> _BuildingForm)
        {           
            MainPolygon = _OutsidePolygon;
            BuildingForm = _BuildingForm;
        }
        public Premises2D()
        {

        }

        public abstract List<Room2D> GetRooms();


        

        public List<PartOfWall> GetOuterWalls2D()
        {
            var list = new List<PartOfWall>();

            for (var i = 0; i < planProcessor2D.OuterBoarderCells.Count; i++)
            {
                list.AddRange(planProcessor2D.OuterBoarderCells[i].PartsOfOutsideWalls);
            }

            return list;

        }

       
    }
}
