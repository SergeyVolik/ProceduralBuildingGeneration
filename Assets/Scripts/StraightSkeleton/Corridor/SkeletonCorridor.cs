using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using StraightSkeleton;

namespace StraightSkeleton.Corridor
{
    public enum OutputType { Random = 4, One = 1, Two = 2, Max = 3, None=0 }
    public enum CorridorWidth { One = 1, Two = 2, Three = 3 }
    public class SkeletonCorridor
    {
        /// <summary> скелет коридора </summary>
        private List<CorridorEdge> skeletOfCorridor;
        /// <summary> коридор в виде дерева </summary>
        public SkeletonTree<CorridorPart> treeOfCorridor;
        /// <summary> ширина коридора  </summary>
        public List<Vector2d> polygonOfCorridor;

        public double _corridorWidth;
        public OutputType _outputType;
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="sr"> прямой скелет для постоения крыши </param>
        /// <param name="corridorWidth"> ширина коридора </param>
        public SkeletonCorridor(SkeletonRoofResult sr, CorridorWidth corridorWidth, OutputType outputType)
        {
            skeletOfCorridor = new List<CorridorEdge>();
            polygonOfCorridor = new List<Vector2d>();             
            _corridorWidth = (double)corridorWidth;
            _outputType = outputType;
            FindCorridor(sr);
            
            CreateExit(sr);

            CreateTree();
            GetRectangularCorridor();



            CreateOuterShell(treeOfCorridor.root, null);

            ToList(treeOfCorridor.root);
        }
        public SkeletonCorridorResult GetCorridorResult()
        {
            return new SkeletonCorridorResult(polygonOfCorridor);
        }

        /// <summary>
        /// функция проверяет лежит ли эта точка на концевом ребре
        /// </summary>
        /// <param name="point"> точка для проверки являеться ли она концевой</param>
        /// <returns> true если являеться концевой точкой </returns>
        private bool IsEndPoint(Vector2d point)
        {
            int counter = 0;
            for (var i = 0; i < skeletOfCorridor.Count; i++)
            {
                if (point.Equals(skeletOfCorridor[i].Start) || point.Equals(skeletOfCorridor[i].End))
                    counter++;
                if (counter > 1)
                    return false;

            }
            return true;
        }
        
        /// <summary>
        /// Функция в которой осуществляется поиск ребер которые пренадлежат коридору 
        /// (у которых нет общих точек с внешними ребрами)
        /// </summary>
        /// <param name="sr"> прямой скелет для постоения крыши </param>
        private void FindCorridor(SkeletonRoofResult sr)
        {
         
            for (var i = 0; i < sr.Edges.Count; i++)
            {
                var polygon = sr.Edges[i];

                Func<CorridorEdge, bool> checkEdges = (CorridorEdge Edge) =>
                {
                    if (!polygon.Edge.Begin.Equals(Edge.Start) &&
                        !polygon.Edge.Begin.Equals(Edge.End) &&
                        !polygon.Edge.End.Equals(Edge.End) &&
                        !polygon.Edge.End.Equals(Edge.Start) &&
                        !skeletOfCorridor.Contains(Edge, new CorridorEdgeComparer())
                     )
                        return true;
                    return false;
                };
               
                CorridorEdge currEdge;

                for (var j = 0; j < sr.Edges[i].Polygon.Count - 1; j++)
                {

                    //currEdge = new CorridorEdge(CorretiveCorridorCoordinate(polygon.Polygon[j]), CorretiveCorridorCoordinate(polygon.Polygon[j+1]));
                    currEdge = new CorridorEdge(polygon.Polygon[j], polygon.Polygon[j + 1]);
                    if (checkEdges(currEdge))
                        skeletOfCorridor.Add(new CorridorEdge(currEdge.Start, currEdge.End));
                    
                }
                currEdge = new CorridorEdge(polygon.Polygon[0], polygon.Polygon[polygon.Polygon.Count - 1]);
                //currEdge = new CorridorEdge(CorretiveCorridorCoordinate(polygon.Polygon[0]), CorretiveCorridorCoordinate(polygon.Polygon[polygon.Polygon.Count - 1]));
                if (checkEdges(currEdge))
                    skeletOfCorridor.Add(new CorridorEdge(currEdge.Start, currEdge.End));
                
            }


                 


        }
       


