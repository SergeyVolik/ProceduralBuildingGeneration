using ArchitectureGrid;
using Assets.Scripts.Builders;
using Assets.Scripts.Plan3D.ApartamentPanelHouse3d;
using Assets.Scripts.Plan3D.Buildings.Entrance3D.Floors3D.Rooms;
using Assets.Scripts.Premies.Buildings.Floors;
using Floor;
using Rooms;
using System;
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

        protected PanelHouseSettings _settings;

        public APH_Floor3D(Floor2D floor2d, GameObject floorsRoot, GameObject buildingRoot,
            PanelHouseSettings settings, List<RoomSetting> buildingPossiblePrefabs) : base(floor2d, floorsRoot, buildingRoot, buildingPossiblePrefabs)
        {
            _settings = settings;
        }

        public override void Visualize()
        {

            List<PartOfWall> instantiatedWalls = new List<PartOfWall>();
            var rooms = _floor2D.GetRooms();

            rooms3D = new List<Room3D>();

            floorRoot = new GameObject("Floor " + _floor2D.Floor);
            floorRoot.transform.SetParent(floorsRoot.transform);

            for (var i = 0; i < rooms.Count; i++)
            {
                Room3D room3d = null;

                if (_floor2D.Floor == 0)
                {
                    if (rooms[i].RoomType != RoomType.Stairs)
                        room3d = new APH_Room3D(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, true, true);                   

                    else room3d = new APH_Room3d_Stairs(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, false, true);
                }

                else if (_settings.floorsNumber + 1 == _floor2D.Floor)
                {
                    if (rooms[i].RoomType != RoomType.Stairs)
                        room3d = new APH_Room3D(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, false, false);                
                    else room3d = new APH_Room3d_Stairs(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, true, false);
                }

                else {
                   
                    if (rooms[i].RoomType != RoomType.Stairs)
                        room3d = new APH_Room3D(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, true, true);                  

                    else room3d = new APH_Room3d_Stairs(rooms[i], floorRoot, _buildingRoot, _settings, _buildingPossiblePrefabs, _floor2D.Floor, instantiatedWalls, false, false);

                  
                }

                room3d.Visualize();
                rooms3D.Add(room3d);

            }
        }
    }
}
