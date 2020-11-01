using ArchitectureGrid;
using Assets.Scripts.Plan3D.Buildings.Entrance3D.Floors3D;
using Assets.Scripts.Premies.Buildings.Entrace3D;
using Assets.Scripts.Premies.Buildings.Floors;
using Floor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Premies.Buildings
{
    public class APH_Entrance3D : Entrance3D
    {
        protected PanelHouseSettings _settings;

        public APH_Entrance3D(Entrance2D entrace2D, GameObject entracesRoot, GameObject buildingRoot,
            PanelHouseSettings settings, List<RoomSetting> buildingPossiblePrefabs) : base(entrace2D, entracesRoot, buildingRoot, buildingPossiblePrefabs)
        {
            _settings = settings;
        }

        public override void Visualize()
        {
            
            entraceRoot = new GameObject("Entrace");
            entraceRoot.transform.parent = entracesRoot.transform;
            floors3D = new List<Floor3D>();

            entrace2D.floors.ForEach(f => {
                var floor3D = new APH_Floor3D(f, entraceRoot, buildingRoot, _settings, buildingPossiblePrefabs);
                floor3D.Visualize();
                floors3D.Add(floor3D);
            });

            
        }

        
    }
}
