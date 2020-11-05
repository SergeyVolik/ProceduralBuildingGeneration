using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    [CreateAssetMenu(fileName = "RoomData2D", menuName = "Building/2D/RoomsSettings")]
    public class RoomRequisite : ScriptableObject
    {
        #region Private Fields

        
       
        [SerializeField]
        private string roomName;
        [SerializeField]
        private bool needWindow;
        [SerializeField]
        private RoomType roomType;
        [SerializeField]
        private ZoneType zoneType;
        [SerializeField, Range(2, 100), Space(5)]
        private int widthMax;
        [SerializeField, Range(2, 100), Space(5)]
        private int widthMin;
        [SerializeField, Range(2, 100), Space(5)]
        private int depthMax;
        [SerializeField, Range(2, 100), Space(5)]
        private int depthMin;
        [SerializeField, Range(2, 100), Space(5)]
        private int areaMin;
        [SerializeField, Range(2, 100), Space(5)]
        private int areaMax;
        [SerializeField]
        private int priority;

        [SerializeField]
        private bool growthIfMaxSize = true;

        [SerializeField]
        private int maxDoorConections = 1;


    #endregion

    #region Public Props

    public string RoomName  => roomName; 
        public bool NeedWindow => needWindow;  
        public RoomType RoomType  => roomType;  
        public ZoneType ZoneType  => zoneType;
        public double WidthMax => widthMax;  
        public double WidthMin  => widthMin; 
        public double DepthMax  => depthMax;  
        public double DepthMin  => depthMin;  
        public double AreaMin  => areaMin;  
        public double AreaMax  => areaMax; 
        public int Priority  => priority;

        public bool GrowthIfMaxSize => growthIfMaxSize;

        public int MaxDoorConections => maxDoorConections;


    #endregion
}

