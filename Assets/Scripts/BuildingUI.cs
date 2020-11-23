using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StraightSkeleton;
using StraightSkeleton.Primitives;
using StraightSkeleton.Corridor;
using Buldings;
using Assets.Scripts.Buildings;
using Floor;
using UnityStandardAssets.Characters.FirstPerson;
using ArchitectureGrid;
using Rooms;
using Assets.Scripts.Builders;
using TMPro;

[RequireComponent(typeof(BuildingManager))]
public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private GameObject img;
   
    [SerializeField]
    private TMP_Text textTime;
    [SerializeField]
    private TMP_Text title2D;

    [SerializeField] public List<Vector3> points;

    List<Vector2d> pointsSkeleton;

    [SerializeField]
    float multCoef = 10f;


    SkeletonCorridor sk;
    BuildingManager building;
    GameObject target;
    ApartamentPanelHouse3D currentBUilding;

    [SerializeField]
    Slider area;
    [SerializeField]
    Slider entrances;
    [SerializeField]
    Slider floors;

    [SerializeField]
    Dropdown roofType;

    [SerializeField]
    Toggle useDefSettings;
    public void Awake()
    {

        pointsSkeleton = new List<Vector2d>();

        building = GetComponent<BuildingManager>();


        currentSetting = Instantiate(examplesSettings[0]);
    }
  



    [SerializeField]
    List<PanelHouseSettings> examplesSettings;

    PanelHouseSettings currentSetting;

    int currentSettingIndex = 0;
    public void SelectExample1()
    {
        currentSettingIndex = 0;
        
    }
    public void SelectExample2()
    {
        currentSettingIndex = 1;
    }
    public void SelectExample3()
    {
        currentSettingIndex = 2;
    }
    public void SelectExample4()
    {
        currentSettingIndex = 3;
    }
    public void SelectExample5()
    {
        currentSettingIndex = 4;
    }
    public void SelectExample6()
    {
        currentSettingIndex = 5;
    }
    public void SelectExample7()
    {
        currentSettingIndex = 6;
    }
    public void SelectExample8()
    {
        currentSettingIndex = 7;
    }
    public void SelectExample9()
    {
        currentSettingIndex = 8;
    }
    public void BtnClickSkipAnimation()
    {
        ClearBuilding();
        ClearPanel();

        DateTime now = DateTime.Now;
        currentSetting = Instantiate(examplesSettings[currentSettingIndex]);

        if (!useDefSettings.isOn)
        {
            //UpdateEntaces();
            //UpdateFloors();
            UpdateArea();
            UpdateRoofType();
        }
        
        building.settings = currentSetting;
        currentBUilding = building.CreateHouse();

        Debug.LogError("TIme for calc = " + (DateTime.Now - now));

        textTime.SetText((DateTime.Now - now).Seconds.ToString());

      
        //ShowRoomsHowWalls();
    }

    void UpdateFloors()
    {
        bool exit = false;
        while (!exit)
        {
            exit = true;
            currentSetting.entraces.ForEach(e =>
            {
                if (e.FloorsSettings.Count > floors.value)
                {
                    exit = false;
                    e.FloorsSettings.Remove(e.FloorsSettings[e.FloorsSettings.Count - 1]);
                }
                else if (e.FloorsSettings.Count < floors.value)
                {
                    exit = false;
                    e.FloorsSettings.Add(e.FloorsSettings[e.FloorsSettings.Count - 1]);
                }
            });
         
        }
    }

    

    void UpdateEntaces()
    {
        while (currentSetting.entraces.Count != Convert.ToInt32(entrances.value))
        {
            if (currentSetting.entraces.Count > entrances.value)
                currentSetting.entraces.Remove(currentSetting.entraces[currentSetting.entraces.Count - 1]);
            else if(currentSetting.entraces.Count < entrances.value) currentSetting.entraces.Add(currentSetting.entraces[currentSetting.entraces.Count - 1]);
        }
    }

    void UpdateArea()
    {
        currentSetting.areaForEntrace = Convert.ToInt32(area.value - (area.value % 4));
    }

    void UpdateRoofType()
    {
        RoofType roof;

        if (roofType.value == 0)
            roof = RoofType.FLAT;
        else roof = RoofType.CASCADE;

        currentSetting.RoofType = roof;
    }
    public void ShowRoomsHowWalls()
    {
       
        if (currentBUilding != null)
        {
            title2D.SetText("Rooms walls");
            ClearPanel();
            currentBUilding.Entaraces3D.ForEach(e => ShowRoomsHowWalls(e.floors[1].GetRooms2D()));
           
        }
    }

    public void ShowRoomsHowCells()
    {
        if (currentBUilding != null)
        {
            title2D.SetText("Rooms cells");
            ClearPanel();
            currentBUilding.Entaraces3D.ForEach(e => ShowRoomsHowCells(e.floors[1].GetRooms2D()));

        }
    }

    public void ShowFlatsHowCells()
    {
        if (currentBUilding != null)
        {
            title2D.SetText("Flat cells");
            ClearPanel();
            currentBUilding.Entaraces3D.ForEach(e => ShowRoomsHowCells(e.floors[1].GetRooms2D()));
        }
    }


    void ShowList(List<Vector2d> pointsSkeleton, Color color, string name="", float multCoef = 10f)
    {
        for (var i = 0; i < pointsSkeleton.Count - 1; i++)
        {
            DrawLine(new Vector3((float)pointsSkeleton[i].X * multCoef, 0, (float)pointsSkeleton[i].Y * multCoef),
                 new Vector3((float)pointsSkeleton[i + 1].X * multCoef, 0, (float)pointsSkeleton[i + 1].Y * multCoef), color);
        }
        DrawLine(new Vector3((float)pointsSkeleton[0].X * multCoef, 0, (float)pointsSkeleton[0].Y * multCoef),
                           new Vector3((float)pointsSkeleton[pointsSkeleton.Count - 1].X * multCoef, 0, (float)pointsSkeleton[pointsSkeleton.Count - 1].Y * multCoef), color);
    }

    void DrawLine(Vector3 first, Vector3 second, Color image, string name="")
    {
        var clone = Instantiate(img, Vector3.zero ,Quaternion.identity);
        clone.transform.SetParent(Panel.transform);
        clone.GetComponent<Image>().color = image;

        var imageRectTransform = clone.GetComponent<Image>().rectTransform;
        Vector3 pointA = new Vector3(first.x, first.z, 0);
        Vector3 pointB = new Vector3(second.x, second.z, 0);
        Vector3 differenceVector = pointB - pointA;
        clone.transform.localScale = Vector3.one;
        imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, 2);
        imageRectTransform.pivot = new Vector2(0, 0.5f);
        imageRectTransform.localPosition = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        imageRectTransform.localRotation = Quaternion.Euler(0, 180, angle);
        clone.gameObject.name = name;
        clone.transform.localScale = Vector3.one;

    }
    public void ClearBuilding()
    {
       
        building.Clear();

    }

    public void ClearPanel()
    {
        foreach (Transform child in Panel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    

    Color RndColor()
    {
        return new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
    }
    void ShowOuterPolygon()
    {
        

    }
   
    //void ShowCorridor()
    //{
    //    var corridor = PrivateHouse.FirstFloorCorridor;
    //    ShowOuterPolygon();
    //    ShowList(corridor.polygonOfCorridor, new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f)));
    //}
    void ShowArchGrid(BasePlanProcessor2D PlanProcessor2D, Color insideColor)
    {
        
        for (var i = 0; i < PlanProcessor2D.Grid.GetLength(0); i++)
        {
            for (var j = 0; j < PlanProcessor2D.Grid.GetLength(1); j++)
            {

                if (PlanProcessor2D.Grid[i, j].Tag == ArchitectureGrid.PlanCellTag.Inside)
                {
                    ShowList(PlanProcessor2D.Grid[i, j].Square, insideColor, multCoef: 8f);
                }
               
            }

        }
    }

    
    void ShowRoomsHowWalls(List<Room2D> rooms)
    {
        //var rooms = floor1.GetRooms();


        for (var i = 0; i < rooms.Count; i++)
        {



            var roomWalls = rooms[i].Walls;

            var rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));

            //if (rooms[i].RoomType == Rooms.RoomType.Corridor || rooms[i].RoomType == Rooms.RoomType.Stairs || rooms[i].RoomType == Rooms.RoomType.Lift)
            //    continue;
            //for (var j = 0; j < rooms[i].Cells.Count; j++)
            //{
            //    ShowList(rooms[i].Cells[j].Square, rnd, rooms[i].RoomName, 8f);
            //}
            //var rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
            if (roomWalls != null)
                for (var j = 0; j < roomWalls.Count; j++)
                {

                    ShowList(new List<Vector2d>() { roomWalls[j].V1, roomWalls[j].V2 }, rnd);
                }
            // rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
            // ShowList(rooms[i].MainPolygon, rnd);
        }

        //for (var i = 0; i < floor1.ArchPlan.neighbourCells.Count; i++)      
        //    ShowList(floor1.ArchPlan.neighbourCells[i].Square, Color.yellow);

    }
    void ShowRoomsHowCells(List<Room2D> rooms)
    {
        
        for (var i = 0; i < rooms.Count; i++)
        {
         
            var rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));

            if (rooms[i].RoomType == Rooms.RoomType.Corridor || rooms[i].RoomType == Rooms.RoomType.Stairs || rooms[i].RoomType == Rooms.RoomType.Lift)
                continue;
            for (var j = 0; j < rooms[i].Cells.Count; j++)
            {
                ShowList(rooms[i].Cells[j].Square, rnd, rooms[i].RoomName, 8f);
            }
        
        }
       

    }

    void ShowGridCoridor()
    {
        
        //var floor1 = PrivateHouse.floors[0];
        //for (var i = 0; i < floor1.planProcessor2D.Grid.GetLength(0); i++)
        //{
        //    for (var j = 0; j < floor1.planProcessor2D.Grid.GetLength(1); j++)
        //    {
        //        if (floor1.planProcessor2D.Grid[i, j].room != null)
        //        {
        //            if (floor1.planProcessor2D.Grid[i, j].room.RoomRequisite.RoomType == Rooms.RoomType.Corridor)
        //            {
        //                ShowList(floor1.planProcessor2D.Grid[i, j].Square, corridor);
        //            }
        //        }              
             
        //    }

        //}
    }
    void ShowBoarderCells(Premises2D floor1)
    {

     
        for (var i = 0; i < floor1.PlanProcessor2D.Grid.GetLength(0); i++)
        {
            for (var j = 0; j < floor1.PlanProcessor2D.Grid.GetLength(1); j++)
            {
                //var rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
                //ShowList(floor1.planProcessor2D.Grid[i, j].Square, rnd);
                if (floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls != null)
                {
                    
                    for (var k = 0; k < floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls.Count; k++)
                    {
                        var rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
                        ShowList(new List<Vector2d>() { floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V1, floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V2 }, rnd);
                        //ShowList(floor1.planProcessor2D.Grid[i, j].Square, rnd);

                    }

                    //Debug.Log(floor1.planProcessor2D.Grid[i, j].PartsOfOutsideWalls.Count);

                }

            }

        }
    }

    //void ShowWindowsAndDoor(Premises2D floor1)
    //{

      
    //    for (var i = 0; i < floor1.PlanProcessor2D.Grid.GetLength(0); i++)
    //    {
    //        for (var j = 0; j < floor1.PlanProcessor2D.Grid.GetLength(1); j++)
    //        {
    //            if (floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls != null)
    //            {
    //                for (var k = 0; k < floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls.Count; k++)
    //                {
    //                    if (floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].WallType == ArchitectureGrid.WallType.WallWithDoor)
    //                    {
    //                        ShowList(new List<Vector2d>() { floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V1, floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V2 }, colorExit);
    //                    }
    //                    else if (floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].WallType == ArchitectureGrid.WallType.WallWithWindow)
    //                    {
    //                        ShowList(new List<Vector2d>() { floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V1, floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V2 }, colorWindow);
    //                    }
    //                }
    //                Debug.Log(floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls.Count);
    //            }
    //        }

    //    }
    //}
    //void ShowCorridorAndCorrectCell()
    //{

    //    for (var i = 0; i < PrivateHouse.floors[0].planProcessor2D.CorrectCells.Count; i++)
    //        ShowList(PrivateHouse.floors[0].planProcessor2D.CorrectCells[i].Square, correct
    //              );
    //    for (var i = 0; i < PrivateHouse.floors[0].planProcessor2D.SelectedCellsForGrowth.Count; i++)
    //        ShowList(PrivateHouse.floors[0].planProcessor2D.SelectedCellsForGrowth[i].Square, correct
    //              );

    //}
    //void ShowCorridorAndSelectedCells()
    //{

    //    for (var i = 0; i < PrivateHouse.floors[0].planProcessor2D.SelectedCellsForGrowth.Count; i++)
    //        ShowList(PrivateHouse.floors[0].planProcessor2D.SelectedCellsForGrowth[i].Square, selected
    //              );
    //    for (var i = 0; i < PrivateHouse.floors[0].planProcessor2D.CorrectCells.Count; i++)
    //        ShowList(PrivateHouse.floors[0].planProcessor2D.CorrectCells[i].Square, inside
    //              );

    //}



    bool value = false;
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {            
            
            value = !value;

        }
    }



}
