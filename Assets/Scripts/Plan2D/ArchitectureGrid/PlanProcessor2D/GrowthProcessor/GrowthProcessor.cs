
using Assets.Scripts.Tools;
using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
    public abstract class GrowthProcessor : BaseGridOperations
    {
        public PlanCell[,] Grid { get; set; }
        public List<PlanCell> GridVector { get; set; }
        public List<Room2D> Rooms { get; set; }
        public List<Room2D> RoomsToGrowth { get; set; }
        public List<PlanCell> CorrectCells { get; private set; }
        public List<PlanCell> SelectedCellsForGrowth { get; private set; }
        public PlanCell ExitCell { get; private set; }

        public int StartedFreeArea { get; private set; }

        protected double distance;

        //protected double minXOfPolygon = 9999999;
        //protected double minYOfPolygon = 9999999;
        //protected double maxXOfPolygon = -999999;
        //protected double maxYOfPolygon = -999999;
        //social rule param
        protected double maxDistance;
        public GrowthProcessor(PlanCell[,] _Grid, List<PlanCell> _GridVector,  List<Room2D> _Rooms, PlanCell exitcell)
        {
            GridVector = _GridVector;
            Grid = _Grid;
            Rooms = _Rooms;
            ExitCell = exitcell;
            CorrectCells = new List<PlanCell>();
            SelectedCellsForGrowth = new List<PlanCell>();
            RoomsToGrowth = new List<Room2D>();
            //ExitCell = _ExitCell;

            distance = 2;

            
        }

       
        public void GrowthOfRooms()
        {
            //FindBoardsPoits();
            GetAreaFree();
            FirstExpansionStep();
            SecondExpansionStep();
            FixingStep();
           
            
        }

        //void FindBoardsPoits()
        //{
        //    for (var i = 0; i < Grid.GetLength(0); i++)
        //    {
        //        for (var j = 0; j < Grid.GetLength(1); j++)
        //        {
        //            if (Grid[i, j].Center.X > maxXOfPolygon)
        //                maxXOfPolygon = Grid[i, j].Center.X;

        //            if (Grid[i, j].Center.Y > maxYOfPolygon)
        //                maxYOfPolygon = Grid[i, j].Center.Y;

        //            if (Grid[i, j].Center.X < maxXOfPolygon)
        //                minXOfPolygon = Grid[i, j].Center.X;

        //            if (Grid[i, j].Center.X < maxXOfPolygon)
        //                minYOfPolygon = Grid[i, j].Center.Y;
        //        }
        //    }
        //}

        protected bool NextCellsForGrowthByWay(Room2D room, out List<PlanCell> NextCellsOnWay,
            Func<PlanCell, bool> condition, Func<Room2D, List<PlanCell>> FindRoomCellsForExpantion,
            Func<PlanCell[,], (int, int), Func<PlanCell, bool>, bool> CheckCellsOnTheWay)
        {
            var extremeCells = FindRoomCellsForExpantion(room);
            
            PlanCell cell = null;
            var localNextByWayCells = new List<PlanCell>();
            extremeCells.ForEach(e =>
            {
                var indelxes = Grid.CoordinatesOf(e);
                if (!CheckCellsOnTheWay(Grid, indelxes, (c) => { if (condition(c)) { cell = c; return true; } else return false; }))
                {
                    //canGrowth = false;
                    //localNextByWayCells.Clear();
                }
                else localNextByWayCells.Add(cell);
            });
            if (localNextByWayCells.Count != 0)
            {
                NextCellsOnWay = localNextByWayCells;
                return true;
            }
            NextCellsOnWay = null;
            return false;
        }



        protected void GetAreaFree()
        {
            int Area = 0;
            foreach (var cell in Grid)
                if (cell.IsFree) Area += cell.Area;

            StartedFreeArea = Area;
        }


        protected List<PlanCell> FindRoomCellsForExpantionLeft(Room2D room)
        {
            var minX = room.Cells.Min(c => c.Center.X);

            return room.Cells.FindAll(c => c.Center.X == minX);

            //var maxX = room.Cells.Max(c => c.Center.X);

            //var allMaxX = room.Cells.FindAll(c => c.Center.X == maxX);

            //List<int> jCoordinbates = new List<int>();
            //allMaxX.ForEach(e => jCoordinbates.Add(Grid.CoordinatesOf(e).j));
            //bool freeCellsFinded = false;




            //var candidates = new List<PlanCell>();

            //for (var i = Grid.CoordinatesOf(allMaxX[0]).i; i > 0; i--)
            //{
            //    for (var j = 0; j < jCoordinbates.Count; j++)
            //    {
            //        if (Grid[i, jCoordinbates[j]].IsFree)
            //        {
            //            freeCellsFinded = true;
            //            candidates.Add(Grid[i, jCoordinbates[j]]);
            //        }
            //    }
            //    if (freeCellsFinded)
            //        return candidates;

            //}


            //return null;


        }
        protected List<PlanCell> FindRoomCellsForExpantionRight(Room2D room)
        {
            var maxX = room.Cells.Max(c => c.Center.X);

            return room.Cells.FindAll(c => c.Center.X == maxX);
        }
        protected List<PlanCell> FindRoomCellsForExpantionTop(Room2D room)
        {
            var maxY = room.Cells.Max(c => c.Center.Y);

            return room.Cells.FindAll(c => c.Center.Y == maxY);

        }
        protected List<PlanCell> FindRoomCellsForExpantionBottom(Room2D room)
        {
            var minY = room.Cells.Min(c => c.Center.Y);

            return room.Cells.FindAll(c => c.Center.Y == minY);
        }

        protected abstract void FirstExpansionStep();
        //Цель шага в топ чтобы помочь комнатам которые (заросли) и 
        //не смошли расшириться до своего минимального размера
        protected abstract void FixingStep();

        //в данном шаге комнаты расшряються чтобы заполнить все пустое пространство.
        protected abstract void SecondExpansionStep();

        protected void PlacingRoomsInCells()
        {

            for (var i = 0; i < Rooms.Count; i++)
            {

                var scores = new List<PlanCellScore>();
                var publicCells = new List<PlanCell>();
                var privateCells = new List<PlanCell>();

                for (var j = 0; j < SelectedCellsForGrowth.Count; j++)
                {
                    if (SelectedCellsForGrowth[j].room.RoomRequisite.ZoneType == ZoneType.Public)
                        publicCells.Add(SelectedCellsForGrowth[j]);
                    else privateCells.Add(SelectedCellsForGrowth[j]);
                }



                for (var j = 0; j < CorrectCells.Count; j++)
                {
                    var rooms = Rooms[i].RoomRequisite.ZoneType == ZoneType.Public ? publicCells : privateCells;


                    scores.Add(new PlanCellScore(CorrectCells[j], Rooms[i].RoomRequisite.NeedWindow, Rooms[i].RoomRequisite.ZoneType, ExitCell, maxDistance, rooms, null));
                }

                PlanCellScore.NormalizeScores(scores);

                var sortedScores = scores.OrderBy(score => score.Normalized).ToList();
                
                var notSelectedRoom = true;
                for (var j = sortedScores.Count - 1; j >= 0; j--)
                {

                    if (UnityEngine.Random.Range(0f, 1f) < sortedScores[j].Normalized)
                    {                       
                        Rooms[i].AddCell(sortedScores[j].cell);
                        CorrectCells.Remove(sortedScores[j].cell);
                        SelectedCellsForGrowth.Add(sortedScores[j].cell);

                        notSelectedRoom = false;
                        break;
                    }
                }
                if (notSelectedRoom)
                {
                    Rooms[i].AddCell(sortedScores[sortedScores.Count - 1].cell);                  
                    CorrectCells.Remove(sortedScores[sortedScores.Count - 1].cell);
                    SelectedCellsForGrowth.Add(sortedScores[sortedScores.Count - 1].cell);
                }
            }
        }
        protected void AddCellsToCorrectCells()
        {

            CorrectCells = new List<PlanCell>();

            CorrectCells.Add(ExitCell);
        

            for (var i = 0; i < Grid.GetLength(0); i++)
            {
                for (var j = 0; j < Grid.GetLength(1); j++)
                {
                    if (CorrectCells.Contains(Grid[i, j]))
                        continue;

                    if (Grid[i, j].PartsOfOutsideWalls != null && Grid[i, j].PartsOfOutsideWalls.Exists(p => p.WallType == WallType.WallWithWindow))
                    {
                        CorrectCells.Add(Grid[i, j]);
                        continue;
                    }

                    if (DistanceIsNormal(Grid[i, j]) && Grid[i, j].Tag == PlanCellTag.Inside && Grid[i, j].room == null)
                        CorrectCells.Add(Grid[i, j]);
                }
            }

            FindMaxDistance();
        }

        void FindMaxDistance()
        {
            var list = new List<double>();
            CorrectCells.ForEach(c => list.Add(c.Center.DistanceTo(ExitCell.Center)));

            maxDistance = list.Max();

        }

      
        protected bool DistanceIsNormal(PlanCell cell)
        {
            var AllDistanceIsNormal = true;
            for (var k = 0; k < CorrectCells.Count; k++)
            {
                if (CorrectCells[k].Center.DistanceTo(cell.Center) < distance)
                {
                    AllDistanceIsNormal = false;
                    break;
                }

            }
            return AllDistanceIsNormal;
        }

        protected void TryGrowthRoomAsSqure(List<PlanCell> neighbourCells, RoomType type, string name, int lenghtRoom, int widthRoom)
        {
            Room2D room = new Room2D(type, name);

            //двумерный список ячеек комнаты
            List<PlanCell> cells = new List<PlanCell>();
            List<PlanCell> intermediateCells = new List<PlanCell>();
            List<PlanCell> currentLine = new List<PlanCell>();


            for (var i = 0; i < neighbourCells.Count; i++)
            {

                PlanCell cell = null;


                //cells.Add(currentLine);

                int w = Grid.GetLength(0); // width
                int h = Grid.GetLength(1); // height

                var indexes = Grid.CoordinatesOf(neighbourCells[i]);


                cells.Clear();
                cells.Add(neighbourCells[i]);


                if (CheckTopCellByCondition(Grid, indexes, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }))
                {

                    //пытаемя вырастить комнату в длину
                    while (CheckTopCellByCondition(Grid, indexes, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }) && cells.Count < lenghtRoom)
                    {
                        cells.Add(cell);
                        indexes = Grid.CoordinatesOf(cell);
                    }

                    if (lenghtRoom != cells.Count)
                    {
                        cells.Clear();
                        cells.Add(neighbourCells[i]);
                        indexes = Grid.CoordinatesOf(neighbourCells[i]);

                        while (CheckTopCellByCondition(Grid, indexes, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }) && cells.Count < widthRoom)
                        {
                            cells.Add(cell);
                            indexes = Grid.CoordinatesOf(cell);

                        }

                        if (widthRoom == cells.Count)
                        {
                            currentLine = cells;
                            for (var k = 1; k < lenghtRoom; k++)
                            {
                                intermediateCells = currentLine;
                                currentLine = new List<PlanCell>();
                                for (var j = 0; j < intermediateCells.Count; j++)
                                {
                                    var indexes1 = Grid.CoordinatesOf(intermediateCells[j]);

                                    if (CheckLeftCellByCondition(Grid, indexes1, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }))
                                    {
                                        currentLine.Add(cell);

                                    }
                                }

                                cells.AddRange(currentLine);


                            }
                        }

                    }
                    else if (lenghtRoom == cells.Count)
                    {
                        currentLine = cells;
                        for (var k = 1; k < widthRoom; k++)
                        {
                            intermediateCells = currentLine;
                            currentLine = new List<PlanCell>();
                            for (var j = 0; j < intermediateCells.Count; j++)
                            {
                                var indexes1 = Grid.CoordinatesOf(intermediateCells[j]);


                                if (CheckLeftCellByCondition(Grid, indexes1, (c) => { if (CellIsFree(c)) { cell = c; return true; } else return false; }))
                                {
                                    currentLine.Add(cell);

                                }

                            }
                            cells.AddRange(currentLine);


                        }


                    }

                    //все хорошо у нас получилось вырастить комнату
                    if (cells.Count == lenghtRoom * widthRoom)
                        break;
                }

                //var iMinus = indexes.i - 1;
                //if (BottomIsFree(Grid[iMinus, indexes.j], iMinus))
                //{


                //}
                //var jPlus = indexes.j + 1;

                //if ()
                //var jMinus = indexes.j - 1;



                //else cells.Clear();
                //cell = Grid[iMinus, indexes.j];
                //if (iMinus > 0)
                //    while (cell.IsFree() && neighbourCells.Contains(cell) && iPlus < w && cells.Count < LIFT_LENGHT)
                //        cells.Add(cell);

                //cell = Grid[indexes.i, jPlus];
                //if (jPlus < h)
                //    while (cell.IsFree() && neighbourCells.Contains(cell) && iPlus < w && cells.Count < LIFT_LENGHT)
                //        cells.Add(cell);

                //cell = Grid[indexes.i, jMinus];
                //if (jMinus > 0)
                //    while (cell.IsFree() && neighbourCells.Contains(cell) && iPlus < w && cells.Count < LIFT_LENGHT)
                //        cells.Add(cell);
            }

            room.Cells = cells;
            Rooms.Add(room);
        }
    }
}
