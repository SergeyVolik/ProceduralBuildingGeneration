using DataStructures.PriorityQueue;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureGrid
{
    public abstract class BasePlanProcessor2D : BaseGridOperations
    {
        protected BasePlanProcessor2D(List<Vector2d> polygon, List<Vector2d> buildingForm, Vector2d exit)
        {
            BuildingForm = buildingForm;
            OuterBoarderCells = new List<PlanCell>();
            MainPolygon = polygon;
            CreateGridByPolygon();
            Rooms = new List<Room2D>();



        }
        protected List<PartOfWall> windows;
        public List<Room2D> Rooms { get; set; }

        public PriorityQueue<Room2D, int> PriorityRooms;

        public Room2D Corridor;

        public PlanCell[,] Grid { get; set; }
        public List<PlanCell> GridVector { get; set; }
        protected GrowthProcessor growthProcessor { get; set; }
        public PlanCell ExitCell { get; set; }
        public List<PlanCell> OuterBoarderCells { get; set; }

        public double Area { get; private set; }

        private List<PartOfWall> partsOfOutSideWalls;
        public List<PartOfWall> PartsOfOutsideWalls
        {
            get {
                if (partsOfOutSideWalls == null)
                {
                    partsOfOutSideWalls = new List<PartOfWall>();
                    OuterBoarderCells.ForEach(c => {

                        partsOfOutSideWalls.AddRange(c.PartsOfOutsideWalls);
                    });
                     
                }
                return partsOfOutSideWalls;
            }
        }
        private List<PartOfWall> partsOfOutSideWallsWithWindow;
        public List<PartOfWall> Windows
        {
            get
            {
                if (partsOfOutSideWallsWithWindow == null)
                {
                    partsOfOutSideWallsWithWindow = new List<PartOfWall>();
                    OuterBoarderCells.ForEach(c => {

                        c.PartsOfOutsideWalls.ForEach(w => {

                            if (PrimitiveUtils.IsPointOnBoarder(w.V1, BuildingForm) && PrimitiveUtils.IsPointOnBoarder(w.V2, BuildingForm))
                            {
                                w.WallType = WallType.WallWithWindow;
                                partsOfOutSideWallsWithWindow.Add(w);
                            }
                        });

                    });

                }
                return partsOfOutSideWallsWithWindow;
            }
        }

        protected void CreateGridByPolygon()
        {
            int Step = 2;

            var startPoint = new Vector2d(MainPolygon.Min(v => v.X), MainPolygon.Min(v => v.Y));
            var endPoint = new Vector2d(MainPolygon.Max(v => v.X), MainPolygon.Max(v => v.Y));

            int columnSize = 0;
            double currPoint;

            for (currPoint = startPoint.X; currPoint < endPoint.X; currPoint += Step)
                columnSize += 1;
            if (currPoint < endPoint.X)
                columnSize += 1;

            int rowSize = 0;
            for (currPoint = startPoint.Y; currPoint < endPoint.Y; currPoint += Step)
                rowSize += 1;
            if (currPoint < endPoint.Y)
                rowSize += 1;

            Grid = new PlanCell[rowSize, columnSize];
            GridVector = new List<PlanCell>();
            double x, y;


            y = startPoint.Y;
            for (var i = 0; i < rowSize; i++)
            {
                x = startPoint.X;
                for (var j = 0; j < columnSize; j++)
                {
                    var point1 = new Vector2d(x, y);
                    var point2 = new Vector2d(x, y + Step);
                    var point3 = new Vector2d(x + Step, y + Step);
                    var point4 = new Vector2d(x + Step, y);
                    var square = new List<Vector2d>()
                    {
                       point1,point2,point3,point4
                    };

                    var cell = new PlanCell(square, MainPolygon, BuildingForm);

                    Grid[i, j] = cell;
                    GridVector.Add(cell);

                    if (Grid[i, j].PartsOfOutsideWalls != null)
                        OuterBoarderCells.Add(Grid[i, j]);
                    x += Step;

                    if (Grid[i, j].Tag == PlanCellTag.Inside)
                        Area++;
                }
                y += Step;
            }


            for (var i = 0; i < Grid.GetLength(0); i++)
            {
                for (var j = 0; j < Grid.GetLength(1); j++)
                {
                    if (i != 0)
                        Grid[i, j].BottomCell = Grid[i - 1, j];

                    if (i != Grid.GetLength(0)-1)
                        Grid[i, j].TopCell = Grid[i + 1, j];

                    if (j != 0)
                        Grid[i, j].LeftCell = Grid[i , j-1];

                    if (j != Grid.GetLength(1) - 1)
                        Grid[i, j].RightCell = Grid[i , j+1];

                }
            }


        }
       

        public abstract void CreatePlan();

    }
}
