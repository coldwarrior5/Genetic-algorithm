using System;
using System.Threading.Tasks;
using Genetic_Algorithm.Handlers;

namespace Genetic_Algorithm
{
	public class GenerationGa : Ga
	{
		private readonly Genome[] _tempPopulation;
		private readonly InputData _data;

		public GenerationGa(float[][] inputs, float[] desiredOutputs, InputData data) : base(inputs, desiredOutputs)
		{
			_data = data;
			Population = new Genome[_data.PopulationSize];
			_tempPopulation = new Genome[_data.PopulationSize];
			RandomPopulation(Functions.ParamSize, _tempPopulation);
		}

		public override Genome Start()
		{
			int i = 0;
			Genome lastBest = new Genome(null);
			RandomPopulation(Functions.ParamSize);
			Console.Write(i + " iteration. Current best: ");
			Program.PrintParameters(BestGenome.Genes);
			Console.WriteLine("with fitness: " + BestGenome.Fitness.ToString("G10"));
			while (BestGenome.Fitness > _data.MinError && ++i < _data.MaxIterations)
			{
				lastBest.Copy(BestGenome);
				if (_data.Elitism)
					_tempPopulation[0].Copy(BestGenome);
				double unused = CalculateFitness();
				Parallel.For(_data.Elitism ? 1 : 0, _data.PopulationSize, SingleThread);
				SwapBuffers();
				DeterminePopulationFitness();
				if (!(BestGenome.Fitness < lastBest.Fitness)) continue;
				Console.Write(i + " iteration. Current best: ");
				Program.PrintParameters(BestGenome.Genes);
				Console.WriteLine("with fitness: " + BestGenome.Fitness.ToString("G10"));
			}
			return BestGenome;
		}

		private void SingleThread(int index)
		{
			Random rand = new Random(index);
			int firstParentId = RouletteWheelSelection(rand);
			int secondParentId = RouletteWheelSelection(rand);
			while (secondParentId == firstParentId)
			{
				secondParentId = RouletteWheelSelection(rand);
			}
			

			Genome temp = new Genome(new float[5]);
			double randCross = rand.NextDouble();
			if (randCross < _data.CrossoverProbability)
			{
				Crossover(Population[firstParentId], Population[secondParentId], ref temp);
			}
			else
			{
				for (int i = 0; i < temp.Genes.Length; i++)
				{
					temp.Genes[i] = Functions.NewParamValue(rand);
				}
			}
			for (int i = 0; i < _tempPopulation[index].Genes.Length; i++)
			{
				if (rand.NextDouble() < _data.MutationProbability)
					Mutation(ref _tempPopulation[index], i);
			}
			_tempPopulation[index].Copy(temp);
		}

		private void SwapBuffers()
		{
			Parallel.For(0, _data.PopulationSize, i =>
			{
				Population[i].Copy(_tempPopulation[i]);
			});
		}

		private double CalculateFitness()
		{
			double sum = 0;
			for (int i = 0; i < Population.Length; i++)
			{
				sum += Population[i].Fitness;
			}
			return sum;
		}
	}
}