using ArchitectureGrid;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Premies.Buildings.Floors;
using Floor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Premies.Buildings.Entrace3D
{
    public abstract class Entrance3D : Entrance2D, IVisualizer, IBuildingPremises3D
    {

        //protected Entrance2D m_entrace2D;
        protected EntraceSetting m_EntraceSettings;
        protected GameObject m_entracesRoot;
        protected GameObject m_buildingRoot;
        protected GameObject m_entraceRoot;

        protected List<RoomSetting> buildingPossiblePrefabs;

        protected List<IFloor3D> floors3D;

        protected Material m_outerWallMaterial;

        public Entrance3D(Entrance2D entrace2D, EntraceSetting settings, GameObject entracesRoot, GameObject buildingRoot, List<RoomSetting> buildingPossiblePrefabs, Material outerWallMaterial=null) : base(entrace2D)
        {
           
            m_outerWallMaterial = outerWallMaterial;
            m_EntraceSettings = settings;         

            this.m_entracesRoot = entracesRoot;
            this.m_buildingRoot = buildingRoot;

            this.buildingPossiblePrefabs = buildingPossiblePrefabs;

           
        }

        public abstract void Visualize();
       
    }
}
