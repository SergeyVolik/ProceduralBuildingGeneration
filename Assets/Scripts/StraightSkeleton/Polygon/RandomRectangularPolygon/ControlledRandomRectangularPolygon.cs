using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using StraightSkeleton.Polygon.Utils;
using System.CodeDom;

namespace StraightSkeleton.Polygon.RandomRectangularPolygon
{
    //public enum PolygonalFroms {
    //    Square = 0, Rectangle = 1, LShape = 2, TShape = 3, CShape = 4,
    //    StairsShape = 5, TLShape = 6, FShape = 7, HShape = 8, PlusShape = 9, EShape=10, TTShape = 11
    //}   
    public enum Point4 { Square = 0, Rectangle = 1 }
    public enum Point6 { LShape = 0 }
    public enum Point8 { TShape = 0, CShape = 1, StairsShape = 2 }
    public enum Point10 { TLShape = 0, FShape = 1 }
    public enum Point12 { HShape = 0, PlusShape = 1, EShape = 2, TTShape = 3 }

    class ControlledRandomRectangularPolygon
    {
        
        private double _recomArea;
        private int _numberOfPoints;
        public ControlledRandomRectangularPolygon(int pointsNumber, double area)
        {


            _recomArea = area;
            _numberOfPoints = pointsNumber;
            
            if(_recomArea % 4 != 0 || _recomArea <= 0)
                throw new ArgumentException("Invalid value: _recomArea ");
            if (_numberOfPoints % 2 != 0)
                throw new ArgumentException("Invalid value: NumberOfPoints isn't even number");
            if (_numberOfPoints < 4 || _numberOfPoints > 12)
                throw new ArgumentException("Invalid value: NumberOfPoints");

           
        }
        public List<Vector2d> Next()
        {
            var points = SelectionForNumberOfPoints();//SelectionForNumberOfPoints();
            
            return points;
        }

        private List<Vector2d> SelectionForNumberOfPoints()
        {
            List<Vector2d> polygon = new List<Vector2d>();
            switch (_numberOfPoints) { 
                case 4:
                    polygon =  GenerateFor4Point();
                    break;
                case 6:
                    polygon = GenerateFor6Point();
                    break;
                case 8:
                    polygon = GenerateFor8Point();
                    break;
                case 10:
                    polygon = GenerateFor10Point();
                    break;
                case 12:
                    polygon = GenerateFor12Point();
                    break;
            }
            
            return polygon;
        }
        #region 4 points
        private List<Vector2d> GenerateFor4Point()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            Random rnd = new Random();

            polygon = CreateRectangle();
            //Point4 ShapeType = (Point4)(rnd.Next(0, 2));

            //switch (ShapeType)
            //{
            //    case Point4.Rectangle:
            //        polygon = CreateRectangle();
            //        break;
            //    //case Point4.Square:
            //    //    polygon = CreateSquare();
            //    //    break;
            //}

            return polygon;

        }
        public List<Vector2d> CreateRectangle(float Lenght = 20, float Width = 10)
        {
            if (Lenght % 2 != 0 || Width % 2 != 0)
                throw new Exception("Ширина или высота прямоугольника не кратна 2");

            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(Lenght, 0));
            polygon.Add(new Vector2d(Lenght, Width));
            polygon.Add(new Vector2d(0, Width));


            polygon.AdjustArea(_recomArea, false, true);

