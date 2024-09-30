using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07.PrimMST_re
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph();

            g.ReadGraph("graph2.txt");
            g.PrintGraph();

            g.Prim(0);  // 0번 버텍스에서 시작하는 MST
        }
    }
}
