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
			Console.Write("Best result is: ", result.Fitness + ", with parameters: ");
			PrintParameters(result.Genes);
			Console.WriteLine("with fitness: " + result.Fitness.ToString("G10"));
        }

	    static Genome Start(string[] args)
	    {
		    InputData inputData = IoHandler.GetParameters(args);
			FileHandler.ParseLines(inputData.FileName, out float[][] inputParams, out float[] desiredOutput);
		    Ga algorithm = null;
		    switch (inputData.Type)
		    {
				case AlgorithmType.Generation:
					algorithm = new GenerationGa(inputParams, desiredOutput, inputData);
					break;
				case AlgorithmType.Elimination:
					algorithm = new EliminationGa(inputParams, desiredOutput, inputData);
					break;
			}
		    Genome bestSolution = algorithm?.Start();
		    //EliminationGa ab = new EliminationGa(inputParams, desiredOutput, inputData);
		    //Genome bestSolution = ab.Test();
			return bestSolution;
	    }

	    public static void PrintParameters(float[] param)
	    {
		    foreach (float f in param)
		    {
				Console.Write(f + ", ");
		    }
	    }
    }
}
