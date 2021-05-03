using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1_2MIW
{
    public class Edge
    {
        public Node Node1;
        public Node Node2;
        public double Weight;

        public Edge(Node node1, Node node2, double weight)
        {
            this.Node1 = node1;
            this.Node2 = node2;
            this.Weight = weight;
        }

    }
}
