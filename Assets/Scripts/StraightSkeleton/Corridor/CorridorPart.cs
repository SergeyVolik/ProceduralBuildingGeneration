using StraightSkeleton.Primitives;
using System.Collections.Generic;

namespace StraightSkeleton.Corridor
{
    
    /// <summary>
    /// CorridorPart - структура данных для хранения части коридора.
    /// Тоисть у нас есть коридо в виде дерева, а часть коридора это само ребро и его оболочка
    /// (левая часть, права часть, передняя - если лист дерева, заднее- если корень дерева).
    /// Эти части храняттся в виде списка.
    /// </summary>
    /// 
    public class CorridorPart 
    {
        /// <summary> ребро дерева </summary>
        public CorridorEdge PathEdge { get; set; }
        /// <summary> внешяя часть коридора </summary>
        public List<CorridorEdge> OuterWalls { get; private set; }

        /// <summary>
        ///  Конструктор
        /// </summary>
        /// <param name="start"> начало ребра коридора </param>
        /// <param name="end"> конец ребра коридора </param>
        public CorridorPart(Vector2d start, Vector2d end)
        {
            PathEdge = new CorridorEdge(start, end);          
        }
        public CorridorPart(CorridorEdge edge)
        {
            PathEdge = new CorridorEdge(edge.Start, edge.End);
        }
        /// <summary>
        /// добавление  нового ребра
        /// </summary>
        /// <param name="edge"> ребро внешней стены </param>
        public void AddEdge(CorridorEdge edge)
        {
            if (OuterWalls == null)
                OuterWalls = new List<CorridorEdge>();

            OuterWalls.Add(edge);
        }
        
    }
    
   
}
