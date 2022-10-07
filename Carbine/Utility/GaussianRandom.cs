using System;

namespace Carbine.Utility
{
    public class GaussianRandom
    {
        public static double Next(double mean, double stdDev)
        {
            double d = Engine.Random.NextDouble();
            double num = Engine.Random.NextDouble();
            double num2 = Math.Sqrt(-2.0 * Math.Log(d)) * Math.Sin(6.283185307179586 * num);
            return mean + stdDev * num2;
        }
    }
}
