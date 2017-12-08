using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Genetic_Algorithm.Handlers;

namespace Genetic_Algorithm
{
	public class EliminationGa : Ga
	{
		public static int ParamSize { get; } = 5;
		private readonly InputData _data;

		public EliminationGa(float[][] inputs, float[] desiredOutputs, InputData data) : base(inputs, desiredOutputs)
		{
			_data = data;
			Population = new Genome[data.PopulationSize];
		}

		public override Genome Start()
		{
			int i = 0;
			int howManyDies = (int)(_data.Mortality * _data.PopulationSize);
			Genome lastBest = new Genome(null);
			RandomPopulation();
			while (BestGenome.Fitness > _data.MinError && ++i < _data.MaxIterations)
			{
				lastBest.Copy(BestGenome);
				Parallel.For(0, howManyDies, ThreeTournament);	// Mortality determines how many times we should do the Tournaments
				DetermineBestFitness();
				if (!(BestGenome.Fitness < lastBest.Fitness)) continue;
				Console.Write(i + " iteration. Current best: ");
				Program.PrintParameters(BestGenome.Genes);
				Console.WriteLine("with fitness: " +  BestGenome.Fitness.ToString("G10"));
			}
			return BestGenome;
		}

		private void ThreeTournament(int index)
		{
			Random rnd = new Random(index);
			List<int> choices = new List<int>(3);
			while (true)
			{
				int randNum = rnd.Next(1, _data.PopulationSize);
				if (choices.Contains(randNum)) continue;
				choices.Add(randNum);
				if(choices.Count == 3)
					break;
			}

			List<Genome> order = new List<Genome>(3);
			for (int i = 0; i < 3; i++)
			{
				Genome choice = Population[choices[i]];
				order.Add(choice);
			}

			Genome temp = new Genome(order[2].Genes);
			Order(order);
			temp.Copy(order[2]);

			Crossover(order[0], order[1], ref temp);
			if (Rand.NextDouble() < _data.MutationProbability)
				Mutation(ref temp);
			DetermineGenomeFitness(ref temp);
			order[2].Copy(temp);
		}

		// ReSharper disable once RedundantAssignment
		private static void Order(List<Genome> order)
		{
			Genome temp;
			Genome temp2;
			double worstFitness = float.MinValue;
			int worstIndex = 2;
			double bestFitness = float.MaxValue;
			int bestIndex = 0;

			for (int i = 0; i < 3; i++)
			{
				if (order[i].Fitness > worstFitness)
				{
					worstFitness = order[i].Fitness;
					worstIndex = i;
				}
					
				if (order[i].Fitness < bestFitness)
				{
					bestFitness = order[i].Fitness;
					bestIndex = i;
				}
			}

			switch (bestIndex)
			{
				case 0 when worstIndex == 2:
					return;
				case 0:
					temp = order[worstIndex];
					order[worstIndex] = order[2];
					order[2] = temp;
					break;
				case 1 when worstIndex == 2:
					temp = order[bestIndex];
					order[bestIndex] = order[0];
					order[0] = temp;
					break;
				case 1:
					temp = order[worstIndex];
					order[worstIndex] = order[2];
					order[2] = temp;
					temp2 = order[bestIndex];
					order[bestIndex] = order[0];
					order[0] = temp2;
					break;
				case 2 when worstIndex == 0:
					temp = order[bestIndex];
					order[bestIndex] = order[0];
					order[0] = temp;
					break;
				case 2:
					temp = order[bestIndex];
					order[bestIndex] = order[0];
					order[0] = temp;
					temp2 = order[worstIndex];
					order[worstIndex] = order[2];
					order[2] = temp2;
					break;
			}
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

		public Genome Test()
		{
			Random rnd = new Random();
			RandomPopulation();
			List<int> choices = new List<int>(3);
			while (true)
			{
				int randNum = rnd.Next(1, _data.PopulationSize);
				if (choices.Contains(randNum)) continue;
				choices.Add(randNum);
				if (choices.Count == 3)
					break;
			}

			List<Genome> order = new List<Genome>(3);
			for (int i = 0; i < 3; i++)
			{
				Genome choice = Population[choices[i]];
				order.Add(choice);
			}

			Genome temp = new Genome(new float[]{});
			Order(order);
			temp.Copy(order[2]);

			Crossover(order[0], order[1], ref temp);
			if (Rand.NextDouble() < _data.MutationProbability)
				Mutation(ref temp);
			DetermineGenomeFitness(ref temp);
			order[2] = temp;
			return BestGenome;
		}
	}
}