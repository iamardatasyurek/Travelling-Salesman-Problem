using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling_Salesman_Problem___ACO
{
    class City
    {
        public int axis_x;
        public int axis_y;
        public int id;
        public City(int x,int y, int id)
        {
            this.axis_x = x;
            this.axis_y = y;
            this.id = id;
        }

    }
}
