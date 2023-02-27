namespace Travelling_Salesman_Problem___ACO
{
    class City
    {
        public int AxisX { get; set; }
        public int AxisY { get; set; }
        public int Id { get; set; }
        public City(int x,int y, int id)
        {
            this.AxisX = x;
            this.AxisY = y;
            this.Id = id;
        }

    }
}
