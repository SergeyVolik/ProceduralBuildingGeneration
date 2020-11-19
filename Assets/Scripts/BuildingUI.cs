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

[RequireComponent(typeof(BuildingManager))]
public class BuildingUI : MonoBehaviour
{
    public GameObject toggle;
    public GameObject Panel;
    public GameObject img;
    public GameObject skelet;
    public GameObject text;
    public GameObject angleCount;
    public GameObject avrArea;
    public GameObject SizeMult;
    public GameObject title;

    PrivateHouse2D PrivateHouse;
    ApartamentPanelHouse2D ApartmentPanelHouse;
    [SerializeField]
    public BuildingManager creator;
    [SerializeField] public List<Vector3> points;

    [SerializeField]
    GameObject Canvas;
    List<Vector2d> pointsSkeleton;
    //Skeleton sk;
    Color color, corridor, outside, colorWindow, colorExit, selected, correct, inside;
    ShowState state;
    [SerializeField]
    float multCoef = 10f;

    public void Start()
    {

        
        inside = Color.gray;
        corridor = Color.red;
        outside = Color.black;
        colorWindow = Color.yellow;
        colorExit = Color.magenta;      
        pointsSkeleton = new List<Vector2d>();
        selected = Color.green;
        correct = Color.blue;

        
    }
    SkeletonCorridor sk;