        #region Add exits to corridor
        /// <summary>
        /// функция продлевает концевое ребро к внешнему ребру
        /// </summary>
        /// <param name="sr"> прямой скелет для постоения крыши </param>
        private void CreateExit(SkeletonRoofResult sr)
        {
            switch(_outputType)
            {
                case OutputType.One:
                    CreateOneExit(sr);
                    break;
                case OutputType.Two:
                    CreateTwoExits(sr);
                    break;
                case OutputType.Random:
                    CreateRandomNumberOfExits(sr);
                    break;
                case OutputType.Max:
                    CreateMaxExits(sr);
                    break;
                case OutputType.None:
                    break;

            }
            
        }
        private void CreateOneExit(SkeletonRoofResult sr)
        {
            System.Random rnd = new System.Random();

            var indexes = GetListOfEndPointIndexes();
            var randomOutputs = new List<int>();
            
            for (int i = 0; i < 1; i++)
            {
                var output = indexes[rnd.Next(0, indexes.Count)];
                indexes.Remove(output);
                randomOutputs.Add(output);
            }
            AddExits(randomOutputs, sr);
        }
        private void CreateTwoExits(SkeletonRoofResult sr)
        {
            System.Random rnd = new System.Random();

            var indexes = GetListOfEndPointIndexes();
            var randomOutputs = new List<int>();
            
            for (int i = 0; i < 2; i++)
            {
                var output = indexes[rnd.Next(0, indexes.Count)];
                indexes.Remove(output);
                randomOutputs.Add(output);
            }
            AddExits(randomOutputs, sr);
        }
        private void CreateRandomNumberOfExits(SkeletonRoofResult sr)
        {
            System.Random rnd = new System.Random();
            
            var indexes = GetListOfEndPointIndexes();
            var randomOutputs = new List<int>();
            var numberOfOutputs = rnd.Next(0, indexes.Count);
            for (int i = 0; i < numberOfOutputs; i++)
            {
                var output = indexes[rnd.Next(0, indexes.Count)];
                indexes.Remove(output);
                randomOutputs.Add(output);
            }
            AddExits(randomOutputs, sr);
        }
        private void CreateMaxExits(SkeletonRoofResult sr)
        {
            var indexes = GetListOfEndPointIndexes();
            AddExits(indexes, sr);

        }
        private void AddExits(List<int> indexes, SkeletonRoofResult sr)
        {
            for (var i = 0; i < indexes.Count; i++)
            {
                if (IsEndPoint(skeletOfCorridor[indexes[i]].Start))
                    skeletOfCorridor[indexes[i]].Start = ExitEdgeIntersect(sr, skeletOfCorridor[indexes[i]], skeletOfCorridor[indexes[i]].Start);

                else if (IsEndPoint(skeletOfCorridor[indexes[i]].End))
                    skeletOfCorridor[indexes[i]].End = ExitEdgeIntersect(sr, skeletOfCorridor[indexes[i]], skeletOfCorridor[indexes[i]].End);

            }
        }
        private List<int> GetListOfEndPointIndexes()
        {
            var indexes = new List<int>();

            for (var i = 0; i < skeletOfCorridor.Count; i++)
            
                if (IsEndPoint(skeletOfCorridor[i].Start) || IsEndPoint(skeletOfCorridor[i].End))
                    indexes.Add(i);
 
            return indexes;
        }

        /// <summary>
        /// поиск ближайшей точки пересечения в внешними ребрами
        /// </summary>
        /// <param name="sr"> прямой скелет для постоения крыши</param>
        /// <param name="edge"> ребро которое мы будем продлевать </param>
        /// <param name="pointForDistance"> точка ребра которая есть конечной</param>
        /// <returns></returns>
        Vector2d ExitEdgeIntersect(SkeletonRoofResult sr, CorridorEdge edge, Vector2d pointForDistance)
        {
            var intersections = new Dictionary<double, Vector2d>();
            var distances = new List<double>();
            CorridorEdge longEdge;
            CorridorEdge prevgEdge;

            if (Math.Round(edge.Start.X,5) == Math.Round(edge.End.X,5))
            {
                longEdge = new CorridorEdge(new Vector2d(edge.Start.X, -10000), new Vector2d(edge.End.X, 10000));
                prevgEdge = new CorridorEdge(new Vector2d(-10000, edge.Start.Y), new Vector2d(10000, edge.End.Y));
            }
            else
            {
                longEdge = new CorridorEdge(new Vector2d(-10000, edge.Start.Y), new Vector2d(10000, edge.End.Y));
                prevgEdge = new CorridorEdge(new Vector2d(edge.Start.X, -10000), new Vector2d(edge.End.X, 10000));
            }

            for (var i = 0; i < sr.Edges.Count; i++)
            {
                var segment = new LineSegment2d(sr.Edges[i].Edge.Begin, sr.Edges[i].Edge.End);
                var line = new LineLinear2d(sr.Edges[i].Edge.Begin, sr.Edges[i].Edge.End);
        
                if (segment.IsCollide(longEdge.segment) && !line.Colinear(longEdge.line))
                {
                    var collideValue = line.Collide(longEdge.line);
                    var distance = pointForDistance.DistanceTo(collideValue);
                    if (!distances.Contains(distance))
                    {
                        distances.Add(distance);
                        intersections.Add(distance, collideValue);
                    }
                }
            }
           
            return intersections[distances.Min()];

        }
        #endregion

