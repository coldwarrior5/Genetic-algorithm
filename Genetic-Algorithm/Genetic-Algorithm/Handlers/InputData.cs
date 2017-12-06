namespace Genetic_Algorithm.Handlers
{
	public enum AlgorithmType { Generation, Elimination}

	public static class AlgorithmTypeString
	{
		public static AlgorithmType? Decode(string choice)
		{
			switch (choice)
			{
				case "E":
					return AlgorithmType.Elimination;
				case "G":
					return AlgorithmType.Generation;
				default:
					return null;
			}
		}
	}

	public class InputData
	{
		private string _fileName;
		private AlgorithmType _algorithmType;
		private int _populationSize;
		private float _mutationProbability;
		private float _crossoverProbability;
		private float _mortality;
		private bool _elitism;


		public string FileName
		{
			get => _fileName;
			set => _fileName = value;
		}

		public float MutationProbability
		{
			get => _mutationProbability;
			set => _mutationProbability = value;
		}

		public AlgorithmType Type
		{
			get => _algorithmType;
			set => _algorithmType = value;
		}

		public float CrossoverProbability
		{
			get => _crossoverProbability;
			set => _crossoverProbability = value;
		}

		public float Mortality
		{
			get => _mortality;
			set => _mortality = value;
		}

		public bool Elitism
		{
			get => _elitism;
			set => _elitism = value;
		}

		public int PopulationSize
		{
			get => _populationSize;
			set => _populationSize = value;
		}
	}
}