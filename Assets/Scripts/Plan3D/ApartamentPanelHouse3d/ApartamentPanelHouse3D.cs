using ArchitectureGrid;
using Assets.Scripts.Buildings;
using Assets.Scripts.Premies.Buildings;
using Assets.Scripts.Premies.Buildings.Building2D;
using Assets.Scripts.Premies.Buildings.Entrace3D;
using Assets.Scripts.Premies.Buildings.Floors;

using Assets.Scripts.Tools;
using JetBrains.Annotations;
using Newtonsoft.Json.Bson;
using Plan3d;
using Rooms;
using StraightSkeleton;
using StraightSkeleton.Polygon.RandomRectangularPolygon;
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
    public class ApartamentPanelHouse3D : Building3D
    {
        private const int BUILDING_WIDTH = 20;

        PanelHouseSettings _settings;
        public ApartamentPanelHouse3D(PanelHouseSettings settings, GameObject root) : base(
            settings.possibleRooms, null, settings.possibleRooms, root)
        {

            _settings = settings;

            var roomRequisites = GetListRoomRequisite();
            var random = new ControlledRandomRectangularPolygon(4, settings.areaForEntrace * settings.entrances);

            var MainPolygon = random.CreateRectangle(BUILDING_WIDTH, BUILDING_WIDTH);

            _house2D = new ApartamentPanelHouse2D(settings.floorsNumber, settings.areaForEntrace, settings.entrances, roomRequisites, MainPolygon);

        }

        public List<RoomRequisite> GetListRoomRequisite()
        {
            var requisite = new List<RoomRequisite>();

            _settings.possibleRooms.ForEach((r) => { requisite.Add(r.Requisite); });

            return requisite;
        }
        protected override void InitializeSpaces3D()
        {
            var entraces = _house2D.Entraces;
            var EntracesRoot = new GameObject("Entraces");
            EntracesRoot.transform.parent = BuildingRoot.transform;

            Entaraces3D = new List<Entrance3D>();



            for (var j = 0; j < entraces.Count; j++)
            {
                var entrance3d = new APH_Entrance3D(entraces[j], EntracesRoot, BuildingRoot, _settings, buildingPossiblePrefabs);
                entrance3d.Visualize();
            }




            //VisualizeRoof();
            RainDrainVisualize();


        }

        public IEnumerator VisualizeaAnimationCorotine()
        {
            var entraces = _house2D.Entraces;
            var EntracesRoot = new GameObject("Entraces");
            EntracesRoot.transform.parent = BuildingRoot.transform;

            Entaraces3D = new List<Entrance3D>();



            for (var j = 0; j < entraces.Count; j++)
            {
                var entrance3d = new APH_Entrance3D(entraces[j], EntracesRoot, BuildingRoot, _settings, buildingPossiblePrefabs);
                yield return entrance3d.VisualizeAnimation();
            }




            //VisualizeRoof();
            RainDrainVisualize();

        }

        public void StartAnimation()
        {
            Camera.main.GetComponentInParent<CameraRotateAround>().StartCoroutine(VisualizeaAnimationCorotine());
        }

        string tag = "CombineMesh";

        private void CombineMeshes()
        {
            var meshesRenderers = BuildingRoot.GetComponentsInChildren<MeshRenderer>().ToList();

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
                        combine.transform.parent = BuildingRoot.transform;
                        combine.AddComponent<CombineMesh>();

                        toCombine.ForEach(toc => toc.transform.SetParent(combine.transform));

                        combine.GetComponent<CombineMesh>().CombineMeshes(material);
                    }


                }
            }
        }

        void WallPartToSingleDiraction()
        {
            
        }
        void RainDrainVisualize()
        {
           var rainDrainpositions = _house2D.GetPartOfBuilding();

            Debug.Log("rainDrainpositions-> " + rainDrainpositions.Count);

            var RainDrain = new GameObject("RainDrain");
            RainDrain.transform.SetParent(BuildingRoot.transform);
            rainDrainpositions.ForEach(partWall =>
            {
                InstantiateWallPrefab(partWall, _settings.buildingRainDrainHorizontal, RainDrain.transform, BuildingRoot.transform, _settings.floorsNumber, _settings.AdditionalOffsetForRainDrainHorozontal, _settings.OffsetForRainDrainHorozontal, false);
            });

            var points = GetAnglePoints(rainDrainpositions);

            for (var i = 1; i < _settings.floorsNumber+1; i++)
            {
                points.ForEach(angle =>
                {
                    InstacntiateAngleObject(angle, _settings.buildingRainDrainVerticalTop, RainDrain.transform, BuildingRoot.transform, i);
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
            var roof = SkeletonBuilder.BuildRoof(_house2D.BuildingForm);
            var meshData = new RoofMeshData(roof, _house2D.BuildingForm, _house2D.NumberOfFloors, Building2D.FloorHight);
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
                meshRender.material = _settings.defalutRoofMaterial;

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
 

