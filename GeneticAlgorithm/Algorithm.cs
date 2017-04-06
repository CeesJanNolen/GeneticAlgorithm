using System;
using System.Linq;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm<TInd>
    {
        private readonly double _crossoverRate;
        private readonly double _mutationRate;
        private readonly bool _elitism;
        private readonly int _populationSize;
        private readonly int _numIterations;
        private readonly Random _random = new Random();

        public GeneticAlgorithm(double crossoverRate, double mutationRate, bool elitism, int populationSize,
            int numIterations)
        {
            _crossoverRate = crossoverRate;
            _mutationRate = mutationRate;
            _elitism = elitism;
            _populationSize = populationSize;
            _numIterations = numIterations;
        }

        public TInd Run(Func<TInd> createIndividual, Func<TInd, double> computeFitness,
            Func<TInd[], double[], Func<Tuple<TInd, TInd>>> selectTwoParents,
            Func<Tuple<TInd, TInd>, Tuple<TInd, TInd>> crossover, Func<TInd, double, TInd> mutation)
        {
            // initialize the first population
            var initialPopulation = Enumerable.Range(0, _populationSize).Select(i => createIndividual()).ToArray();

            var currentPopulation = initialPopulation;

            for (var generation = 0; generation < _numIterations; generation++)
            {
                // compute fitness of each individual in the population
                var fitnesses = Enumerable.Range(0, _populationSize)
                    .Select(i => computeFitness(currentPopulation[i]))
                    .ToArray();

                var nextPopulation = new TInd[_populationSize];

                // apply elitism
                int startIndex;
                if (_elitism)
                {
                    startIndex = 1;
                    var populationWithFitness = currentPopulation.Select(
                        (individual, index) => new Tuple<TInd, double>(individual, fitnesses[index]));
                    var populationSorted =
                        populationWithFitness.OrderByDescending(tuple => tuple.Item2); // item2 is the fitness
                    var bestIndividual = populationSorted.First();
                    nextPopulation[0] = bestIndividual.Item1;
                }
                else
                {
                    startIndex = 0;
                }

                // initialize the selection function given the current individuals and their fitnesses
                var getTwoParents = selectTwoParents(currentPopulation, fitnesses);

                // create the individuals of the next generation
                for (var newInd = startIndex; newInd < _populationSize; newInd++)
                {
                    // select two parents
                    var parents = getTwoParents();

                    // do a crossover between the selected parents to generate two children (with a certain probability, crossover does not happen and the two parents are kept unchanged)
                    Tuple<TInd, TInd> offspring;
                    if (_random.NextDouble() < _crossoverRate)
                        offspring = crossover(parents);
                    else
                        offspring = parents;

                    // save the two children in the next population (after mutation)
                    nextPopulation[newInd++] = mutation(offspring.Item1, _mutationRate);
                    if (newInd < _populationSize) //there is still space for the second children inside the population
                        nextPopulation[newInd] = mutation(offspring.Item2, _mutationRate);
                }

                // the new population becomes the current one
                currentPopulation = nextPopulation;

                // in case it's needed, check here some convergence condition to terminate the generations loop earlier
            }

            // recompute the fitnesses on the final population and return the best individual
            var finalFitnesses = Enumerable.Range(0, _populationSize)
                .Select(i => computeFitness(currentPopulation[i]))
                .ToArray();

            var topIndividual = currentPopulation
                .Select((individual, index) => new Tuple<TInd, double>(individual, finalFitnesses[index]))
                .OrderByDescending(tuple => tuple.Item2)
                .First();

            Console.WriteLine("*************************");
            Console.WriteLine("End of Genetic Algorithm");
            Console.WriteLine("*************************");
            Console.WriteLine("The average fitness of the last population: " + finalFitnesses.Average());
            Console.WriteLine("The fitness of the best Individual: " + topIndividual.Item2);
            Console.WriteLine("The best Individual: " + topIndividual.Item1 + ". The integer value of him: " +
                              topIndividual.Item1.ToString().BinaryConvert(2));

            return currentPopulation
                .Select((individual, index) => new Tuple<TInd, double>(individual, finalFitnesses[index]))
                .OrderByDescending(tuple => tuple.Item2)
                .First()
                .Item1;
        }
    }
}