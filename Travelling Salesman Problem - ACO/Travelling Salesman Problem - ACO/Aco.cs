using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelling_Salesman_Problem___ACO
{
    class Aco
    {
        public int ant_population = 10;
        public int city_count = 10;
        public double evaporation_coefficient = 0.5;
        public double initial_pheromone = 1;

        public Aco()
        {
            List<City> cities = create_cities(city_count);
            //write(cities);

            List<Ant> ants = create_ants(ant_population);
            //write(ants);

            double[,] pheromones = initial_pheromones(city_count, initial_pheromone);
            //write(pheromones, city_count);

            double[,] city_distances = calcute_distance(cities);
            //write(city_distances, city_count);

            // rasgele şehirden başlayacak
            // kümülatif olasılık ile rasgele yolu seçeçek
            // geçtiği yolun feromen değeri değişecek

            // gitiiği şehir listeye eklenecek oraya bir daha gidemeyecek ?











        }

        List<City> create_cities(int city_count)
        {
            Random rnd = new Random();
            List<City> cities = new List<City>();
            int id = 1;
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
        List<Ant> create_ants(int ant_population)
        {
            Random rnd = new Random();
            List<Ant> ants = new List<Ant>();
            for (int i = 0; i < ant_population; i++)
            {
                ants.Add(new Ant(rnd.Next(1,4),rnd.Next(1,4)));
            }
            return ants;
        }   
        double[,] initial_pheromones(int length,double value)
        {
            double[,] array = new double[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    array[i, j] = value;
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
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
