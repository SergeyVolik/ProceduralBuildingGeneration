using ArchitectureGrid;
using Assets.Scripts.Plan3D.Buildings.Entrance3D.Floors3D;
using Assets.Scripts.Premies.Buildings.Entrace3D;
using Assets.Scripts.Premies.Buildings.Floors;
using Floor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Premies.Buildings
{
    public class APH_Entrance3D : Entrance3D
    {
        protected PanelHouseSettings m_PanelHouseSettings;

        public APH_Entrance3D(Entrance2D entrace2D, EntraceSetting e_settings,  GameObject entracesRoot, GameObject buildingRoot,
            PanelHouseSettings settings, List<RoomSetting> buildingPossiblePrefabs, Material outerWallMaterial=null) : base(entrace2D, e_settings, entracesRoot, buildingRoot, buildingPossiblePrefabs, outerWallMaterial)
        {
            m_PanelHouseSettings = settings;
        }

        public override void Visualize()
        {
            
            m_entraceRoot = new GameObject("Entrace");
            m_entraceRoot.transform.parent = m_entracesRoot.transform;
            floors3D = new List<IFloor3D>();

            var material = m_outerWallMaterial;

            var basement = new APH_BasementFloor3D(floors[0] as APH_BasementFloor2D, m_EntraceSettings.FloorsSettings[0], m_entraceRoot, m_buildingRoot, m_PanelHouseSettings, buildingPossiblePrefabs, m_EntraceSettings.FloorsSettings.Count, RoofType, material);

            basement.Visualize();
            floors3D.Add(basement);

            int floorsNumber = 0;
            if (RoofType == RoofType.FLAT)
            {
                floorsNumber = floors.Count - 1;
                var roof = new APH_RoofFloor3D(floors[floors.Count-1] as APH_RoofFloor2D, m_EntraceSettings.FloorsSettings[floors.Count - 1], m_entraceRoot, m_buildingRoot, m_PanelHouseSettings, buildingPossiblePrefabs, m_EntraceSettings.FloorsSettings.Count, RoofType, material);

                roof.Visualize();
                floors3D.Add(roof);
            }
            else floorsNumber = floors.Count;

            for (var i = 1; i < floorsNumber; i++)
            {
                

                if (m_EntraceSettings.FloorsSettings[i].FloorOuterWallMaterial)
                    material = m_EntraceSettings.FloorsSettings[i].FloorOuterWallMaterial;

                var floor3D = new APH_DefaultFloor3D(floors[i] as APH_DefaulFloor2D, m_EntraceSettings.FloorsSettings[i] , m_entraceRoot, m_buildingRoot, m_PanelHouseSettings, buildingPossiblePrefabs, m_EntraceSettings.FloorsSettings.Count, RoofType, material);

                floor3D.Visualize();
                floors3D.Add(floor3D);
            }

            
        }

        public IEnumerator VisualizeAnimation()
        {

           // m_entraceRoot = new GameObject("Entrace");
           // m_entraceRoot.transform.parent = m_entracesRoot.transform;
           // floors3D = new List<Floor3D>();

           // foreach(var f in m_entrace2D.floors)
           //{
           //     var floor3D = new APH_Floor3D(f, m_entraceRoot, m_buildingRoot, _settings, buildingPossiblePrefabs);
           //     yield return floor3D.VisualizeAnimation();
           //     floors3D.Add(floor3D);
           // }

            yield return null;


        }


    }
}
