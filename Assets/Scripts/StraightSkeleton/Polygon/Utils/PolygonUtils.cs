using StraightSkeleton.Primitives;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace StraightSkeleton.Polygon.Utils
{
    /// <summary>
    /// Статический класс PolygonUtils
    /// Назначение в расширении методов полигона
    /// </summary>
    static class PolygonUtils
    {

        /// <summary> Коеф. для подсчета шага приближения</summary>
        private const double multFactor = 0.02d;
        /// <summary> Мин граница  при которой корректировка может закончиться</summary>
        private const double minFactor = 0.99d;
        /// <summary> Макс граница  при которой корректировка может закончиться </summary>
        private const double maxFactor = 1.01d;
        /// <summary>
        ///  Расширяюший метод AdjustArea
        ///  коректирует Площядь полигона(многоугольника)
        ///  с помощью разбиения на треугольники и подсчета суммы их площядей
        /// </summary>
        /// <param name="polygon">Список точек полигона</param>
        /// <param name="recomendedArea">приблизительно необходимая площядь</param>
        /// <returns>в случаи невозможности подсчета площяди выполнить операцию невозможно</returns>
        public static bool AdjustArea(this List<Vector2d> polygon, double recomendedArea, bool ignoreX = false, bool ignoreY = false)
        {

            //получаем площядь
            var area = CalculateArea(polygon);

            if (area == null || ignoreX && ignoreY)
                return false;
            //вычисляем отношение реком. площади к имеющейся 

                double factor = recomendedArea / (double)area;
            //подсчитываем текущий шаг
            double step;

            if (factor > 1)
                step = 1 + factor * multFactor;
            else step = 1 - factor * multFactor;

            //цикл приближения
            while (factor >= maxFactor || factor <= minFactor)
            {
                //приближения с умнажение на шагом
                for (var i = 0; i < polygon.Count; i++)
                    polygon[i] = new Vector2d(!ignoreX ? polygon[i].X * step : polygon[i].X, !ignoreY ? polygon[i].Y * step : polygon[i].Y);

                //подсчет площяди
                area = CalculateArea(polygon);

                //снова вычисляем отношение реком. площади к имеющейся 
                factor = recomendedArea / (double)area;

                //снова подсчитываем текущий шаг
                if (factor > 1)
                    step = 1 + factor * multFactor;
                else step = 1 - factor * multFactor;

            }

            for (var i = 0; i < polygon.Count; i++)
                polygon[i] = new Vector2d(Math.Round(polygon[i].X), Math.Round(polygon[i].Y));

            area = CalculateArea(polygon);

            
            step = 1;

            if (area > recomendedArea)               
                step *= -1;


            for (var i = 0; i < polygon.Count; i++)
            {
                var p = polygon[i];
                if (p.X != 0 && p.X % 2 != 0)
                    p.X += step;
                if (p.Y != 0 && p.Y % 2 != 0)
                    p.Y += step;

                polygon[i] = p;
            }
           
            

           
                        
            return true;
        }
       

        static List<Vector2d>  CreateCopyPolygon(List<Vector2d> polygon)
        {
            var pol = new List<Vector2d>();

            for (var i = 0; i < polygon.Count; i++)
            {
                pol.Add(new Vector2d(polygon[i].X, polygon[i].Y));
            }

            return pol;

        }
        /// <summary>
        /// CalculateArea - расширающий метод 
        /// для подсчета площяди
        /// </summary>
        /// <param name="polygon"> Список точек полигона </param>
        /// <returns>
        ///  Площадь многоугльника(полигона) или null если посчитать полощадь невозможно
        /// </returns>
        /// 

        public static double? CalculateArea(this List<Vector2d> polygon)
        {
            double Area = 0;
            //var polygon = CreateCopyPolygon(_polygon);
            //если полигон
            if (polygon.Count >= 4)
            {             
                //триангулируем полигон
                var triangles = Triangulator.Triangulate(polygon);

                //если ошибка
                if (triangles == null)
                    return null;

                //сумируем площяди триуголников
                for (var i = 0; i < triangles.Count; i++)
                    Area += triangles[i].GetArea();

            }
            //иначе если прямоугольник 
            else if (polygon.Count == 4)           
                Area = polygon[0].DistanceTo(polygon[1]) * polygon[1].DistanceTo(polygon[2]);
            

            return Area;
        }       
        /// <summary> Коеф. мин приемлевого отношения минимальной к макс длинне ребра </summary>
        private static double Epsilon = 0.1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon">Список точек полигона  </param>
        /// <returns> если расстояним</returns>
        public  static bool CheckingDistanceBetweenPoints(this List<Vector2d> polygon)
        {   
            // список расстояний между точками полигона
            List<double> distances = new List<double>();

            //потсчет расстояний
            for (var i = 0; i < polygon.Count-1; i++)            
                distances.Add(polygon[i].DistanceTo(polygon[i + 1]));                          
            distances.Add(polygon[0].DistanceTo(polygon[polygon.Count - 1]));

            //если отношение мин к макс больше Epsilon
            if (distances.Min() / distances.Max() >= Epsilon)
                return true;

            return false;
        }

        public static Vector2d CenterMasFormula(List<Vector2d> points)
        {
            float sumRadiusVectorX = 0;
            float sumRadiusVectorY = 0;

            int mass = 1;

            for (var i = 0; i < points.Count; i++)
            {
                sumRadiusVectorX += (float)points[i].X * mass;
                sumRadiusVectorY += (float)points[i].Y * mass;
            }

            return new Vector2d(sumRadiusVectorX / points.Count, sumRadiusVectorY / points.Count);
        }

        public static Vector3 CenterMassUnityVector3(this List<Vector3> points)
        {
            float sumRadiusVectorX = 0;
            float sumRadiusVectorY = 0;
            float sumRadiusVectorZ = 0;

            int mass = 1;

            for (var i = 0; i < points.Count; i++)
            {
                sumRadiusVectorX += (float)points[i].x * mass;
                sumRadiusVectorY += (float)points[i].y * mass;
                sumRadiusVectorZ += (float)points[i].z * mass;
            }

            return new Vector3(sumRadiusVectorX / points.Count, sumRadiusVectorY / points.Count, sumRadiusVectorZ / points.Count);
        }
    }
}
