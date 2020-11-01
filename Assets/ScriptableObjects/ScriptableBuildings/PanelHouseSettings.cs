using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PanelBuildingData", menuName = "Building/BuildingSettings", order = 1)]
public class PanelHouseSettings : DefaultBuildingSettings
{
    [Space(10), Header("Panel Building Prefabs"), Tooltip("Для панельного дома нужно создать лесницу 2х2х4 м где 4 - длинна" + RoomSetting.notSet)]

    public GameObject stairsFirstFloor;    
    public GameObject stairsnextFloor;

    public Material stairsWallMaterial;


    public Material corridorWallMaterial;
    public Material corridoCeilingMaterial;
    public Material corridorFloorMaterial;

    [Space(10), Header("Main Building Settings")]
    [SerializeField, Range(1, 20)]
    public int floorsNumber = 6;
    [HideInInspector]
    public int BildingAngles = 4;

    [SerializeField, Range(600, 1200), Space(5)]
    public int areaForEntrace = 1000;

    [SerializeField, Range(1, 10), Space(5)]
    public int entrances = 4;


}
