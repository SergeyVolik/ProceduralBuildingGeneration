using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buldings;
using StraightSkeleton.Primitives;
using ArchitectureGrid;
using Plan3d;
using Assets.Scripts.Builders.Builder3D;
using StraightSkeleton.Polygon.RandomRectangularPolygon;
using Assets.Scripts;
using Assets.Scripts.Premies.Buildings.Floors;
using Assets.Scripts.Builders;
using Assets.Scripts.Buildings;
using System.Linq;
using StraightSkeleton.Polygon.Utils;
using System;

//[System.Serializable]
//public class FloorSettings
//{

//    [SerializeField, Range(1, 4), Space(5)]
//    public int windowIndent;

//    [Space(10), Header("Corridor settings")]

//    [SerializeField, Space(5)]
//    public bool needCorridor;

//    [SerializeField, Range(1, 3), Space(5)]
//    public int corridorWidth;

//}


public class BuildingManager : MonoBehaviour
{

    [Space(10), Header("Building Settings")]

    [SerializeField]
    public PanelHouseSettings settings;


    [Space(20), Header("Other")]

    [SerializeField]
    private GameObject spawnPosition;
   

    public IBuilding3D visualizator;

    private void Awake()
    {
        position = spawnPosition.transform.position;
        rotation = spawnPosition.transform.rotation;
        //spawnPosition.name = "BuildingRoot";


        //settings.PoolElement.ToList().ForEach(e =>  ObjectsPool.Instance.AddToPoolObjects(e.PrefabToPool, e.numberPrefabsToPool));
        // ObjectsPool.Instance.AddToPoolObjects(settings.RoomCombiner, 5000, true);
    }

    public Vector3 position;
    public Quaternion rotation;
    public ApartamentPanelHouse3D CreateHouse()
    {
       

        Clear();
        

        spawnPosition = new GameObject();
        spawnPosition.transform.position = position;
        spawnPosition.transform.rotation = rotation;

       var house = new ApartamentPanelHouse3D(settings, spawnPosition);



        house.Visualize();
           
        return house;
         
    }
    public void Clear()
    {

        Destroy(spawnPosition);

    }

   


}
