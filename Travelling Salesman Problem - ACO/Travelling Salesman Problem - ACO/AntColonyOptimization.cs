using System;
using System.Collections.Generic;
using System.Linq;

namespace Travelling_Salesman_Problem___ACO
{
    class AntColonyOptimization
    {
        private int _antPopulation;
        private int _cityCount;
        private double _evaporationCoefficient;
        private double _initialPheromone;

        private List<City> _cities;
        private List<Ant> _ants;
        private double[,] _pheromones;
        private double[,] _cityDistances;
        private Random _rnd;

        public AntColonyOptimization(int antPopulation, int cityCount, double evaporationCoefficient, double initialPheromone)
        {
            _antPopulation = antPopulation;
            _cityCount = cityCount;
            _evaporationCoefficient = evaporationCoefficient;
            _initialPheromone = initialPheromone;
            _rnd = new Random();

            _cities = CreateCities();
            _ants = CreateAnts();
            _pheromones = InitialPheromones();
            _cityDistances = CalcuteDistance();
   
            List<double> antsDistanceCovered = new List<double>();
            List<int[]> antsDirection = new List<int[]>();

            int ant = 0;
            while (ant < _antPopulation)
            {
                Console.WriteLine($"___{ant+1}___");
                Walk(_ants[ant], antsDistanceCovered, antsDirection);
                UpdatePheromones(antsDistanceCovered[ant], antsDirection[ant]);
                Write(antsDirection[ant], antsDistanceCovered[ant]);
                Console.WriteLine();
                Write(_pheromones, _cityCount);
                Console.WriteLine();
                Console.WriteLine("-------------------------------------");
                ant++;
            }

            int max = 0;
            int min = 0;

            bool isMaxFound = false;
            bool isMinFound = false;

            for (int i = 0; i < antsDistanceCovered.Count; i++)
            {
                if (antsDistanceCovered[i] == antsDistanceCovered.Max())
                {
                    max = i;
                    isMaxFound = true;
                }
                else if (antsDistanceCovered[i] == antsDistanceCovered.Min())
                {
                    min = i;
                    isMinFound = true;
                }

                if(isMaxFound && isMinFound)
                {
                    break;
                }
            }          
            Console.Write($"Max: {antsDistanceCovered.Max()} /// ");
            Write(antsDirection[max].ToArray());
            Console.WriteLine();
            Console.Write($"Min: {antsDistanceCovered.Min()} /// ");
            Write(antsDirection[min].ToArray());
        }

        List<City> CreateCities()
        {
            List<City> cities = new List<City>();
            City firstCity = new City(_rnd.Next(-500,500),_rnd.Next(-500, 500),0);
            cities.Add(firstCity);
            for (int i = 1; i < _cityCount; i++)
            {
                bool control = true;
                bool isExist = false;
                while(control)
                {
                    City city = new City(_rnd.Next(-500, 500), _rnd.Next(-500, 500), i);
                    for (int j = 0; j < cities.Count; j++)
                    {
                        if (cities[j].AxisX == city.AxisX && cities[j].AxisY == city.AxisY)
                        {
                            isExist = true;
                            j = _cityCount;
                        }
                    }
                    if (isExist)
                    {
                        control = true;
                    }
                    else
                    {
                        cities.Add(city);
                        control = false;
                    }               
                }
            }
            return cities;
        }
        List<Ant> CreateAnts()
        {
            List<Ant> ants = new List<Ant>();
            for (int i = 0; i < _antPopulation; i++)
            {
                ants.Add(new Ant(_rnd.Next(1,5),_rnd.Next(1,5)));
            }
            return ants;
        }   
        double[,] InitialPheromones()
        {
            double[,] pheromones = new double[_cityCount, _cityCount];
            for (int i = 0; i < _cityCount; i++)
            {
                for (int j = 0; j < _cityCount; j++)
                {
                    pheromones[i, j] = _initialPheromone;
                }
            }
            return pheromones;
        }
        double[,] CalcuteDistance()
        {
            double[,] distances = new double[_cityCount, _cityCount];
            for (int i = 0; i < _cityCount; i++)
            {
                for (int j = 0; j < _cityCount; j++)
                {
                    distances[i, j] = Math.Sqrt(Math.Pow(_cities[i].AxisX - _cities[j].AxisX, 2) + Math.Pow(_cities[i].AxisY - _cities[j].AxisY, 2));
                }
            }
            return distances;
        }
        void Walk(Ant ant, List<double> antsDistanceCovered, List<int[]> antsDirection)
        {
            double distanceCovered = 0;
            List<int> direction = new List<int>();
            int firstCity = _rnd.Next(0, _cityCount);
            direction.Add(firstCity);            
            for (int i = 1; i < _cityCount; i++)
            {
                int currentCity = direction.Last();
                double[] citiesProbabilities = CalcuteProbabilities(direction, ant, currentCity);
                double[] cumulatives = CumulativeSum(citiesProbabilities);              
                bool control = true;           
                while (control)
                {
                    double random = _rnd.NextDouble();
                    int nextCity = 0;
                    for (int j = 0; j < cumulatives.Length; j++)
                    {
                        if (random < cumulatives[j])
                        {
                            nextCity = j;
                            break;
                        }
                    }
                    if (!direction.Contains(nextCity))
                    {
                        control = false;
                        direction.Add(nextCity);
                        distanceCovered += _cityDistances[currentCity, nextCity];
                    }
                    else
                    {
                        control = true;
                    }
                }               
            }
            antsDistanceCovered.Add(distanceCovered);
            antsDirection.Add(direction.ToArray());
        }
        double[] CalcuteProbabilities(List<int> direction,Ant ant,int index)
        {
            double[] probabilities = new double[_cityCount];
            double totalProbability = 0;

            for (int i = 0; i < _cityCount; i++)
            {
                if (!direction.Contains(i))
                {
                    probabilities[i] = Math.Pow(_pheromones[index, i], ant.Alpha) * Math.Pow(_cityDistances[index, i], ant.Beta);
                }
                else
                {
                    probabilities[i] = 0;
                }
                totalProbability += probabilities[i];
            }
            for (int i = 0; i < _cityCount; i++)
            {
                probabilities[i] /= totalProbability;
            }
 
            return probabilities;
        }
        double[] CumulativeSum(double[] probability)
        {
            double[] cum = new double[probability.Length];
            double total = 0;
            for (int i = 0; i < probability.Length; i++)
            {
                total += probability[i];
                cum[i] = total;
            }
            return cum;
        }
        void UpdatePheromones(double antsDistanceCovered, int[] antsDirection)
        {
            for (int i = 0; i < antsDirection.Length-1; i++)
            {
                _pheromones[antsDirection[i], antsDirection[i + 1]] = ((1 - _evaporationCoefficient) * _pheromones[antsDirection[i], antsDirection[i + 1]]) + 
                    (_cityDistances[antsDirection[i], antsDirection[i + 1]] / antsDistanceCovered);
            }
        }
        void Write(double[,] array, int length)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.Write($"{array[i, j].ToString("0.00")} ");
                }
                Console.WriteLine();
            }
        }
        void Write(int[] array,double value)
        {
            foreach (var item in array)
            {
                Console.Write($"{item} - ");
            }
            Console.Write($" - {value}");
            Console.WriteLine();
        }
        void Write(int[] array)
        {
            foreach (var item in array)
            {
                Console.Write($"{item} - ");
            }
        }
    
    }
}
