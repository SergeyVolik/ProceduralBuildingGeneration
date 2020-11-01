using System.Collections.Generic;
using StraightSkeleton.Primitives;
using System;
using System.Linq;
using UnityEngine;

namespace StraightSkeleton.Polygon
{
    /// <summary>
    /// Triangulator - триангулирует многоугольник 
    /// </summary>
    /// <see cref="https://ru.wikipedia.org/wiki/%D0%97%D0%B0%D0%B4%D0%B0%D1%87%D0%B0_%D0%BE_%D1%82%D1%80%D0%B8%D0%B0%D0%BD%D0%B3%D1%83%D0%BB%D1%8F%D1%86%D0%B8%D0%B8_%D0%BC%D0%BD%D0%BE%D0%B3%D0%BE%D1%83%D0%B3%D0%BE%D0%BB%D1%8C%D0%BD%D0%B8%D0%BA%D0%B0"/>
    public static class Triangulator
    {
        //вершины нашего многоугольника
        private static Vector2d[] points;
        //треугольники, на которые разбит наш многоугольник
        private static Triangle[] triangles;
        //была ли рассмотрена i-ая вершина многоугольника
        private static bool[] taken;
        #region For straight skeleton
        //триангуляция
        public static List<Triangle> Triangulate(List<Vector2d> polygon) 
        {         
            points = MakeCounterClockwise(polygon);

            //ошибка, если не многоугольник
            if (points.Length % 2 == 1 || points.Length < 4)
                throw new Exception(); 
            taken = new bool[points.Length];
            triangles = new Triangle[points.Length - 2];
            int trainPos = 0;
            //сколько осталось рассмотреть вершин
            int leftPoints = points.Length;

            //текущие вершины рассматриваемого треугольника
            int ai = findNextNotTaken(0);
            int bi = findNextNotTaken(ai + 1);
            int ci = findNextNotTaken(bi + 1);

            //количество шагов
            int count = 0;

            //пока не остался один треугольник
            while (leftPoints > 3) 
            {
                //если можно построить треугольник
                if (isLeft(points[ai], points[bi], points[ci]) && canBuildTriangle(ai, bi, ci)) 
                {
                    //новый треугольник
                    triangles[trainPos++] = new Triangle(points[ai], points[bi], points[ci]);
                    //исключаем вершину b
                    taken[bi] = true; 
                    leftPoints--;
                    bi = ci;
                    //берем следующую вершину
                    ci = findNextNotTaken(ci + 1); 
                }
                else
                { 
                    //берем следующие три вершины
                    ai = findNextNotTaken(ai + 1);
                    bi = findNextNotTaken(ai + 1);
                    ci = findNextNotTaken(bi + 1);
                }

                //если по какой-либо причине (например, многоугольник задан по часовой стрелке) триангуляцию провести невозможно, выходим
                if (count > points.Length * points.Length)               
                    return null;                

                count++;
            }

            //если триангуляция была проведена успешно
            if (triangles != null) 
                triangles[trainPos] = new Triangle(points[ai], points[bi], points[ci]);

            return triangles.ToList();
        }
        private static Vector2d[] MakeCounterClockwise(List<Vector2d> polygon)
        {
            if (!PrimitiveUtils.IsClockwisePolygon(polygon))           
                polygon.Reverse();

            return polygon.ToArray();
        }

        //найти следущую нерассмотренную вершину
        private static int findNextNotTaken(int startPos) 
        {
            startPos %= points.Length;
            if (!taken[startPos])
                return startPos;

            int i = (startPos + 1) % points.Length;
            while (i != startPos)
            {
                if (!taken[i])
                    return i;
                i = (i + 1) % points.Length;
            }

            return -1;
        }

        //левая ли тройка векторов
        private static bool isLeft(Vector2d a, Vector2d b, Vector2d c) 
        {
            double abX = b.X - a.X;
            double abY = b.Y - a.Y;
            double acX = c.X - a.X;
            double acY = c.Y - a.Y;

            return abX * acY - acX * abY < 0;
        }

        //находится ли точка p внутри треугольника abc
        private static bool isPointInside(Vector2d a, Vector2d b, Vector2d c, Vector2d p) 
        {
            double ab = (a.X - p.X) * (b.Y - a.Y) - (b.X - a.X) * (a.Y - p.Y);
            double bc = (b.X - p.X) * (c.Y - b.Y) - (c.X - b.X) * (b.Y - p.Y);
            double ca = (c.X - p.X) * (a.Y - c.Y) - (a.X - c.X) * (c.Y - p.Y);

            return (ab >= 0 && bc >= 0 && ca >= 0) || (ab <= 0 && bc <= 0 && ca <= 0);
        }


