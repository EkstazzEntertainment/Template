namespace Ekstazz.Tools.BST
{
    using System;
    using System.Collections.Generic;
    
    
    public class RankedBST<T> where T : IComparable<T>
    {
        private RankedBSTNode<T> Root { get; set; }

        public int Count => Root.Count;
        public int Height => Root?.GetHeight() ?? -1;

        
        public int GetDepth(T value)
        {
            var current = Root;
            var depth = 0;

            while (current != null)
            {
                var compareValue = current.Value.CompareTo(value);
                if (compareValue == 0)
                {
                    break;
                }

                current = compareValue > 0 ? current.Left : current.Right;

                ++depth;
            }

            if (current != null)
            {
                return depth;
            }
            throw new Exception("DepthNode NotFound");
        }

        public void Insert(T value)
        {
            var node = new RankedBSTNode<T>(value);
            InsertNode(node);
        }
        
        private void InsertNode(RankedBSTNode<T> node)
        {
            node.Count = 1;
            if (Root == null)
            {
                Root = node;
            }
            else
            {
                var current = Root;
                while (true)
                {
                    ++current.Count;
                    var compareResult = current.Value.CompareTo(node.Value);

                    if (compareResult >= 0)
                    {
                        if (current.Left != null)
                        {
                            current = current.Left;
                        }
                        else //found leaf - insert here
                        {
                            current.Left = node;
                            break;
                        }
                    }
                    //value is larger than current.Value
                    else
                    {
                        if (current.Right != null)
                        {
                            current = current.Right;
                        }
                        else // found leaf - insert here
                        {
                            current.Right = node;
                            break;
                        }
                    }
                    
                }
            }
        }

        public void Delete(T value)
        {
            Root = DeleteNode(Root, value);
        }
        
        private RankedBSTNode<T> DeleteNode(RankedBSTNode<T> node, T value)
        {
            if (node == null)
            {
                return null;
            }
            var comparingResult = value.CompareTo(node.Value);
            if (comparingResult < 0)
            {
                node.Left = DeleteNode(node.Left, value);
            }
            else if (comparingResult > 0)
            {
                node.Right = DeleteNode(node.Right, value);
            }
            else
            {
                if (node.Right == null)
                {
                    return node.Left;
                }

                if (node.Left == null)
                {
                    return node.Right;
                }

                var tempNode = node;
                node = GetMinInSubtree(tempNode.Right);
                node.Right = DeleteMin(tempNode.Right);
                node.Left = tempNode.Left;
            }

            node.Count = 1 + GetNodeSize(node.Left) + GetNodeSize(node.Right);
            return node;
        }

        private RankedBSTNode<T> DeleteMin(RankedBSTNode<T> node)
        {
            if (node.Left == null)
            {
                return node.Right;
            }
            node.Left = DeleteMin(node.Left);
            node.Count = 1 + GetNodeSize(node.Left) + GetNodeSize(node.Right);
            return node;
        }

        private RankedBSTNode<T> GetMinInSubtree(RankedBSTNode<T> node)
        {
            var current = node;
            while (current.Left != null)
            {
                current = current.Left;
            }

            return current;
        }

        public T FindKthSmallest(int k)
        {
            if (k > Count)
            {
                throw new Exception("K is more than tree nodes count");
            }
            var currentNode = Root;
            var orderNumber = 1 + GetNodeSize(currentNode.Left);
            while (true)
            {
                if (orderNumber == k)
                {
                    return currentNode.Value;
                }
                if (orderNumber < k)
                {
                    currentNode = currentNode.Right;
                    orderNumber += 1 + GetNodeSize(currentNode.Left);
                }
                else
                {
                    currentNode = currentNode.Left;
                    orderNumber -= 1 + GetNodeSize(currentNode.Right);
                }
            }
        }

        /// <summary>
        /// Iterates through the tree using the in order traversal algorithm
        /// 
        /// O(n)
        /// </summary>
        public IEnumerable<T> InOrderIterator
        {
            get
            {
                var current = Root;
                var parentStack = new Stack<RankedBSTNode<T>>();

                while (current != null || parentStack.Count != 0)
                {
                    if (current != null)
                    {
                        parentStack.Push(current);
                        current = current.Left;
                    }
                    else
                    {
                        current = parentStack.Pop();
                        yield return current.Value;
                        current = current.Right;
                    }
                }
            }
        }

        /// <summary>
        /// Iterates through the tree using the in post order traversal algorithm
        /// 
        /// O(n)
        /// </summary>
        public IEnumerable<T> PostOrderIterator
        {
            get
            {
                RankedBSTNode<T> current;
                RankedBSTNode<T> previous = null;
                var nodeStack = new Stack<RankedBSTNode<T>>();

                if (Root != null)
                {
                    nodeStack.Push(Root);
                }

                while (nodeStack.Count > 0)
                {
                    current = nodeStack.Peek();
                    if (previous == null || previous.Left == current || previous.Right == current)
                    {
                        if (current.Left != null)
                        {
                            nodeStack.Push(current.Left);
                        }
                        else if (current.Right != null)
                        {
                            nodeStack.Push(current.Right);
                        }
                    }
                    else if (current.Left == previous)
                    {
                        if (current.Right != null)
                        {
                            nodeStack.Push(current.Right);
                        }
                    }
                    else
                    {
                        yield return current.Value;
                        nodeStack.Pop();
                    }

                    previous = current;
                }
            }
        }

        /// <summary>
        /// Iterates through the tree using the in pre order traversal algorithm
        /// 
        /// O(n)
        /// </summary>
        public IEnumerable<T> PreOrderIterator
        {
            get
            {
                var parentStack = new Stack<RankedBSTNode<T>>();

                var current = Root;

                while (parentStack.Count > 0 || current != null)
                {
                    if (current != null)
                    {
                        yield return current.Value;

                        parentStack.Push(current.Right);
                        current = current.Left;
                    }
                    else
                    {
                        current = parentStack.Pop();
                    }
                }
            }
        }

        private int GetNodeSize(RankedBSTNode<T> node)
        {
            return node?.Count ?? 0;
        }

        public void AssertValidTree()
        {
            if (Root != null)
            {
                var first = true;
                var previousValue = default(T);
                foreach (var value in InOrderIterator)
                {
                    if (!first && previousValue.CompareTo(value) >= 0)
                    {
                        throw new Exception("Invalid tree");
                    }

                    previousValue = value;
                    first = false;
                }
            }
        }
    }
}