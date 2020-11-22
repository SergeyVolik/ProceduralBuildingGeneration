using ArchitectureGrid;
using Assets.Scripts.Builders;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Plan3D.ApartamentPanelHouse3d;
using Assets.Scripts.Plan3D.Buildings.Entrance3D.Floors3D.Rooms;
using Assets.Scripts.Premies.Buildings.Floors;
using Floor;
using Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore;

namespace Assets.Scripts.Plan3D.Buildings.Entrance3D.Floors3D
{
    public class APH_DefaultFloor3D : APH_DefaulFloor2D, IVisualizer, IFloor3D
    {

        protected PanelHouseSettings m_panelHouseSettings;
        protected FloorSettings m_floorSettings;
        int m_floorNumber;

        protected List<Room3D> m_floors3D;

        protected GameObject m_floorRoot;
        protected GameObject m_floorsRoot;
        protected GameObject m_buildingRoot;

        protected Material m_outerWallMaterial;



        protected List<RoomSetting> _buildingPossiblePrefabs;

        protected List<Room3D> rooms3D;

        public RoofType RoofType { get; private set; }

        public APH_DefaultFloor3D(APH_DefaulFloor2D floor2d, FloorSettings floorSettings, GameObject floorsRoot, GameObject buildingRoot,
            PanelHouseSettings settings, List<RoomSetting> buildingPossiblePrefabs, int floorsNumber, RoofType roof, Material outerWallMaterial=null) : base(floor2d)
        {
            

            RoofType = roof;
            m_outerWallMaterial = outerWallMaterial;

            this.m_floorsRoot = floorsRoot;

            m_buildingRoot = buildingRoot;

            _buildingPossiblePrefabs = buildingPossiblePrefabs;

            m_floorNumber = floorsNumber;
            m_floorSettings = floorSettings;
            m_panelHouseSettings = settings;
        }
        public void Visualize()
        {

            List<PartOfWall> instantiatedWalls = new List<PartOfWall>();
            var rooms = GetRooms2D();

            rooms3D = new List<Room3D>();

            m_floorRoot = new GameObject("Floor " + Floor);
            m_floorRoot.transform.SetParent(m_floorsRoot.transform);


            for (var i = 0; i < rooms.Count; i++)
            {
                var flatSettings = m_floorSettings.GetFlatById(rooms[i].FlatId);
                Material outerWallMaterial = null;
                if(flatSettings!= null)                   
                  outerWallMaterial = flatSettings.FloorOuterWallMaterial;


                if (rooms[i].FlatId == null || !outerWallMaterial)
                    outerWallMaterial = m_outerWallMaterial;
         
                var roomRoot = new GameObject("Rooms");
                roomRoot.transform.parent = m_floorRoot.transform;
                Room3D room3d = null;

                
                if (m_floorNumber-2 == Floor && RoofType == RoofType.CASCADE)
                {
                    if (rooms[i].RoomType == RoomType.Lift)
                    {
                        room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, Floor, m_floorNumber, instantiatedWalls, true, false, outerWallMaterial);
                    }
                    else if (rooms[i].RoomType != RoomType.Stairs)
                        room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, Floor, m_floorNumber, instantiatedWalls, true, true, outerWallMaterial);    
                   
                    else room3d = new APH_Room3d_Stairs(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, Floor, m_floorNumber, instantiatedWalls, true, false, RoofType, outerWallMaterial);
                }              
                else {
                    if (rooms[i].RoomType == RoomType.Lift)
                    {
                        if(m_floorNumber - 1 == Floor)
                            room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, Floor, m_floorNumber, instantiatedWalls, true, false, outerWallMaterial);
                        else room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, Floor, m_floorNumber, instantiatedWalls, false, false, outerWallMaterial);
                    }
                    else if(rooms[i].RoomType == RoomType.Stairs)
                        room3d = new APH_Room3d_Stairs(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, Floor, m_floorNumber, instantiatedWalls, false, false, RoofType, outerWallMaterial);
                   else
                        room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, Floor, m_floorNumber, instantiatedWalls, true, true, outerWallMaterial);                  

       
                }

                room3d.Visualize();

               
                rooms3D.Add(room3d);

            }
        }

        public IEnumerator VisualizeAnimation()
        {
            yield return null;
            //List<PartOfWall> instantiatedWalls = new List<PartOfWall>();
            //var rooms = _floor2D.GetRooms();

            //rooms3D = new List<Room3D>();

            //floorRoot = new GameObject("Floor " + _floor2D.Floor);
            //floorRoot.transform.SetParent(floorsRoot.transform);

            //for (var i = 0; i < rooms.Count; i++)
            //{
            //    Room3D room3d = null;

            //    if (_floor2D.Floor == 0)
            //    {
            //        if (rooms[i].RoomType != RoomType.Stairs)
            //            room3d = new APH_Room3D(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, true, true);

            //        else room3d = new APH_Room3d_Stairs(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, false, true);
            //    }

            //    else if (_settings.floorsNumber + 1 == _floor2D.Floor)
            //    {
            //        if (rooms[i].RoomType != RoomType.Stairs)
            //            room3d = new APH_Room3D(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, false, false);
            //        else room3d = new APH_Room3d_Stairs(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, true, false);
            //    }

            //    else
            //    {

            //        if (rooms[i].RoomType != RoomType.Stairs)
            //            room3d = new APH_Room3D(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, true, true);

            //        else room3d = new APH_Room3d_Stairs(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, false, false);


            //    }

            //    yield return room3d.VisualizeAnimation();
            //    rooms3D.Add(room3d);

            //}
        }

        public List<Room3D> GetRooms3D()
        {
            return rooms3D;
        }
    }
}
