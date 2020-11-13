using Assets.Scripts;
using Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PoolElementData
{
    public GameObject PrefabToPool;
    [Range(1, 5000)]
    public int numberPrefabsToPool = 100;
}

public abstract class DefaultBuildingSettings : ScriptableObject
{
    [Space(10), Header("Optimization")]

    [SerializeField, Tooltip("create pool of object for more faster creation"), Space(5)]
    public PoolElementData[] PoolElement;

    public GameObject RoomCombiner;

    [Space(10), Header("Wall prefabs")]

    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject houseEnterWall;
    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject houseEnterWallForMaterial;

    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultWall;
    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultWallForMaterial;


    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultWallForDoor;
    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultWallForDoorMaterial;

    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject BasementWindow;

    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject RoofBoarder;
    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject StairsRoofFaceWall;
    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultWallWithWindow;
    [SerializeField, Tooltip(RoomSetting.wallRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultWallWithWindowForMaterial;



    [Space(10), Header("floor, ceiling  prefabs")]
    [SerializeField, Tooltip(RoomSetting.ceilingAndFloorRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultFloorRoomPrefab;
    [SerializeField, Tooltip(RoomSetting.ceilingAndFloorRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultCeilingRoomPrefab;

    [SerializeField, Tooltip(RoomSetting.ceilingAndFloorRequirement + RoomSetting.notSet), Space(5)]
    public GameObject defaultFloorPrefab;

    [Space(10), Header("building rain drain prefabs")]
    public GameObject buildingRainDrainVerticalTop;
    public GameObject buildingRainDrainVerticalMidle;
    public GameObject buildingRainDrainVerticalBottom;
  

    public GameObject buildingRainDrainHorizontal;
    public float OffsetForRainDrainHorozontal = 0.3f;
    public Vector3 AdditionalOffsetForRainDrainHorozontal = new Vector3(0, 0.75f, 0);


    [Space(10), Header("Buillding Materials settings")]
    [SerializeField, Tooltip(RoomSetting.notSet), Space(5)]
    public Material defalutRoofMaterial;
    [SerializeField, Tooltip(RoomSetting.notSet), Space(5)]
    public Material defaultFloorMaterial;
    [SerializeField, Tooltip(RoomSetting.notSet), Space(5)]
    public Material defaultCeilingMaterial;



    //[SerializeField]
    //public List<FloorSettings> floors;

    [Space(10), Header("Rooms settings")]

    [SerializeField, Space(10)]
    public List<RoomSetting> possibleRooms;
    [SerializeField, Space(10)]
    public List<RoomLink> RoomLinks;

    
}
