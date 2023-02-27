using System;

namespace Travelling_Salesman_Problem___ACO
{
    class Program
    {
        static void Main(string[] args)
        {
            AntColonyOptimization aco = new AntColonyOptimization(1000, 10,0.5,1.0);
        }
    }
}