        #region Creating corridor tree
        /// <summary>
        /// Создание дерева для коридора
        /// </summary>
        private void CreateTree()
        {          
            var corridorParts = new List<CorridorPart>();
            for (var i = 0; i < skeletOfCorridor.Count; i++)
                corridorParts.Add(new CorridorPart(skeletOfCorridor[i]));

            treeOfCorridor = new SkeletonTree<CorridorPart>();
            for (var i = 0; i < corridorParts.Count; i++)
            {
                if (IsEndPoint(corridorParts[i].PathEdge.Start))
                {
                    treeOfCorridor.root.data = corridorParts[i];
                    corridorParts.Remove(corridorParts[i]);
                    break;
                }
                else if (IsEndPoint(corridorParts[i].PathEdge.End))
                {
                    var temp = corridorParts[i].PathEdge.Start;
                    corridorParts[i].PathEdge.Start = corridorParts[i].PathEdge.End;
                    corridorParts[i].PathEdge.End = temp;
                    treeOfCorridor.root.data = corridorParts[i];
                    corridorParts.Remove(corridorParts[i]);
                    break;
                }
            }
            
            InsertNode(treeOfCorridor.root, corridorParts);
        }
        /// <summary>
        /// Добавление след узла дерева
        /// </summary>
        /// <param name="node"></param>
        /// <param name="corridorLocal"></param>
        private void InsertNode(Node<CorridorPart> node, List<CorridorPart> corridorLocal)
        {
            var removeList = new List<CorridorPart>();
            for (var i = 0; i < corridorLocal.Count; i++)
            {
                if (node.data.PathEdge.End.Equals(corridorLocal[i].PathEdge.Start))
                {
                    node.AddChild(corridorLocal[i]);
                    removeList.Add(corridorLocal[i]);                 
                }
                else if (node.data.PathEdge.End.Equals(corridorLocal[i].PathEdge.End) || node.data.PathEdge.Start.Equals(corridorLocal[i].PathEdge.Start))
                {
                    var temp = corridorLocal[i].PathEdge.Start;
                    corridorLocal[i].PathEdge.Start = corridorLocal[i].PathEdge.End;
                    corridorLocal[i].PathEdge.End = temp;
                    node.AddChild(corridorLocal[i]);
                    removeList.Add(corridorLocal[i]);
                  
                }              
            }

            for (var i = 0; i < removeList.Count; i++)           
                corridorLocal.Remove(removeList[i]);
            
            if (node.nodes != null)           
                for (var i = 0; i < node.nodes.Count; i++)             
                    InsertNode(node.nodes[i], corridorLocal);
                          
        }

        /// <summary>
        /// Функция для получения прямоугольного коридора, необходима при наличии косых ребер.
        /// </summary>
        private void GetRectangularCorridor()
        {
            var edge = treeOfCorridor.root.data.PathEdge;
            var line = new LineLinear2d(new Vector2d(0, 0), new Vector2d(1, 0));
            var colinear = line.Colinear(edge.line);
            var orthogonal = line.Orthogonal(edge.line);

            if (!colinear && !orthogonal)
            {
                var oldRoot = treeOfCorridor.root;
                var line1 = LineLinear2d.OrthogonalLineFromOtherLineDirection(line.direction, edge.End);
                line1 = LineLinear2d.OrthogonalLineFromOtherLineDirection(line1.direction, edge.End);
                var line2 = LineLinear2d.OrthogonalLineFromOtherLineDirection(line.direction, edge.Start);

                var point = line1.Collide(line2);

                var newRoot = new Node<CorridorPart>() { data = new CorridorPart(edge.Start, point) };
                newRoot.AddChild(oldRoot);
                skeletOfCorridor.Add(newRoot.data.PathEdge);
                oldRoot.data.PathEdge = new CorridorEdge(point, edge.End);
                treeOfCorridor.root = newRoot;

            }

            NextNodeRectangularCorridor(treeOfCorridor.root);
        }
        /// <summary>
        ///  Функция для получения прямоугольного коридора, необходима при наличии косых ребер.
        /// </summary>
        /// <param name="part"></param>
        private void NextNodeRectangularCorridor(Node<CorridorPart> part)
        {
            var removeList = new List<Node<CorridorPart>>();
            if (part.nodes != null)
            {
                for (var i = 0; i < part.nodes.Count; i++)
                {
                    var colinear = part.data.PathEdge.line.Colinear(part.nodes[i].data.PathEdge.line);
                    var orthogonal = part.data.PathEdge.line.Orthogonal(part.nodes[i].data.PathEdge.line);
                    if (!colinear && !orthogonal)
                    {
                        var line1 = LineLinear2d.OrthogonalLineFromOtherLineDirection(part.data.PathEdge.line.direction, part.nodes[i].data.PathEdge.End);
                        line1 = LineLinear2d.OrthogonalLineFromOtherLineDirection(line1.direction, part.nodes[i].data.PathEdge.End);
                        var line2 = LineLinear2d.OrthogonalLineFromOtherLineDirection(part.data.PathEdge.line.direction, part.data.PathEdge.End);

                        var point = line1.Collide(line2);

                        var newNode = new Node<CorridorPart>() { data = new CorridorPart(part.data.PathEdge.End, point) };
                        part.AddChild(newNode);
                        newNode.AddChild(part.nodes[i]);
                        skeletOfCorridor.Add(newNode.data.PathEdge);
                        part.nodes[i].data.PathEdge = new CorridorEdge(point, part.nodes[i].data.PathEdge.End);

                        removeList.Add(part.nodes[i]);


                    }
                }
                for (var i = 0; i < removeList.Count; i++)               
                    part.nodes.Remove(removeList[i]);
                
                for (var i = 0; i < part.nodes.Count; i++)               
                    NextNodeRectangularCorridor(part.nodes[i]);
                
            }
        }

