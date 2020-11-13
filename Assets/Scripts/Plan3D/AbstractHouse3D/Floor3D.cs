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
    public abstract class Floor3D : UnityEngine.Object, IVisualizer
    {

        protected List<Premises3D> m_floors3D;
        

        protected Floor2D m_floor2D;


        protected GameObject m_floorRoot;
        protected GameObject m_floorsRoot;
        protected GameObject m_buildingRoot;

        protected Material m_outerWallMaterial;

     

        protected List<RoomSetting> _buildingPossiblePrefabs;

        protected List<Room3D> rooms3D;

        public Floor3D(Floor2D floor2d, GameObject floorsRoot, GameObject buildingRoot, List<RoomSetting> buildingPossiblePrefabs, Material outerWallMaterial=null)
        {
            m_outerWallMaterial = outerWallMaterial;
            m_floor2D = floor2d;
            this.m_floorsRoot = floorsRoot;
           
            m_buildingRoot = buildingRoot;

            _buildingPossiblePrefabs = buildingPossiblePrefabs;

          
        }

        public abstract void Visualize();

    }
}
