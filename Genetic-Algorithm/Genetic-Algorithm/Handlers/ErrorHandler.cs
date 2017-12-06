using System;

namespace Genetic_Algorithm.Handlers
{
	public enum ErrorCode { UserTermination = 1, WrongAlgorithmChoice = 2, NoSuchFile = 3, CannotParse = 4 }

	public static class ErrorHandler
	{
		public static void TerminateExecution(int errorCode)
		{
			Console.WriteLine("Application has been abruptly stopped");
			Environment.Exit(errorCode);
		}
	}
}