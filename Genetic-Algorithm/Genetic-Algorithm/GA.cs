using System;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
	public class GA
	{
		private int _testSize;
		private int _populationSize;
		private readonly float[][] _inputs;
		private readonly float[] _desiredOutput;
		private Genome[] Population;

        public GA(float[][] inputs, float[] desiredOutputs)
		{
			if(inputs.Length != desiredOutputs.Length)
				throw new Exception("Number of inputs is not equal to number of outputs");
			_testSize = _inputs.Length;
			_inputs = inputs;
			_desiredOutput = desiredOutputs;
		}

		public void DeterminePopulationFitness()
		{
			Parallel.For(0, _populationSize, i =>
			{
				DetermineGenomeFitness(Population[i]);
			});
		}

        public void DetermineGenomeFitness(Genome genome)
		{
			float[] givenOutput = new float[_testSize];
			Parallel.For(0, _testSize, i =>
			{
				givenOutput[i] = Functions.F1(_inputs[i][0], _inputs[i][1], genome.Genes);
			});
			genome.Fitness = FitnessFunctions.Fitness1(_desiredOutput, givenOutput);
		}
	}
}