        #endregion

        #region Creating outer shell

        /// <summary>
        /// создание внешней оболочки для коридора
        /// </summary>
        /// <param name="node"></param>
        /// <param name="prevNode"></param>
        private void CreateOuterShell(Node<CorridorPart> node, Node<CorridorPart> prevNode)
        {
            if (prevNode == null)
                AddFirstPart(node);
            else
            {
                if (node.nodes != null)
                    AddNextPart(node, prevNode);
                else AddLastPart(node, prevNode);
            }
        }           

        /// <summary>
        /// Определяет по какой координате изменять след точку. (по Х или по Y)
        /// </summary>
        /// <param name="last1"></param>
        /// <param name="last2"></param>
        /// <returns></returns>
        bool ChangeXOrY(Vector2d last1, Vector2d last2)
        {
            if (last1.X == last2.X)
                return true;
            else return false;
        }
        

        #region AddFirstPart

        /// <summary>
        /// добавляет для первого (root) узла бокорые и стартовую стену
        /// </summary>
        /// <param name="node"></param>
        private void AddFirstPart(Node<CorridorPart> node)
        {


            var part = node.data;
            var currEdge = part.PathEdge;

            var edges = CreateColinearEdges(!ChangeXOrY(currEdge.Start, currEdge.End), currEdge);
            var edge1 = edges.edge1;
            var edge2 = edges.edge2;
            var line1 = currEdge.line;
            var line2 = edges.edge1.line;

            if (line2.Colinear(line1) && !line2.Contains(currEdge.Start))
            {
                CorrectionEdgePointEnd(edge1, edge2, node, ChangeXOrY(currEdge.Start, currEdge.End), _corridorWidth / 2);
            }
            else
            {
                edges = CreateColinearEdges(ChangeXOrY(currEdge.Start, currEdge.End), currEdge);
                edge1 = edges.edge1;
                edge2 = edges.edge2;
                CorrectionEdgePointEnd(edge1, edge2, node, !ChangeXOrY(currEdge.Start, currEdge.End), _corridorWidth / 2);

            }

            
            part.AddEdge(edges.edge1);
            part.AddEdge(new CorridorEdge(edge1.Start, edge2.Start));
            part.AddEdge(edges.edge2);
           

            if (node.nodes != null)
                for(var i = 0; i < node.nodes.Count; i++)
                CreateOuterShell(node.nodes[i], node);
            else
            {
                part.AddEdge(new CorridorEdge(edge1.End, edge2.End));
            }
        }

        
        /// <summary>
        /// Функция создает паралельные линии с маленьким шагом, 
        /// необходима если след линия слишком короткая и отсутствует пересечение  
        /// </summary>
        /// <param name="x"> по какой координате сдвигать</param>
        /// <param name="currEdge"> текущие ребро</param>
        /// <returns> возвращает два фейковых ребра </returns>
        private (CorridorEdge fakeEdge1, CorridorEdge fakeEdge2) CreateFakeEdges(bool x, CorridorEdge currEdge)
        {
            CorridorEdge fakeEdge1;
            CorridorEdge fakeEdge2;
            if (!x)
            {
                fakeEdge2 = new CorridorEdge(
                         new Vector2d(currEdge.Start.X - 0.1f, currEdge.Start.Y),
                         new Vector2d(currEdge.End.X - 0.1f, currEdge.End.Y)
                     );
                fakeEdge1 = new CorridorEdge(
                   new Vector2d(currEdge.Start.X + 0.1f, currEdge.Start.Y),
                     new Vector2d(currEdge.End.X + 0.1f, currEdge.End.Y)
              );
            }
            else
            {
                fakeEdge2 = new CorridorEdge(
                       new Vector2d(currEdge.Start.X, currEdge.Start.Y - 0.1f),
                       new Vector2d(currEdge.End.X, currEdge.End.Y - 0.1f)
                   );
                fakeEdge1 = new CorridorEdge(
                   new Vector2d(currEdge.Start.X, currEdge.Start.Y + 0.1f),
                     new Vector2d(currEdge.End.X, currEdge.End.Y + 0.1f)
                    );
            }

            return (fakeEdge1, fakeEdge2);
        }

        /// <summary>
        /// создает две прямые равно удаленные от заданой на определенную длинну
        /// </summary>
        /// <param name="x"> по какой координате сдвиг</param>
        /// <param name="currEdge"> текущие ребро</param>
        /// <returns> тюпл из 2 равноудаленных ребер</returns>
        private (CorridorEdge edge1, CorridorEdge edge2) CreateColinearEdges(bool x, CorridorEdge currEdge)
        {
            CorridorEdge edge1;
            CorridorEdge edge2;
            if (x)
            {
                edge1 = new CorridorEdge(
                    new Vector2d(currEdge.Start.X + _corridorWidth / 2, currEdge.Start.Y),
                    new Vector2d(currEdge.End.X + _corridorWidth / 2, currEdge.End.Y)
                );
                edge2 = new CorridorEdge(
                        new Vector2d(currEdge.Start.X - _corridorWidth / 2, currEdge.Start.Y),
                        new Vector2d(currEdge.End.X - _corridorWidth / 2, currEdge.End.Y)
                    );
            }
            else
            {
                edge1 = new CorridorEdge(
                   new Vector2d(currEdge.Start.X, currEdge.Start.Y + _corridorWidth / 2),
                   new Vector2d(currEdge.End.X, currEdge.End.Y + _corridorWidth / 2)
               );
                edge2 = new CorridorEdge(
                   new Vector2d(currEdge.Start.X, currEdge.Start.Y - _corridorWidth / 2),
                   new Vector2d(currEdge.End.X, currEdge.End.Y - _corridorWidth / 2)
               );
            }

            return (edge1, edge2);
        }

