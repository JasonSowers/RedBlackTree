using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBlackTree;

namespace RedBlackTreeTests
{
    [TestClass]
    public class RedblackTreeTests
    {



        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
            "Object reference not set to an instance of an object.")]
        public void InsertNull()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            Tree.Add(null);
        }

        [TestMethod]
        public void CheckClear()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            for (int i = 0; i < 100; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(i,i)));
            }
            Tree.Clear();
            Assert.AreEqual(0, Tree.Count);
            Assert.AreEqual(null, Tree.Root);
        }

        [TestMethod]
        public void InsertInOrder()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            for (int i = 0; i < 100; i++)
            {
                Tree.Add(new Node<int, int>(i,i));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(i, i)));
            }
            Assert.AreEqual(100,Tree.Count);
        }

        [TestMethod]
        public void InsertRandom()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            var count = 100;
            var rnd = new Random();
            var numbers = Enumerable.Range(1, count)
                .OrderBy(x => rnd.Next())
                .ToArray();

            for (int i = 0; i < count; i++)
            {
                int j = numbers[i];
                Tree.Add(new Node<int, int>(j, j));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(j, j)));
            }
            Assert.AreEqual(numbers.Length, Tree.Count);
        }

        [TestMethod]
        public void DeleteInOrder()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            for (int i = 0; i < 100; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(i, i)));
            }
            Assert.AreEqual(100, Tree.Count);
            for (int i = 0; i < 100; i++)
            {
                Tree.Delete(new Node<int, int>(i, i));
                Assert.IsFalse(Tree.HasItem(new Node<int, int>(i, i)));
            }
            Assert.AreEqual(0, Tree.Count);
            Assert.AreEqual(null, Tree.Root);
        }

        [TestMethod]
        public void CheckRotation()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            for (int i = 0; i < 5; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(i, i)));
            }
            Assert.AreEqual(1,Tree.Root.Key);
            
            for (int i = 5; i < 9; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(i, i)));
            }
            Assert.AreEqual(3, Tree.Root.Key);
            for (int i = 9; i < 33; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(i, i)));
            }
            Assert.AreEqual(7, Tree.Root.Key);
        }

        [TestMethod]
        public void CheckMin()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            for (int i = 0; i < 5; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
            }
            Assert.AreEqual(0, Tree.FindMin().Key);
            
            for (int i = 5; i >= 5; i--)
            {
                Tree.Add(new Node<int, int>(i, i));
            }
            Assert.AreEqual(0, Tree.FindMin().Key);
        }
        [TestMethod]
        public void CheckMax()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            for (int i = 0; i < 5; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
            }
            Assert.AreEqual(4, Tree.FindMax().Key);

            for (int i = 5; i >= 5; i--)
            {
                Tree.Add(new Node<int, int>(i, i));
            }
            Assert.AreEqual(5, Tree.FindMax().Key);
        }

        [ExpectedException(typeof(Exception),"Node Already exists")]
        [TestMethod]
        public void InsertDuplicate()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            Tree.Add(new Node<int, int>(0, 0));
            Tree.Add(new Node<int, int>(0, 0));
        }

        [TestMethod]
        public void CheckHeight()
        {
            RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();
            Assert.AreEqual(Tree.GetHeight(), -1);

            Tree.Add(new Node<int, int>(0, 0));
            Assert.AreEqual(Tree.GetHeight(), 0);

            Tree.Add(new Node<int, int>(1, 1));
            Assert.AreEqual(Tree.GetHeight(), 1);

            Tree.Add(new Node<int, int>(2, 2));
            Tree.Add(new Node<int, int>(3, 3));
            Tree.Add(new Node<int, int>(4, 4));
            Assert.AreEqual(Tree.GetHeight(), 2);

            Tree.Delete(new Node<int, int>(0, 0));
            Tree.Delete(new Node<int, int>(1, 1));
            Tree.Delete(new Node<int, int>(2, 2));
            Assert.AreEqual(Tree.GetHeight(), 1);

            Tree.Delete(new Node<int, int>(3, 3));
            Assert.AreEqual(Tree.GetHeight(), 0);

            Tree.Delete(new Node<int, int>(4,4));
            Assert.AreEqual(Tree.GetHeight(), -1);
        }

        [TestMethod]
        public void RedBlackTree_AccuracyTest()
        {
            var nodeCount = 1000;

            var rnd = new Random();
            var numbers = Enumerable.Range(1, nodeCount)
                                .OrderBy(x => rnd.Next())
                                .ToList();

            var Tree = new RedBlackTree<int,int>();

            for (int i = 0; i < nodeCount; i++)
            {
                int j = numbers[i];
                Tree.Add(new Node<int, int>(j, j));
                Assert.IsTrue(Tree.HasItem(new Node<int, int>(j, j)));

                var actualHeight = Tree.GetHeight();

                //http://doctrina.org/maximum-height-of-red-black-tree.html
                var maxHeight = 2 * Math.Log(nodeCount + 1, 2);

                Assert.IsTrue(actualHeight < maxHeight);
                Assert.IsTrue(Tree.Count == i + 1);
            }

            numbers = Enumerable.Range(1, nodeCount)
                                   .OrderBy(x => rnd.Next())
                                   .ToList();

            for (int i = 0; i < nodeCount; i++)
            {
                int j = numbers[i];
                Tree.Delete(new Node<int, int>(j, j));

                var height = Tree.GetHeight();

                //http://doctrina.org/maximum-height-of-red-black-tree.html
                var maxHeight = 2 * Math.Log(nodeCount + 1, 2);

                Assert.IsTrue(height < maxHeight);
                Assert.IsTrue(Tree.Count == nodeCount - 1 - i);
            }

            Assert.IsTrue(Tree.Count == 0);
        }

        



    }
}
