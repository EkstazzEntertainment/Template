namespace Ekstazz.Tools.BST
{
    using System;
    using System.Collections.Generic;

    
    public class RankedBSTNode<T> where T : IComparable<T>
    {
        public T Value { get; set; }
        public RankedBSTNode<T> Left { get; set; }
        public RankedBSTNode<T> Right { get; set; }
        public int Count { get; set; }
        
        public RankedBSTNode(T value)
        {
            Value = value;
        }
        

        public int GetHeight()
        {
            var nodes = new Queue<RankedBSTNode<T>>();
            nodes.Enqueue(this);

            var height = -1;

            while (nodes.Count > 0)
            {
                var nodesAtCurrentLevel = nodes.Count;
                for (var i = 0; i < nodesAtCurrentLevel; ++i)
                {
                    if (i == 0)
                    {
                        ++height;
                    }

                    var current = nodes.Dequeue();

                    if (current.Left != null)
                    {
                        nodes.Enqueue(current.Left);
                    }
                    if (current.Right != null)
                    {
                        nodes.Enqueue(current.Right);
                    }
                }
            }

            return height;
        }

        public RankedBSTNode<T> GetInOrderPredecessor()
        {
            RankedBSTNode<T> previous = null;
            var current = Left;
            while (current != null)
            {
                previous = current;
                current = current.Right;
            }

            return previous;
        }

        public RankedBSTNode<T> GetInOrderSuccessor()
        {
            RankedBSTNode<T> previous = null;
            var current = Right;
            while (current != null)
            {
                previous = current;
                current = current.Left;
            }

            return previous;
        }

        public override string ToString()
        {
            var left = Left == null ? "null" : Left.Value.ToString();
            var right = Right == null ? "null" : Right.Value.ToString();
            return $"{Value}, Left={left}, Right={right}";
        }
    }
}