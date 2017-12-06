using System.IO;
using System.Threading.Tasks;

namespace Genetic_Algorithm.Handlers
{
	public static class FileHandler
	{
		private static void LoadFile(string fileName, out string[] allLines)
		{
			allLines = File.ReadAllLines(fileName);
		}

		public static void ParseLines(string fileName, out float[][] inputParams, out float[] desiredOutput)
		{
			LoadFile(fileName, out string[] allLines);
			float[][] inputs = new float[allLines.Length][];
			float[] outputs = new float[allLines.Length];

			Parallel.For(0, allLines.Length, index => {

				// Parsing output f(x,y)
				string[] values = allLines[index].Split();
				float.TryParse(values[values.Length - 1], out outputs[index]);

				// Parsing input parameters x, y
				inputs[index] = new float[values.Length - 1];
				for(int i = 0; i < values.Length - 1; i++)
					float.TryParse(values[i], out inputs[index][i]);
			});

			inputParams = inputs;
			desiredOutput = outputs;
		}
	}
}