using StraightSkeleton.Primitives;
using System.Collections.Generic;

namespace StraightSkeleton.Corridor
{
    public class SkeletonCorridorResult
    {
        public List<Vector2d> polygonOfCorridor;

        public SkeletonCorridorResult(List<Vector2d> vertx)
        {
            polygonOfCorridor = vertx;
        }
    }
}
