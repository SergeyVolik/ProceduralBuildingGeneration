using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using StraightSkeleton.Polygon.Utils;

namespace StraightSkeleton.Polygon.RandomRectangularPolygon
{

    /// <summary>
    /// RandomRectangularOuterShell - рандомизирует прямоугольный полигон без пересечений
    /// </summary>
    static class RandomRectangularPolygon
    {
        ///<summary> необходимая конечная площадь многоугольника</summary>
        private static double _recomendedArea;
        /// <summary> количество точек полигона</summary>
        private static int _numberOfPoints;

        /// <summary>
        /// Next - статическая функция для создания прямоугольного полигона
        /// </summary>
        /// <param name="numberOfPoints"></param>
        /// <param name="recomendedArea"></param>
        /// <returns></returns>
        public static List<Vector2d> Next(int numberOfPoints, double recomendedArea) {

            _recomendedArea = recomendedArea;
            _numberOfPoints = numberOfPoints;

            //Задаем приемлимые условия
            if(_recomendedArea < 50 || _recomendedArea > 1000)
                throw new ArgumentException("Invalid value: recomendedArea need in range 50-1000");
            if (_numberOfPoints < 4 || _numberOfPoints > 12)
                throw new ArgumentException("Invalid value: NumberOfPoints");
            if (_numberOfPoints % 2 != 0)
                throw new ArgumentException("Invalid value: NumberOfPoints isn't even number");
          
            //генерируем прямоугольный полигон
            return InternalGenerate();
        }
       
   
        /// <summary> </summary>
        private static double PointEps;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        private static bool CheckPointPos(Vector2d point, List<Vector2d> polygon)
        {
            bool checkPoint = true;
           
            for (var i = 0; i < polygon.Count; i++)
            {
                if (point.X != polygon[i].X &&
                    Math.Abs(point.X - polygon[i].X) < PointEps &&
                    Math.Abs(polygon[i].X - point.X) < PointEps)
                {
                    checkPoint = false;
                    break;
                }
                else if (point.Y != polygon[i].Y &&
                        Math.Abs(point.Y - polygon[i].Y) < PointEps &&
                        Math.Abs(polygon[i].Y - point.Y) < PointEps
                        )
                {
                    checkPoint = false;
                    break;
                }
                else if (point.X == polygon[i].X && point.Y == polygon[i].Y)
                {
                    checkPoint = false;
                    break;
                }
            }
            
            return checkPoint;
        }

