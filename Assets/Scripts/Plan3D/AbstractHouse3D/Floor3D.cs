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

        protected List<Premises3D> _floors3D;

        protected Floor2D _floor2D;

        protected GameObject floorRoot;
        protected GameObject floorsRoot;
        protected GameObject _buildingRoot;


     

        protected List<RoomSetting> _buildingPossiblePrefabs;

        protected List<Room3D> rooms3D;

        public Floor3D(Floor2D floor2d, GameObject floorsRoot, GameObject buildingRoot, List<RoomSetting> buildingPossiblePrefabs)
        {
            _floor2D = floor2d;
            this.floorsRoot = floorsRoot;
           
            _buildingRoot = buildingRoot;

            _buildingPossiblePrefabs = buildingPossiblePrefabs;

          
        }

        public abstract void Visualize();

    }
}
