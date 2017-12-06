using System.Threading.Tasks;

namespace Genetic_Algorithm
{
	public class GA
	{
		public void FindResult()
		{
			Parallel.For(0, allLines.Length, x =>
			{
				TestReadingAndProcessingLinesFromFile_DoStuff(allLines[x]);
			});
		}
	}
}