        /// <summary>
        /// Генерация случайного полигона
        /// </summary>
        /// <returns> Список точек полигона </returns>
        private static List<Vector2d> InternalGenerate()
        {
            var polygon = new List<Vector2d>();

            var minRange = 0;
            var maxRange = 0;
            var rnd = new System.Random();
          
            var point = new Vector2d();

            //есть ли пересечение отрезков прямых полигона
            var haveIntersect = false;
            //возможно ли отрегулировать площадь
            var canAdjustment = false;
            //прошлая точка
            var prevPoint = new Vector2d();

            var counter = 0;
            double nextValue = 0;
            //если прямоугольник
            if (_numberOfPoints == 4)
            {
                //точка 1 
                point = new Vector2d(0, 0);
                polygon.Add(point);

                //граници рандома
                minRange = Convert.ToInt32(_recomendedArea / 4 / 4d);
                maxRange = Convert.ToInt32(_recomendedArea / 4 / 2d );
             
                //точка2           
                point = new Vector2d(point.X + nextValue, point.Y);
                    
                //поиск 2 ребра по площади
                var lengthSecondEdge = _recomendedArea / polygon[0].DistanceTo(point);
                if (rnd.Next(0, 2) == 0)
                    lengthSecondEdge *= -1;

                //точка3             
                polygon.Add(new Vector2d(point.X, point.Y + lengthSecondEdge));
                //точка4
                polygon.Add(new Vector2d(polygon[0].X, polygon[polygon.Count - 1].Y));
            }
            //если больше 4 точек
            else
            {
                //граници рандома для многоугольника
                minRange = Convert.ToInt32(_recomendedArea / 20d / (_numberOfPoints - 2));
                maxRange = Convert.ToInt32(_recomendedArea / 10d / (_numberOfPoints - 2));

                PointEps = _recomendedArea / 10d / (_numberOfPoints - 2) / 10;

                do
                {
                    polygon = new List<Vector2d>();

                    //1 точка
                    point = new Vector2d(0, 0);
                    polygon.Add(point);


                    var currPoint = 1;
                    //пока кол точек не будет _numberOfPoints - 1 
                    while (currPoint < _numberOfPoints - 1)
                    {
                        //прошлая точка
                        prevPoint = polygon[polygon.Count - 1];

                        //отвечает за то если будет зацикливание, то есть невозможно будет добавть новую точку
                        counter = 0;

                        //цикл для добавления след точки
                        for (; ; )
                        {
                            //координаты прошлой точки
                            point = new Vector2d(prevPoint.X, prevPoint.Y);

                            //сдвиг для новой точки
                            nextValue = rnd.Next(minRange, maxRange);

                            //умножаем или нет на -1
                            if (rnd.Next(0, 2) == 0)
                                nextValue *= -1;

                            //если четная плюс к Х иначе к Y
                            if (currPoint % 2 == 0)
                                point.X += nextValue;
                            else
                                point.Y += nextValue;


                            //прооверка если последняя точка
                            if (currPoint - 1 == _numberOfPoints - 2)
                            {
                                //проверяем последнее ребро и предпоследнее
                                if (!CheckPointPos(new Vector2d(point.X, polygon[0].Y), polygon) || !CheckPointPos(new Vector2d(point.X, polygon[0].Y), polygon))
                                    continue;
                            }
                            else
                            {
                                //если с точой что-то не так то пропускаем итерацию
                                if (!CheckPointPos(point, polygon))
                                    continue;
                            }

                            //если последнее добавленое ребро имеет пересечение и точек больше 1
                            if (LastAddHaveIntersect(polygon, false) && polygon.Count != 1)
                            {
                                polygon.Remove(polygon[polygon.Count - 1]);
                                currPoint--;
                                continue;
                            }
                            
                           
                            polygon.Add(point);


                            //если точка не удавлитворяет растоянию между точками то повторить все выше сделаное пока не будет коректно
                            while (!polygon.CheckingDistanceBetweenPoints())
                            {
                                polygon.Remove(point);

                                point = new Vector2d(prevPoint.X, prevPoint.Y);

                                nextValue = rnd.Next(minRange, maxRange);

                                if (rnd.Next(0, 2) == 0)
                                    nextValue *= -1;

                                if (currPoint % 2 == 0)
                                    point.X += nextValue;
                                else
                                    point.Y += nextValue;



                                if (currPoint - 1 == _numberOfPoints - 2)
                                {
                                    if (!CheckPointPos(new Vector2d(point.X, polygon[0].Y), polygon) || !CheckPointPos(new Vector2d(point.X, polygon[0].Y), polygon))
                                        continue;
                                }
                                else
                                {
                                    if (!CheckPointPos(point, polygon))
                                        continue;
                                }

                                if (LastAddHaveIntersect(polygon, false) && polygon.Count != 1)
                                {
                                    polygon.Remove(polygon[polygon.Count - 1]);
                                    currPoint--;
                                    continue;
                                }

                                polygon.Add(point);
                            }


                            //еслм нет пересечений выйти
                            if (!LastAddHaveIntersect(polygon, false))
                            {
                                currPoint++;
                                break;
                            }
                            else polygon.Remove(polygon[polygon.Count - 1]);

                            counter++;
                            if (counter > 100)
                            {
                                polygon = new List<Vector2d>();
                                point = new Vector2d(0, 0);
                                polygon.Add(point);
                                prevPoint = polygon[polygon.Count - 1];
                                currPoint = 1;
                                counter = 0;

                            }
                        }                      
                    }

                    //добаляем последнюю точку
                    polygon.Add(new Vector2d(polygon[polygon.Count - 1].X, polygon[0].Y));

                    if (polygon.Count % 2 == 1)
                        throw new ArgumentException("Invalid value: polygon.Count % 2 == 1");

                    //если есть прересечение последнего ребра
                    haveIntersect = LastAddHaveIntersect(polygon, true);
                  
                    if (!haveIntersect)
                        canAdjustment = polygon.AdjustArea(_recomendedArea);//возможно ли отредактировать площадь

                    //продолжить если есть пересечение или невозможно отредактировать площадь
                } while (haveIntersect || !canAdjustment);
               
            }


            return polygon;
        }

        /// <summary>
        /// Проверка на наличие пересечений в полигоне
        /// для последней добавленной вершины без учета соседних ребер
        /// </summary>
        /// <param name="polygon"> Список с вершинами полигона </param>
        /// <param name="withLastEdge"> Флаг нужна ли проверка с послезней першиной </param>
        /// <returns> если есть пересечение true </returns>
        private static bool LastAddHaveIntersect(List<Vector2d> polygon, bool withLastEdge)
        {
            //если больше 3 вершин
            if (polygon.Count > 3)
            {
                //берем последнюю добавленную вершину
                var line1 = new LineSegment2d(polygon[polygon.Count - 2], polygon[polygon.Count - 1]);
                LineSegment2d line2;

                //идем по остальным отрезкам
                for (var j = 0; j < polygon.Count - 2; j++)
                {
                    line2 = new LineSegment2d(polygon[j], polygon[j + 1]);
                    //если это соседняя или эта же вершина
                    if (line1.A.Equals(line2.A) || line1.A.Equals(line2.B) || line1.B.Equals(line2.A) || line1.B.Equals(line2.B))
                        continue;
                    //если есть пересечение отрезков
                    if (LineSegment2d.IsCollide(line1, line2))
                        return true;
                }
                
                //флаг на проверку последнего ребра
                if (withLastEdge)
                {
                    line2 = new LineSegment2d(polygon[0], polygon[polygon.Count - 1]);

                    //провека пересечения последнего и первого ребра и это не сосернее и тоже ребро
                    if (LineSegment2d.IsCollide(line1, line2) && !line1.A.Equals(line2.A) && !line1.A.Equals(line2.B) && !line1.B.Equals(line2.A) && !line1.B.Equals(line2.B))
                        return true;
                }
            }

            return false;

        }
    }
}
