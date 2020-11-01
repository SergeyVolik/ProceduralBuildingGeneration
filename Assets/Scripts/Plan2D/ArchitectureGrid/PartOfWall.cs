using StraightSkeleton.Primitives;
using System;

namespace ArchitectureGrid
{
    public enum WallType { WallWithDoor = 0, WallWithWindow = 1, SimpleWall = 2, NoWall = 3 }
    public class PartOfWall
    {
        public Vector2d V1;
        public Vector2d V2;

        public WallType WallType;

        public bool IsWindow { get => WallType == WallType.WallWithWindow ? true : false; }
        public bool IsDoor { get => WallType == WallType.WallWithDoor || WallType == WallType.NoWall ? true : false; }

        public bool buildingPolygonPart;
        
     
        public PartOfWall(Vector2d v1, Vector2d v2, WallType wallType, bool buildingPolygonPart = false)
        {
            WallType = wallType;
            V1 = v1;
            V2 = v2;

            this.buildingPolygonPart = buildingPolygonPart;

        }

        public bool Equals(PartOfWall c2)
        {
            var c1 = this;

            return c1 == c2
                || c1.V1.Equals(c2.V1)
                && c1.V2.Equals(c2.V2)
                || c1.V1.Equals(c2.V2)
                && c1.V2.Equals(c2.V1);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                return V1.GetHashCode() ^ V2.GetHashCode();
            }
        }

    }

}
