using System;
using System.IO;
using System.Linq;

namespace Genetic_Algorithm.Handlers
{
	public static class IoHandler
	{
		private static readonly string[] TerminationExpressions = {"quit", "stop", "exit", "terminate", "q"};

		public static InputData GetParameters(string[] args)
		{
			InputData data;

			switch (args.Length)
			{
				case 0:
					data = UserInput();
					break;
				case 5:
					data = HandleArgs(args);
					break;
				case 6:
					data = HandleArgs(args);
					break;
				default:
					ErrorHandler.TerminateExecution(ErrorCode.InvalidNumberOfArguments);
					data = null;
					break;
			}
			return data;
		}

		/// <summary>
		/// Special method that ensures correct input parameters for specified genetic algorithm from user
		/// </summary>
		/// <returns>Struct defining input parameters</returns>
		private static InputData UserInput()
		{
			InputData inputData = new InputData();
			NotifyUserOfTermination();
			Console.WriteLine("Filename, including path, of test data.");
			inputData.FileName = AskForFileName();
			GetAlgorithmType(ref inputData);
			Console.WriteLine("Size of the population?");
			inputData.PopulationSize = AskForInput<int>();
			Console.WriteLine("Mutation probability?");
			inputData.MutationProbability = AskForProbability();
			switch (inputData.Type)
			{
				case AlgorithmType.Generation:
					Console.WriteLine("Crossover probability?");
					inputData.CrossoverProbability = AskForProbability();
					Console.WriteLine("Elitism?");
					inputData.Elitism = AskForInput<bool>();
					break;
				case AlgorithmType.Elimination:
					Console.WriteLine("Mortality in percentage?");
					inputData.Mortality = AskForProbability();
					break;
			}
			return inputData;
		}

		/// <summary>
		/// Special method that ensures correct input parameters for specified genetic algorithm from console arguments
		/// </summary>
		/// <param name="args">Console arguments</param>
		/// <returns>Struct defining input parameters</returns>
		private static InputData HandleArgs(string[] args)
		{
			InputData inputData = new InputData();

			if(!File.Exists(args[0]))
				ErrorHandler.TerminateExecution(ErrorCode.NoSuchFile);
			inputData.FileName = args[0];

			var result = AlgorithmTypeString.Decode(args[1]);
			if (result == null)
				ErrorHandler.TerminateExecution(ErrorCode.WrongAlgorithmChoice);

			if(!TryParse(args[2], out int populationSize))
				ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
			inputData.PopulationSize = populationSize;

			if (!TryParse(args[3], out float mutationProb))
				ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
			inputData.MutationProbability = mutationProb;

			switch (inputData.Type)
			{
				case AlgorithmType.Generation:
					if (!TryParse(args[4], out float crossoverProb))
						ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
					inputData.CrossoverProbability = crossoverProb;
					if (args.Length != 6)
					{
						inputData.Elitism = true;
					}
					else
					{
						if (!TryParse(args[5], out bool elitism))
							ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
						inputData.Elitism = elitism;
					}
					break;
				case AlgorithmType.Elimination:
					if (!TryParse(args[2], out float mortality))
						ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
					inputData.Mortality = mortality;
					break;
			}
			return inputData;
		}

		private static void GetAlgorithmType(ref InputData inputData)
		{
			Console.WriteLine("Which type of genetic algorithm do you wish to use? (E.g. G for generational, E for eliminating)");
			bool correctInput = false;
			do
			{
				var result = AlgorithmTypeString.Decode(AskForInput<string>());
				if (result == null)
					continue;

				correctInput = true;
				inputData.Type = (AlgorithmType)result;
			} while (!correctInput);
		}

		private static float AskForProbability()
		{
			float result;
			bool correctInput = false;
			const int limit = 1;
			do
			{
				result = AskForInput<float>();
				if (result < 0 || result > limit)
					continue;
				correctInput = true;
			} while (!correctInput);
			return result;
		}

		private static string AskForFileName()
		{
			string result;
			bool correctInput = false;
			do
			{
				result = AskForInput<string>();
				if (!File.Exists(result))
				{
					Console.WriteLine("Such file does not exist.");
					continue;
				}
				correctInput = true;
			} while (!correctInput);
			return result;
		}

		private static T AskForInput<T>() where T : IConvertible
		{
			bool correctInput;
			T result;
			do
			{
				string input = Console.ReadLine();
				CheckIfTerminating(input);
				correctInput = TryParse(input, out result);
			} while (!correctInput);
			return result;
		}

		private static bool TryParse<T>(string input, out T thisType) where T: IConvertible
		{
			bool success;
			thisType = typeof(T) == typeof(String) ? (T)(object)String.Empty : default(T);
			if (thisType == null)
				return false;

			var typeCode = thisType.GetTypeCode();

			switch (typeCode)
			{
				case TypeCode.Boolean:
					success = Boolean.TryParse(input, out var b);
					thisType = (T)Convert.ChangeType(b, typeCode);
					break;
				case TypeCode.Double:
					success = double.TryParse(input, out var d);
					thisType = (T)Convert.ChangeType(d, typeCode);
					break;
				case TypeCode.Single:
					success = float.TryParse(input, out var f);
					thisType = (T)Convert.ChangeType(f, typeCode);
					break;
				case TypeCode.Int32:
					success = int.TryParse(input, out var i);
					thisType = (T)Convert.ChangeType(i, typeCode);
					break;
				case TypeCode.String:
					success = true;
					thisType = (T)Convert.ChangeType(input, typeCode);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return success;
		}

		private static void NotifyUserOfTermination()
		{
			Console.Write("If you wish to terminate program, type one of following expressions: ");
			foreach (var expression in TerminationExpressions)
			{
				if(expression != null && expression !=  (string) TerminationExpressions.GetValue(TerminationExpressions.Length - 1))
					Console.Write(expression + ", ");
				else
					Console.WriteLine(expression + ".");
			}
		}

		private static void CheckIfTerminating(string input)
		{
			if(TerminationExpressions.Contains(input))
				ErrorHandler.TerminateExecution(ErrorCode.UserTermination);
		}
	}
}