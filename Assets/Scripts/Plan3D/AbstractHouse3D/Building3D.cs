using ArchitectureGrid;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Premies.Buildings;
using Assets.Scripts.Premies.Buildings.Building2D;
using Assets.Scripts.Premies.Buildings.Entrace3D;
using Assets.Scripts.Premies.Buildings.Floors;
using Floor;
using StraightSkeleton.Polygon.Utils;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Assets.Scripts.Builders
{
    /// <summary>
    /// Абстрактный класс который определяет общие действия для всех Домов
    /// </summary>
    public abstract class Building3D : Premises3D, IBuilding3D, IVisualizer
    {
        ///пустышка корень дома
        public GameObject BuildingRoot { get; set; }

        public Building2D House { get => _house2D; /*set => _house2D = value;*/ }

        protected List<RoomSetting> buildingPossiblePrefabs;
        protected Building2D _house2D;

        public Vector3 Center { 
            get {

                var center = PolygonUtils.CenterMasFormula(House.BuildingForm);

                return new Vector3((float)center.X, BuildingRoot.transform.position.y, (float)center.Y);
            } 
        }
       
        //
        protected Floor3D roof3D;
        protected List<Entrance3D> Entaraces3D;
        protected Floor3D basemante3D;

        public Material RoofMaterial { get; set; }

        public List<RoomSetting> RoomsSettings { get; set; }

        protected Building3D(List<RoomSetting> buildingPrefabs, Material _RoofMaterial, List<RoomSetting> roomsPrefabs, GameObject root)
        {
            buildingPossiblePrefabs = buildingPrefabs;
            RoofMaterial = _RoofMaterial;
            RoomsSettings = roomsPrefabs;
            BuildingRoot = root;
        }

        protected Building3D() 
        {
            RoomsSettings = new List<RoomSetting>();
        }

       


        public override void Visualize()
        {
            InitializeSpaces3D();

            //roof3D.Visualize();

            Entaraces3D.ForEach(f => f.Visualize());

            //basemante3D.Visualize();
          
        }


        protected abstract void InitializeSpaces3D();
        
        
       
    }
}
