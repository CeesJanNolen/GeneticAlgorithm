using System;
using System.Linq;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm<Ind>
    {
        double crossoverRate;
        double mutationRate;
        bool elitism;
        int populationSize;
        int numIterations;
        Random r = new Random();

        public GeneticAlgorithm(double crossoverRate, double mutationRate, bool elitism, int populationSize,
            int numIterations)
        {
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.elitism = elitism;
            this.populationSize = populationSize;
            this.numIterations = numIterations;
        }

        public Ind Run(Func<Ind> createIndividual, Func<Ind, double> computeFitness,
            Func<Ind[], double[], Func<Tuple<Ind, Ind>>> selectTwoParents,
            Func<Tuple<Ind, Ind>, Tuple<Ind, Ind>> crossover, Func<Ind, double, Ind> mutation)
        {
            // initialize the first population
            var initialPopulation = Enumerable.Range(0, populationSize).Select(i => createIndividual()).ToArray();

            var currentPopulation = initialPopulation;

            for (int generation = 0; generation < numIterations; generation++)
            {
                // compute fitness of each individual in the population
                var fitnesses = Enumerable.Range(0, populationSize)
                    .Select(i => computeFitness(currentPopulation[i]))
                    .ToArray();

                var nextPopulation = new Ind[populationSize];

                // apply elitism
                int startIndex;
                if (elitism)
                {
                    startIndex = 1;
                    var populationWithFitness = currentPopulation.Select(
                        (individual, index) => new Tuple<Ind, double>(individual, fitnesses[index]));
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
                for (int newInd = startIndex; newInd < populationSize; newInd++)
                {
                    // select two parents
                    var parents = getTwoParents();

                    // do a crossover between the selected parents to generate two children (with a certain probability, crossover does not happen and the two parents are kept unchanged)
                    Tuple<Ind, Ind> offspring;
                    if (r.NextDouble() < crossoverRate)
                        offspring = crossover(parents);
                    else
                        offspring = parents;

                    // save the two children in the next population (after mutation)
                    nextPopulation[newInd++] = mutation(offspring.Item1, mutationRate);
                    if (newInd < populationSize) //there is still space for the second children inside the population
                        nextPopulation[newInd] = mutation(offspring.Item2, mutationRate);
                }

                // the new population becomes the current one
                currentPopulation = nextPopulation;

                // in case it's needed, check here some convergence condition to terminate the generations loop earlier
            }

            // recompute the fitnesses on the final population and return the best individual
            var finalFitnesses = Enumerable.Range(0, populationSize)
                .Select(i => computeFitness(currentPopulation[i]))
                .ToArray();
            return currentPopulation
                .Select((individual, index) => new Tuple<Ind, double>(individual, finalFitnesses[index]))
                .OrderByDescending(tuple => tuple.Item2)
                .First()
                .Item1;
        }
    }
}