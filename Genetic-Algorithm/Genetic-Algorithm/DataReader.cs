using System.IO;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
	public class DataReader
	{
		public const int InputVectorSize = 2; // Number of params

		private string _fileName;

		public void DefineFile(string fileName)
		{
			_fileName = fileName;
		}

		public void LoadFile(out string[] allLines)
		{
			allLines = File.ReadAllLines(_fileName);
		}

		public void ParseLines(string[] allLines, out float[][] inputParams, out float[] desiredOutput)
		{
			float[][] inputs = new float[allLines.Length][];
			float[] outputs = new float[allLines.Length];

			Parallel.ForEach(allLines, (line, state, index ) => {

				// Parsing output f(x,y)
				string[] values = line.Split(string.Empty);
				float.TryParse(values[values.Length], out outputs[index]);

				// Parsing input parameters x, y
				inputs[index] = new float[values.Length - 1];
				for(int i = 0; i < values.Length - 1; i++)
					float.TryParse(values[values.Length], out inputs[index][i]);
			});

			inputParams = inputs;
			desiredOutput = outputs;
		}
	}
}