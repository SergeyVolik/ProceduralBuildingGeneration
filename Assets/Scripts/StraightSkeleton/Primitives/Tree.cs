using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraightSkeleton.Primitives
{
    public class SkeletonTree<T>
    {
        public Node<T> root;

        public SkeletonTree()
        {
            root = new Node<T>();
        }

    }

    public class Node<T> {
        public List<Node<T>> nodes;
        public T data;
      
        public Node()
        {
            
        }
        public void AddChild(T child)
        {
            if (nodes == null)
                nodes = new List<Node<T>>();

            nodes.Add(new Node<T>() { data = child });
        }
        public void AddChild(Node<T> child)
        {
            if (nodes == null)
                nodes = new List<Node<T>>();

            nodes.Add(child);
        }
    }
}
