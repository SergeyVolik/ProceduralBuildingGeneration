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

namespace Assets.Scripts.Plan3D.ApartamentPanelHouse3d
{
    public class APH_Room3d_Stairs : Room3D
    {
        protected PanelHouseSettings settings;

        public APH_Room3d_Stairs(Room2D room2D, GameObject roomsRoot, GameObject buildingRoot, PanelHouseSettings _settings,
            List<RoomSetting> buildingPossiblePrefabs, int floor, List<PartOfWall> instantiatedWalls, bool ceiling, bool needFloor) : base(room2D, roomsRoot, buildingRoot,
                buildingPossiblePrefabs, floor, instantiatedWalls, _settings.floorsNumber, null, ceiling, needFloor)
        {
            settings = _settings;
        }


        protected override Vector3 CustomHightOfWall(Vector2d center, float high, PartOfWall partWall)
        {
            Vector3 position;

            if (partWall.WallType == WallType.WallWithDoor)
                position = new Vector3((float)center.X, high - 1.25f, (float)center.Y) + buildingRoot.transform.position;
            else
                position = new Vector3((float)center.X, high, (float)center.Y) + buildingRoot.transform.position;

            return position;
        }

        protected override void WallSelection(PartOfWall wall)
        {
            if (wall.WallType == WallType.WallWithDoor && floor == 1)
            {
                currPrefab = settings.houseEndertWall;
                currPrefabForMaterial = settings.houseEndertWallForMaterial;
            }
            
            else if (wall.WallType == WallType.WallWithDoor)
            {
                currPrefab = settings.defaultWallWithWindow;
                currPrefabForMaterial = settings.defaultWallWithWindowForMaterial;
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
                else
                    currPrefab = settings.BasementWindow;
            }


            if (floor == settings.floorsNumber + 1 && wall.WallType == WallType.WallWithDoor)
                currPrefab = settings.StairsRoofFaceWall;
        }
      

        protected override void CustomRoomVisualization()
        {
            float high = floor * Building2D.FloorHight;




            if (floor != 0)
            {                
                GameObject stairs;
                var vec2d = room2D.CenterOfRoom;
                var vec3 = new Vector3((float)vec2d.X, high - 3.75f, (float)vec2d.Y) + buildingRoot.transform.position;

                if (floor == 1)
                    stairs = Instantiate(settings.stairsFirstFloor, vec3, Quaternion.identity);
                else stairs = Instantiate(settings.stairsnextFloor, vec3, Quaternion.identity);

                stairs.transform.parent = roomRoot.transform;
            }

        }
    }
}
