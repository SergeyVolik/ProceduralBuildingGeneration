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

    private GameObject defaultCubeForFoundation;
    private GameObject defaultFloorPrefab;
    private GameObject defaultCeilingPrefab;
    private GameObject defaultWallWithWindow;
    private GameObject defaultWall;
    private GameObject defaultWallForDoor;
    private Material defaultCeilingMaterial;
    private Material defaultFloorMaterial;
    private Material defalutRoofMaterial;
   

    public IBuilding3D visualizator;

    private void Awake()
    {

        spawnPosition.name = "BuildingRoot";       
       
        LoadMaterials();
        LoadPrefabs();
    }

    GameObject target;

    public ApartamentPanelHouse3D CreateHouse()
    {
        
        Clear();

        if (settings == null)
        {
            settings = Resources.Load("PanelBuildingData1") as PanelHouseSettings;
        }

        var house = new ApartamentPanelHouse3D(settings, spawnPosition);

        house.Visualize();

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

    private void LoadMaterials()
    {
        defalutRoofMaterial = Resources.Load("Materials/Floor_concrete") as Material;
        defaultCeilingMaterial = Resources.Load("Materials/Ceiling") as Material;
        defaultFloorMaterial = Resources.Load("Materials/Floor_Wood") as Material;
    }
    private void LoadPrefabs()
    {
        defaultCubeForFoundation = Resources.Load("Prefabs/CubeForFoundation") as GameObject;
        defaultWall = Resources.Load("Prefabs/Wall") as GameObject;
        defaultWallWithWindow = Resources.Load("Prefabs/WallandWindow") as GameObject;
        defaultWallForDoor = Resources.Load("Prefabs/wallForDoor") as GameObject;
        defaultFloorPrefab = Resources.Load("Prefabs/Floor") as GameObject;
        defaultCeilingPrefab = Resources.Load("Prefabs/Ceiling") as GameObject;
    }


}