            return polygon;

        }
        public List<Vector2d> CreateSquare()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(10, 0));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(0, 10));


            polygon.AdjustArea(_recomArea);
            return polygon;
        }
        #endregion
        #region 6 points
        private List<Vector2d> GenerateFor6Point()
        {       
            return CreateLShape();
        }
        public List<Vector2d> CreateLShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();
            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(20, 0));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(0, 20));

            return polygon;
        }
        #endregion
        #region 8 points
        private List<Vector2d> GenerateFor8Point()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            Random rnd = new Random();

            Point8 ShapeType = (Point8)(rnd.Next(0, 3));

            switch (ShapeType)
            {
                case Point8.CShape:
                    polygon = CreateCShape();
                    break;
                case Point8.StairsShape:
                    polygon = CreateStairsShape();
                    break;
                case Point8.TShape:
                    polygon = CreateTShape();
                    break;
            }

            return polygon;
        }
        public List<Vector2d> CreateCShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(20, 0));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(20, 30));
            polygon.Add(new Vector2d(0, 30));


            polygon.AdjustArea(_recomArea);
            return polygon;

        }
        public List<Vector2d> CreateTShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(10, 0));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(10, 30));
            polygon.Add(new Vector2d(0, 30));

            polygon.AdjustArea(_recomArea);
            return polygon;

        }
        public List<Vector2d> CreateStairsShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(10, 0));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(30, 20));
            polygon.Add(new Vector2d(30, 30));
            polygon.Add(new Vector2d(0, 30));

            polygon.AdjustArea(_recomArea);

            return polygon;

        }
        #endregion
        #region 10 points
        private List<Vector2d> GenerateFor10Point()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            Random rnd = new Random();

            Point10 ShapeType = (Point10)(rnd.Next(0, 2));

            switch (ShapeType)
            {
                case Point10.FShape:
                    polygon = CreateFShape();
                    break;
                case Point10.TLShape:
                    polygon = CreateTLShape();
                    break;
               
            }

            return polygon;
        }
        public List<Vector2d> CreateFShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(40, 0));
            polygon.Add(new Vector2d(40, 20));
            polygon.Add(new Vector2d(30, 20));
            polygon.Add(new Vector2d(30, 10));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(0, 10));

            polygon.AdjustArea(_recomArea);

            return polygon;

        }
        public List<Vector2d> CreateTLShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(20, 0));
            polygon.Add(new Vector2d(20, -10));
            polygon.Add(new Vector2d(30, -10));
            polygon.Add(new Vector2d(30, 10));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(0, 10));

            polygon.AdjustArea(_recomArea);

            return polygon;

        }

        #endregion
        #region 12 points
        private List<Vector2d> GenerateFor12Point()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            Random rnd = new Random();

            Point12 ShapeType = (Point12)(rnd.Next(0, 4));

            switch (ShapeType)
            {
                case Point12.EShape:
                    polygon = CreateEShape();
                    break;
                case Point12.HShape:
                    polygon = CreateHShape();
                    break;
                case Point12.PlusShape:
                    polygon = CreatePlusShape();
                    break;            
                case Point12.TTShape:
                    polygon = CreateTTShape();
                    break;

            }

            polygon.AdjustArea(_recomArea);

            return polygon;
        }
        public List<Vector2d> CreateEShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(50, 0));
            polygon.Add(new Vector2d(50, 20));
            polygon.Add(new Vector2d(40, 20));
            polygon.Add(new Vector2d(40, 10));
            polygon.Add(new Vector2d(30, 10));
            polygon.Add(new Vector2d(30, 20));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(0, 20));

            polygon.AdjustArea(_recomArea);

            return polygon;

        }
        public List<Vector2d> CreateHShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(10, 0));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(20, 0));
            polygon.Add(new Vector2d(30, 0));
            polygon.Add(new Vector2d(30, 30));
            polygon.Add(new Vector2d(20, 30));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(10, 30));
            polygon.Add(new Vector2d(0, 30));

            return polygon;

        }
        public List<Vector2d> CreateTTShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(50, 0));
            polygon.Add(new Vector2d(50, 10));
            polygon.Add(new Vector2d(40, 10));
            polygon.Add(new Vector2d(40, 20));
            polygon.Add(new Vector2d(30, 20));
            polygon.Add(new Vector2d(30, 10));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(0, 10));

            polygon.AdjustArea(_recomArea);

            return polygon;

        }
        public List<Vector2d> CreatePlusShape()
        {
            List<Vector2d> polygon = new List<Vector2d>();

            polygon.Add(new Vector2d(0, 0));
            polygon.Add(new Vector2d(10, 0));
            polygon.Add(new Vector2d(10, -10));
            polygon.Add(new Vector2d(20, -10));
            polygon.Add(new Vector2d(20, 0));
            polygon.Add(new Vector2d(30, 0));
            polygon.Add(new Vector2d(30, 10));
            polygon.Add(new Vector2d(20, 10));
            polygon.Add(new Vector2d(20, 20));
            polygon.Add(new Vector2d(10, 20));
            polygon.Add(new Vector2d(10, 10));
            polygon.Add(new Vector2d(0, 10));

            polygon.AdjustArea(_recomArea);

            return polygon;

        }
        
        #endregion
    }
}
