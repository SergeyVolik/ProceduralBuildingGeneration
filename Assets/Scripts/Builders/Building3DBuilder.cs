using Buldings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Builders.Builder3D
{   

    /// <summary>
    /// Начальный класс билдера, необходим 
    /// </summary>
    class Building3DBuilder
    {
        private readonly Building3D visualizator;

        public Building3DBuilder(Building3D _visualizator)
        {
            visualizator = _visualizator;
        }

        public Building3DBuilder SetArea(float Area)
        {
            visualizator.House.Area = Area;

            return this;
        }
        public Building3DBuilder SetAngles(int Angles)
        {
            visualizator.House.Angles = Angles;

            return this;
        }
        public Building3DBuilder SetNumberOfFloors(int NumberOfFloors)
        {
            visualizator.House.NumberOfFloors = NumberOfFloors;
            return this;
        }

        public Building3DBuilder SetBuildingPrefabs(RoomSetting prefabs, Material roofMaterial)
        {
            //visualizator.PremisesPrefabs = prefabs;
            visualizator.RoofMaterial = roofMaterial;

            return this;
        }


        public void Build(GameObject spawnPoint)
        {
            visualizator.BuildingRoot = spawnPoint;

            visualizator.Visualize();          
        }

    }

   
   

  
}
        
