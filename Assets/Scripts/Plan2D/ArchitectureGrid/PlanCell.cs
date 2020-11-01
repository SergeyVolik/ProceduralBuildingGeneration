using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
    public enum PlanCellTag { Inside = 0, Outside = 1, None = 2 }
    public class PlanCell : IComparable<PlanCell>
    {
        public PlanCell LeftCell;
        public PlanCell RightCell;
        public PlanCell TopCell;
        public PlanCell BottomCell;

        public PlanCellTag Tag;
        public List<Vector2d> Square;

        public List<PartOfWall> PartsOfOutsideWalls;

        public List<PartOfWall> BuildingWalls
        {
            get
            {
                List<PartOfWall> walls = new List<PartOfWall>();
                
                PartsOfOutsideWalls?.ForEach(p => {
                    if (p.IsWindow)
                        walls.Add(p);
                    });

                return walls;
            }
        }

        public Vector2d Center;
        public Room2D room;


        public List<PlanCell> NotFreeNeighbours { 
            get {

                var list = new List<PlanCell>();

                if (LeftCell != null && LeftCell.room != null)
                    list.Add(LeftCell);
                if (RightCell != null && RightCell.room != null)
                    list.Add(RightCell);
                if (TopCell != null && TopCell.room != null)
                    list.Add(TopCell);
                if (BottomCell != null && BottomCell.room != null)
                    list.Add(BottomCell);

                return list;
            } }


        public int Area { 
            get 
            {               
                return 4;
            } 
        }



        public bool IsFree {
            get
            {
                if (room == null && Tag == PlanCellTag.Inside)
                    return true;
                else return false;
            }
        }

        public PlanCell(List<Vector2d> vertexes, List<Vector2d> mainPolygon, List<Vector2d> buildingPolygon)
        {
            
            Square = vertexes;           
            
            FindCenter();                 
            DetermineTag(mainPolygon);
            DetermineWindows(buildingPolygon);


        }

        private void FindCenter()
        {
            var line1 = new LineLinear2d(Square[0], Square[2]);
            var line2 = new LineLinear2d(Square[1], Square[3]);
            Center = line1.Collide(line2);
        }

        private void DetermineWindows(List<Vector2d> buildingPolygon)
        {
            if (PartsOfOutsideWalls != null) {
                PartsOfOutsideWalls.ForEach(p => {
                    if (PrimitiveUtils.IsPointOnBoarder(p.V1, buildingPolygon))
                        p.WallType = WallType.WallWithWindow;
                        p.buildingPolygonPart = true;
                });
            }
        }

        private void FindPartsOfWalls(List<Vector2d> polygon)
        {

            bool noOnePointInBoard = true;
            for (var i = 0; i < Square.Count; i++)
                if (PrimitiveUtils.IsPointOnBoarder(Square[i], polygon))
                    noOnePointInBoard = false;

                PartsOfOutsideWalls = new List<PartOfWall>();
            for (var i = 0; i < Square.Count - 1; i++)
                if (PrimitiveUtils.IsPointOnBoarder(Square[i], polygon) && PrimitiveUtils.IsPointOnBoarder(Square[i + 1], polygon))
                    PartsOfOutsideWalls.Add(new PartOfWall(Square[i], Square[i + 1], WallType.SimpleWall));


            if (PrimitiveUtils.IsPointOnBoarder(Square[0], polygon) && PrimitiveUtils.IsPointOnBoarder(Square[Square.Count - 1], polygon))
                PartsOfOutsideWalls.Add(new PartOfWall(Square[0], Square[Square.Count - 1], WallType.SimpleWall));


            if (noOnePointInBoard)
                PartsOfOutsideWalls = null;
        }
        
        private void DetermineTag(List<Vector2d> polygon)
        {
            if (PrimitiveUtils.IsPointInsidePolygon(Center, polygon))
            {
                Tag = PlanCellTag.Inside;              
                FindPartsOfWalls(polygon);
            }
            else Tag = PlanCellTag.Outside;
            
           
        }
       
        public bool IsOuterWallCell()
        {
            if (PartsOfOutsideWalls != null)
                return true;
            return false;
        }

        public int CompareTo(PlanCell other)
        {

            return other.Center.X.CompareTo(Center.X);
            
        }
    }
}
