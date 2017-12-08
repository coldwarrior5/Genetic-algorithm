using Genetic_Algorithm.Handlers;

namespace Genetic_Algorithm
{
	public class GenerationGa : Ga
	{
		private readonly InputData _data;

		public GenerationGa(float[][] inputs, float[] desiredOutputs, InputData data) : base(inputs, desiredOutputs)
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