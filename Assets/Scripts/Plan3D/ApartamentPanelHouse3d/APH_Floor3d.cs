using ArchitectureGrid;
using Assets.Scripts.Builders;
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
    public class APH_Floor3D : Floor3D
    {

        protected PanelHouseSettings m_panelHouseSettings;
        protected FloorSettings m_floorSettings;
        int m_floorNumber;
      
        public APH_Floor3D(Floor2D floor2d, FloorSettings floorSettings, GameObject floorsRoot, GameObject buildingRoot,
            PanelHouseSettings settings, List<RoomSetting> buildingPossiblePrefabs, int floorsNumber, Material outerWallMaterial=null) : base(floor2d, floorsRoot, buildingRoot, buildingPossiblePrefabs, outerWallMaterial)
        {
            m_floorNumber = floorsNumber;
            m_floorSettings = floorSettings;
            m_panelHouseSettings = settings;
        }

        public override void Visualize()
        {

            List<PartOfWall> instantiatedWalls = new List<PartOfWall>();
            var rooms = m_floor2D.GetRooms();

            rooms3D = new List<Room3D>();

            m_floorRoot = new GameObject("Floor " + m_floor2D.Floor);
            m_floorRoot.transform.SetParent(m_floorsRoot.transform);


            for (var i = 0; i < rooms.Count; i++)
            {
                var outerWallMaterial = m_outerWallMaterial;

                //var roomsettings = m_panelHouseSettings.possibleRooms.Find(r => r.Requisite.RoomName == rooms[i].RoomName);

                var roomRoot = ObjectsPool.Instance.GetObjectFromPool(m_panelHouseSettings.RoomCombiner, true);
                roomRoot.transform.parent = m_floorRoot.transform;
                Room3D room3d = null;

                if (m_floor2D.Floor == 0)
                {
                    if (rooms[i].RoomType != RoomType.Stairs)
                        room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, m_floor2D.Floor, m_floorNumber, instantiatedWalls, true, true, outerWallMaterial);                   

                    else room3d = new APH_Room3d_Stairs(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, m_floor2D.Floor, m_floorNumber, instantiatedWalls, false, true, outerWallMaterial);
                }

                else if (m_floorNumber-1 == m_floor2D.Floor)
                {
                    if (rooms[i].RoomType != RoomType.Stairs)
                        room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, m_floor2D.Floor, m_floorNumber, instantiatedWalls, false, false, outerWallMaterial);                
                    else room3d = new APH_Room3d_Stairs(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, m_floor2D.Floor, m_floorNumber, instantiatedWalls, true, false, outerWallMaterial);
                }

                else {
                   
                    if (rooms[i].RoomType != RoomType.Stairs)
                        room3d = new APH_Room3D(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, m_floor2D.Floor, m_floorNumber, instantiatedWalls, true, true, outerWallMaterial);                  

                    else room3d = new APH_Room3d_Stairs(rooms[i], roomRoot, m_buildingRoot, m_panelHouseSettings, _buildingPossiblePrefabs, m_floor2D.Floor, m_floorNumber, instantiatedWalls, false, false, outerWallMaterial);

                  
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
    }
}
