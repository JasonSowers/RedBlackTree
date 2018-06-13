using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new RedBlackTree<int,int>();

            for (int i = 1; i < 11; i++)
            {
                if (i==8)
                {
                    var nachos = i;
                }
                //tree.Add(new Node<int, int>(i,i));
                tree.Add(null);
            }


            var Nachos = tree.Root;
            Console.ReadLine();

        }
    }
}
