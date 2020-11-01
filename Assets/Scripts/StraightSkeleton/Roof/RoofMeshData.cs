using StraightSkeleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using StraightSkeleton.Polygon;
using StraightSkeleton.Primitives;

namespace Plan3d
{
    class RoofMeshData
    {

        public readonly List<List<int>> indicesOfPolygons;

        public readonly List<List<Vector3>> verticesOfPolygons;

        public RoofMeshData(SkeletonRoofResult roof, List<Vector2d> outerPolygon, int numberOfFloors, double floorHeight)
        {
            indicesOfPolygons = new List<List<int>>();
            verticesOfPolygons = new List<List<Vector3>>();
            CalcMeshData(roof, outerPolygon, numberOfFloors, floorHeight);
        }

        private void CalcMeshData(SkeletonRoofResult roof, List<Vector2d> outerPolygon, int numberOfFloors, double floorHeight)
        {
            for (var i = 0; i < roof.Edges.Count; i++)
            {
                var indices = Triangulator.Triangulate<Mesh>(roof.Edges[i].Polygon.ToArray());

                // Create the Vector3 vertices
                var polygonOFRoof = roof.Edges[i].Polygon;
                Vector3[] vertices = new Vector3[polygonOFRoof.Count];
                float hight = (float)(numberOfFloors * floorHeight + floorHeight/2f);
                for (int j = 0; j < vertices.Length; j++)
                {
                    if (PrimitiveUtils.IsPointOnBoarder(polygonOFRoof[j], outerPolygon))
                        vertices[j] = new Vector3((float)polygonOFRoof[j].X, hight, (float)polygonOFRoof[j].Y);
                    else vertices[j] = new Vector3((float)polygonOFRoof[j].X, (float)(hight + floorHeight), (float)polygonOFRoof[j].Y);
                }


                indicesOfPolygons.Add(indices.ToList());
                verticesOfPolygons.Add(vertices.ToList());

            }
        }

       
    }
}
