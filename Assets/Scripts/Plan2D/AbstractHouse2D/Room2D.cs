using ArchitectureGrid;
using Assets.Scripts.Premies.Buildings.Building2D;
using StraightSkeleton.Polygon.Utils;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rooms
{

     
    public enum RoomType { None, Corridor, Bedroom, Restroom, Kitchen, Livingroom, Bathroom, Hall, Porch, Garage, Closet, Diningroom, Stairs, Lift, Flat, Building }
    public enum ZoneType { Private, Public, Service, None }
    public enum DoorType { OpenWall, Door }


    public class Angle 
    {
        public PartOfWall FirstWall { get; private set; }
        public PartOfWall SecondWall { get; private set; }

        public Vector2d AnglePoint { get; private set; }

        public Angle(PartOfWall first, PartOfWall second, Vector2d point)
        {
            FirstWall = first;
            SecondWall = second;

            AnglePoint = point;
        }
    }

    public class Room2D : Polygon, IFloorPremises2D
    {
        public int? FlatId = null;
        private RoomRequisite m_roomRequisite;
        private List<PartOfWall> m_walls;
        private List<PlanCell> m_cells;
        private string m_roomName;
        private RoomType m_roomType = RoomType.None;

        public List<PartOfWall> Walls
        {
            get
            {
                if (m_walls == null)
                    m_walls = GetRoomWalls();
                return m_walls;
            }
            private set { m_walls = value; }
        }
       

      
        public List<PlanCell> Cells {
            get { return m_cells; }
            set {

                value.ForEach(c => c.room = this);
                m_cells = value;
            }
        }


       
        public string RoomName {
            get {
                if (RoomRequisite)
                    return RoomRequisite.RoomName;
                else return m_roomName;
            }
        }

        public int Area { get => Cells.Count * 4; }

       

        public RoomRequisite RoomRequisite => m_roomRequisite;


        public bool HaveDoor => Walls.Exists(w => w.WallType == WallType.WallWithDoor || w.WallType == WallType.NoWall);

        public RoomType RoomType {
            get {
                if (RoomRequisite)
                    return RoomRequisite.RoomType;
                else return m_roomType;
            }
        }

        public int DoorCount => Walls.Count(w => w.IsDoor);


        public Vector2d CenterOfRoom;

        public void SetCellsRoom()
        {
            Cells.ForEach(c => c.room = this);
        }


        public Room2D(RoomType roomType, string name)
        {
            m_roomType = roomType;
            Cells = new List<PlanCell>();
            m_roomName = name;

        }
        public Room2D(RoomRequisite roomAtributs, string name)
        {
            m_roomRequisite = roomAtributs;
            Cells = new List<PlanCell>();
            m_roomName = name;

        }
        public Room2D(RoomRequisite roomAtributs)
        {
            m_roomRequisite = roomAtributs;
            Cells = new List<PlanCell>();         
        }
        public Room2D(string name)
        {
          
            Cells = new List<PlanCell>();
            m_roomName = name;
            

        }

        public void AddCell(PlanCell cell)
        {
            cell.room = this;
            m_cells.Add(cell);
        }

        //по формуле центра масс часный случай для прямоуголника или квадрата
        
        public Vector2d FindCenterOfRoomForRectangle()
        {
            var minX = Cells.Min((cell1) => cell1.Square.Min(cc => cc.X));
            var minY = Cells.Min((cell1) => cell1.Square.Min(cc => cc.Y));
            var maxX = Cells.Max((cell1) => cell1.Square.Max(cc => cc.X));
            var maxY = Cells.Max((cell1) => cell1.Square.Max(cc => cc.Y));



            List<Vector2d> points = new List<Vector2d>();
            points.Add(new Vector2d(minX, minY));
            points.Add(new Vector2d(maxX, minY));
            points.Add(new Vector2d(maxX, maxY));
            points.Add(new Vector2d(minX, maxY));

            return PolygonUtils.CenterMasFormula(points);
        }

        public bool AddDoorBetweenRooms(Room2D neighboarRoom)
        {
            var nighboarWalls1 = new List<PartOfWall>();
            var nighboarWalls2 = new List<PartOfWall>();
            neighboarRoom.Walls.ForEach(w =>
            {
                if (Walls.Exists(w2 => w2.Equals(w)))
                {
                   
                  
                    nighboarWalls2.Add(w);
                }

            });


            if (nighboarWalls2.Count > 0)
            {
                var wall = nighboarWalls2[UnityEngine.Random.Range(0, nighboarWalls2.Count)];

                wall.WallType = WallType.NoWall;
                var wall2 = Walls.FirstOrDefault(w2 => w2.Equals(wall));
                wall2.WallType = WallType.NoWall;

                return true;
            }
               
            return false;
        }
        

        private PartOfWall AngleCell(PlanCell PrevCell, PartOfWall prevEdge)
        {
            PartOfWall edge = null;
            if (Cells.Count != 1)
            {
                while (Cells.Find(cell => cell != PrevCell && cell.Square.Contains(prevEdge.V2)) == null)
                {
                    var nextVert = PrevCell.Square.Find(
                            v => prevEdge.V1.X != v.X && prevEdge.V1.Y != v.Y && prevEdge.V2.X == v.X ||
                            prevEdge.V1.X != v.X && prevEdge.V1.Y != v.Y && prevEdge.V2.Y == v.Y
                        );


                    MainPolygon.Add(prevEdge.V2);
                    edge = new PartOfWall(prevEdge.V2, nextVert, WallType.SimpleWall);
                    prevEdge = edge;
                    if (m_walls.Find(e => e.V1 == edge.V1 && e.V1 == edge.V1) == null)
                    {
                        m_walls.Add(edge);
                        
                    }

                }
            }
            else {
                edge = new PartOfWall(PrevCell.Square[0], PrevCell.Square[1], WallType.SimpleWall);
                m_walls.Add(edge);
                edge = new PartOfWall(PrevCell.Square[1], PrevCell.Square[2], WallType.SimpleWall);
                m_walls.Add(edge);
                edge = new PartOfWall(PrevCell.Square[2], PrevCell.Square[3], WallType.SimpleWall);
                m_walls.Add(edge);
                edge = new PartOfWall(PrevCell.Square[3], PrevCell.Square[0], WallType.SimpleWall);
                m_walls.Add(edge);

                MainPolygon.Add(PrevCell.Square[0]);
                MainPolygon.Add(PrevCell.Square[1]);
                MainPolygon.Add(PrevCell.Square[2]);
                MainPolygon.Add(PrevCell.Square[3]);
                prevEdge = edge;
            }

            return edge;
        }

        private int NeighboringPoints(PlanCell c1, PlanCell c2)
        {
            int counter = 0;

            foreach (var p1 in c1.Square)
                foreach (var p2 in c2.Square)
                    if (p1.Equals(p2))
                        counter++;

            //Debug.Log(counter);
            return counter;
        }
        private List<PartOfWall> GetRoomWalls()
        {
            try
            {
                 MainPolygon = new List<Vector2d>();
                 Vector2d minVertex, firstVert;

                PartOfWall prevEdge;

                double minX;
                double minY;

                PlanCell StartCell, CurrentCell, PrevCell;


                if (Cells.Count == 0)
                    return null;


                m_walls = new List<PartOfWall>();

                
                minY = Cells.Min((cell1) => cell1.Center.Y);


                var cells = Cells.FindAll(cell => cell.Center.Y == minY);

                minX = cells.Min((cell1) => cell1.Center.X);
                StartCell = cells.Find(cell => cell.Center.Y == minY && cell.Center.X == minX);

                minX = StartCell.Square.Min((v1) => v1.X);
                minY = StartCell.Square.Min((v1) => v1.Y);

                minVertex = StartCell.Square.Find(v => v.X == minX && v.Y == minY);

                firstVert = StartCell.Square.Find(v => v.X == minVertex.X && v.Y != minVertex.Y);
               


                prevEdge = new PartOfWall(minVertex, firstVert, WallType.SimpleWall);
                m_walls.Add(prevEdge);
               

                CurrentCell = null;
                PrevCell = StartCell;
                

                int i = 0;
                Vector2d nextVert;
                AngleCell(PrevCell, prevEdge);

                if (Cells.Count > 1)
                {
                    while (CurrentCell != StartCell)
                    {
                        i++;
                        if (i > 100)
                            break;

                        var edge = AngleCell(PrevCell, prevEdge);


                        prevEdge = edge == null ? prevEdge : edge;

                        var allPossibleCells = Cells.FindAll(cell => cell != PrevCell && cell.Square.Contains(prevEdge.V2));
                        if (allPossibleCells.Count == 1)
                        {
                            CurrentCell = allPossibleCells[0];

                            nextVert = CurrentCell.Square.Find(
                               v => v.X == prevEdge.V1.X && prevEdge.V2.X == v.X && prevEdge.V2.Y != v.Y ||
                               v.Y == prevEdge.V1.Y && prevEdge.V2.Y == v.Y && prevEdge.V2.X != v.X
                            );

                        }
                        else
                        {

                            CurrentCell = allPossibleCells.Find(cell => NeighboringPoints(cell, PrevCell) == 1);



                            nextVert = CurrentCell.Square.Find(
                             v => v.X != prevEdge.V1.X && prevEdge.V1.Y != v.Y && prevEdge.V2.X == v.X && prevEdge.V2.Y != v.Y ||
                                  v.Y != prevEdge.V1.Y && prevEdge.V1.X != v.X && prevEdge.V2.X != v.X && prevEdge.V2.Y == v.Y                              
                          );

                            MainPolygon.Add(prevEdge.V2);
                        }




                        if (nextVert != null)
                        {
                            prevEdge = new PartOfWall(prevEdge.V2, nextVert, WallType.SimpleWall);

                            m_walls.Add(prevEdge);
                        }
                        PrevCell = CurrentCell;


                    }
                    AngleCell(PrevCell, prevEdge);
                }

                

                return m_walls;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }
            
           
        }

        private bool EqualAtThreePoints(Vector2d point1, Vector2d point2, Vector2d point3, bool x)
        {
            if (x)
                return point1.X == point2.X && point1.X == point3.X;
            else
                return point1.Y == point2.Y && point1.Y == point3.Y;
        }
        private int WallPointInCell(PlanCell cell1)
        {
            int counter = 0;

            for (var i = 0; i < m_walls.Count; i++)
            {
                if (cell1.Square.Contains(m_walls[i].V1))
                    counter++;
                if (cell1.Square.Contains(m_walls[i].V2))
                    counter++;
            }

            return counter;
        }

    }
}
