using System;

namespace StraightSkeleton.Primitives
{
    /// <summary>
    /// LineSegment - класс для вычисления отрезка прямой
    /// </summary>
    /// <see cref="https://ru.wikipedia.org/wiki/%D0%9E%D1%82%D1%80%D0%B5%D0%B7%D0%BE%D0%BA"/>
    public class LineSegment2d
    {
        /// <summary> первая точка отрезка </summary>
        public Vector2d A;
        /// <summary> вторая точка отрезка </summary>
        public Vector2d B;

        /// <summary>
        ///  Конструктор отрезка
        /// </summary>
        /// <param name="A">первая точка отрезка</param>
        /// <param name="B">вторая точка отрезка</param>
        public LineSegment2d(Vector2d A, Vector2d B)
        {
            this.A = A;
            this.B = B;
        }

        /// <summary>
        ///  Функция обертка пересечения 
        /// </summary>
        /// <param name="segment"> другой отрезок </param>
        /// <returns> если отрезки пересикаются </returns>
        public bool IsCollide(LineSegment2d segment)
        {
            return  Intersect(segment.A, segment.B, A, B);
        }
        /// <summary>
        ///  Статическая функция обертка проверки пересечения 
        /// </summary>
        /// <param name="segmen1"> первый отрезок </param>
        ///  <param name="segment2"> второй отрезок </param>
        /// <returns> если отрезки пересикаются </returns>
        public static bool IsCollide(LineSegment2d segment1, LineSegment2d segment2)
        {
            return Intersect(segment1.A, segment1.B, segment2.A, segment2.B);
        }

        public Vector2d Center()
        {
            return new Vector2d((A.X + B.X) / 2, (A.Y + B.Y) / 2);
        }
        public static Vector2d Center(Vector2d A, Vector2d B)
        {
            return new Vector2d((A.X + B.X) / 2, (A.Y + B.Y) / 2);
        }

        /*
         * (x-x1)/(x2-x1)=(y-y1)/(y2-y1)
            вот уравнение

            тут вместо x и y. Подставляешь координаты искомой точки.
            x1 y1-начало отрезка
            x2 y2-конец отрезка
         */

        #region Contains
        public bool Contains(Vector2d P)
        {
            var line = new LineLinear2d(A, B);


            var vec1 = new Vector2d(A.X - P.X, A.Y - P.Y);
            var vec2 = new Vector2d(B.X - P.X, B.Y - P.Y);
            return line.Contains(P) && vec1.Dot(vec2) <= 0;
        }
    
    #endregion



    /// <summary>
    /// Функция проверки наличия пересечения
    /// </summary>
    /// <param name="a"> первая точка первого отрезка</param>
    /// <param name="b">вторая точка первого отрезка</param>
    /// <param name="c">первая точка второго отрезка</param>
    /// <param name="d">вторая точка второго отрезка</param>
    /// <returns> если отрезки пересикаются </returns>
    private static bool Intersect(Vector2d a, Vector2d b, Vector2d c, Vector2d d)
        {
            double x1 = a.X;
            double y1 = a.Y;
            double x2 = b.X;
            double y2 = b.Y;
            double x3 = c.X;
            double y3 = c.Y;
            double x4 = d.X;
            double y4 = d.Y;

            double Ua, Ub, numerator_a, numerator_b, denominator;
          
            denominator = (y4 - y3) * (x1 - x2) - (x4 - x3) * (y1 - y2);
            if (denominator == 0)
            {
                if ((x1 * y2 - x2 * y1) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x1) == 0 && (x1 * y2 - x2 * y1) * (y4 - y3) - (x3 * y4 - x4 * y3) * (y2 - y1) == 0)
                    return true;
                else return false;
            }
            else
            {
                numerator_a = (x4 - x2) * (y4 - y3) - (x4 - x3) * (y4 - y2);
                numerator_b = (x1 - x2) * (y4 - y2) - (x4 - x2) * (y1 - y2);
                Ua = numerator_a / denominator;
                Ub = numerator_b / denominator;
                return (Ua >= 0 && Ua <= 1 && Ub >= 0 && Ub <= 1 ? true : false);
            }
        }
    }
}