        /// <summary>
        /// функция ищит ближайшее ребро из двух к заданой точке
        /// </summary>
        /// <param name="edge1" > 1 ребро</param>
        /// <param name="edge2"> 2 ребро</param>
        /// <param name="fakes"> фейковые ребра 1,2 </param>
        /// <param name="point"> точка от которой мы ищим ближайщее ребро</param>
        /// <returns> тюпл (дальнее ребро, ближ ребро, дальний фейк, ближ фейк)</returns>
        private (CorridorEdge far, CorridorEdge nearest, CorridorEdge farFake, CorridorEdge nearestFake)
            TakeFarAndNearestEdges(CorridorEdge edge1, CorridorEdge edge2, (CorridorEdge fakeEdge1, CorridorEdge fakeEdge2) fakes, Vector2d point)
        {

            var dist1 = edge1.End.DistanceTo(point);
            var dist2 = edge2.End.DistanceTo(point);

            var far = edge2;
            var nearest = edge1;
            var farFake = fakes.fakeEdge2;
            var nearestFake = fakes.fakeEdge1;
            if (dist1 > dist2)
            {
                far = edge1;
                nearest = edge2;
                farFake = fakes.fakeEdge1;
                nearestFake = fakes.fakeEdge2;
            }

            return (far, nearest, farFake, nearestFake);
        }

        #endregion

        #region AddNextPart

        /// <summary>
        /// добавление внешних стен для следующих елем дерева
        /// </summary>
        /// <param name="node"> текущий узел</param>
        /// <param name="prevNode"> прошлый узел</param>
        private void AddNextPart(Node<CorridorPart> node, Node<CorridorPart> prevNode)
        {
            var part = node.data;
            var currEdge = part.PathEdge;

            var edges = CreateColinearEdges(!ChangeXOrY(currEdge.Start, currEdge.End), currEdge);
            var edge1 = edges.edge1;
            var edge2 = edges.edge2;
            var prevEdge = prevNode.data.PathEdge;


            bool x;
            double distance = _corridorWidth / 2;
            if (edge1.line.Colinear(currEdge.line) && !edge1.line.Contains(currEdge.Start))
            {
                x = ChangeXOrY(currEdge.Start, currEdge.End);
                CorrectionEdgePointStart(edge1, edge2, prevNode, node, x, distance);
                CorrectionEdgePointEnd(edge1, edge2, node, x, distance);
               
            }
            else
            {
                edges = CreateColinearEdges(ChangeXOrY(currEdge.Start, currEdge.End), currEdge);
                edge1 = edges.edge1;
                edge2 = edges.edge2;
                x = !ChangeXOrY(currEdge.Start, currEdge.End);
                CorrectionEdgePointStart(edge1, edge2, prevNode, node, x, distance);
                CorrectionEdgePointEnd(edge1, edge2, node, x, distance);
            }

            part.AddEdge(edges.edge1);
            part.AddEdge(edges.edge2);

            if (node.nodes != null)
            {
                for(var i = 0; i < node.nodes.Count; i++)
                CreateOuterShell(node.nodes[i], node);
            }
        }



        #endregion

        #region AddLastPart
        /// <summary>
        /// добавляет последнему конечному узлу (листу дерева) стену выход и боковые
        /// </summary>
        /// <param name="node"> текущий узел</param>
        /// <param name="prevNode"> прошлый узел</param>
        private void AddLastPart(Node<CorridorPart> node, Node<CorridorPart> prevNode)
        {
            var part = node.data;
            var currEdge = part.PathEdge;
            var prevEdge = prevNode.data.PathEdge;


            var edges = CreateColinearEdges(!ChangeXOrY(currEdge.Start, currEdge.End), currEdge);
            var edge1 = edges.edge1;
            var edge2 = edges.edge2;
            var line1 = currEdge.line;
            var line2 = edges.edge1.line;


            if (line2.Colinear(line1) && !line2.Contains(currEdge.Start))
            {               
                CorrectionEdgePointStart(edge1, edge2, prevNode, node, ChangeXOrY(currEdge.Start, currEdge.End), _corridorWidth / 2);
            }
            else
            {
                edges = CreateColinearEdges(ChangeXOrY(currEdge.Start, currEdge.End), currEdge);
                edge1 = edges.edge1;
                edge2 = edges.edge2;
               
                CorrectionEdgePointStart(edge1, edge2, prevNode, node, !ChangeXOrY(currEdge.Start, currEdge.End), _corridorWidth / 2);
            }

            part.AddEdge(edges.edge1);
            part.AddEdge(new CorridorEdge(edge1.End, edge2.End));
            part.AddEdge(edges.edge2);
           


        }


