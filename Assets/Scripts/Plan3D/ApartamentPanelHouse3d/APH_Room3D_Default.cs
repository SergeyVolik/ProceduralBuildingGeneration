using ArchitectureGrid;
using Assets.Scripts.Builders;
using Assets.Scripts.Premies.Buildings.Building2D;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Plan3D.Buildings.Entrance3D.Floors3D.Rooms
{
    public class APH_Room3D : Room3D
    {
        protected PanelHouseSettings settings;

        public APH_Room3D(Room2D room2D, GameObject roomsRoot, GameObject buildingRoot, PanelHouseSettings _settings,
            List<RoomSetting> buildingPossiblePrefabs, int floor, int floorsNumber , List<PartOfWall> instantiatedWalls, bool ceiling, bool needFloor, Material outerWallMaterial = null) : base(room2D, roomsRoot, buildingRoot,
                buildingPossiblePrefabs, floor, instantiatedWalls, floorsNumber, _settings.possibleRooms.Find(r => r.Requisite.RoomName == room2D.RoomName), ceiling, needFloor, outerWallMaterial)
        {
            settings = _settings;
        }


        

        protected override void CustomRoomVisualization()
        {
           
        }

        protected override void WallSelection(PartOfWall wall)
        {
          
            if (m_settings != null)
                m_wallMaterial = m_settings.WallMaterial;

            else if (room2D.RoomType == RoomType.Corridor)
            {
                m_wallMaterial = settings.corridorWallMaterial;

            }

            if (wall.WallType == WallType.WallWithWindow)
            {
                if (floor > 1)
                {
                    currPrefab = buildingPossiblePrefabs[0].GetRandowmWindow();
                    currPrefabForMaterial = null;
                }
                else {
                    currPrefab = buildingPossiblePrefabs[0].WallWithWindowPrefab;
                    currPrefabForMaterial = null;
                }
            }

            else
            {
                currPrefab = buildingPossiblePrefabs[0].SelectWallPrefab(wall.WallType);
                currPrefabForMaterial = buildingPossiblePrefabs[0].SelectWallPrefabForMaterial(wall.WallType);
            }


            if (floor == 0)
            {
                if (wall.WallType == WallType.NoWall)
                    currPrefab = null;
                else if (wall.WallType == WallType.SimpleWall)
                    currPrefab = settings.BasementWindow;

            }

            else if (floor == m_floorsNumber - 1 && room2D.RoomType == RoomType.Flat)
            {
                currPrefab = settings.RoofBoarder;
                currPrefabForMaterial = null;
            }

           
        }
    }
}
