namespace Genetic_Algorithm
{
	public class EliminationGa : Ga
	{
		private readonly float _mutationProbability;
		private readonly float _mortality;

		public EliminationGa(float[][] inputs, float[] desiredOutputs, int populationSize, float mutationProbability, float mortality) : base(inputs, desiredOutputs)
		{
			PopulationSize = populationSize;
			Population = new Genome[PopulationSize];
			_mutationProbability = mutationProbability;
			_mortality = mortality;
		}

		public override Genome Start()
		{
			// TODO main logic

			return BestGenome;
		}
	}
}