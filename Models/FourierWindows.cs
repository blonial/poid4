using System;

namespace poid.Models
{
    public static class FourierWindows
    {
        #region Enums

        public enum WindowType { Hamming, Hanning, Rectangular }

        public enum ZeroFillingMethod { CausalFilter, CauselessFilter }

        #endregion

        #region Static Methods

        public static double Hamming(int i, int N)
        {
            return 0.53836 - (0.46164 * Math.Cos((2.0 * Math.PI * i) / (N - 1.0)));
        }

        public static double Hanning(int i, int N)
        {
            return 0.5 * (0.5 * Math.Cos((2.0 * Math.PI * i) / (N - 1.0)));
        }

        public static double Rectangular(int i, int N)
        {
            if (i >= -(N - 1) / 2 && i <= (N - 1) / 2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static double[] MultiplyByWindowFunction(double[] samples, WindowType windowType)
        {
            Func<int, int, double> window;
            switch (windowType)
            {
                default:
                case WindowType.Hamming:
                    window = Hamming;
                    break;
                case WindowType.Hanning:
                    window = Hanning;
                    break;
                case WindowType.Rectangular:
                    window = Rectangular;
                    break;
            }

            double[] result = new double[samples.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = samples[i] * window(i, result.Length);
            }

            return result;
        }

        public static double[] GetWindowFactors(int lenght, WindowType windowType)
        {
            Func<int, int, double> window;
            switch (windowType)
            {
                default:
                case WindowType.Hamming:
                    window = Hamming;
                    break;
                case WindowType.Hanning:
                    window = Hanning;
                    break;
                case WindowType.Rectangular:
                    window = Rectangular;
                    break;
            }

            double[] result = new double[lenght];
            for (int i = 0; i < lenght; i++)
            {
                result[i] = window(i, result.Length);
            }

            return result;
        }

        #endregion
    }
}
