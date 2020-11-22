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
    PanelHouseSettings settings;


    [Space(20), Header("Other")]
    [SerializeField]
    public CameraRotateAround cameraRotator;

    [SerializeField]
    private GameObject[] spawnPositions;
   

    public IBuilding3D visualizator;

    private void Awake()
    {

        //spawnPosition.name = "BuildingRoot";


        //settings.PoolElement.ToList().ForEach(e =>  ObjectsPool.Instance.AddToPoolObjects(e.PrefabToPool, e.numberPrefabsToPool));
       // ObjectsPool.Instance.AddToPoolObjects(settings.RoomCombiner, 5000, true);
    }

    GameObject target;

    public void CreateHouseWithAnimation()
    {
        
       
       // Clear();

       // if (settings == null)
       // {
       //     settings = Resources.Load("PanelBuildingData1") as PanelHouseSettings;
       // }

       // var house = new ApartamentPanelHouse3D(settings, spawnPosition);

       // //house.Visualize();

       // house.StartAnimation();


       //var childTrans = spawnPosition.GetComponentsInChildren<MeshRenderer>().ToList();



       // var points = new List<Vector3>();

       // childTrans.ForEach(r => points.Add(r.transform.position));

       // var center = points.CenterMassUnityVector3();

       // if (target)
       //     Destroy(target);

       // target = new GameObject("Target");
       // target.transform.position = center;
       // Camera.main.GetComponent<CameraRotateAround>().target = target.transform;


       

        //return house;
     
    }
    public ApartamentPanelHouse3D CreateHouse()
    {
        DateTime now = DateTime.Now;

        Clear();

        //foreach (var spawnPosition in spawnPositions)
        //{
           

            if (settings == null)
            {
                settings = Resources.Load("PanelBuildingData1") as PanelHouseSettings;
            }

            

            var house = new ApartamentPanelHouse3D(settings, spawnPositions[0]);



            house.Visualize();

            return house;
            //house.StartAnimation();


            //var childTrans = spawnPosition.GetComponentsInChildren<MeshRenderer>().ToList();



            //var points = new List<Vector3>();

            //childTrans.ForEach(r => points.Add(r.transform.position));

            //var center = points.CenterMassUnityVector3();

            //if (target)
            //    Destroy(target);

            //target = new GameObject("Target");
            //target.transform.position = center;
            //Camera.main.GetComponent<CameraRotateAround>().target = target.transform;


            //spawnPosition.GetComponent<CombineMesh>().CombineMeshes();


        //}
        Debug.LogError("TIme for calc = " + (DateTime.Now - now));
        Debug.Break();

    }
    public void Clear()
    {
        foreach(var spawnPosition in spawnPositions)
            foreach (Transform child in spawnPosition.transform)           
                GameObject.Destroy(child.gameObject);
            
    }

   


}
