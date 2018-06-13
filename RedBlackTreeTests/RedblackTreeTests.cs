using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedBlackTree;

namespace RedBlackTreeTests
{
    [TestClass]
    public class RedblackTreeTests
    {
        private RedBlackTree<int, int> Tree = new RedBlackTree<int, int>();

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException),
            "Object reference not set to an instance of an object.")]
        public void InsertNull()
        {
            Tree.Add(null);
        }

        [TestMethod]
        public void InsertInOrder()
        {
            for (int i = 0; i < 100; i++)
            {
                Tree.Add(new Node<int, int>(i,i));
            }

            Assert.AreEqual(100,Tree.Count);
        }

        [TestMethod]
        public void DeleteInOrder()
        {
            for (int i = 0; i < 100; i++)
            {
                Tree.Add(new Node<int, int>(i, i));
            }
            Assert.AreEqual(100, Tree.Count);
            for (int i = 0; i < 100; i++)
            {
                Tree.Delete(new Node<int, int>(i, i));
            }
            Assert.AreEqual(0, Tree.Count);
            Assert.AreEqual(null, Tree.Root);
        }

        //helpers
    }
}
