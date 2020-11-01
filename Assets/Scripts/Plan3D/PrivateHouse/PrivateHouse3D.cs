using UnityEngine;
using Buldings;
using StraightSkeleton.Primitives;
using ArchitectureGrid;
using Plan3d;
using System;
using System.Collections.Generic;
using Assets.Scripts.Premies.Buildings;
using Assets.Scripts.Premies.Buildings.Building2D;
using StraightSkeleton;

namespace Assets.Scripts.Builders.Builder3D
{


    public class PrivateHouse3D : Building3D
    {


        private const float StartY = 0.01f;

        //private GameObject _CubeForFoundation;

        //public GameObject BuildingRoot { get; set; }



        //public PrivateHouse3D(GameObject root, List<PremisesPrefabs> buildingPrefabs, Material RoofMaterial, List<RoomSetting> roomsPrefabs) : base(buildingPrefabs, RoofMaterial, roomsPrefabs)
        //{
        //    //_house2D = new PrivateHouse2D();
        //    BuildingRoot = root;
        //}

        //public PrivateHouse3D()
        //{
            
        //}


             

        #region OuterWalls     

        //protected override void VisualizeWalls()
        //{
        //    var floors = _house2D.floors;
        //    var OuterWallRoot = new GameObject("OuterWallRoot");
        //    OuterWallRoot.transform.parent = BuildingRoot.transform;

        //    for (var j = 0; j < floors.Count; j++)
        //    {
        //        var wallpositions = floors[j].GetOuterWalls2D();
        //        for (var i = 0; i < wallpositions.Count; i++)
        //        {
        //            int high = 1 + j * 2;
        //            GameObject curWall;
        //            GameObject currPrefab;
        //            var center = LineSegment2d.Center(wallpositions[i].V1, wallpositions[i].V2);
        //            var position = new Vector3((float)center.X, high, (float)center.Y) + BuildingRoot.transform.position;

        //            currPrefab = premisesPrefabs.SelectWallPrefab(wallpositions[i].WallType);

        //            curWall = Instantiate(currPrefab, position, Quaternion.identity);

        //            curWall.transform.parent = OuterWallRoot.transform;
        //            if (!CheckRotation(curWall, wallpositions[i], high))
        //                curWall.transform.rotation = Quaternion.Euler(curWall.transform.rotation.x, curWall.transform.rotation.y + 90, curWall.transform.rotation.z);
        //        }


        //    }
        //}
       
        #endregion

        #region Roof
        protected void VisualizeRoof()
        {
            //var roof = SkeletonBuilder.BuildRoof();
            //var meshData = new RoofMeshData(_house2D.Roof, _house2D.OutsidePolygon, _house2D.NumberOfFloors, Building2D.FloorHight);
            //var RoofRoot = new GameObject("RoofRoot");

            //RoofRoot.transform.parent = BuildingRoot.transform;

            //for (var i = 0; i < meshData.verticesOfPolygons.Count; i++)
            //{
            //    Mesh msh = new Mesh();
            //    for (var j = 0; j < meshData.verticesOfPolygons[i].Count; j++)
            //    {
            //        meshData.verticesOfPolygons[i][j] += BuildingRoot.transform.position;
            //    }
            //    msh.vertices = meshData.verticesOfPolygons[i].ToArray();
            //    msh.triangles = meshData.indicesOfPolygons[i].ToArray();
            //    msh.RecalculateNormals();
            //    msh.RecalculateBounds();

            //    var emptyObj = new GameObject("roofPart" + i.ToString());
            //    emptyObj.transform.parent = RoofRoot.transform;
            //    // Set up game object with mesh;
            //    emptyObj.AddComponent(typeof(MeshRenderer));
            //    MeshFilter filter = emptyObj.AddComponent(typeof(MeshFilter)) as MeshFilter;
            //    filter.mesh = msh;

            //    var meshRender = emptyObj.GetComponent<MeshRenderer>();
            //    meshRender.material = premisesPrefabs.CeilingMaterial;
            //}

        }
        #endregion

        #region Foundation
        protected void VisualizeFoundation()
        {

            //var RoofFoundation = new GameObject("RoofFoundation");
            //RoofFoundation.transform.parent = BuildingRoot.transform;
            //var archGrid = house.floors[0].ArchPlan;

            //for (var i = 0; i < archGrid.Grid.GetLength(0); i++)
            //{
            //    for (var j = 0; j < archGrid.Grid.GetLength(1); j++)
            //    {
            //        var position = new Vector3((float)archGrid.Grid[i, j].Center.X, -0.5f, (float)archGrid.Grid[i, j].Center.Y) + BuildingRoot.transform.position;
            //        GameObject Cube = Instantiate(_CubeForFoundation, position, Quaternion.identity);
            //        Cube.transform.parent = RoofFoundation.transform;
            //    }
            //}
        }
        #endregion

        #region Rooms

      
       
        public void InstantiateFloorAndCeiling()
        {

            //var floors = house.floors;

            //var FloorsRoot = new GameObject("FloorsRoot");
            //var CeilingRoot = new GameObject("CeilingRoot");

            //float high;
            //Vector2d center;
            //Vector3 position;

            //GameObject local;

            //FloorsRoot.transform.parent = BuildingRoot.transform;
            //CeilingRoot.transform.parent = BuildingRoot.transform;

            //premisesPrefabs.CeilingPrefab.GetComponent<Renderer>().material = premisesPrefabs.CeilingMaterial;
            //premisesPrefabs.FloorPrefab.GetComponent<Renderer>().material = premisesPrefabs.FloorMaterial;

            //for (var i = 0; i < floors.Count; i++)
            //{
            //    var Grid = floors[i].ArchPlan.Grid;

            //    for (var j = 0; j < Grid.GetLength(0); j++)
            //    {
            //        for (var k = 0; k < Grid.GetLength(1); k++)
            //        {
            //            if (Grid[j, k].Tag == PlanCellTag.Inside)
            //            {
            //                center = Grid[j, k].Center;

            //                high = i * 2 + StartY;

            //                position = new Vector3((float)center.X, high, (float)center.Y) + BuildingRoot.transform.position;

            //                local = Instantiate(premisesPrefabs.FloorPrefab, position, Quaternion.identity);
            //                local.transform.parent = FloorsRoot.transform;

            //                high = (i + 1) * 2 - StartY;

            //                position = new Vector3((float)center.X, high, (float)center.Y) + BuildingRoot.transform.position;

            //                local = Instantiate(premisesPrefabs.CeilingPrefab, position, Quaternion.identity);
            //                local.transform.parent = CeilingRoot.transform;


            //            }
            //        }
            //    }


            //}

        }

        protected override void InitializeSpaces3D()
        {
            throw new NotImplementedException();
        }







        #endregion

    }

}
