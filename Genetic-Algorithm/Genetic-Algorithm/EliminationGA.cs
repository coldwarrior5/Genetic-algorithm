using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Genetic_Algorithm.Handlers;

namespace Genetic_Algorithm
{
	public class EliminationGa : Ga
	{
		public static int ParamSize { get; } = 4;
		private readonly InputData _data;

		public EliminationGa(float[][] inputs, float[] desiredOutputs, InputData data) : base(inputs, desiredOutputs)
		{
			_data = data;
			Population = new Genome[data.PopulationSize];
		}

		public override Genome Start()
		{
			int i = 0;
			Genome lastBest = new Genome(null);
			RandomPopulation();
			while (BestGenome.Fitness > _data.MinError && ++i < _data.MaxIterations)
			{
				lastBest.Copy(BestGenome);
				Parallel.For(0, _data.PopulationSize, ThreeTournament);
				if(BestGenome.Fitness < lastBest.Fitness)
					Console.WriteLine(i + " iteration. Current best: " + BestGenome.Genes + ", with fitness: " + BestGenome.Fitness);
			}

			return BestGenome;
		}

		private void ThreeTournament(int index)
		{
			List<int> choices = new List<int>(3);
			while (true)
			{
				int randNum = (Rand.Next(0, _data.PopulationSize) + index) % _data.PopulationSize;
				if (choices.Contains(randNum)) continue;
				choices.Add(randNum);
				break;
			}
			SortedList<float, Genome> order = new SortedList<float, Genome>(3);
			for (int i = 0; i < 3; i++)
			{
				Genome choice = Population[choices[i]];
				order.Add(choice.Fitness, choice);
			}

			var genome = order[2];
			Crossover(order[0], order[1], ref genome);
			if(Rand.NextDouble() < _data.MutationProbability)
				Mutation(ref genome);
		}

		private void RandomPopulation()
		{
			Parallel.For(0, _data.PopulationSize, i =>
			{
				float[] field = new float[ParamSize];
				for (int j = 0; j < ParamSize; j++)
				{
					field[j] = Functions.NewParamValue(Rand);
				}
				Population[i] = new Genome(field);
			});
			DeterminePopulationFitness();
		}
	}
}