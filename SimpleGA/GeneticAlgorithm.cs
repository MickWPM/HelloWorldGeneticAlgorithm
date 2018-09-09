using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGA
{
    class GeneticAlgorithm
    {
        System.Random rng;
        System.Func<Gene, int> FitnessFunction;
        int maxGenerations = 10000;
        int population = 500;
        int eliteCount = 5;
        int parentCount = 250;
        double mutationRate = 0.05;
        int mutationRange = 10;

        Gene[] genes;

        string output;

        public GeneticAlgorithm(System.Func<Gene, int> fitnessFunction)
        {
            this.FitnessFunction = fitnessFunction;
            rng = new System.Random();
        }


        public void Run()
        {
            output = "";
            //Prelim
            PopulateGenes();
            Console.WriteLine("Running...");
            //Generations
            bool newGeneration = true;
            bool allResults = false;
            int generationNumber = 0;
            int outputPeriod = maxGenerations / 20;
            while (newGeneration)
            {
                MutatePopulation();
                //0 index is lowest fitness
                Sort();

                //Temp 'selection'
                Select();
                Reproduce();
                if (generationNumber % outputPeriod == 0)
                    Console.WriteLine("Still running... " + 100*(float)generationNumber / maxGenerations + "%");

                //Write results if we want
                if (allResults)
                {
                    Sort();
                    output += " " + AssessFitness(genes[population - 1]);
                    Console.WriteLine("Generation " + generationNumber + ":");
                    ConsoleResults();
                }

                Sort();
                int bestFitness = AssessFitness(genes[population - 1]);
                output += " " + bestFitness;
                if (bestFitness == 60)
                    newGeneration = false;
                //Final check
                if (++generationNumber >= maxGenerations) newGeneration = false;
            }
            Console.WriteLine("Final results after " + generationNumber + " generations:");
            ConsoleResults();


            output += "";
            string[] lines = new string[1];
            lines[0] = output;
            System.IO.File.WriteAllLines(@"D:\GAResults.txt", lines);

        }

        Gene[] elites;
        Gene[] parents;
        
        void Select()
        {
            elites = new Gene[eliteCount];
            for (int i = 0; i < eliteCount; i++)
            {
                elites[i] = genes[population - i - 1];
            }

            parents = new Gene[parentCount];
            for (int i = 0; i < parentCount; i++)
            {
                parents[i] = genes[population - i - 1];
                //    Gene parentA = genes[population - i * 2 - 1];
                //    Gene parentB = genes[population - i * 2 - 2];
            }
        }

        void Reproduce()
        {
            Gene[] newGenes = new Gene[population];
            int j = 0;
            for (int i = 0; i < elites.Length; i++)
            {
                j++;
                newGenes[i] = elites[i];
            }
            for (int i = 0; i < parents.Length-1; i++)
            {
                j++;
                newGenes[elites.Length+i] = Crossover(parents[i], parents[i+1]);
            }

            for (int i = j; i < genes.Length; i++)
            {
                newGenes[i] = new Gene();
            }
            genes = newGenes;
        }



        Gene Crossover(Gene A, Gene B)
        {
            char[] childGenes = new char[Gene.GeneLength];
            for (int i = 0; i < A.genes.Length; i++)
            {
                if (rng.NextDouble() > 0.5)
                {
                    childGenes[i] = A.genes[i];
                } else
                {
                    childGenes[i] = B.genes[i];
                }
            }
            return new Gene(childGenes);
        }

        
        void MutatePopulation()
        {
            for (int i = 0; i < population; i++)
            {
                Gene g = genes[i];
                for (int j = 0; j < g.genes.Length; j++)
                {
                    if (rng.NextDouble() < mutationRate)
                    {
                        g.Mutate(j, rng.Next(1, mutationRange + 1));
                    }
                }
            }
        }

        void Sort()
        {
            Array.Sort(genes,
            delegate (Gene x, Gene y) { return AssessFitness(x).CompareTo(AssessFitness(y)); });

        }

        void Writeall()
        {
            for (int i = 0; i < genes.Length; i++)
            {
                Console.WriteLine("Gene: '" + genes[i] + "', fitness = " + AssessFitness(genes[i]));
            }
        }


        void ConsoleResults()
        {
            int maxFitness = int.MinValue;
            int bestIndex = -1;
            for (int i = 0; i < genes.Length; i++)
            {
                int thisFitness = AssessFitness(genes[i]);
                if (thisFitness > maxFitness)
                {
                    bestIndex = i;
                    maxFitness = thisFitness;
                }
            }

            Console.WriteLine("Best gene: " + genes[bestIndex] + " with a fitness score of " + maxFitness);
        }

        void PopulateGenes()
        {
            genes = new Gene[population];
            for (int i = 0; i < population; i++)
            {
                genes[i] = new Gene();
            }
        }


        public int AssessFitness(Gene gene)
        {
            return FitnessFunction(gene);
        }
        
    }
}
