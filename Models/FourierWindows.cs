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

        public static double[][] SplitSamplesToWindows(double[] samples, int windowLength)
        {
            double[][] result = new double[samples.Length / windowLength][];
            for (int i = 0; i < samples.Length / windowLength; i++)
            {
                result[i] = new double[windowLength];
                Array.Copy(samples, i * windowLength, result[i], 0, windowLength);
            }
            return result;
        }

        public static double[] FillWithZeros(double[] samples, int length, ZeroFillingMethod zeroFillingMethod)
        {
            Func<double[], int, double[]> method;
            switch (zeroFillingMethod)
            {
                default:
                case ZeroFillingMethod.CausalFilter:
                    method = FillWithZerosCausalFilter;
                    break;
                case ZeroFillingMethod.CauselessFilter:
                    method = FillWithZerosCauselessFilter;
                    break;
            }

            return method(samples, length);
        }

        private static double[] FillWithZerosCausalFilter(double[] samples, int length)
        {
            double[] result = new double[length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = samples.Length > i ? samples[i] : 0;
            }

            return result;
        }

        private static double[] FillWithZerosCauselessFilter(double[] samples, int length)
        {
            double[] result = new double[length];

            double[] samplesFirstHalf = new double[samples.Length / 2];
            double[] samplesSecondHalf = new double[samples.Length / 2];

            Array.Copy(samples, 0, samplesFirstHalf, 0, samples.Length / 2);
            Array.Copy(samples, samples.Length / 2, samplesSecondHalf, 0, samples.Length / 2);

            for (int i = 0; i < samplesSecondHalf.Length; i++)
            {
                result[i] = samplesSecondHalf[i];
            }

            for (int i = samplesSecondHalf.Length; i < result.Length - samplesFirstHalf.Length; i++)
            {
                result[i] = 0;
            }

            for (int i = result.Length - samplesFirstHalf.Length; i < result.Length; i++)
            {
                result[i] = samplesFirstHalf[i - (result.Length - samplesFirstHalf.Length)];
            }

            return result;
        }

        public static double[] GetResultWithoutZeros(double[] samples, int windowSize, ZeroFillingMethod zeroFillingMethod)
        {
            Func<double[], int, double[]> method;
            switch (zeroFillingMethod)
            {
                default:
                case ZeroFillingMethod.CausalFilter:
                    method = GetResultWithoutZerosCausalFilter;
                    break;
                case ZeroFillingMethod.CauselessFilter:
                    method = GetResultWithoutZerosCauselessFilter;
                    break;
            }

            return method(samples, windowSize);
        }

        private static double[] GetResultWithoutZerosCausalFilter(double[] samples, int windowSize)
        {
            double[] result = new double[windowSize];
            Array.Copy(samples, 0, result, 0, windowSize);
            return result;
        }

        private static double[] GetResultWithoutZerosCauselessFilter(double[] samples, int windowSize)
        {
            double[] result = new double[windowSize];
            double[] firstHalf = new double[windowSize / 2];
            double[] secondHalf = new double[windowSize / 2];

            Array.Copy(samples, 0, secondHalf, 0, windowSize / 2);
            Array.Copy(samples, samples.Length - (windowSize / 2), firstHalf, 0, windowSize / 2);

            for (int i = 0; i < firstHalf.Length; i++)
            {
                result[i] = firstHalf[i];
            }

            for (int i = 0; i < secondHalf.Length; i++)
            {
                result[i + windowSize / 2] = secondHalf[i];
            }

            return result;
        }

        #endregion
    }
}
