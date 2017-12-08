using Genetic_Algorithm.Handlers;

namespace Genetic_Algorithm
{
	public class EliminationGa : Ga
	{
		private readonly InputData _data;

		public EliminationGa(float[][] inputs, float[] desiredOutputs, InputData data) : base(inputs, desiredOutputs)
		{
			_data = data;
			Population = new Genome[data.PopulationSize];
		}

		public override Genome Start()
		{
			// TODO main logic

			return BestGenome;
		}
	}
}