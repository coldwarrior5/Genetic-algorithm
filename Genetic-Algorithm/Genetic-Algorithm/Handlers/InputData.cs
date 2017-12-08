namespace Genetic_Algorithm.Handlers
{
	public enum AlgorithmType { Generation, Elimination}

	public static class AlgorithmTypeString
	{
		public static AlgorithmType? Decode(string choice)
		{
			switch (choice)
			{
				case "e":
				case "E":
					return AlgorithmType.Elimination;
				case "g":
				case "G":
					return AlgorithmType.Generation;
				default:
					return null;
			}
		}
	}

	public class InputData
	{
		public string FileName;
		public AlgorithmType Type;
		public int PopulationSize;
		public float MutationProbability;
		public float CrossoverProbability;
		public float Mortality;
		public bool Elitism;
		public int MaxIterations;
		public double MinError;
	}
}