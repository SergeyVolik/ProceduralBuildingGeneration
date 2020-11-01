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
    public abstract class Entrance3D : UnityEngine.Object, IVisualizer
    {
        protected Entrance2D entrace2D;
       
        protected GameObject entracesRoot;
        protected GameObject buildingRoot;
        protected GameObject entraceRoot;

        protected List<RoomSetting> buildingPossiblePrefabs;

        protected List<Floor3D> floors3D;
        
        public Entrance3D(Entrance2D entrace2D, GameObject entracesRoot, GameObject buildingRoot, List<RoomSetting> buildingPossiblePrefabs)
        {
            this.entrace2D = entrace2D;
            this.entracesRoot = entracesRoot;
            this.buildingRoot = buildingRoot;

            this.buildingPossiblePrefabs = buildingPossiblePrefabs;

           
        }

        public abstract void Visualize();
       
    }
}
