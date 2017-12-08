using System;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
	public abstract class Ga
	{
		private readonly int _testSize;
		private readonly float[][] _inputs;
		private readonly float[] _desiredOutput;
		protected Genome[] Population;
		protected readonly Genome BestGenome;
		protected readonly Random Rand;

		protected Ga(float[][] inputs, float[] desiredOutputs)
		{
			if(inputs.Length != desiredOutputs.Length)
				throw new Exception("Number of inputs is not equal to number of outputs");
			_testSize = inputs.Length;
			_inputs = inputs;
			_desiredOutput = desiredOutputs;
			Rand = new Random();
			BestGenome = new Genome(new float[]{}, float.MaxValue);
		}

		public abstract Genome Start();

		protected void DeterminePopulationFitness()
		{
			object syncObject = new object();

			Parallel.ForEach(Population, ()=> new Genome(new float[]{}, float.MaxValue), (genome, loopState, localState) =>
			{
				DetermineGenomeFitness(ref genome);
				return genome.Fitness < localState.Fitness ? genome : localState;
			},
			localState =>
			{
				lock (syncObject)
				{
					if(localState.Fitness < BestGenome.Fitness)
						BestGenome.Copy(localState);
				} 
			});
		}
		
		protected void DetermineBestFitness()
		{
			object syncObject = new object();
			foreach (Genome t in Population)
			{
				lock (syncObject)
				{
					if(t.Fitness < BestGenome.Fitness)
						BestGenome.Copy(t);
				}
			}
		}

        protected void DetermineGenomeFitness(ref Genome genome)
		{
			float[] givenOutput = new float[_testSize];
			var gene = genome.Genes;
			Parallel.For(0, _testSize, i =>
			{
				givenOutput[i] = Functions.F1(_inputs[i][0], _inputs[i][1], gene);
			});
			genome.Fitness = FitnessFunctions.Fitness1(_desiredOutput, givenOutput);
		}

		protected void Crossover(Genome first, Genome second, ref Genome child)
		{
			int which =Rand.Next(0, 4);
			switch (which)
			{
				case 0:
					CrossoverMethods.DiscreteRecombination(first, second, ref child, Rand);
					break;
				case 1:
					CrossoverMethods.SimpleArithmeticRecombination(first, second, ref child, Rand);
					break;
				case 2:
					CrossoverMethods.SingleArithmeticRecombination(first, second, ref child, Rand);
					break;
				case 3:
					CrossoverMethods.WholeArithmeticRecombination(first, second, ref child, Rand);
					break;
				default:
					child = null;
					break;
			}
		}

		protected void Mutation(ref Genome gene)
		{
			int which = Rand.Next(0, 2);
			switch (which)
			{
				case 0:
					MutationMethods.SimpleMutation(ref gene, Rand);
					break;
				case 1:
					MutationMethods.BoundaryMutation(ref gene, Rand);
					break;
				default:
					gene = null;
					break;
			}
		}
	}

	public static class CrossoverMethods
	{
		public static void DiscreteRecombination(Genome firstParent, Genome secondParent, ref Genome firstChild, Random rand)
		{
			firstChild.Fitness = Single.MaxValue;
			for (int i = 0; i < firstChild.Genes.Length; i++)
			{
				firstChild.Genes[i] = rand.NextDouble() > 0.5 ? firstParent.Genes[i] : secondParent.Genes[i];
			}
		}

		public static void SimpleArithmeticRecombination(Genome firstParent, Genome secondParent, ref Genome firstChild, Random rand)
		{
			int location = rand.Next(0, firstChild.Genes.Length);
			firstChild.Copy(firstParent);
			firstChild.Fitness = Single.MaxValue;
			for (int i = location; i < firstParent.Genes.Length; i++)
			{
				firstChild.Genes[i] = Average(firstParent.Genes[i], secondParent.Genes[i]);
			}
		}

		public static void SingleArithmeticRecombination(Genome firstParent, Genome secondParent, ref Genome firstChild, Random rand)
		{
			int location = rand.Next(0, firstChild.Genes.Length);
			firstChild.Copy(firstParent);
			firstChild.Fitness = Single.MaxValue;
			firstChild.Genes[location] = Average(firstParent.Genes[location], secondParent.Genes[location]);
		}

		public static void WholeArithmeticRecombination(Genome firstParent, Genome secondParent, ref Genome firstChild, Random rand)
		{
			firstChild.Fitness = Single.MaxValue;
			for (int i = 0; i < firstParent.Genes.Length; i++)
			{
				firstChild.Genes[i] = Average(firstParent.Genes[i], secondParent.Genes[i]);
			}
		}

		private static float Average(float first, float second)
		{
			return first + (second - first) / 2;
		}
	}

	public static class MutationMethods
	{
		public static void SimpleMutation(ref Genome gene, Random rand)
		{
			int location = rand.Next(0, gene.Genes.Length);
			gene.Genes[location] = Functions.NewParamValue(rand);
		}

		public static void BoundaryMutation(ref Genome gene, Random rand)
		{
			int location = rand.Next(0, gene.Genes.Length);
			gene.Genes[location] = rand.Next(0, 2) > 0 ? Functions.MaxValue : Functions.MinValue;
		}
	}
}