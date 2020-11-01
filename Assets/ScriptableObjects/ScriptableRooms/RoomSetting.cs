using ArchitectureGrid;
using Assets.Scripts.Premies.Buildings.Floors;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Класс ответсвенный за Префабы для помещений 
    /// </summary>
    [CreateAssetMenu(fileName = "RoomSettingData", menuName = "Building/3D/RoomsSetting", order = 1)]
    public class RoomSetting : ScriptableObject
    {
        #region Private Fields
        public const string notSet = "Если не задать то будет использано дефолтный префаб дома.";
        public const string wallRequirement = "Требование по размерам высота=2м, ширина=1м, толщина от 10 до 20 см.";
        public const string ceilingAndFloorRequirement = "Требование по размерам высота=0.01м, ширина=1м, толщина=1м.";

        //[SerializeField, Header("Префабы для стен"), Space(10), Tooltip(wallRequirement + notSet)]
        [SerializeField]
        protected GameObject _wallPrefab;

        //[SerializeField, Header("Префабы для стен"), Space(10), Tooltip(wallRequirement + notSet)]


        //[SerializeField, Tooltip(wallRequirement + ", нужна выемка для двери, а также сама дверь в ней." + notSet)]
        [SerializeField]
        protected GameObject _wallForDoorPrefab;

        //[SerializeField, Tooltip(wallRequirement + ", нужна выемка для окна, а также само окно в ней." + notSet)]
        [SerializeField]
        protected GameObject _wallWithWindowPrefab;

        [SerializeField]
        protected List<GameObject> _wallWithWindowPrefabForRandom;

        //[SerializeField, Space(10), Header("Префабы для пола и потолка"), Tooltip(ceilingAndFloorRequirement + notSet)]
        [SerializeField]
        protected GameObject _floorPrefab;

        [SerializeField]
        protected RoomRequisite m_requisite;


        //[SerializeField, Tooltip(ceilingAndFloorRequirement + notSet)]
        [SerializeField]
        protected GameObject _ceilingPrefab;

        [SerializeField, Tooltip(notSet)]
        protected Material _wallMaterial;              

        [SerializeField, Tooltip(notSet)]
        protected Material _ceilingMaterial;

        [SerializeField, Tooltip(notSet)]
        protected Material _floorMaterial;


        [SerializeField, Tooltip(notSet)]
        private GameObject wallPrefabForMaterial;

        [SerializeField, Tooltip(notSet)]
        private GameObject wallForDoorPrefabForMaterial;

        [SerializeField, Tooltip(notSet)]
        private GameObject wallWithWindowPrefabForMaterial;
        #endregion

        #region Public Props
        public GameObject WallPrefab  => _wallPrefab; 
        public GameObject WallForDoorPrefab  => _wallForDoorPrefab; 
        public GameObject WallWithWindowPrefab  => _wallWithWindowPrefab; 
        public GameObject FloorPrefab  => _floorPrefab; 
        public GameObject CeilingPrefab => _ceilingPrefab; 
       
        public Material CeilingMaterial  => _ceilingMaterial;
        public Material FloorMaterial  => _floorMaterial;
        public Material WallMaterial  => _wallMaterial;
        public GameObject WallPrefabForMaterial => wallPrefabForMaterial;
        public GameObject WallForDoorPrefabForMaterial => wallForDoorPrefabForMaterial;
        public GameObject WallWithWindowPrefabForMaterial => wallWithWindowPrefabForMaterial;

        #endregion

       
        public RoomRequisite Requisite => m_requisite;
        public GameObject SelectWallPrefab(WallType WallType)
        {

            if (WallType.SimpleWall == WallType)
                return WallPrefab;
            else if (WallType.WallWithDoor == WallType)
                return WallForDoorPrefab;
            else if (WallType == WallType.WallWithWindow)
                return WallWithWindowPrefab;
            else if (WallType == WallType.NoWall)
                return null;
            else return null;
        }

        public GameObject SelectWallPrefabForMaterial(WallType WallType)
        {

            if (WallType.SimpleWall == WallType)
                return WallPrefabForMaterial;
            else if (WallType.WallWithDoor == WallType)
                return WallForDoorPrefabForMaterial;
            else if (WallType == WallType.WallWithWindow)
                return WallWithWindowPrefabForMaterial;
            else if (WallType == WallType.NoWall)
                return null;
            else return null;
        }

        public GameObject GetRandowmWindow()
        {
            return _wallWithWindowPrefabForRandom[UnityEngine.Random.Range(0, _wallWithWindowPrefabForRandom.Count)];
        }


    }
}
