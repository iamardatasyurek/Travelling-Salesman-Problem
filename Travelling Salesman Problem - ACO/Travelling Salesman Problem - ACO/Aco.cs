using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling_Salesman_Problem___ACO
{
    class Aco
    {
        public int ant_population = 10000;
        public int city_count = 50;
        public double evaporation_coefficient = 0.5;
        public double initial_pheromone = 1;

        public Aco()
        {
            List<City> cities = create_cities();
            //write(cities);

            List<Ant> ants = create_ants();
            //write(ants);

            double[,] pheromones = initial_pheromones();
            //write(pheromones, city_count);

            double[,] city_distances = calcute_distance(cities);
            //write(city_distances, city_count);
   
            List<double> ants_distance_covered = new List<double>();
            List<int[]> ants_direction = new List<int[]>();

           
            int ant = 0;
            while (ant < ant_population)
            {
                Console.WriteLine("___"+ant+"___");
                walk(ants[ant], city_distances, pheromones, ants_distance_covered, ants_direction);
                update_pheromones(pheromones, city_distances, ants_distance_covered[ant], ants_direction[ant]);
                write(ants_direction[ant], ants_distance_covered[ant]);
                Console.WriteLine();
                write(pheromones, city_count);
                Console.WriteLine();
                Console.WriteLine("-------------------------------------");
                ant++;
            }

            int max = 0;
            int min = 0;
            for (int i = 0; i < ants_distance_covered.Count; i++)
            {
                if (ants_distance_covered[i] == ants_distance_covered.Max())
                    max = i;
                else if (ants_distance_covered[i] == ants_distance_covered.Min())
                    min = i;
            }          
            Console.Write("Max: " + ants_distance_covered.Max() + " /// ");
            write(ants_direction[max].ToArray());
            Console.WriteLine();
            Console.Write("Min: " + ants_distance_covered.Min() + " /// ");
            write(ants_direction[min].ToArray());
        }

        List<City> create_cities()
        {
            Random rnd = new Random();
            List<City> cities = new List<City>();
            int id = 0;
            City first_city = new City(rnd.Next(-500,500),rnd.Next(-500, 500),id);
            cities.Add(first_city);
            id++;
            for (int i = 1; i < city_count; i++)
            {
                bool control = true;
                int control_counter = 0;
                while(control)
                {
                    City city = new City(rnd.Next(-500, 500), rnd.Next(-500, 500), id);
                    for (int j = 0; j < cities.Count; j++)
                    {
                        if (cities[j].axis_x == city.axis_x && cities[j].axis_y == city.axis_y)
                        {
                            j = city_count;
                            control_counter++;
                        }
                    }
                    if (control_counter != 0)
                        control = true;
                    else
                    {
                        cities.Add(city);
                        control = false;
                    }               
                }
                id++;
            }
            return cities;
        }
        List<Ant> create_ants()
        {
            Random rnd = new Random();
            List<Ant> ants = new List<Ant>();
            for (int i = 0; i < ant_population; i++)
            {
                ants.Add(new Ant(rnd.Next(1,4),rnd.Next(1,4)));
            }
            return ants;
        }   
        double[,] initial_pheromones()
        {
            double[,] array = new double[city_count, city_count];
            for (int i = 0; i < city_count; i++)
            {
                for (int j = 0; j < city_count; j++)
                {
                    array[i, j] = initial_pheromone;
                }
            }
            return array;
        }
        double[,] calcute_distance(List<City> cities)
        {
            double[,] distances = new double[cities.Count, cities.Count];
            for (int i = 0; i < cities.Count; i++)
            {
                for (int j = 0; j < cities.Count; j++)
                {
                    distances[i, j] = Math.Sqrt(Math.Pow(cities[i].axis_x - cities[j].axis_x, 2) + Math.Pow(cities[i].axis_y - cities[j].axis_y, 2));
                }
            }
            return distances;
        }
        void walk(Ant ant, double[,] city_distances, double[,] pheromones, List<double> ants_distance_covered, List<int[]> ants_direction)
        {
            Random rnd = new Random();

            double distance_covered = 0;

            List<int> direction = new List<int>();

            int first_city = rnd.Next(0, city_count);
            direction.Add(first_city); 
            
            for (int i = 0; i < city_count; i++)
            {
                first_city = direction.Last();
                double[] cities_probability = calcute_probability(city_distances, pheromones, ant, first_city);
                double[] cumulative = cumulative_sum(cities_probability);
               
                bool control = true;           
                while (control)
                {
                    double random = rnd.NextDouble();
                    int next_city = 0;
                    for (int j = 0; j < cumulative.Length; j++)
                    {
                        if (random < cumulative[j])
                        {
                            next_city = j;
                            break;
                        }
                    }
                    if (!direction.Contains(next_city))
                    {
                        control = false;
                        direction.Add(next_city);
                        distance_covered += city_distances[first_city, next_city];
                    }
                    else
                        control = true;
                }
                if (direction.Count == city_count-1)
                {
                    int[] temp = new int[city_count];
                    for (int k = 0; k < direction.Count; k++)
                    {
                        temp[direction[k]]++;
                    }
                    for (int j = 0; j < temp.Length; j++)
                    {
                        if (temp[j] == 0)
                        {
                            direction.Add(j);
                            break;
                        }
                    }
                    distance_covered += city_distances[direction[city_count - 2], direction[city_count-1]];
                    break;
                }              
            }
            ants_distance_covered.Add(distance_covered);
            ants_direction.Add(direction.ToArray());
        }
        double[] calcute_probability(double[,] city_distances, double[,] pheromones,Ant ant,int index)
        {
            double[] probabilities = new double[city_count];
            double total_probability = 0;

            for (int i = 0; i < city_count; i++)
            {
                probabilities[i] = Math.Pow(pheromones[index, i], ant.alfa) * Math.Pow(city_distances[index, i], ant.beta);
                total_probability += probabilities[i];
            }
            for (int i = 0; i < city_count; i++)
            {
                probabilities[i] /= total_probability;
            }
 
            return probabilities;
        }
        double[] cumulative_sum(double[] probability)
        {
            double[] cum = new double[probability.Length];
            double toplam = 0;
            for (int i = 0; i < probability.Length; i++)
            {
                toplam += probability[i];
                cum[i] = toplam;
            }
            return cum;
        }
        void update_pheromones(double[,] pheromones, double[,] city_distances, double ants_distance_covered, int[] ants_direction)
        {
            for (int i = 0; i < ants_direction.Length-1; i++)
            {
                pheromones[ants_direction[i], ants_direction[i + 1]] = (1 - evaporation_coefficient) * pheromones[ants_direction[i], ants_direction[i + 1]] + city_distances[ants_direction[i], ants_direction[i + 1]] / ants_distance_covered;
            }
        }





        void write(double[,] array, int length)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.Write(array[i,j]+" ");
                }
                Console.WriteLine();
            }
        }
        void write(List<City> cities)
        {
            for (int i = 0; i < cities.Count; i++)
            {
                Console.WriteLine(cities[i].axis_x+" / "+ cities[i].axis_y+" / "+ cities[i].id);
            }
        }
        void write(List<Ant> ants)
        {
            for (int i = 0; i < ants.Count; i++)
            {
                Console.WriteLine(ants[i].alfa + " / " + ants[i].beta);
            }
        }
        void write(List<double> list)
        {
            foreach(double d in list)
                Console.WriteLine(d);
        }
        void write(List<int[]> list)
        {
            foreach(int[] array in list)
            {
                foreach(int i in array)
                {
                    Console.Write(i+" + ");
                }
                Console.WriteLine();
            }
        }
        void write(int[] array,double value)
        {
            foreach (var item in array)
            {
                Console.Write(item+" - ");
            }
            Console.Write(" - "+value);
            Console.WriteLine();
        }
        void write(int[] array)
        {
            foreach (var item in array)
            {
                Console.Write(item+" - ");
            }
        }
    }
}
