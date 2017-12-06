namespace Genetic_Algorithm
{
	public class GenerationGa : Ga
	{
		private readonly float _mutationProbability;
		private readonly float _crossoverProbability;
		private readonly bool _elitism;
		public GenerationGa(float[][] inputs, float[] desiredOutputs, int populationSize, float mutationProbability, float crossoverProbability, bool elitism) : base(inputs, desiredOutputs)
		{
			PopulationSize = populationSize;
			Population = new Genome[PopulationSize];
			_mutationProbability = mutationProbability;
			_crossoverProbability = crossoverProbability;
			_elitism = elitism;
		}

		public override Genome Start()
		{
			// TODO main logic

			return BestGenome;
		}
	}
}