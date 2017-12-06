using System;

namespace Genetic_Algorithm
{
	public static class Functions
	{
        public static float F1(float x, float y, float[] param) => MathF.Sin(param[0] + param[1] * x) + param[2] * MathF.Cos(x * (param[3] + y)) * (1f / (1 + MathF.Pow(MathF.E, MathF.Pow(x - param[4], 2))));
    }
}