        #endregion
        /// <summary>
        ///  коректирование end точки для ребра
        /// </summary>
        /// <param name="edge1"> превое паралельное ребро к текущему</param>
        /// <param name="edge2">второе паралельное ребро к текущему</param>
        /// <param name="currNode"> текущий узел дерева</param>
        /// <param name="x"> по какой координате изменяем</param>
        /// <param name="distance"> шаг сдвига </param>
        private void CorrectionEdgePointEnd(CorridorEdge edge1, CorridorEdge edge2, Node<CorridorPart> currNode, bool x, double distance)
        {
            var nextParts = currNode.nodes;
            var currEdge = currNode.data.PathEdge;

            var fakes = CreateFakeEdges(x, currEdge);
            if (nextParts != null)
            {
                if (nextParts.Count == 1)
                {

                    var NextEdge = nextParts[0].data.PathEdge;

                    if (NextEdge.line.Orthogonal(currEdge.line))
                    {
                        var nearestAndFar = TakeFarAndNearestEdges(edge1, edge2, fakes, NextEdge.End);

                        var nearestEnd = nearestAndFar.nearest.End;
                        var farEnd = nearestAndFar.far.End;

                        ChangePointPosition(NextEdge, nearestAndFar.nearestFake, ref nearestEnd, ref farEnd, distance, x, true);

                        nearestAndFar.nearest.End = nearestEnd;
                        nearestAndFar.far.End = farEnd;
                    }


                }
                else if (nextParts.Count > 1)
                {
                    for (var i = 0; i < nextParts.Count; i++)
                    {
                        var NextEdge = nextParts[i].data.PathEdge;
                        if (NextEdge.line.Orthogonal(currEdge.line))
                        {
                            var nearestAndFar = TakeFarAndNearestEdges(edge1, edge2, fakes, NextEdge.End);

                            var nearestEnd = nearestAndFar.nearest.End;
                            var farEnd = nearestAndFar.far.End;

                            ChangePointPosition(NextEdge, nearestAndFar.nearestFake, ref nearestEnd, ref farEnd, distance, x, true);

                            nearestAndFar.nearest.End = nearestEnd;
                        }
                    }
                }

            }

        }
        /// <summary>
        /// коректирование start точки для ребра
        /// </summary>
        /// <param name="edge1"> превое паралельное ребро к текущему</param>
        /// <param name="edge2">второе паралельное ребро к текущему</param>
        /// <param name="currNode"> текущий узел дерева</param>
        /// <param name="x"> по какой координате изменяем</param>
        /// <param name="distance"> шаг сдвига </param>
        /// <param name="prevNode"> прошлый узел дерева</param>      
        private void CorrectionEdgePointStart(CorridorEdge edge1, CorridorEdge edge2, Node<CorridorPart> prevNode, Node<CorridorPart> currNode, bool x, double distance)
        {
            var nextParts = prevNode.nodes;
            var currEdge = currNode.data.PathEdge;
            CorridorEdge nextEdge;
            var fakes = CreateFakeEdges(x, currEdge);

            if (nextParts.Count - 1 == 0)
            {
                nextEdge = prevNode.data.PathEdge;
                if (nextEdge.line.Orthogonal(currEdge.line))
                {


                    var FirstPoint = edge1.Start;
                    var SecondPoint = edge2.Start;

                    ChangePointPosition(nextEdge, fakes.fakeEdge1, ref FirstPoint, ref SecondPoint, distance, x, false);

                    edge1.Start = FirstPoint;
                    edge2.Start = SecondPoint;


                    if (edge2.segment.IsCollide(nextEdge.segment))
                    {
                        ChangePointPosition(nextEdge, fakes.fakeEdge1, ref SecondPoint, ref FirstPoint, distance, x, false);
                        ChangePointPosition(nextEdge, fakes.fakeEdge1, ref SecondPoint, ref FirstPoint, distance, x, false);

                        edge1.Start = FirstPoint;
                        edge2.Start = SecondPoint;
                    }
                }

            }
            else if (nextParts.Count > 1)
            {
                for (var i = 0; i < nextParts.Count; i++)
                {
                    nextEdge = nextParts[i].data.PathEdge;
                    if (nextEdge == currEdge)
                        continue;
                    if (nextEdge.line.Orthogonal(currEdge.line))
                    {
                        var nearestAndFar = TakeFarAndNearestEdges(edge1, edge2, fakes, nextEdge.End);

                        var nearestEnd = nearestAndFar.nearest.Start;
                        var farEnd = nearestAndFar.far.Start;

                        ChangePointPosition(nextEdge, nearestAndFar.nearestFake, ref nearestEnd, ref farEnd, distance, x, false);

                        nearestAndFar.nearest.Start = nearestEnd;

                    }

                }
                nextEdge = prevNode.data.PathEdge;

                if (nextEdge.line.Orthogonal(currEdge.line))
                {
                    var nearestAndFar = TakeFarAndNearestEdges(edge1, edge2, fakes, nextEdge.Start);

                    var nearestEnd = nearestAndFar.nearest.Start;
                    var farEnd = nearestAndFar.far.Start;

                    ChangePointPosition(nextEdge, nearestAndFar.nearestFake, ref nearestEnd, ref farEnd, distance, x, false);

                    nearestAndFar.nearest.Start = nearestEnd;

                }
            }
        }
        /// <summary>
        ///  функция которая непосредственной изменяет координаты ребер
        /// </summary>
        /// <param name="otherEdge"> ребро след или прошлое от текущего</param>
        /// <param name="nearestFake"> ближ фейковое ребро</param>
        /// <param name="nearestPoint">точка ближ ребра </param>
        /// <param name="farPoint"> точка дальнего ребра</param>
        /// <param name="distance"> длинна сдвига</param>
        /// <param name="x"> пока какой координате сдвиг</param>
        /// <param name="ChangeEnd"> изменяем end точку или start</param>
        private void ChangePointPosition(CorridorEdge otherEdge, CorridorEdge nearestFake, ref Vector2d nearestPoint, ref Vector2d farPoint, double distance, bool x, bool ChangeEnd)
        {
            LineSegment2d segment;

            if (!x)
            {
                if(ChangeEnd)
                    segment = new LineSegment2d(nearestFake.Start, new Vector2d(nearestFake.End.X, nearestFake.End.Y + distance));
                else segment = new LineSegment2d(new Vector2d(nearestFake.Start.X, nearestFake.Start.Y + distance), nearestFake.End);


                if (segment.IsCollide(otherEdge.segment))
                {
                    nearestPoint.Y -= distance;
                    farPoint.Y += distance;
                }
                else
                {
                    nearestPoint.Y += distance;
                    farPoint.Y -= distance;
                }

            }
            else
            {
                if (ChangeEnd)
                    segment = new LineSegment2d(nearestFake.Start, new Vector2d(nearestFake.End.X + distance, nearestFake.End.Y));
                else segment = new LineSegment2d(new Vector2d(nearestFake.Start.X + distance, nearestFake.Start.Y), nearestFake.End);
                if (segment.IsCollide(otherEdge.segment))
                {
                    nearestPoint.X -= distance;
                    farPoint.X += distance;
                }
                else
                {
                    nearestPoint.X += distance;
                    farPoint.X -= distance;
                }

            }
        }


