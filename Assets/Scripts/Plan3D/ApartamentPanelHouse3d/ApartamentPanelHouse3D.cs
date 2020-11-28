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

        List<IRoof3D> _roofs;
        public Vector3 Center
        {
            get
            {

                var center = PolygonUtils.CenterMasFormula(BuildingForm);

                return new Vector3((float)center.X, BuildingRoot.transform.position.y, (float)center.Y);
            }
        }

        public List<Entrance3D> Entaraces3D;
 

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
            settings.entraces.Select(e => e.FloorsSettings.Count).ToList(),
            settings.areaForEntrace,
            settings.entraces.Count,
            settings.possibleRooms.Select(rs => rs.Requisite).ToList(),
            new ControlledRandomRectangularPolygon(4, settings.areaForEntrace * settings.entraces.Count).
                CreateRectangle(BUILDING_WIDTH, BUILDING_WIDTH),
            settings.entraces.Select(e => e.NeedPassage).ToList(),
            settings.entraces.Select(e => e.RoofType).ToList()

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
            _roofs = new List<IRoof3D>();



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

                var entrance3d = new APH_Entrance3D(entraces[j] as Entrance2D, m_panelHousesettings.entraces[j], EntracesRoot, BuildingRoot, m_panelHousesettings, buildingPossiblePrefabs, outerWallMaterial);
                entrance3d.Visualize();
                Entaraces3D.Add(entrance3d);
            }

           
            VisualizeRoof();

            RainDrainVisualize();

            var meshes = BuildingRoot.GetComponentsInChildren<MeshFilter>().ToList();
            var Ingore = meshes.Where(mf => mf.CompareTag("IgnoreMeshCombiner") || mf.CompareTag("AfterGenerationMeshCollider"));

            meshCombiner.meshFiltersToSkip = Ingore.ToArray();
            meshCombiner.CombineMeshes(true);
            meshCombiner.CreateMeshCollider();

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

                LiftController.transform.parent = BuildingRoot.transform;
            });



            //ObjectsPool.Instance.UnblockAllObjects();

            BuildingRoot.transform.rotation = Quaternion.Euler(BuildingRoot.transform.rotation.eulerAngles + BuildingRoot.transform.rotation.eulerAngles);
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
           
            if (m_panelHousesettings.entraces.All(e => e.RoofType == RoofType.CASCADE && e.FloorsSettings.Count == m_panelHousesettings.entraces[0].FloorsSettings.Count))
            {
                var roof = new CascadeRoof(BuildingForm, FloorHight, NumberOfFloors, BuildingRoot.transform, m_panelHousesettings.defalutRoofMaterial);
                _roofs.Add(roof);
                roof.VisualizeRoof();
            }
            else {

                var CascadeRoofs = new List<List<Vector2d>>();
                var roofFloorForCreationRoof = new List<int>();
                var roofontinuationFloor = new List<int>();
                
                var curretRoofForm = new List<Vector2d>();

                Entaraces3D.ForEach(e =>
                {

                    if (e.HaveRoof && e.RoofType == RoofType.FLAT)
                    {
                        e.Roof.VisualizeRoof();

                        if (curretRoofForm.Count != 0)
                        {
                            CascadeRoofs.Add(curretRoofForm);
                            roofFloorForCreationRoof.Add(roofontinuationFloor[0]);
                            roofontinuationFloor.Clear();
                            curretRoofForm = new List<Vector2d>();
                        }


                    }
                    else if (e.RoofType == RoofType.CASCADE)
                    {
                        if (roofontinuationFloor.Exists(floor => floor == e.FloorNumber) || roofontinuationFloor.Count == 0)
                        {
                            curretRoofForm.AddRange(e.MainPolygon);
                            roofontinuationFloor.Add(e.FloorNumber);
                        }
                        else
                        {
                            CascadeRoofs.Add(curretRoofForm);
                            roofFloorForCreationRoof.Add(roofontinuationFloor[0]);
                            roofFloorForCreationRoof.Clear();
                            curretRoofForm = new List<Vector2d>();
                        }
                    }

                });

                if (curretRoofForm.Count != 0)
                {
                    CascadeRoofs.Add(curretRoofForm);
                    roofFloorForCreationRoof.Add(roofontinuationFloor[0]);
                }

                for (var i = 0; i < CascadeRoofs.Count; i++)
                {
                    var roofForm = CascadeRoofs[i];
                    var maxX = roofForm.Max(v => v.X);
                    var maxY = roofForm.Max(v => v.Y);

                    var minX = roofForm.Min(v => v.X);
                    var minY = roofForm.Min(v => v.Y);

                    List<Vector2d> roofFOrm = new List<Vector2d>()
                    {
                        new Vector2d(minX, minY),
                        new Vector2d(minX, maxY),
                        new Vector2d(maxX, maxY),
                        new Vector2d(maxX, minY),
                    };

                    IRoof3D roof = new CascadeRoof(roofFOrm, FloorHight, roofFloorForCreationRoof[i], BuildingRoot.transform, m_panelHousesettings.defalutRoofMaterial);
                    roof.VisualizeRoof();
                    _roofs.Add(roof);

               }
            }
                                   
        }       


    }


}
 

