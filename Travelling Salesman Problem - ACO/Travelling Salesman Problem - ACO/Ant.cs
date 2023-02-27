namespace Travelling_Salesman_Problem___ACO
{
    class Ant
    {
        public int Beta { get; set; }
        public int Alpha { get; set; }
        public Ant(int beta, int alpha)
        {
            this.Beta = beta;
            this.Alpha = alpha;
        }
    }
}
