using System;

namespace SimpleGA
{
    class Program
    {
        static void Main(string[] args)
        {
            FitFunc fitFunc = new FitFunc();
            GeneticAlgorithm ga = new GeneticAlgorithm(fitFunc.Fitness);

            ga.Run();
            Console.ReadLine();
        }


        public class FitFunc
        {
            char[] goal = { 'H', 'e', 'l', 'l', 'o', ' ', 'W', 'o', 'r', 'l', 'd', '!' };
            public int Fitness(Gene g)
            {
                int fitness = goal.Length * 5;
                for (int i = 0; i < g.genes.Length; i++)
                {
                    int diff = Math.Abs(g.genes[i] - goal[i]);
                    fitness -= diff;
                }
                return fitness;
            }

        }

    }
}