    GameObject target;
    public enum ShowState { ShowOuterPolygon, ShowRoof, ShowCoridor, ShowArchGrid, ShowGridCoridor, ShowWindowsAndDoor, ShowCorridorAndCorrectCell, ShowCorridorAndSelectedCells, Show3d }
    public void BtnClick()
    {
        playerCOntroller.gameObject.SetActive(false);
        rotateCamera.gameObject.SetActive(true);
        foreach (Transform child in Panel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
       /* var house = */GetComponent<BuildingManager>().CreateHouseWithAnimation();

      
        //ShowRooms(house.House.Entraces[0].floors[1]);

    }

    public RigidbodyFirstPersonController playerCOntroller;
    public Camera rotateCamera;
    public void BtnClickSkipAnimation()
    {
        playerCOntroller.StopAllCoroutines();
        playerCOntroller.gameObject.SetActive(true);
        rotateCamera.gameObject.SetActive(false);
        /*var house = */GetComponent<BuildingManager>().CreateHouse();
    }

        //void ShowTree(SkeletonCorridor sk)

        //{
        //    DrawLine(new Vector3((float)sk.treeOfCorridor.root.data.PathEdge.Start.X * 10 + 400, 0, (float)sk.treeOfCorridor.root.data.PathEdge.Start.Y * 10 + 200),
        //                new Vector3((float)sk.treeOfCorridor.root.data.PathEdge.End.X * 10 + 400, 0, (float)sk.treeOfCorridor.root.data.PathEdge.End.Y * 10 + 200), color);

        //    if (sk.treeOfCorridor.root.data.OuterWalls != null)
        //        for (var j = 0; j < sk.treeOfCorridor.root.data.OuterWalls.Count; j++)
        //        {
        //            DrawLine(new Vector3((float)sk.treeOfCorridor.root.data.OuterWalls[j].Start.X * 10 + 400, 0, (float)sk.treeOfCorridor.root.data.OuterWalls[j].Start.Y * 10 + 200),
        //            new Vector3((float)sk.treeOfCorridor.root.data.OuterWalls[j].End.X * 10 + 400, 0, (float)sk.treeOfCorridor.root.data.OuterWalls[j].End.Y * 10 + 200), color);
        //        }
        //    ShowNode(sk.treeOfCorridor.root);

        //}
        //void ShowNode(Node<CorridorPart> node)
        //{
        //    if (node.nodes != null)
        //        for (var i = 0; i < node.nodes.Count; i++)
        //        {
        //            if (node.nodes[i].data.OuterWalls != null)
        //                for (var j = 0; j < node.nodes[i].data.OuterWalls.Count; j++)
        //                {
        //                    DrawLine(new Vector3((float)node.nodes[i].data.OuterWalls[j].Start.X * 10 + 400, 0, (float)node.nodes[i].data.OuterWalls[j].Start.Y * 10 + 200),
        //                    new Vector3((float)node.nodes[i].data.OuterWalls[j].End.X * 10 + 400, 0, (float)node.nodes[i].data.OuterWalls[j].End.Y * 10 + 200), color);
        //                }
        //            DrawLine(new Vector3((float)node.nodes[i].data.PathEdge.Start.X * 10 + 400, 0, (float)node.nodes[i].data.PathEdge.Start.Y * 10 + 200),
        //            new Vector3((float)node.nodes[i].data.PathEdge.End.X * 10 + 400, 0, (float)node.nodes[i].data.PathEdge.End.Y * 10 + 200), color);
        //            ShowNode(node.nodes[i]);
        //        }
        //}




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
    public void PanelClick()
    {

    }
    void DrawLine(Vector3 first, Vector3 second, Color image, string name="")
    {
        var clone = Instantiate(img);
        clone.transform.SetParent(Panel.transform);
        clone.GetComponent<Image>().color = image;

        var imageRectTransform = clone.GetComponent<Image>().rectTransform;
        Vector3 pointA = new Vector3(first.x, first.z, 0);
        Vector3 pointB = new Vector3(second.x, second.z, 0);
        Vector3 differenceVector = pointB - pointA;

        imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, 2);
        imageRectTransform.pivot = new Vector2(0, 0.5f);
        imageRectTransform.localPosition = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
        clone.gameObject.name = name;
    }
    public void Clear()
    {
        
        foreach (Transform child in Panel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        creator.Clear();


    }
    void TitleSetText(string text)
    {
        title.GetComponent<Text>().text = text;
    }

    public void NextLine()
    {
        //var regions = PrivateHouse.floors[0];

       

        switch (state)
        {
            case ShowState.ShowOuterPolygon:
                TitleSetText("Форма здания");
                Clear();
                ShowOuterPolygon();
                state = ShowState.ShowRoof;
                break;
            case ShowState.ShowRoof:
                TitleSetText("Крыша");
                Clear();
                //ShowRoof();
                state = ShowState.ShowCoridor;
                break;
            case ShowState.ShowCoridor:
                TitleSetText("Коридор");
                Clear();
                //ShowCorridor();
                state = ShowState.ShowArchGrid;
                break;
            case ShowState.ShowArchGrid:
                TitleSetText("Архитектурная сетка с внешней и внутренней частью");
                Clear();
                //ShowArchGrid();
                state = ShowState.ShowGridCoridor;
                break;
            case ShowState.ShowGridCoridor:
                TitleSetText("Архитектурная сетка после добавления коридором");
                ShowGridCoridor();
                state = ShowState.ShowWindowsAndDoor;
                break;
            case ShowState.ShowWindowsAndDoor:
                TitleSetText("Архитектурная сетка после добавления окон и двери ");
                //ShowWindowsAndDoor();
                state = ShowState.ShowCorridorAndCorrectCell;
                break;
            case ShowState.ShowCorridorAndCorrectCell:
                TitleSetText("Архитектурная сетка после определения ячеек кандидатов для роста комнат ");
                //ShowCorridorAndCorrectCell();
                state = ShowState.ShowCorridorAndSelectedCells;
                break;
            case ShowState.ShowCorridorAndSelectedCells:
                TitleSetText("Архитектурная сетка после выбора ячеек кандидатов для роста комнат ");
                //ShowCorridorAndSelectedCells();
                state = ShowState.Show3d;
                break;
            case ShowState.Show3d:
                state = ShowState.ShowOuterPolygon;
                break;


        }
    }

    Color RndColor()
    {
        return new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
    }
    void ShowOuterPolygon()
    {
        var polygon = PrivateHouse.MainPolygon;
        ShowList(polygon, new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f)));
    }
   
    //void ShowCorridor()
    //{
    //    var corridor = PrivateHouse.FirstFloorCorridor;
    //    ShowOuterPolygon();
    //    ShowList(corridor.polygonOfCorridor, new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f)));
    //}
    void ShowArchGrid(Premises2D floor1, Color insideColor)
    {
        
        for (var i = 0; i < floor1.PlanProcessor2D.Grid.GetLength(0); i++)
        {
            for (var j = 0; j < floor1.PlanProcessor2D.Grid.GetLength(1); j++)
            {

                if (floor1.PlanProcessor2D.Grid[i, j].Tag == ArchitectureGrid.PlanCellTag.Inside)
                {
                    ShowList(floor1.PlanProcessor2D.Grid[i, j].Square, insideColor, multCoef: 8f);
                }
               
            }

        }
    }

    
    void ShowRooms(Premises2D floor1)
    {
        var rooms = floor1.GetRooms();


        for (var i = 0; i < rooms.Count; i++)
        {



            var roomWalls = rooms[i].Walls;

            var rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));

            if (rooms[i].RoomType == Rooms.RoomType.Corridor || rooms[i].RoomType == Rooms.RoomType.Stairs || rooms[i].RoomType == Rooms.RoomType.Lift)
                continue;
            for (var j = 0; j < rooms[i].Cells.Count; j++)
            {
                ShowList(rooms[i].Cells[j].Square, rnd, rooms[i].RoomName, 8f);
            }
            //var rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
            ////if (roomWalls != null)
            ////    for (var j = 0; j < roomWalls.Count; j++)
            ////    {

            ////        ShowList(new List<Vector2d>() { roomWalls[j].V1, roomWalls[j].V2 }, rnd);
            ////    }
            //rnd = new Color((float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f), (float)UnityEngine.Random.Range(0, 1f));
            //ShowList(rooms[i].MainPolygon, rnd);
        }

        //for (var i = 0; i < floor1.ArchPlan.neighbourCells.Count; i++)      
        //    ShowList(floor1.ArchPlan.neighbourCells[i].Square, Color.yellow);

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

    void ShowWindowsAndDoor(Premises2D floor1)
    {

      
        for (var i = 0; i < floor1.PlanProcessor2D.Grid.GetLength(0); i++)
        {
            for (var j = 0; j < floor1.PlanProcessor2D.Grid.GetLength(1); j++)
            {
                if (floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls != null)
                {
                    for (var k = 0; k < floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls.Count; k++)
                    {
                        if (floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].WallType == ArchitectureGrid.WallType.WallWithDoor)
                        {
                            ShowList(new List<Vector2d>() { floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V1, floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V2 }, colorExit);
                        }
                        else if (floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].WallType == ArchitectureGrid.WallType.WallWithWindow)
                        {
                            ShowList(new List<Vector2d>() { floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V1, floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls[k].V2 }, colorWindow);
                        }
                    }
                    Debug.Log(floor1.PlanProcessor2D.Grid[i, j].PartsOfOutsideWalls.Count);
                }
            }

        }
    }
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
