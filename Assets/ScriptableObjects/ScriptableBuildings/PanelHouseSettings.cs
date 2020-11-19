using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EntraceSetting
{
    [Tooltip("При отсутсвии ошибла не возникнет, если у квартиры или этажа задан материал то он имеет больший приоритет")]

    [SerializeField]
    public Material EntraceOuterWallMaterial;

    [Header("Floor outer wall materias")]
    public List<FloorSettings> FloorsSettings = new List<FloorSettings>();

}

[Serializable]
public class FloorSettings
{
    [Tooltip("При отсутсвии ошибла не возникнет, если у квартиры задан материал то он имеет больший приоритет")]
    [SerializeField]
    public Material FloorOuterWallMaterial;

    [Header("Flat outer wall materias")]
    [SerializeField]

    public FlatSettings Flat1;
    [SerializeField]
    private FlatSettings Flat2;
    [SerializeField]
    private FlatSettings Flat3;
    [SerializeField]
    private FlatSettings Flat4;

    public FlatSettings GetFlatById(int? id)
    {
        switch (id)
        {
            case 0:
                return Flat1;
            case 1:
                return Flat2;
            case 2:
                return Flat3;
            case 3:
                return Flat4;
        }
        return null;
    }
}

[Serializable]
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

    [SerializeField, Tooltip("True - если то создаст кришу для всего здания, если False то создаст касадную кришу для каждлго подезда отдельно ")]
    public bool RoofForAllBuilding = true;

    [Header("Entrance outer wall materias")]
    [SerializeField]
    public List<EntraceSetting> entraces;

    [Space(10), Tooltip("Для панельного дома нужно создать лесницу 2х2х4 м где 4 - длинна" + RoomSetting.notSet)]
    [Header("Panel Building Prefabs")]
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
