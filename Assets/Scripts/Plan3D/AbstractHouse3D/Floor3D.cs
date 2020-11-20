using ArchitectureGrid;
using Assets.Scripts.Builders;
using Assets.Scripts.Interfaces;
using Floor;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Premies.Buildings.Floors
{
    public abstract class Floor3D : Floor2D, IVisualizer, IBuildingPremises3D, IFloor3D
    {

        protected List<Room3D> m_floors3D;

        protected GameObject m_floorRoot;
        protected GameObject m_floorsRoot;
        protected GameObject m_buildingRoot;

        protected Material m_outerWallMaterial;

     

        protected List<RoomSetting> _buildingPossiblePrefabs;

        protected List<Room3D> rooms3D;

        public RoofType RoofType { get; private set; }

        public Floor3D(Floor2D floor2d, GameObject floorsRoot, GameObject buildingRoot, List<RoomSetting> buildingPossiblePrefabs, RoofType roof, Material outerWallMaterial=null) : base(floor2d)
        {
            RoofType = roof;
            m_outerWallMaterial = outerWallMaterial;

            this.m_floorsRoot = floorsRoot;
           
            m_buildingRoot = buildingRoot;

            _buildingPossiblePrefabs = buildingPossiblePrefabs;

          
        }

        public abstract void Visualize();

    }
}
