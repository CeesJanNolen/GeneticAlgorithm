using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GeneticAlgorithm
{
    internal class Program
    {
        public static Random Random = new Random();

        private static void Main(string[] args)
        {
            /* FUNCTIONS TO DEFINE (for each problem):
            Func<Ind> createIndividual;                                 ==> input is nothing, output is a new individual
            Func<Ind,double> computeFitness;                            ==> input is one individual, output is its fitness
            Func<Ind[],double[],Func<Tuple<Ind,Ind>>> selectTwoParents; ==> input is an array of individuals (population) and an array of corresponding fitnesses, output is a function which (without any input) returns a tuple with two individuals (parents)
            Func<Tuple<Ind, Ind>, Tuple<Ind, Ind>> crossover;           ==> input is a tuple with two individuals (parents), output is a tuple with two individuals (offspring/children)
            Func<Ind, double, Ind> mutation;                            ==> input is one individual and mutation rate, output is the mutated individual
            */

            const int populationSize = 20;
            const int maxIterations = 100;
            const double crossoverRate = 0.85;
            const double mutationRate = 0.1;
            const bool elitism = true;

            // Create new stopwatch.
            var stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            var geneticAlgorithm =
                new GeneticAlgorithm<Binary>(crossoverRate, mutationRate, elitism, populationSize,
                    maxIterations);
            var solution = geneticAlgorithm.Run(
                CreateIndividual,
                ComputeFitness,
                SelectTwoParents,
                Crossover,
                Mutation
            );
            // Stop timing.
            stopwatch.Stop();

            // Write result.
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

        }

        private static Binary CreateIndividual()
        {
            return new Binary(5, Random.Next(0, 32));
        }

        private static double ComputeFitness(Binary individual)
        {
            var value = individual.ToInt();
            return -Math.Pow(value, 2) + 7 * value;
        }

        private static Func<Tuple<Binary, Binary>> SelectTwoParents(Binary[] individuals, double[] fitnesses)
        {
            var parents = new List<Binary>();
            var lowestFitness = fitnesses.Min();
            fitnesses = fitnesses.Select(item => item + Math.Abs(lowestFitness)).ToArray();
            var totalFitness = fitnesses.Sum();
            //we need 2 individuals (parents)
            while (parents.Count < 2)
            {
                var randomvalue = new Random().NextDouble() * totalFitness;
                for (var j = 0; j < fitnesses.Length; j++)
                {
                    randomvalue -= fitnesses[j];
                    if (randomvalue <= 0)
                    {
                        parents.Add(individuals[j]);
                        break;
                    }
                }
            }
            return () => Tuple.Create(parents[0], parents[1]);
        }


        private static Tuple<Binary, Binary> Crossover(Tuple<Binary, Binary> parents)
        {
            var item1 = parents.Item1;
            var item2 = parents.Item2;
            var tmp = parents.Item2;

            var crossoverpoint = Random.Next(0, item1.Size);

            item2 = new Binary(item2.GetPart(0, crossoverpoint).Merge(item1.GetPart(crossoverpoint, item1.Size)));
            item1 = new Binary(item1.GetPart(0, crossoverpoint).Merge(tmp.GetPart(crossoverpoint, tmp.Size)));

            return Tuple.Create(item1, item2);
        }

        private static Binary Mutation(Binary individual, double mutationRate)
        {
            for (var i = 0; i < individual.Size; i++)
            {
                //do something with the values
                if (Random.Next(100) < mutationRate)
                {
                    individual.Switch(i);
                }
            }
            return individual;
        }
    }
}