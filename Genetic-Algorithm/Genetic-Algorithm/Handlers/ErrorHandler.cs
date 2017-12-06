using System;

namespace Genetic_Algorithm.Handlers
{
	public enum ErrorCode { UserTermination = 1, WrongAlgorithmChoice = 2, NoSuchFile = 3, CannotParse = 4, InvalidNumberOfArguments = 5 }
	
	public static class ErrorHandler
	{
		public static void TerminateExecution(ErrorCode code)
		{
			Console.WriteLine("Application stopped.\n Reason: " + ErrorMessage(code));
			Environment.Exit((int) code);
		}

		private static string ErrorMessage(ErrorCode code)
		{
			string explanation = "";
			switch (code)
			{
				case ErrorCode.CannotParse:
					explanation = "Data type cannot be parsed";
					break;
				case ErrorCode.NoSuchFile:
					explanation = "No such file";
					break;
				case ErrorCode.InvalidNumberOfArguments:
					explanation = "Invalid number of arguments";
					break;
				case ErrorCode.UserTermination:
					explanation = "User termination";
					break;
				case ErrorCode.WrongAlgorithmChoice:
					explanation = "Improper choice of algorithm";
					break;
			}
			return explanation;
		}
	}
}