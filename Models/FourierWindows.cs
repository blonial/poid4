using System;

namespace poid.Models
{
    public static class FourierWindows
    {
        #region Enums

        public enum WindowType { Hamming, Hanning, Rectangular }

        #endregion

        #region Static Methods

        public static double Hamming(int i, int N)
        {
            return 0.53836 - (0.46164 * Math.Cos((2.0 * Math.PI * i) / (N - 1.0)));
        }

        public static double Hanning(int i, int N)
        {
            return 0.5 * (1.0 - Math.Cos((2.0 * Math.PI * i) / (N - 1.0)));
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

        public static double[] MaultiplyByWindowFunction(double[] channel, WindowType windowType)
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

            double[] result = new double[channel.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = channel[i] * window(i, result.Length);
            }

            return result;
        }

        public static double[][] SplitSamplesToWindows(double[] channel, int windowLength)
        {
            double[][] result = new double[channel.Length / windowLength][];
            for (int i = 0; i < channel.Length / windowLength; i++)
            {
                result[i] = new double[windowLength];
                Array.Copy(channel, i * windowLength, result[i], 0, windowLength);
            }
            return result;
        }

        #endregion
    }
}
