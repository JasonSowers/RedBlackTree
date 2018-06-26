using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public int GetHeight()
        {
            return GetHeight(Root);
        }

        public bool HasNode(Node<TKey, TValue> node)
        {
            if (Root == null)
            {
                return false;
            }

            return Find(Root, node) != null;
        }
        internal List<Node<TKey, TValue>> GetAllNodes()
        {
            var allNodes = new List<Node<TKey, TValue>>();

            GetAllNodes(allNodes, Root);

            return allNodes;
        }

        public void Delete(Node<TKey, TValue> node)
        {
            if (Root == null)
            {
                throw new Exception("Empty Tree");
            }

            Delete(Root, node);
            Count--;
        }

        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        public bool HasItem(Node<TKey, TValue> node)
        {
            if (Root == null)
            {
                return false;
            }

            return Find(Root, node) != null;
        }

        public Node<TKey, TValue> FindMax()
        {
            return FindMax(Root);
        }

        public Node<TKey, TValue> FindMin()
        {
            return FindMin(Root);
        }

        public void Add(Node<TKey, TValue> node)
        {
            //empty tree
            if (Root == null)
            {
                Root = node;
                Root.NodeColor = NodeColor.Black;
                Count++;
                return;
            }

            Add(Root, node);
            Count++;
        }

        public int GetHeight(Node<TKey, TValue> node)
        {
            if (node == null)
            {
                return -1;
            }

            return Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        }

    }



    public class Node<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
        internal Node<TKey, TValue> Parent { get; set; }

        internal Node<TKey, TValue> Left { get; set; }
        internal Node<TKey, TValue> Right { get; set; }

        internal bool IsLeaf => Left == null && Right == null;
        internal NodeColor NodeColor { get; set; }

        internal Node<TKey, TValue> Sibling => this.Parent.Left == this ? this.Parent.Right : this.Parent.Left;

        internal bool IsLeftChild => this.Parent.Left == this;
        internal bool IsRightChild => this.Parent.Right == this;

        public Node(TKey key, TValue value)
        {
            NodeColor = NodeColor.Red;
            Key = key;
            Value = value;
        }
    }
}
