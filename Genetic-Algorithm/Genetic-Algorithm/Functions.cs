using System;

namespace Genetic_Algorithm
{
	public class Parameters
	{
		public float B0 { get; }
		public float B1 { get; }
		public float B2 { get; }
		public float B3 { get; }
		public float B4 { get; }

		public Parameters(float b0, float b1, float b2, float b3, float b4)
		{
			B0 = b0;
			B1 = b1;
			B2 = b2;
			B3 = b3;
			B4 = b4;
		}
	}

	public class Functions
	{
		public static float F1(float x, float y, Parameters param)
		{
			return MathF.Sin(param.B0 + param.B1 * x) + param.B2 * MathF.Cos(x * (param.B3 + y)) * (1f / (1 + MathF.Pow(MathF.E, MathF.Pow(x - param.B4, 2))));
		}
	}
}