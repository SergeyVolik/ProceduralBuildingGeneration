﻿using ArchitectureGrid;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Premies.Buildings.Building2D;
using Newtonsoft.Json.Bson;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Builders
{
    public abstract class Room3D : Premises3D, IRoom3D
    {

        private const float ceilingOffset = -0.01f;
        private const float floorOffset = 0.01f;

        protected GameObject wallsWithDoor;
        protected List<GameObject> Walls;
        protected GameObject OutsideDoor;

        protected GameObject buildingRoot;
        protected GameObject roomsRoot;
        protected GameObject roomRoot;

        protected Room2D room2D;

        protected List<RoomSetting> buildingPossiblePrefabs;
        protected int floor;
        protected List<PartOfWall> instantiatedWalls;

        protected Material m_wallMaterial;
        protected GameObject currPrefab;
        protected GameObject currPrefabForMaterial;

        protected int m_floorsNumber;
       
        protected RoomSetting m_settings;
        private GameObject ceilingRoot;
        private GameObject wallsRoot;

        public Room3D(Room2D room2D, GameObject roomsRoot, GameObject buildingRoot, List<RoomSetting> buildingPossiblePrefabs, int floor, List<PartOfWall> instantiatedWalls, int floorsNumber, RoomSetting settings, bool visualizeCeiling, bool visualizeFloor)
        {
            this.room2D = room2D;
            this.roomsRoot = roomsRoot;
            this.buildingRoot = buildingRoot;

            this.buildingPossiblePrefabs = buildingPossiblePrefabs;
            this.floor = floor;
            this.instantiatedWalls = instantiatedWalls;

            m_floorsNumber = floorsNumber;
            m_settings = settings;

            NeedCeiling = visualizeCeiling;
            NeedFloor = visualizeFloor;
        }

        public GameObject RoomRoot { get => roomRoot; }
        public GameObject FloorsRoot { get; private set; }

        bool NeedCeiling { get; set; }
        bool NeedFloor { get; set; }

        string tag = "CombineMesh";
        public override void Visualize()
        {
           
            roomRoot = new GameObject("Room " + room2D.RoomType + "  "+ room2D.Name);
            //roomRoot.AddComponent<CombineMesh>();
            roomRoot.transform.SetParent(roomsRoot.transform);

            CustomRoomVisualization();

            if (NeedCeiling)
                VisualizeCeiling();

            if (NeedFloor)
                VisualizeFloor();

            VisualizeWalls();

            CombineMeshes();



            //roomRoot.GetComponent<CombineMesh>().CombineMeshes();
        }

        private void CombineMeshes()
        {
            var meshesRenderers = roomRoot.GetComponentsInChildren<MeshRenderer>().ToList();

            var toRemove = new List<MeshRenderer>();


            meshesRenderers.RemoveAll(r => !r.CompareTag(tag));


            List<Material> DifferentMaterials = new List<Material>();

            if (meshesRenderers.Count > 0)
            {
                DifferentMaterials.Add(meshesRenderers[0].material);

                foreach (var renderer in meshesRenderers)
                {
                    if (DifferentMaterials.Exists(m => m.name == renderer.material.name))
                        continue;
                    else DifferentMaterials.Add(renderer.material);
                }

                foreach (var material in DifferentMaterials)
                {

                    var toCombine = meshesRenderers.FindAll(r => r.material.name == material.name);

                    if (toCombine.Count > 1)
                    {
                        GameObject combine = new GameObject("combine with materia -> " + material.name);
                        combine.transform.parent = roomRoot.transform;
                        combine.AddComponent<CombineMesh>();

                        toCombine.ForEach(toc => toc.transform.SetParent(combine.transform));

                        combine.GetComponent<CombineMesh>().CombineMeshes(material);
                    }


                }
            }
        }
        protected abstract void WallSelection(PartOfWall wall);
        protected abstract void CustomRoomVisualization();
        protected virtual Vector3 CustomHightOfWall(Vector2d center, float high, PartOfWall partWall)
        {
            return  new Vector3((float)center.X, high, (float)center.Y) + buildingRoot.transform.position;
        }

        private void VisualizeFloor()
        {
            
            var roomCells = room2D.Cells;

            FloorsRoot = new GameObject("Floor");
            FloorsRoot.transform.SetParent(roomRoot.transform);

            roomCells.ForEach(cell =>
            {
                var center = cell.Center;

                var high = floor * Building2D.FloorHight - Building2D.FloorHight/2;

                var position = new Vector3((float)center.X, high, (float)center.Y) + buildingRoot.transform.position;

                var local = Instantiate(buildingPossiblePrefabs[0].FloorPrefab, position, Quaternion.identity);
                local.transform.parent = FloorsRoot.transform;


            });
            


            
        }

        private void VisualizeCeiling()
        {
           
            var roomCells = room2D.Cells;

            ceilingRoot = new GameObject("Ceiling");
            ceilingRoot.transform.SetParent(roomRoot.transform);

            roomCells.ForEach(cell =>
            {

                var center = cell.Center;

                var high = (floor + 1) * Building2D.FloorHight - Building2D.FloorHight / 2;

                var position = new Vector3((float)center.X, high, (float)center.Y) + buildingRoot.transform.position;

                var local = Instantiate(buildingPossiblePrefabs[0].CeilingPrefab, position, Quaternion.identity);
                local.transform.parent = ceilingRoot.transform;
            });
            
        }

        private void VisualizeWalls()
        {
            var wallpositions = room2D.Walls;
            wallsRoot = new GameObject("Walls");
            wallsRoot.transform.SetParent(roomRoot.transform);

            for (var k = 0; k < wallpositions.Count; k++)
            {
                WallSelection(wallpositions[k]);

                VisualizeWall(wallpositions[k], currPrefab, currPrefabForMaterial, floor * Building2D.FloorHight, m_floorsNumber);
            }
        }

        protected void VisualizeWall(PartOfWall partWall, GameObject currPrefab, GameObject currPrefabForMaterial, float high, int floors)
        {

            
            GameObject curWall;
            Vector3 position;
            float xOffset, zoffset;
                
            var rotationY = GiveWallRotation(partWall, out xOffset, out zoffset);
            var center = LineSegment2d.Center(partWall.V1, partWall.V2);

            
            position = CustomHightOfWall(center, high, partWall);
            var positionForMaterial = new Vector3(position.x + xOffset, position.y, position.z + zoffset);


            if (floor != 0 && floor <= floors)
                if (currPrefabForMaterial != null)
                {
                  
                    //var m_wallMaterial = m_settings?.prefabs.WallMaterial;
                    if (m_wallMaterial != null)
                        currPrefabForMaterial.GetComponent<MeshRenderer>().material = m_wallMaterial;


                    curWall = Instantiate(currPrefabForMaterial, positionForMaterial, Quaternion.identity);
                    curWall.name = "wallForMaterial(" + partWall.V1 + " "+partWall.V2+")";
                    curWall.transform.parent = wallsRoot.transform;

                    curWall.transform.rotation = Quaternion.Euler(curWall.transform.rotation.x, curWall.transform.rotation.y + rotationY, curWall.transform.rotation.z);

                    
                }

            if (instantiatedWalls.Exists(w => w.V1 == partWall.V1 && partWall.V2 == w.V2 || w.V2 == partWall.V1 && partWall.V2 == w.V1))
                return;

            if (currPrefab != null)
            {
                 
                curWall = Instantiate(currPrefab, position, Quaternion.identity);
                curWall.name = "wall(" + partWall.V1 + " " + partWall.V2 + ")"; ;
                curWall.transform.parent = wallsRoot.transform;

                curWall.transform.rotation = Quaternion.Euler(curWall.transform.rotation.x, curWall.transform.rotation.y + rotationY, curWall.transform.rotation.z);


            }

            instantiatedWalls.Add(partWall);
            
        }
       





    }
}