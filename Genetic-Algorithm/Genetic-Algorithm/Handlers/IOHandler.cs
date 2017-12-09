using System;
using System.IO;
using System.Linq;

namespace Genetic_Algorithm.Handlers
{
	public static class IoHandler
	{
		private static readonly string[] TerminationExpressions = {"quit", "stop", "exit", "terminate", "q"};
		public const int MaxIterations = 10000;
		public const double MinError = 1;

		public static InputData GetParameters(string[] args)
		{
			InputData data;

			switch (args.Length)
			{
				case 0:
					data = UserInput();
					break;
				case 5:
				case 6:
				case 7:
				case 8:
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
					inputData.Elitism = AskForInput(true);
					break;
				case AlgorithmType.Elimination:
					Console.WriteLine("Mortality in percentage?");
					inputData.Mortality = AskForProbability();
					break;
			}
			Console.WriteLine("Maximum number of iterations? (Default is 10,000)");
			inputData.MaxIterations = AskForInput(MaxIterations);
			Console.WriteLine("Minimum desired error? (Default is 1)");
			inputData.MinError = AskForInput(MinError);

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

			if (!File.Exists(args[0]))
				ErrorHandler.TerminateExecution(ErrorCode.NoSuchFile);
			inputData.FileName = args[0];

			var result = AlgorithmTypeString.Decode(args[1]);
			if (result == null)
				ErrorHandler.TerminateExecution(ErrorCode.WrongAlgorithmChoice);
			else
				inputData.Type = (AlgorithmType) result;

			SetVariable(args[2], out inputData.PopulationSize);
			SetVariable(args[3], out inputData.MutationProbability);

			switch (inputData.Type)
			{
				case AlgorithmType.Generation:
					SetVariable(args[4], out inputData.CrossoverProbability);
					break;
				case AlgorithmType.Elimination:
					SetVariable(args[4], out inputData.Mortality);
					break;
			}

			switch (args.Length)
			{
				case 5:
					inputData.Elitism = true;
					inputData.MaxIterations = MaxIterations;
					inputData.MinError = MinError;
					break;
				case 6:
					SetVariable(args[5], out inputData.Elitism);
					inputData.MaxIterations = MaxIterations;
					inputData.MinError = MinError;
					break;
				case 7:
					SetVariable(args[5], out inputData.MaxIterations);
					SetVariable(args[6], out inputData.MinError);
					break;
				case 8:
					SetVariable(args[5], out inputData.Elitism);
					SetVariable(args[6], out inputData.MaxIterations);
					SetVariable(args[7], out inputData.MinError);
					break;
			}
			if (inputData.MaxIterations < 0)
				inputData.MaxIterations = MaxIterations;
			if (inputData.MinError < 0)
				inputData.MinError = MinError;

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

		private static T AskForInput<T>(T defaultValue) where T : IConvertible
		{
			string input = Console.ReadLine();
			CheckIfTerminating(input);
			bool correctInput = TryParse(input, out T result);
			if (!correctInput)
				result = defaultValue;

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

		private static void SetVariable(string pertinentArgument, out bool inputVariable)
		{
			if (!TryParse(pertinentArgument, out bool boolean))
				ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
			inputVariable = boolean;
		}

		private static void SetVariable(string pertinentArgument, out int inputVariable)
		{
			if (!TryParse(pertinentArgument, out int integer))
				ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
			inputVariable = integer;
		}

		private static void SetVariable(string pertinentArgument, out float inputVariable)
		{
			if (!TryParse(pertinentArgument, out float floating))
				ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
			inputVariable = floating;
		}

		private static void SetVariable(string pertinentArgument, out double inputVariable)
		{
			if (!TryParse(pertinentArgument, out double precise))
				ErrorHandler.TerminateExecution(ErrorCode.CannotParse);
			inputVariable = precise;
		}
	}
}