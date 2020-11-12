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

[System.Serializable]
public class FloorSettings
{

    [SerializeField, Range(1, 4), Space(5)]
    public int windowIndent;

    [Space(10), Header("Corridor settings")]

    [SerializeField, Space(5)]
    public bool needCorridor;

    [SerializeField, Range(1, 3), Space(5)]
    public int corridorWidth;

}


public class BuildingManager : MonoBehaviour
{

    [Space(10), Header("Building Settings")]

    [SerializeField]
    PanelHouseSettings settings;


    [Space(20), Header("Other")]
    [SerializeField]
    public CameraRotateAround cameraRotator;

    [SerializeField]
    private GameObject spawnPosition;
   

    public IBuilding3D visualizator;

    private void Awake()
    {

        spawnPosition.name = "BuildingRoot";

        Debug.LogError("DefaultBuildingSettings Awake ");

        ObjectsPool.Instance.AddToPoolObjects(settings.houseEnterWall, 10);
        ObjectsPool.Instance.AddToPoolObjects(settings.houseEnterWallForMaterial, 10);

        ObjectsPool.Instance.AddToPoolObjects(settings.defaultWall, 100);
        ObjectsPool.Instance.AddToPoolObjects(settings.defaultWallForMaterial, 100);

        ObjectsPool.Instance.AddToPoolObjects(settings.defaultWallForDoor, 20);
        ObjectsPool.Instance.AddToPoolObjects(settings.defaultWallForDoorMaterial, 20);

        ObjectsPool.Instance.AddToPoolObjects(settings.defaultWallWithWindow, 40);
        ObjectsPool.Instance.AddToPoolObjects(settings.defaultWallWithWindowForMaterial, 40);

        ObjectsPool.Instance.AddToPoolObjects(settings.defaultFloorRoomPrefab, 100);
        ObjectsPool.Instance.AddToPoolObjects(settings.defaultCeilingRoomPrefab, 100);
        ObjectsPool.Instance.AddToPoolObjects(settings.defaultFloorPrefab, 100);

        ObjectsPool.Instance.AddToPoolObjects(settings.BasementWindow, 100);
        ObjectsPool.Instance.AddToPoolObjects(settings.RoofBoarder, 100);
    }

    GameObject target;

    public ApartamentPanelHouse3D CreateHouseWithAnimation()
    {
        
        Clear();

        if (settings == null)
        {
            settings = Resources.Load("PanelBuildingData1") as PanelHouseSettings;
        }

        var house = new ApartamentPanelHouse3D(settings, spawnPosition);

        //house.Visualize();

        house.StartAnimation();


       var childTrans = spawnPosition.GetComponentsInChildren<MeshRenderer>().ToList();



        var points = new List<Vector3>();

        childTrans.ForEach(r => points.Add(r.transform.position));

        var center = points.CenterMassUnityVector3();

        if (target)
            Destroy(target);

        target = new GameObject("Target");
        target.transform.position = center;
        Camera.main.GetComponent<CameraRotateAround>().target = target.transform;


       

        return house;
     
    }
    public ApartamentPanelHouse3D CreateHouse()
    {

        Clear();

        if (settings == null)
        {
            settings = Resources.Load("PanelBuildingData1") as PanelHouseSettings;
        }

        var house = new ApartamentPanelHouse3D(settings, spawnPosition);

        house.Visualize();

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

        return house;

    }
    public void Clear()
    {
        foreach (Transform child in spawnPosition.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

   


}
