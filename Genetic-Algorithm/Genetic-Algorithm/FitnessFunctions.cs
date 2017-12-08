using System;

namespace Genetic_Algorithm
{
	public static class FitnessFunctions
	{
		public static float Fitness1(float[] desiredOutput, float[] givenOutput)
		{
			float error = 0;

			if(desiredOutput.Length != givenOutput.Length)
				throw new Exception("Arrays of given output is different size than that of the desired output");

			// ReSharper disable once LoopCanBeConvertedToQuery Speed is of essence here
			for (int i = 0; i < desiredOutput.Length; i++)
			{
				error += MathF.Pow(desiredOutput[i] - givenOutput[i], 2);
			}
			return error;
		}
	}
}