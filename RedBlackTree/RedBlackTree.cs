using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    internal enum NodeColor
    {
        Black,
        Red
    }

    public partial class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public Node<TKey, TValue> Root { get; private set; }
        public int Count { get; private set; }

        internal void GetAllNodes(List<Node<TKey, TValue>> allNodes, Node<TKey, TValue> node)
        {
            while (true)
            {
                if (node == null)
                {
                    return;
                }

                allNodes.Add(node);

                GetAllNodes(allNodes, node.Left);
                node = node.Right;
            }
        }

        internal void Clear()
        {
            Root = null;
            Count = 0;
        }

        private Node<TKey, TValue> FindMax(Node<TKey, TValue> node)
        {
            while (true)
            {
                if (node.Right == null) return node;
                node = node.Right;
            }
        }

        private Node<TKey, TValue> FindMin(Node<TKey, TValue> node)
        {
            while (true)
            {
                if (node.Left == null) return node;
                node = node.Left;
            }
        }


        internal Node<TKey, TValue> FindNode(Node<TKey, TValue> node)
        {
            return Root == null ? null : Find(Root, node);
        }

 
        internal Node<TKey, TValue> Find(Node<TKey, TValue> node)
        {
            var result = FindNode(node);

            return result;
        }


        internal bool Exists(Node<TKey, TValue> node)
        {
            return FindNode(node) != null;
        }

        private Node<TKey, TValue> Find(Node<TKey, TValue> current, Node<TKey, TValue> node)
        {
            while (true)
            {
                if (current == null)
                {
                    return null;
                }

                if (current.Key.CompareTo(node.Key) == 0)
                {
                    return current;
                }

                var left = Find(current.Left, node);

                if (left != null)
                {
                    return left;
                }

                current = current.Right;
            }
        }

        private void RightRotate(Node<TKey, TValue> node)
        {
            var prevRoot = node;
            var leftRightChild = prevRoot.Left.Right;

            var newRoot = node.Left;

            //make left child as root
            prevRoot.Left.Parent = prevRoot.Parent;

            if (prevRoot.Parent != null)
            {
                if (prevRoot.Parent.Left == prevRoot)
                {
                    prevRoot.Parent.Left = prevRoot.Left;
                }
                else
                {
                    prevRoot.Parent.Right = prevRoot.Left;
                }
            }

            //move prev root as right child of current root
            newRoot.Right = prevRoot;
            prevRoot.Parent = newRoot;

            //move right child of left child of prev root to left child of right child of new root
            newRoot.Right.Left = leftRightChild;
            if (newRoot.Right.Left != null)
            {
                newRoot.Right.Left.Parent = newRoot.Right;
            }

            if (prevRoot == Root)
            {
                Root = newRoot;
            }
        }

        private void LeftRotate(Node<TKey, TValue> node)
        {
            var prevRoot = node;
            var rightLeftChild = prevRoot.Right.Left;

            var newRoot = node.Right;

            //make right child as root
            prevRoot.Right.Parent = prevRoot.Parent;

            if (prevRoot.Parent != null)
            {
                if (prevRoot.Parent.Left == prevRoot)
                {
                    prevRoot.Parent.Left = prevRoot.Right;
                }
                else
                {
                    prevRoot.Parent.Right = prevRoot.Right;
                }
            }

            //move prev root as left child of current root
            newRoot.Left = prevRoot;
            prevRoot.Parent = newRoot;

            //move left child of right child of prev root to right child of left child of new root
            newRoot.Left.Right = rightLeftChild;
            if (newRoot.Left.Right != null)
            {
                newRoot.Left.Right.Parent = newRoot.Left;
            }

            if (prevRoot == Root)
            {
                Root = newRoot;
            }
        }

        private Node<TKey, TValue> Add(Node<TKey, TValue> current, Node<TKey, TValue> node)
        {
            while (true)
            {
                var compareResult = current.Key.CompareTo(node.Key);

                //current node is less than new item
                if (compareResult < 0)
                {
                    //no right child
                    if (current.Right == null)
                    {
                        //insert
                        current.Right = node;
                        node.Parent = current;
                        BalanceInsertion(current.Right);
                        return node;
                    }

                    current = current.Right;
                }
                //current node is greater than new node
                else if (compareResult > 0)
                {
                    if (current.Left == null)
                    {
                        //insert
                        current.Right = node;
                        node.Parent = current;
                        BalanceInsertion(current.Left);
                        return node;
                    }

                    current = current.Left;
                }
                else
                {
                    //duplicate
                    throw new Exception("Node Already exists");
                }
            }
        }

        private void BalanceInsertion(Node<TKey, TValue> node)
        {
            while (true)
            {
                if (node == Root)
                {
                    node.NodeColor = NodeColor.Black;
                    return;
                }

                //if node to balance is red
                if (node.NodeColor == NodeColor.Red)
                {
                    //red-red relation; fix it!
                    if (node.Parent.NodeColor == NodeColor.Red)
                    {
                        //red sibling
                        if (node.Parent.Sibling != null &&
                            node.Parent.Sibling.NodeColor == NodeColor.Red)
                        {
                            //mark both children of parent as black and move up balancing 
                            node.Parent.Sibling.NodeColor = NodeColor.Black;
                            node.Parent.NodeColor = NodeColor.Black;

                            //root is always black
                            if (node.Parent.Parent != Root)
                            {
                                node.Parent.Parent.NodeColor = NodeColor.Red;
                            }

                            node = node.Parent.Parent;
                        }
                        //absent sibling or black sibling
                        else if (node.Parent.Sibling == null ||
                                 node.Parent.Sibling.NodeColor == NodeColor.Black)
                        {
                            if (node.IsLeftChild && node.Parent.IsLeftChild)
                            {
                                var newRoot = node.Parent;
                                SwapColors(node.Parent, node.Parent.Parent);
                                RightRotate(node.Parent.Parent);

                                if (newRoot == Root)
                                {
                                    Root.NodeColor = NodeColor.Black;
                                }

                                node = newRoot;
                            }
                            else if (node.IsLeftChild && node.Parent.IsRightChild)
                            {
                                RightRotate(node.Parent);

                                var newRoot = node;

                                SwapColors(node.Parent, node);
                                LeftRotate(node.Parent);

                                if (newRoot == Root)
                                {
                                    Root.NodeColor = NodeColor.Black;
                                }

                                node = newRoot;
                            }
                            else if (node.IsRightChild && node.Parent.IsRightChild)
                            {
                                var newRoot = node.Parent;
                                SwapColors(node.Parent, node.Parent.Parent);
                                LeftRotate(node.Parent.Parent);

                                if (newRoot == Root)
                                {
                                    Root.NodeColor = NodeColor.Black;
                                }

                                node = newRoot;
                            }
                            else if (node.IsRightChild && node.Parent.IsLeftChild)
                            {
                                LeftRotate(node.Parent);

                                var newRoot = node;

                                SwapColors(node.Parent, node);
                                RightRotate(node.Parent);

                                if (newRoot == Root)
                                {
                                    Root.NodeColor = NodeColor.Black;
                                }

                                node = newRoot;
                            }
                        }
                    }
                }

                if (node.Parent != null)
                {
                    node = node.Parent;
                    continue;
                }

                break;
            }
        }

        private void SwapColors(Node<TKey, TValue> parent, Node<TKey, TValue> node)
        {
            var tmpColor = node.NodeColor;
            node.NodeColor = parent.NodeColor;
            parent.NodeColor = tmpColor;
        }

        //O(log(n)) always
        private void Delete(Node<TKey, TValue> current, Node<TKey, TValue> node)
        {
            Node<TKey, TValue> nodeToBalance = null;

            //var compareResult = current.Key.CompareTo(node.Key);

            //node is less than the search value so move right to find the deletion node
            if (current.Key.CompareTo(node.Key) < 0)
            {
                if (current.Right == null)
                {
                    throw new Exception("Item do not exist");
                }

                Delete(current.Right, node);
            }
            //node is less than the search value so move left to find the deletion node
            else if (current.Key.CompareTo(node.Key) > 0)
            {
                if (current.Left == null)
                {
                    throw new Exception("Item do not exist");
                }

                Delete(current.Left, node);
            }
            else
            {
                //node is a leaf node
                if (current.IsLeaf)
                {
                    //if color is red, we are good; no need to balance
                    if (current.NodeColor == NodeColor.Red)
                    {
                        DeleteLeaf(current);
                        return;
                    }

                    nodeToBalance = HandleDoubleBlack(current);
                    DeleteLeaf(current);
                }
                else
                {
                    //case one - right tree is null (move sub tree up)
                    if (current.Left != null && current.Right == null)
                    {
                        nodeToBalance = HandleDoubleBlack(node);
                        DeleteLeftNode(current);
                    }
                    //case two - left tree is null  (move sub tree up)
                    else if (current.Right != null && current.Left == null)
                    {
                        nodeToBalance = HandleDoubleBlack(current);
                        DeleteRightNode(current);
                    }
                    //case three - two child trees 
                    //replace the node value with maximum element of left subtree (left max node)
                    //and then delete the left max node
                    else
                    {
                        var maxLeftNode = FindMax(current.Left);

                        current.Value = maxLeftNode.Value;

                        //delete left max node
                        Delete(node.Left, maxLeftNode);
                    }
                }
            }

            //handle six cases
            while (nodeToBalance != null)
            {
                nodeToBalance = HandleDoubleBlack(nodeToBalance);
            }
        }

        private void DeleteLeaf(Node<TKey, TValue> node)
        {
            //if node is root
            if (node.Parent == null)
            {
                Root = null;
            }
            //assign nodes parent.left/right to null
            else if (node.IsLeftChild)
            {
                node.Parent.Left = null;
            }
            else
            {
                node.Parent.Right = null;
            }
        }

        private void DeleteRightNode(Node<TKey, TValue> node)
        {
            //root
            if (node.Parent == null)
            {
                Root.Right.Parent = null;
                Root = Root.Right;
                Root.NodeColor = NodeColor.Black;
                return;
            }

            //node is left child of parent
            if (node.IsLeftChild)
            {
                node.Parent.Left = node.Right;
            }
            //node is right child of parent
            else
            {
                node.Parent.Right = node.Right;
            }

            node.Right.Parent = node.Parent;

            if (node.Right.NodeColor != NodeColor.Red)
            {
                return;
            }

            //recolor to black
            node.Right.NodeColor = NodeColor.Black;
        }

        private void DeleteLeftNode(Node<TKey, TValue> node)
        {
            //root
            if (node.Parent == null)
            {
                Root.Left.Parent = null;
                Root = Root.Left;
                Root.NodeColor = NodeColor.Black;
                return;
            }
            
            if (node.IsLeftChild)
            {
                node.Parent.Left = node.Left;
            }

            else
            {
                node.Parent.Right = node.Left;
            }

            node.Left.Parent = node.Parent;

            if (node.Left.NodeColor != NodeColor.Red)
            {
                return;
            }

            //recolor to black
            node.Left.NodeColor = NodeColor.Black;
        }

        private Node<TKey, TValue> HandleDoubleBlack(Node<TKey, TValue> node)
        {
            //case 1
            if (node == Root)
            {
                node.NodeColor = NodeColor.Black;
                return null;
            }

            //case 2
            if (node.Parent != null
                && node.Parent.NodeColor == NodeColor.Black
                && node.Sibling != null
                && node.Sibling.NodeColor == NodeColor.Red
                && ((node.Sibling.Left == null && node.Sibling.Right == null)
                    || (node.Sibling.Left != null && node.Sibling.Right != null
                                                  && node.Sibling.Left.NodeColor == NodeColor.Black
                                                  && node.Sibling.Right.NodeColor == NodeColor.Black)))
            {
                node.Parent.NodeColor = NodeColor.Red;
                node.Sibling.NodeColor = NodeColor.Black;

                if (node.Sibling.IsRightChild)
                {
                    LeftRotate(node.Parent);
                }
                else
                {
                    RightRotate(node.Parent);
                }

                return node;
            }

            //case 3
            if (node.Parent != null
                && node.Parent.NodeColor == NodeColor.Black
                && node.Sibling != null
                && node.Sibling.NodeColor == NodeColor.Black
                && (node.Sibling.Left == null && node.Sibling.Right == null
                    || node.Sibling.Left != null && node.Sibling.Right != null
                                                 && node.Sibling.Left.NodeColor == NodeColor.Black
                                                 && node.Sibling.Right.NodeColor == NodeColor.Black))
            {
                //pushed up the double black problem up to parent
                //so now it needs to be fixed
                node.Sibling.NodeColor = NodeColor.Red;
                return node.Parent;
            }


            //case 4
            if (node.Parent != null
                && node.Parent.NodeColor == NodeColor.Red
                && node.Sibling != null
                && node.Sibling.NodeColor == NodeColor.Black
                && (node.Sibling.Left == null && node.Sibling.Right == null
                    || node.Sibling.Left != null && node.Sibling.Right != null
                                                 && node.Sibling.Left.NodeColor == NodeColor.Black
                                                 && node.Sibling.Right.NodeColor == NodeColor.Black))
            {
                //just swap the color of parent and sibling
                //which will compensate the loss of black count 
                node.Parent.NodeColor = NodeColor.Black;
                node.Sibling.NodeColor = NodeColor.Red;

                return null;
            }


            //case 5
            if (node.Parent != null
                && node.Parent.NodeColor == NodeColor.Black
                && node.Sibling != null
                && node.Sibling.IsRightChild
                && node.Sibling.NodeColor == NodeColor.Black
                && node.Sibling.Left != null
                && node.Sibling.Left.NodeColor == NodeColor.Red
                && node.Sibling.Right != null
                && node.Sibling.Right.NodeColor == NodeColor.Black)
            {
                node.Sibling.NodeColor = NodeColor.Red;
                node.Sibling.Left.NodeColor = NodeColor.Black;
                RightRotate(node.Sibling);

                return node;
            }

            //case 5 mirror
            if (node.Parent != null
                && node.Parent.NodeColor == NodeColor.Black
                && node.Sibling != null
                && node.Sibling.IsLeftChild
                && node.Sibling.NodeColor == NodeColor.Black
                && node.Sibling.Left != null
                && node.Sibling.Left.NodeColor == NodeColor.Black
                && node.Sibling.Right != null
                && node.Sibling.Right.NodeColor == NodeColor.Red)
            {
                node.Sibling.NodeColor = NodeColor.Red;
                node.Sibling.Right.NodeColor = NodeColor.Black;
                LeftRotate(node.Sibling);

                return node;
            }

            //case 6
            if (node.Parent != null
                && node.Parent.NodeColor == NodeColor.Black
                && node.Sibling != null
                && node.Sibling.IsRightChild
                && node.Sibling.NodeColor == NodeColor.Black
                && node.Sibling.Right != null
                && node.Sibling.Right.NodeColor == NodeColor.Red)
            {
                //left rotate to increase the black count on left side by one
                //and mark the red right child of sibling to black 
                //to compensate the loss of Black on right side of parent
                node.Sibling.Right.NodeColor = NodeColor.Black;
                LeftRotate(node.Parent);

                return null;
            }

            //case 6 mirror
            if (node.Parent != null
                && node.Parent.NodeColor == NodeColor.Black
                && node.Sibling != null
                && node.Sibling.IsLeftChild
                && node.Sibling.NodeColor == NodeColor.Black
                && node.Sibling.Left != null
                && node.Sibling.Left.NodeColor == NodeColor.Red)
            {
                //right rotate to increase the black count on right side by one
                //and mark the red left child of sibling to black
                //to compensate the loss of Black on right side of parent
                node.Sibling.Left.NodeColor = NodeColor.Black;
                RightRotate(node.Parent);

                return null;
            }

            return null;
        }
    }
}

