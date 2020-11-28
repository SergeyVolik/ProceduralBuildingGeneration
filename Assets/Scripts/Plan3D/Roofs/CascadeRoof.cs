using Plan3d;
using StraightSkeleton;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CascadeRoof : IRoof3D
{
    private List<Vector2d> _roofBoarder;
    private float _floorHight;
    private int _numberOfFloors;
    private Transform _buildingRoot;
    private Material _roofMaterial;
    public CascadeRoof(List<Vector2d> roofBoarder, float floorHight, int numberOfFloors, Transform buildingRoot, Material roofMaterial) {

        _roofBoarder = roofBoarder;
        _floorHight = floorHight;
        _numberOfFloors = numberOfFloors;
        _buildingRoot = buildingRoot;
        _roofMaterial = roofMaterial;
    }

    public void VisualizeRoof()
    {
        var roof = SkeletonBuilder.BuildRoof(_roofBoarder);
        var meshData = new RoofMeshData(roof, _roofBoarder, _numberOfFloors - 2, _floorHight);
        var RoofRoot = new GameObject("RoofRoot");

        RoofRoot.transform.parent = _buildingRoot;

        for (var i = 0; i < meshData.verticesOfPolygons.Count; i++)
        {
            Mesh msh = new Mesh();
            for (var j = 0; j < meshData.verticesOfPolygons[i].Count; j++)
            {
                meshData.verticesOfPolygons[i][j] += _buildingRoot.transform.position;
            }
            msh.vertices = meshData.verticesOfPolygons[i].ToArray();
            msh.triangles = meshData.indicesOfPolygons[i].ToArray();
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            var emptyObj = new GameObject("roofPart" + i.ToString());
            emptyObj.transform.parent = RoofRoot.transform;
            // Set up game object with mesh;
            emptyObj.AddComponent(typeof(MeshRenderer));

            var meshRender = emptyObj.GetComponent<MeshRenderer>();
            meshRender.material = _roofMaterial;

            Vector3[] vertices = msh.vertices;

            Vector2[] uvs = new Vector2[vertices.Length];

            //for (int k = 0; k < uvs.Length; k++)
            //{
            //    uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            //}

            if (vertices.Length == 4)
            {
                uvs[0] = new Vector2(0, 0);
                uvs[1] = new Vector2(0, 1);
                uvs[2] = new Vector2(1, 1);
                uvs[3] = new Vector2(1, 0);
            }
            else if (vertices.Length == 3)
            {
                uvs[0] = new Vector2(0, 0);
                uvs[1] = new Vector2(0, 1);
                uvs[2] = new Vector2(1, 1);
            }
            msh.uv = uvs;

            MeshFilter filter = emptyObj.AddComponent(typeof(MeshFilter)) as MeshFilter;
            filter.mesh = msh;
        }


    }
}

