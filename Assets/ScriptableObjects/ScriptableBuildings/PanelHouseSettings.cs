using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EntraceSetting
{
    [Tooltip("При отсутсвии ошибла не возникнет, если у квартиры или этажа задан материал то он имеет больший приоритет")]
    public Material EntraceOuterWallMaterial;
    
    public List<FloorSettings> FloorsSettings = new List<FloorSettings>();

}

[Serializable]
public class FloorSettings
{
    [Tooltip("При отсутсвии ошибла не возникнет, если у квартиры задан материал то он имеет больший приоритет")]
    public Material FloorOuterWallMaterial;

    public FlatSettings Flat1;
    public FlatSettings Flat2;
    public FlatSettings Flat3;
    public FlatSettings Flat4;
}

[SerializeField]
public class FlatSettings
{
    public Material FloorOuterWallMaterial;
}



[CreateAssetMenu(fileName = "PanelBuildingData", menuName = "Building/BuildingSettings", order = 1)]
public class PanelHouseSettings : DefaultBuildingSettings
{
    private void Awake()
    {
        Debug.Log("PanelHouseSettings awake");
    }

    [Space(10), Header("Panel Building Prefabs"), Tooltip("Для панельного дома нужно создать лесницу 2х2х4 м где 4 - длинна" + RoomSetting.notSet)]

    [SerializeField]
    public List<EntraceSetting> entraces;

    public GameObject stairsFirstFloor;    
    public GameObject stairsnextFloor;

    public Material stairsWallMaterial;


    public Material corridorWallMaterial;
    public Material corridoCeilingMaterial;
    public Material corridorFloorMaterial;

    [Space(10), Header("Main Building Settings")]



    [HideInInspector]
    public int BildingAngles = 4;

    [SerializeField, Range(600, 1200), Space(5)]
    public int areaForEntrace = 1000;


   

}
