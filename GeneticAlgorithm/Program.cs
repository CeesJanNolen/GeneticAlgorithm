using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            /* FUNCTIONS TO DEFINE (for each problem):
            Func<Ind> createIndividual;                                 ==> input is nothing, output is a new individual
            Func<Ind,double> computeFitness;                            ==> input is one individual, output is its fitness
            Func<Ind[],double[],Func<Tuple<Ind,Ind>>> selectTwoParents; ==> input is an array of individuals (population) and an array of corresponding fitnesses, output is a function which (without any input) returns a tuple with two individuals (parents)
            Func<Tuple<Ind, Ind>, Tuple<Ind, Ind>> crossover;           ==> input is a tuple with two individuals (parents), output is a tuple with two individuals (offspring/children)
            Func<Ind, double, Ind> mutation;                            ==> input is one individual and mutation rate, output is the mutated individual
            */

            //TESTING BINARY CONVERTER
            const string binary = "0001";
            Console.WriteLine(BinaryConvert(binary, 2));

            //TESTING CROSSOVER
            var y = Tuple.Create(11111, 01101);
            y = Crossover(y);
            Console.WriteLine(y.Item1 + " | " + y.Item2);

            //TESTING Mutate
            var mutateY = 10101010;
            mutateY = Mutation(mutateY, 25);
            Console.WriteLine(mutateY);

//            GeneticAlgorithm<int> fakeProblemGA = new GeneticAlgorithm<int>(0.0, 0.0, false, 0, 0); // CHANGE THE GENERIC TYPE (NOW IT'S INT AS AN EXAMPLE) AND THE PARAMETERS VALUES
//            var solution = fakeProblemGA.Run(CreateIndividual<int>, null, null, null, null);
//            Console.WriteLine("Solution: ");
//            Console.WriteLine(solution);
        }


        static Ind CreateIndividual<Ind>()
        {
//            Digit * math.Pow(2, index)
            return default(Ind);
        }

        static double ComputeFitness<Ind>(Ind individual)
        {
            return 0.0;
        }

        static Func<Tuple<Ind, Ind>> selectTwoParents<Ind>(Ind[] individuals, double[] fitnesses)
        {
            return null;
        }


        static Tuple<Ind, Ind> Crossover<Ind>(Tuple<Ind, Ind> parents)
        {
            var item1 = parents.Item1 + "";
            var item2 = parents.Item2 + "";
            var tmp = parents.Item2 + "";

            var random = new Random();
            var crossoverpoint = random.Next(0,item1.Length );

            item2 = item2.Substring(0, crossoverpoint) + item1.Substring(crossoverpoint);
            item1 = item1.Substring(0, crossoverpoint) + tmp.Substring(crossoverpoint);

            var item1Casted = (Ind) Convert.ChangeType(item1, typeof(Ind));
            var item2Casted = (Ind) Convert.ChangeType(item2, typeof(Ind));
            return Tuple.Create(item1Casted, item2Casted);
        }

        static Ind Mutation<Ind>(Ind individual, double mutation_rate)
        {
            var ind = individual + "";
            var random = new Random();

            for (var i = 0; i < ind.Length; i++)
            {
                //do something with the values
                if (random.Next(100) < mutation_rate)
                {
                    var tempind = ind.Substring(0, i);
                    tempind += ind[i] == '1' ? '0' : '1';
                    if (i + 1 < ind.Length)
                        tempind += ind.Substring(i + 1);
                    ind = tempind;
                }
            }

            return (Ind) Convert.ChangeType(ind, typeof(Ind));
        }

        private static int BinaryConvert(string binary, int _base)
        {
            var result = 0;
            for (var i = 0; i < binary.Length; i++)
            {
                var digit = Convert.ToInt32("" + binary[i]);
                result += digit * (int) Math.Pow(_base, (binary.Length - 1) - i);
            }
            return result;
        }
    }
}