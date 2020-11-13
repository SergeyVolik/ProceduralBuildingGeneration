using ArchitectureGrid;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Premies.Buildings.Building2D;
using Newtonsoft.Json.Bson;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.Characters.FirstPerson;

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

        protected Material m_outerWallMaterial;

        public Room3D(Room2D room2D, GameObject roomsRoot, GameObject buildingRoot,
            List<RoomSetting> buildingPossiblePrefabs, int floor,
            List<PartOfWall> instantiatedWalls, int floorsNumber,
            RoomSetting settings, bool visualizeCeiling, bool visualizeFloor, Material outerWallMaterial)
        {
            m_outerWallMaterial = outerWallMaterial;
            this.room2D = room2D;
            this.roomRoot = roomsRoot;
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

       
        public override void Visualize()
        {
           

            VisualizeWalls();

            if (NeedCeiling)
                VisualizeCeiling();

            if (NeedFloor)
                VisualizeFloor();

         

            CustomRoomVisualization();

            
        }

        public IEnumerator VisualizeAnimation()
        {

          
            //roomRoot = new GameObject("Room " + room2D.RoomType + "  " + room2D.Name);
            //var combiner = roomRoot.AddComponent<MeshCombiner>();

            //combiner.CreateMultiMaterialMesh = true;
            //combiner.DeactivateCombinedChildrenMeshRenderers = true;

          
            yield return VisualizeWallsAnim();
         

            
            if (NeedCeiling)
              VisualizeCeiling();

            if (NeedFloor)
               VisualizeFloor();

            //combiner.CombineMeshes(false);


            //CustomRoomVisualization();

            //ObjectsPool.Instance.UnblockAllObjects();


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

                var local = ObjectsPool.Instance.GetObjectFromPool(buildingPossiblePrefabs[0].FloorPrefab);
                local.transform.position = position;

                //var local = Instantiate(buildingPossiblePrefabs[0].FloorPrefab, position, Quaternion.identity);
                local.transform.parent = FloorsRoot.transform;


            });
            


            
        }

        private IEnumerator VisualizeFloorAnim()
        {

            var roomCells = room2D.Cells;

            FloorsRoot = new GameObject("Floor");
            FloorsRoot.transform.SetParent(roomRoot.transform);

            foreach (var cell in roomCells)
            {
                var center = cell.Center;

                var high = floor * Building2D.FloorHight - Building2D.FloorHight / 2;

                var position = new Vector3((float)center.X, high, (float)center.Y) + buildingRoot.transform.position;

                var local = Instantiate(buildingPossiblePrefabs[0].FloorPrefab, position, Quaternion.identity);
                local.transform.parent = FloorsRoot.transform;

                yield return new WaitForSeconds(0.1f);
            }




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

                //var local = Instantiate(buildingPossiblePrefabs[0].CeilingPrefab, position, Quaternion.identity);
                var local = ObjectsPool.Instance.GetObjectFromPool(buildingPossiblePrefabs[0].CeilingPrefab);
                local.transform.position = position;
                local.transform.parent = ceilingRoot.transform;
            });
            
        }

        private IEnumerator VisualizeCeilingAnim()
        {

            var roomCells = room2D.Cells;

            ceilingRoot = new GameObject("Ceiling");
            ceilingRoot.transform.SetParent(roomRoot.transform);

            foreach (var cell in roomCells)
            {
                var center = cell.Center;

                var high = (floor + 1) * Building2D.FloorHight - Building2D.FloorHight / 2;

                var position = new Vector3((float)center.X, high, (float)center.Y) + buildingRoot.transform.position;

                var local = ObjectsPool.Instance.GetObjectFromPool(buildingPossiblePrefabs[0].CeilingPrefab);
                local.transform.position = position;
                local.transform.parent = ceilingRoot.transform;

                yield return new WaitForSeconds(0.1f);

            }
           
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

        private IEnumerator VisualizeWallsAnim()
        {
            var wallpositions = room2D.Walls;
            wallsRoot = new GameObject("Walls");
            wallsRoot.transform.SetParent(roomRoot.transform);

            for (var k = 0; k < wallpositions.Count; k++)
            {
                WallSelection(wallpositions[k]);

                VisualizeWall(wallpositions[k], currPrefab, currPrefabForMaterial, floor * Building2D.FloorHight, m_floorsNumber);

                yield return new WaitForSeconds(0.1f);
            }


        }

        protected void VisualizeWall(PartOfWall partWall, GameObject currPrefab, GameObject currPrefabForMaterial, float high, int floors)
        {

            
            GameObject curWall;
            Vector3 position;
            float xOffset, zoffset;
                
            var rotationY = FindWallRotation(partWall, out xOffset, out zoffset);
            var center = LineSegment2d.Center(partWall.V1, partWall.V2);

            
            position = CustomHightOfWall(center, high, partWall);
            var positionForMaterial = new Vector3(position.x + xOffset, position.y, position.z + zoffset);


            if (floor != 0 && floor <= floors)
                if (currPrefabForMaterial != null)
                {
                  
                    //var m_wallMaterial = m_settings?.prefabs.WallMaterial;
                    if (m_wallMaterial != null)
                        currPrefabForMaterial.GetComponent<Renderer>().material = m_wallMaterial;


                    curWall = ObjectsPool.Instance.GetObjectFromPool(currPrefabForMaterial);

                    //curWall = Instantiate(currPrefabForMaterial, position, Quaternion.identity);
                    //curWall.name = "wallForMaterial(" + partWall.V1 + " "+partWall.V2+")";
                    curWall.transform.parent = wallsRoot.transform;
                    curWall.transform.position = positionForMaterial;
                    curWall.transform.rotation = Quaternion.Euler(curWall.transform.rotation.x, curWall.transform.rotation.y + rotationY, curWall.transform.rotation.z);

                    
                }

            if (instantiatedWalls.Exists(w => w.V1 == partWall.V1 && partWall.V2 == w.V2 || w.V2 == partWall.V1 && partWall.V2 == w.V1))
                return;

            if (currPrefab != null)
            {
                //curWall = Instantiate(currPrefab, position, Quaternion.identity);
                curWall = ObjectsPool.Instance.GetObjectFromPool(currPrefab);
                curWall.transform.position = position;

                if(m_outerWallMaterial)
                    curWall.GetComponent<MeshRenderer>().sharedMaterial = m_outerWallMaterial;

                //curWall.name = "wall(" + partWall.V1 + " " + partWall.V2 + ")"; ;
                curWall.transform.parent = wallsRoot.transform;

                curWall.transform.rotation = Quaternion.Euler(curWall.transform.rotation.x, curWall.transform.rotation.y + rotationY, curWall.transform.rotation.z);


            }

            instantiatedWalls.Add(partWall);
            
        }
       





    }
}
