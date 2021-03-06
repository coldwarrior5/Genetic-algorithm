﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
			int i = 0;
			int howManyDies = (int)(_data.Mortality * _data.PopulationSize);
			Genome lastBest = new Genome(null);
			RandomPopulation(Functions.ParamSize);
			Console.Write(i + " iteration. Current best: ");
			Program.PrintParameters(BestGenome.Genes);
			Console.WriteLine("with fitness: " + BestGenome.Fitness.ToString("G10"));
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
			Random rnd = new Random(index + DateTime.Now.Millisecond);
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

			Crossover(order[0], order[1], ref temp);
			for (int i = 0; i < temp.Genes.Length; i++)
			{
				if (Rand.NextDouble() < _data.MutationProbability)
					Mutation(ref temp, i);
			}
			DetermineGenomeFitness(ref temp);
			order[2].Copy(temp);
		}
	}
}