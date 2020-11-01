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
            List<RoomSetting> buildingPossiblePrefabs, int floor, List<PartOfWall> instantiatedWalls, bool ceiling, bool needFloor) : base(room2D, roomsRoot, buildingRoot,
                buildingPossiblePrefabs, floor, instantiatedWalls, _settings.floorsNumber, _settings.possibleRooms.Find(r => r.Requisite.RoomName == room2D.Name), ceiling, needFloor)
        {
            settings = _settings;
        }


        

        protected override void CustomRoomVisualization()
        {
           
        }

        protected override void WallSelection(PartOfWall wall)
        {

            var current_settings = settings.possibleRooms.FirstOrDefault(r => r.Requisite.RoomType == room2D.RoomType);
            if (current_settings != null)
                m_wallMaterial = current_settings.WallMaterial;
            else if (room2D.RoomType == RoomType.Corridor)
            {
                m_wallMaterial = settings.corridorWallMaterial;

            }

            if (wall.WallType == WallType.WallWithWindow)
            {
                currPrefab = buildingPossiblePrefabs[0].GetRandowmWindow();
                currPrefabForMaterial = null;
            }
            else
            {
                currPrefab = buildingPossiblePrefabs[0].SelectWallPrefab(wall.WallType);
                currPrefabForMaterial = buildingPossiblePrefabs[0].SelectWallPrefabForMaterial(wall.WallType);
            }
            

            if (floor == 0)
                currPrefab = settings.BasementWindow;

            else if (floor > settings.floorsNumber && room2D.RoomType == RoomType.Flat)
                currPrefab = settings.RoofBoarder;

           
        }
    }
}
