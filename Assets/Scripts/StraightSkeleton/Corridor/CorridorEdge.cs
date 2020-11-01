using StraightSkeleton.Primitives;
using System.Collections.Generic;

namespace StraightSkeleton.Corridor
{
    public interface ICloneable<T>
    {
        T Clone();
    }
    /// <summary>
    /// CorridorEdge структура данных для хранения начала и конца ребра,
    /// а также ее отрезка и уравнения прямой
    /// </summary>
    public class CorridorEdge : ICloneable<CorridorEdge>
    {
        /// <summary> свойство начало ребра </summary>
        public Vector2d Start {
            get
            {
                return _start;
            }
            set {
                segment = new LineSegment2d(value, _end);
                line = new LineLinear2d(value, _end);
                _start = value;
            }
        }
        /// <summary> свойство конец ребра </summary>
        public Vector2d End {
            get
            {
                return _end;
            }
            set
            {
                segment = new LineSegment2d(Start, value);
                line = new LineLinear2d(Start, value);
                _end = value;
            }
        }
        /// <summary> поле конец ребра </summary>
        private Vector2d _start;
        /// <summary> поле конец ребра </summary>
        private Vector2d _end;
        /// <summary> уравнения прямой ребра </summary>
        public LineLinear2d line;
        /// <summary> отрезок прямой ребра </summary>
        public LineSegment2d segment;

        public CorridorEdge(Vector2d start, Vector2d end)
        {          
            _start = start;
            _end = end;
            line = new LineLinear2d(start, end);
            segment = new LineSegment2d(start, end);
        }

        public Vector2d GetNeighborPoint(CorridorEdge edge)
        {
            var comparer = new CorridorEdgeComparer();
            if (comparer.Equals(edge, this))
                return Vector2d.Empty;
            if (_start.Equals(edge._start) || _end.Equals(edge._start))
                return edge._end;
            else if (_start.Equals(edge._end) || _end.Equals(edge._end))
                return edge._start;

            return Vector2d.Empty;
        }

        public CorridorEdge Clone()
        {
            return new CorridorEdge(_start, End);
        }
    }
    public class CorridorEdgeComparer : IEqualityComparer<CorridorEdge>
    {
        public bool Equals(CorridorEdge b1, CorridorEdge b2)
        {
            if (b1.Start.Equals(b2.Start) && b1.End.Equals(b2.End) ||
                 b1.End.Equals(b2.Start) && b1.Start.Equals(b2.End))
                return true;
            else return false;
        }

        public int GetHashCode(CorridorEdge obj)
        {
            return obj.GetHashCode();
        }
    }
}

