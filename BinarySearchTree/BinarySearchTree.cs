using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BinarySearchTree
{
    public class BinarySearchTree<T> : IEnumerable<T>, IEnumerable
    {
        private class Node
        {
            public T Value { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public int Count { get; set; }

            public Node(T value)
            {
                Value = value;
                Count++;
            }
        }

        private Node _root;

        private Comparer<T> _comparer;

        public int Count { get; private set; }

        #region .ctors

        public BinarySearchTree(Comparer<T> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public BinarySearchTree() : this(Comparer<T>.Default) { }

        public BinarySearchTree(IEnumerable<T> collection, Comparer<T> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            
            AddRange(collection);
        }

        public BinarySearchTree(IEnumerable<T> collection) : this(collection, Comparer<T>.Default) { }

        #endregion

        /// <summary>
        /// Add item to BinarySearchTree.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <exception cref="ArgumentNullException">Throws if item ref is null.</exception>
        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            
            Node node = new Node(item);

            if (_root == null)
            {
                _root = node;
                return;
            }

            Node current = _root, parent = null;

            while (current != null)
            {
                parent = current;
                if (_comparer.Compare(item, current.Value) == 0)
                {
                    current.Count++;
                    return;
                }
                else if (_comparer.Compare(item, current.Value) < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            if (_comparer.Compare(item, parent.Value) < 0)
            {
                parent.Left = node;
            }
            else
            {
                parent.Right = node;
            }

            Count++;
        }

        /// <summary>
        /// Add collection of items to BinarySearchTree.
        /// </summary>
        /// <param name="collection">Collection of items.</param>
        /// <exception cref="ArgumentNullException">Throws if collection ref is null or at least one of element of collection is null.</exception>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Remove item from BinarySearchTree.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <exception cref="ArgumentNullException">Throws if item ref is null.</exception>
        /// <returns>TRUE: if removing success.</returns>
        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_root == null)
                return false;

            Node current = _root, parent = null;

            for (int cmp = _comparer.Compare(item, current.Value);
                 cmp != 0;
                 cmp = _comparer.Compare(item, current.Value))
            {
                if (cmp < 0)
                {
                    parent = current;
                    current = current.Left;
                }

                if (cmp > 0)
                {
                    parent = current;
                    current = current.Right;
                }

                if (current == null)
                    return false;
            }

            if (current.Count > 1)
            {
                current.Count--;
                return true;
            }

            if (current.Left == current.Right)
            {
                if (_comparer.Compare(item, parent.Value) < 0)
                {
                    parent.Left = null;
                }
                else
                {
                    parent.Right = null;
                }
            }
            else if (current.Left == null)
            {
                if (_comparer.Compare(item, parent.Value) < 0)
                {
                    parent.Left = current.Right;
                }
                else
                {
                    parent.Right = current.Right;
                }
            }
            else if (current.Right == null)
            {
                if (_comparer.Compare(item, parent.Value) < 0)
                {
                    parent.Left = current.Left;
                }
                else
                {
                    parent.Right = current.Left;
                }
            }
            else
            {
                Node successor = current.Right, parentOfSuccessor = current;
                while (successor.Left != null)
                {
                    parentOfSuccessor = successor;
                    successor = successor.Left;
                }

                parentOfSuccessor.Left = successor.Right;
                successor.Left = current.Left;
                successor.Right = current.Right;

                if (_comparer.Compare(item, parent.Value) < 0)
                {
                    parent.Left = successor;
                }
                else
                {
                    parent.Right = successor;
                }
            }

            Count--;
            return true;
        }

        /// <summary>
        /// Search item in BinarySearchTree.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <exception cref="ArgumentNullException">Throws if item ref is null.</exception>
        /// <returns>TRUE: if BinarySearchTree is contain item.</returns>
        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_root == null)
                return false;

            Node current = _root;
            while (current != null)
            {
                int cmp = _comparer.Compare(item, current.Value);
                if (cmp == 0)
                    return true;

                if (cmp < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns enumeration by inorder.
        /// </summary>
        /// <returns>Enumeration.</returns>
        public IEnumerable<T> Inorder()
        {
            if (_root == null)
                yield break;

            Stack<Node> stack = new Stack<Node>();
            Node current = _root;

            while (stack.Count > 0 || current != null)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }
                else
                {
                    current = stack.Pop();
                    for (int i = 0; i < current.Count; i++)
                    {
                        yield return current.Value;
                    }
                    current = current.Right;
                }
            }
        }

        /// <summary>
        /// Returns enumeration by preorder.
        /// </summary>
        /// <returns>Enumeration.</returns>
        public IEnumerable<T> Preorder()
        {
            if (_root == null)
                yield break;

            Stack<Node> stack = new Stack<Node>();
            stack.Push(_root);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();
                for (int i = 0; i < current.Count; i++)
                {
                    yield return current.Value;
                }

                if (current.Right != null)
                    stack.Push(current.Right);

                if (current.Left != null)
                    stack.Push(current.Left);
            }
        }

        /// <summary>
        /// Returns enumeration by postorder.
        /// </summary>
        /// <returns>Enumeration.</returns>
        public IEnumerable<T> Postorder()
        {
            if (_root == null)
                yield break;

            Stack<Node> stack = new Stack<Node>();
            stack.Push(_root);

            while (stack.Count > 0)
            {
                Node current = stack.Peek();
                if (current.Left == current.Right)
                {
                    do
                    {
                        current = stack.Pop();
                        for (int i = 0; i < current.Count; i++)
                        {
                            yield return current.Value;
                        }
                    }
                    while (stack.Peek().Right == current || stack.Peek().Left == current);

                    continue;
                }
                
                if (current.Right != null)
                    stack.Push(current.Right);

                if (current.Left != null)
                    stack.Push(current.Left);
            }
        }

        /// <summary>
        /// Returns enumeration by default(inorder).
        /// </summary>
        /// <returns>Enumeration.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Inorder().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