        #endregion

        #region Corridor from tree to list

      
        bool IsHaveEqualCoordinate(Vector2d point1, Vector2d point2)
        {

            if (Math.Round(point1.X, 10) == Math.Round(point2.X, 10))
                return true;
            if (Math.Round(point1.Y, 10) == Math.Round(point2.Y, 10))
                return true;
            if (point1.X == point2.X)
                return true;
            if (point1.Y == point2.Y)
                return true;
            else return false;
        }
        bool? AddPointToStartOrTOEnd(Vector2d point, bool? addLast)
        {
            if (addLast == true)
            {
                polygonOfCorridor.Add(point);
                return true;
            }
            else if (addLast == false)
            {
                polygonOfCorridor.Insert(0, point);
                return false;
            }
            else
            {
                var line1 = new LineLinear2d(polygonOfCorridor[0], point);
                var line2 = new LineLinear2d(point, polygonOfCorridor[polygonOfCorridor.Count - 1]);
                if (IsHaveEqualCoordinate(polygonOfCorridor[0], point) && IsHaveEqualCoordinate(point, polygonOfCorridor[polygonOfCorridor.Count - 1]) && line1.Colinear(line2))
                {
                    if (point.DistanceTo(polygonOfCorridor[0]) > point.DistanceTo(polygonOfCorridor[polygonOfCorridor.Count - 1]))
                    {
                        polygonOfCorridor.Add(point);
                        lastPoint = point;
                        return true;
                    }
                    else { polygonOfCorridor.Insert(0, point); return false; }
                }
                else if (IsHaveEqualCoordinate(polygonOfCorridor[0], point))
                {
                    polygonOfCorridor.Insert(0, point);
                    return false;

                }
                else if (IsHaveEqualCoordinate(point, polygonOfCorridor[polygonOfCorridor.Count - 1]))
                {
                    polygonOfCorridor.Add(point);
                    return true;
                }
            }

            return null;
        }
        void FirstNode(Node<CorridorPart> root)
        {
           
            var Walls = root.data.OuterWalls;
            
            polygonOfCorridor.Add(Walls[0].Start);
            polygonOfCorridor.Add(Walls[0].End);
         
            var vert = Walls[0].GetNeighborPoint(Walls[1]);
            AddPointToStartOrTOEnd(vert, null);

            vert = Walls[1].GetNeighborPoint(Walls[2]);
            AddPointToStartOrTOEnd(vert, null);

            if (root.nodes != null)
            {

                for (var i = 0; i < root.nodes.Count; i++)
                    if (root.nodes[i].nodes == null)
                    {
                        LastNode(root.nodes[i], false);                        
                        
                    }
                        
                    

                for (var i = 0; i < root.nodes.Count; i++)
                    if (root.nodes[i].nodes != null)
                        NextNode(root.nodes[i]);
            }

        }
        void ToList(Node<CorridorPart> root)
        {

            FirstNode(root);
            RoundPolygonCoordinates();



        }
        private void RoundPolygonCoordinates()
        {
            for (var i = 0; i < polygonOfCorridor.Count; i++)
            {

                polygonOfCorridor[i] = new Vector2d(Math.Ceiling(polygonOfCorridor[i].X), Math.Ceiling(polygonOfCorridor[i].Y));


            }
        }
        static Vector2d lastPoint;
        void LastNode(Node<CorridorPart> node, bool afterNext)
        {
            var edgeStart = new CorridorEdge(polygonOfCorridor[0], polygonOfCorridor[1]);
            var edgeEnd = new CorridorEdge(polygonOfCorridor[polygonOfCorridor.Count - 1], polygonOfCorridor[polygonOfCorridor.Count - 2]);

            var Walls = node.data.OuterWalls;
            Vector2d vert;
           
            vert = edgeEnd.GetNeighborPoint(Walls[0]);
            if (vert != Vector2d.Empty)
            {
                var addLast = AddPointToStartOrTOEnd(vert, null);
                for (var i = 1; i < Walls.Count; i++)
                {
                    edgeEnd = new CorridorEdge(polygonOfCorridor[polygonOfCorridor.Count - 1], polygonOfCorridor[polygonOfCorridor.Count - 2]);
                    vert = edgeEnd.GetNeighborPoint(Walls[i]);
                    if (!polygonOfCorridor.Contains(vert))
                        AddPointToStartOrTOEnd(vert, addLast);
                }
                return;
            }

            vert = edgeEnd.GetNeighborPoint(Walls[Walls.Count - 1]);
            if (vert != Vector2d.Empty)
            {
                var addLast = AddPointToStartOrTOEnd(vert, null);
                for (var i = Walls.Count - 2; i >= 0; i--)
                {
                    edgeEnd = new CorridorEdge(polygonOfCorridor[polygonOfCorridor.Count - 1], polygonOfCorridor[polygonOfCorridor.Count - 2]);
                    vert = edgeEnd.GetNeighborPoint(Walls[i]);
                    if (!polygonOfCorridor.Contains(vert))
                        AddPointToStartOrTOEnd(vert, addLast);
                }
                return;
            }
            vert = edgeStart.GetNeighborPoint(Walls[0]);
                      
            if (vert != Vector2d.Empty)
            {
                var addLast = AddPointToStartOrTOEnd(vert, null);
                for (var i = 1; i < Walls.Count; i++)
                {
                    edgeStart = new CorridorEdge(polygonOfCorridor[0], polygonOfCorridor[1]);
                    vert = edgeStart.GetNeighborPoint(Walls[i]);
                    if (!polygonOfCorridor.Contains(vert))
                        AddPointToStartOrTOEnd(vert, addLast);
                }
                return;
            }

            vert = edgeStart.GetNeighborPoint(Walls[Walls.Count - 1]);
            if (vert != Vector2d.Empty)
            {
                var addLast = AddPointToStartOrTOEnd(vert, null);
                for (var i = Walls.Count - 2; i >= 0; i--)
                {
                    edgeStart = new CorridorEdge(polygonOfCorridor[0], polygonOfCorridor[1]);
                    vert = edgeStart.GetNeighborPoint(Walls[i]);
                    if (!polygonOfCorridor.Contains(vert))
                        AddPointToStartOrTOEnd(vert, addLast);
                }
                return;
            }

           
        }

        

