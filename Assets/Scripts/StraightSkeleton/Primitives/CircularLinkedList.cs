using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StraightSkeleton.Primitives
{
    public class CircularLinkedListNode<T>
    {
        public CircularLinkedListNode(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public CircularLinkedListNode<T> Next { get; set; }
    }


    
    public class CircularLinkedList<T> : IEnumerable<T>  // кольцевой связный список
    {
        CircularLinkedListNode<T> head; // головной/первый элемент
        CircularLinkedListNode<T> tail; // последний/хвостовой элемент
        int count;  // количество элементов в списке

        // добавление элемента
        public void Add(T data)
        {
            CircularLinkedListNode<T> CircularLinkedListNode = new CircularLinkedListNode<T>(data);
            // если список пуст
            if (head == null)
            {
                head = CircularLinkedListNode;
                tail = CircularLinkedListNode;
                tail.Next = head;
            }
            else
            {
                CircularLinkedListNode.Next = head;
                tail.Next = CircularLinkedListNode;
                tail = CircularLinkedListNode;
            }
            count++;
        }
        public CircularLinkedListNode<T> Find(T data)
        {
            var node = head;
            if (head.Data.Equals(data))
                return head;
            node = node.Next;
            while (node != head)
            {
                if (node.Data.Equals(data))
                    return node;
                node = node.Next;
            }

            return null;
            
        }
        public bool Remove(T data)
        {
            CircularLinkedListNode<T> current = head;
            CircularLinkedListNode<T> previous = null;

            if (IsEmpty) return false;

            do
            {
                if (current.Data.Equals(data))
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        // убираем узел current, теперь previous ссылается не на current, а на current.Next
                        previous.Next = current.Next;

                        // Если узел последний,
                        // изменяем переменную tail
                        if (current == tail)
                            tail = previous;
                    }
                    else // если удаляется первый элемент
                    {

                        // если в списке всего один элемент
                        if (count == 1)
                        {
                            head = tail = null;
                        }
                        else
                        {
                            head = current.Next;
                            tail.Next = current.Next;
                        }
                    }
                    count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            } while (current != head);

            return false;
        }

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public bool Contains(T data)
        {
            CircularLinkedListNode<T> current = head;
            if (current == null) return false;
            do
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            while (current != head);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            CircularLinkedListNode<T> current = head;
            do
            {
                if (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != head);
        }
    }
}

