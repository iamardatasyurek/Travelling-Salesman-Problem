using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling_Salesman_Problem___ACO
{
    class Ant
    {
        public int beta;
        public int alfa;
        public Ant(int b, int a)
        {
            this.beta = b;
            this.alfa = a;
        }
    }
}
