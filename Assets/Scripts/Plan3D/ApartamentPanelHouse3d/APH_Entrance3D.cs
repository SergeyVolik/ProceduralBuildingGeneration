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
            floors3D = new List<Floor3D>();


            for (var i = 0; i < m_entrace2D.floors.Count; i++)
            {
                var material = m_outerWallMaterial;

                if (m_EntraceSettings.FloorsSettings[i].FloorOuterWallMaterial)
                    material = m_EntraceSettings.FloorsSettings[i].FloorOuterWallMaterial;

                var floor3D = new APH_Floor3D(m_entrace2D.floors[i], m_EntraceSettings.FloorsSettings[i] , m_entraceRoot, m_buildingRoot, m_PanelHouseSettings, buildingPossiblePrefabs, m_EntraceSettings.FloorsSettings.Count, m_entrace2D.RoofType, material);

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
