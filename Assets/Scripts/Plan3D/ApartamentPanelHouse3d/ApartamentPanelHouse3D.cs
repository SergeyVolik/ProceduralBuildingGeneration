using ArchitectureGrid;
using Assets.Scripts.Buildings;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Premies.Buildings;
using Assets.Scripts.Premies.Buildings.Building2D;
using Assets.Scripts.Premies.Buildings.Entrace3D;
using Assets.Scripts.Premies.Buildings.Floors;

using Assets.Scripts.Tools;
using BuildingUtils;
using Floor;
using JetBrains.Annotations;
using Newtonsoft.Json.Bson;
using Plan3d;
using Rooms;
using StraightSkeleton;
using StraightSkeleton.Polygon.RandomRectangularPolygon;
using StraightSkeleton.Polygon.Utils;
using StraightSkeleton.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Builders
{
    public class ApartamentPanelHouse3D : ApartamentPanelHouse2D, IBuilding3D, IVisualizer, IRoom3DHolder
    {
        public GameObject BuildingRoot { get; set; }  

        protected List<RoomSetting> buildingPossiblePrefabs;
      

        public Vector3 Center
        {
            get
            {

                var center = PolygonUtils.CenterMasFormula(BuildingForm);

                return new Vector3((float)center.X, BuildingRoot.transform.position.y, (float)center.Y);
            }
        }

        //
        public Floor3D roof3D;
        public List<Entrance3D> Entaraces3D;
        public Floor3D basemante3D;

        public Material RoofMaterial { get; set; }

        public List<RoomSetting> RoomsSettings { get; set; }

        private const int BUILDING_WIDTH = 20;

        PanelHouseSettings m_panelHousesettings;

        public List<Room3D> GetRooms3D()
        {
            var Rooms = new List<Room3D>();

            Entaraces3D.ForEach(e => Rooms.AddRange(e.GetRooms3D()));

            return Rooms;
        }
        public ApartamentPanelHouse3D(PanelHouseSettings settings, GameObject root) : base(
            settings.entraces[0].FloorsSettings.Count,
            settings.areaForEntrace,
            settings.entraces.Count,
            settings.possibleRooms.Select(rs => rs.Requisite).ToList(),
            new ControlledRandomRectangularPolygon(4, settings.areaForEntrace * settings.entraces.Count).
                CreateRectangle(BUILDING_WIDTH, BUILDING_WIDTH),
            settings.entraces.Select(e => e.NeedPassage).ToList(),
            settings.RoofType
            
            )
        {

            buildingPossiblePrefabs = settings.possibleRooms;
            RoofMaterial = null;
            RoomsSettings = settings.possibleRooms;
            BuildingRoot = root;

            m_panelHousesettings = settings;

            var roomRequisites = GetListRoomRequisite();
            var random = new ControlledRandomRectangularPolygon(4, settings.areaForEntrace * settings.entraces.Count);

            var MainPolygon = random.CreateRectangle(BUILDING_WIDTH, BUILDING_WIDTH);

            

        }

        public List<RoomRequisite> GetListRoomRequisite()
        {
            var requisite = new List<RoomRequisite>();

            m_panelHousesettings.possibleRooms.ForEach((r) => { requisite.Add(r.Requisite); });

            return requisite;
        }
        public void Visualize()
        {
            var entraces = BuildingPremises2D;
            var EntracesRoot = new GameObject("Entraces");
            EntracesRoot.transform.parent = BuildingRoot.transform;

          
            Entaraces3D = new List<Entrance3D>();
            var meshCombiner = BuildingRoot.AddComponent<MeshCombiner>();

            meshCombiner.DeactivateCombinedChildrenMeshRenderers = true;
            meshCombiner.CreateMultiMaterialMesh = true;
            meshCombiner.DeactivateCombinedChildren = false;



            for (var j = 0; j < entraces.Count; j++)
            {
                var outerWallMaterial = m_panelHousesettings.entraces[j].EntraceOuterWallMaterial;

                var entrance3d = new APH_Entrance3D(entraces[j] as Entrance2D, m_panelHousesettings.entraces[j] ,EntracesRoot, BuildingRoot, m_panelHousesettings, buildingPossiblePrefabs, outerWallMaterial);
                entrance3d.Visualize();
                Entaraces3D.Add(entrance3d);
            }

            if (RoofType == RoofType.CASCADE)
                VisualizeRoof();

            RainDrainVisualize();

            
            meshCombiner.meshFiltersToSkip = BuildingRoot.GetComponentsInChildren<MeshFilter>().ToList().Where(mf => mf.CompareTag("IgnoreMeshCombiner")).ToArray();
            meshCombiner.CombineMeshes(true);
           // meshCombiner.CreateMeshCollider();

            var liftRoomsFirstFloor = new List<Room3D>();

            Entaraces3D.ForEach(e =>
            {
               var lift = e.floors[1].GetRooms2D().FirstOrDefault(r => r.RoomType == RoomType.Lift);

                var center2d = lift.FindCenterOfRoomForRectangle();
                var center = new Vector3((float)center2d.X, Building3D.FloorHight, (float)center2d.Y);

                var LiftController = GameObject.Instantiate(
                    m_panelHousesettings.elevatorPrefab,
                    center + BuildingRoot.transform.position,
                    Quaternion.identity).GetComponent<ElevatorController>();

                LiftController.SetSettings(_numberOfFloors-2, 1, FloorHight);
            });



            //ObjectsPool.Instance.UnblockAllObjects();

            BuildingRoot.transform.rotation = Quaternion.Euler(BuildingRoot.transform.rotation.eulerAngles + BuildingRoot.transform.rotation.eulerAngles);
        }

        public IEnumerator VisualizeaAnimationCorotine()
        {
            //var entraces = _house2D.Entraces;
            //var EntracesRoot = new GameObject("Entraces");
            //EntracesRoot.transform.parent = BuildingRoot.transform;

            //Entaraces3D = new List<Entrance3D>();



            //for (var j = 0; j < entraces.Count; j++)
            //{
            //    var entrance3d = new APH_Entrance3D(entraces[j], EntracesRoot, BuildingRoot, m_panelHousesettings, buildingPossiblePrefabs);
            //    yield return entrance3d.VisualizeAnimation();
            //}




            ////VisualizeRoof();
            //RainDrainVisualize();
            yield return null;

        }

        public void StartAnimation()
        {
            Camera.main.GetComponentInParent<CameraRotateAround>().StartCoroutine(VisualizeaAnimationCorotine());
        }

        void RainDrainVisualize()
        {
           var rainDrainpositions = GetPartOfBuilding();

            Debug.Log("rainDrainpositions-> " + rainDrainpositions.Count);

            var RainDrain = new GameObject("RainDrain");
            RainDrain.transform.SetParent(BuildingRoot.transform);
            rainDrainpositions.ForEach(partWall =>
            {
                Premises3DUtils.InstantiateWallPrefab(partWall, m_panelHousesettings.buildingRainDrainHorizontal, RainDrain.transform, BuildingRoot.transform, m_panelHousesettings.entraces[0].FloorsSettings.Count-2, m_panelHousesettings.AdditionalOffsetForRainDrainHorozontal, m_panelHousesettings.OffsetForRainDrainHorozontal, false);
            });

            var points = GetAnglePoints(rainDrainpositions);

            for (var i = 1; i < m_panelHousesettings.entraces[0].FloorsSettings.Count-1; i++)
            {
                points.ForEach(angle =>
                {
                    Premises3DUtils.InstacntiateAngleObject(angle, m_panelHousesettings.buildingRainDrainVerticalTop, RainDrain.transform, BuildingRoot.transform, i);
                });
            }
        }
        

        List<Angle> GetAnglePoints(List<PartOfWall> parts)
        {
            var angles = new List<Angle>();

            for (var i = 0; i < parts.Count-1; i++)
            {
                if (parts[i].V1.X != parts[i + 1].V2.X && parts[i].V1.Y != parts[i + 1].V2.Y)
                {
                    angles.Add(new Angle(parts[i], parts[i+1], parts[i].V2));
                }
            }

            if (parts[parts.Count - 1].V1.X != parts[0].V2.X && parts[parts.Count - 1].V1.Y != parts[0].V2.Y)
            {

                angles.Add(new Angle(parts[0], parts[parts.Count - 1], parts[0].V1));
            }
            return angles;
        }
        void VisualizeRoof()
        {
            var roof = SkeletonBuilder.BuildRoof(BuildingForm);
            var meshData = new RoofMeshData(roof, BuildingForm, NumberOfFloors-2, Building2D.FloorHight);
            var RoofRoot = new GameObject("RoofRoot");

            RoofRoot.transform.parent = BuildingRoot.transform;

            for (var i = 0; i < meshData.verticesOfPolygons.Count; i++)
            {
                Mesh msh = new Mesh();
                for (var j = 0; j < meshData.verticesOfPolygons[i].Count; j++)
                {
                    meshData.verticesOfPolygons[i][j] += BuildingRoot.transform.position;
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
                meshRender.material = m_panelHousesettings.defalutRoofMaterial;

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


}
 

