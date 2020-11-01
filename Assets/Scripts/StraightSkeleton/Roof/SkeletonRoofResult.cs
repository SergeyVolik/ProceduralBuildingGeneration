using System.Collections.Generic;
using StraightSkeleton.Primitives;
using UnityEngine;

namespace StraightSkeleton
{
    /// <summary> Represents skeleton algorithm results. </summary>
    public class SkeletonRoofResult
    {
        /// <summary> Result of skeleton algorithm for edge. </summary>
        public readonly List<EdgeResult> Edges;

        /// <summary> Distance points from edges. </summary>
        public readonly Dictionary<Vector2d, double> Distances;

       

        /// <summary> Creates instance of <see cref="SkeletonRoofResult"/>. </summary>
        public SkeletonRoofResult(List<EdgeResult> edges, Dictionary<Vector2d, double> distances)
        {
            Edges = edges;
            Distances = distances;
        }

       

       
    }
}