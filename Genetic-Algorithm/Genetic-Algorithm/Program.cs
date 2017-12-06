using System;
using Genetic_Algorithm.Handlers;

namespace Genetic_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Genetic algorithm");
	        Genome result = Start(args);
			Console.WriteLine("Proces is finished");
			Console.WriteLine("Best result is: ", result.Fitness + ", with parameters: " + result.Genes);
        }

	    static Genome Start(string[] args)
	    {
		    InputData inputData = IoHandler.GetParameters(args);
			FileHandler.ParseLines(inputData.FileName, out float[][] inputParams, out float[] desiredOutput);
			Ga algorithm = new GenerationGa(inputParams, desiredOutput, inputData.PopulationSize, inputData.MutationProbability, inputData.CrossoverProbability, inputData.Elitism);
		    Genome bestSolution = algorithm.Start();
		    return bestSolution;
	    }
    }
}