        //false - если внутри есть вершина
        private static bool canBuildTriangle(int ai, int bi, int ci) 
        {
            //рассмотрим все вершины многоугольника
            for (int i = 0; i < points.Length; i++)
                if (i != ai && i != bi && i != ci) //кроме троих вершин текущего треугольника
                    if (isPointInside(points[ai], points[bi], points[ci], points[i]))
                        return false;
            return true;
        }
        #endregion
        #region For unity mesh
        public static int[]  Triangulate<Mesh>(Vector2d[] points)
        {
            List<int> indices = new List<int>();
            var m_points = points.ToList();

            int n = m_points.Count;
            if (n < 3)
                return indices.ToArray();

            int[] V = new int[n];
            if (Area(m_points) > 0)
            {
                for (int v = 0; v < n; v++)
                    V[v] = v;
            }
            else
            {
                for (int v = 0; v < n; v++)
                    V[v] = (n - 1) - v;
            }

            int nv = n;
            int count = 2 * nv;
            for (int v = nv - 1; nv > 2;)
            {
                if ((count--) <= 0)
                    return indices.ToArray();

                int u = v;
                if (nv <= u)
                    u = 0;
                v = u + 1;
                if (nv <= v)
                    v = 0;
                int w = v + 1;
                if (nv <= w)
                    w = 0;

                if (Snip(u, v, w, nv, V, m_points))
                {
                    int a, b, c, s, t;
                    a = V[u];
                    b = V[v];
                    c = V[w];
                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(c);
                    for (s = v, t = v + 1; t < nv; s++, t++)
                        V[s] = V[t];
                    nv--;
                    count = 2 * nv;
                }
            }

            indices.Reverse();
            return indices.ToArray();
        }

        private static double Area(List<Vector2d> m_points)
        {
            int n = m_points.Count;
            double A = 0.0d;
            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector2d pval = m_points[p];
                Vector2d qval = m_points[q];
                A += pval.X * qval.Y - qval.X * pval.Y;
            }
            return (A * 0.5d);
        }

        private static bool Snip(int u, int v, int w, int n, int[] V, List<Vector2d> m_points)
        {
            int p;
            Vector2d A = m_points[V[u]];
            Vector2d B = m_points[V[v]];
            Vector2d C = m_points[V[w]];
            if (Mathf.Epsilon > (((B.X - A.X) * (C.Y - A.Y)) - ((B.Y - A.Y) * (C.X - A.Y))))
                return false;
            for (p = 0; p < n; p++)
            {
                if ((p == u) || (p == v) || (p == w))
                    continue;
                Vector2d P = m_points[V[p]];
                if (InsideTriangle(A, B, C, P))
                    return false;
            }
            return true;
        }

        private static bool InsideTriangle(Vector2d A, Vector2d B, Vector2d C, Vector2d P)
        {
            double ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
            double cCROSSap, bCROSScp, aCROSSbp;

            ax = C.X - B.X; ay = C.Y - B.Y;
            bx = A.X - C.X; by = A.Y - C.Y;
            cx = B.X - A.X; cy = B.Y - A.Y;
            apx = P.X - A.X; apy = P.Y - A.Y;
            bpx = P.X - B.X; bpy = P.Y - B.Y;
            cpx = P.X - C.X; cpy = P.Y - C.Y;

            aCROSSbp = ax * bpy - ay * bpx;
            cCROSSap = cx * apy - cy * apx;
            bCROSScp = bx * cpy - by * cpx;

            return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
        }
        #endregion

    }

    public class Triangle //треугольник
    {
        private Vector2d a, b, c;

        public Triangle(Vector2d a, Vector2d b, Vector2d c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Vector2d getA()
        {
            return a;
        }

        public Vector2d getB()
        {
            return b;
        }

        public Vector2d getC()
        {
            return c;
        }

        //Третий способ. Чтобы найти площадь треугольника , если известны длины всех его трех сторон ,  и , нужно воспользоваться формулой Герона:
        public double GetArea()
        {

            double aEdge = this.a.DistanceTo(b);
            double bEdge = this.b.DistanceTo(c);
            double cEdge = this.c.DistanceTo(a);

            double p = (aEdge + bEdge + cEdge) / 2;

            return Math.Sqrt(p * (p - aEdge) * (p - bEdge) * (p - cEdge));
        }

       

        
    }
   
}
