using System;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
	public abstract class Ga
	{
		private readonly int _testSize;
		protected int PopulationSize;
		private readonly float[][] _inputs;
		private readonly float[] _desiredOutput;
		protected Genome[] Population;
		protected Genome BestGenome;

        protected Ga(float[][] inputs, float[] desiredOutputs)
		{
			if(inputs.Length != desiredOutputs.Length)
				throw new Exception("Number of inputs is not equal to number of outputs");
			_testSize = inputs.Length;
			_inputs = inputs;
			_desiredOutput = desiredOutputs;
		}

		public abstract Genome Start();

		public void DeterminePopulationFitness()
		{
			object syncObject = new object();

			Parallel.ForEach(Population, ()=> new Genome(null, float.MinValue), (genome, loopState, localState) =>
			{
				DetermineGenomeFitness(genome);
				return localState.Fitness > genome.Fitness ? localState : genome;
			},
			localState =>
			{
				lock (syncObject)
					BestGenome = localState.Fitness > BestGenome.Fitness ? localState : BestGenome;
			});
			
			Parallel.For(0, PopulationSize, i =>
			{
				DetermineGenomeFitness(Population[i]);
			});
		}

        private void DetermineGenomeFitness(Genome genome)
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