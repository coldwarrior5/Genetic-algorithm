using System.IO;

namespace Genetic_Algorithm
{
	public class DataReader
	{
		public const int Max = 250; // Number of tests performed by user

		private string _fileName;

		public void DefineFile(string fileName)
		{
			_fileName = fileName;
		}

		public string[] LoadFile()
		{
			string[] allLines = File.ReadAllLines(_fileName);
			return allLines;
		}
	}
}