        void NextNode(Node<CorridorPart> node)
        {
            var edgeStart = new CorridorEdge(polygonOfCorridor[0], polygonOfCorridor[1]);
            var edgeEnd = new CorridorEdge(polygonOfCorridor[polygonOfCorridor.Count - 1], polygonOfCorridor[polygonOfCorridor.Count - 2]);

            var Walls = node.data.OuterWalls;

            var vert1 = edgeStart.GetNeighborPoint(Walls[0]);
            var vert2 = edgeEnd.GetNeighborPoint(Walls[1]);
            if (vert1 != Vector2d.Empty && vert2 != Vector2d.Empty)
            {
                polygonOfCorridor.Insert(0, vert1);
                polygonOfCorridor.Add(vert2);

            }
            vert1 = edgeStart.GetNeighborPoint(Walls[1]);
            vert2 = edgeEnd.GetNeighborPoint(Walls[0]);
            if (vert1 != Vector2d.Empty && vert2 != Vector2d.Empty) { 

                polygonOfCorridor.Insert(0, vert1);
                polygonOfCorridor.Add(vert2);
            }

            if (node.nodes != null)
            {

                for (var i = 0; i < node.nodes.Count; i++)
                    if (node.nodes[i].nodes == null)
                        LastNode(node.nodes[i], true);

                for (var i = 0; i < node.nodes.Count; i++)
                    if (node.nodes[i].nodes != null)
                        NextNode(node.nodes[i]);
            }

        }
        #endregion

    }
}
