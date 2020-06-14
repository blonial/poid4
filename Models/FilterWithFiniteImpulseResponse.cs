using System;

namespace poid.Models
{
    public static class FilterWithFiniteImpulseResponse
    {
        #region Static methods

        public static double[] GetFilterValues(int fc, int fs, int L)
        {
            double[] result = new double[L];
            for (int k = 0; k < L; k++)
            {
                if (k == (L - 1) / 2)
                {
                    result[k] = (2 * fc) / fs;
                }
                else
                {
                    double nominator = Math.Sin(((2.0 * Math.PI * fc) / fs) * (k - ((L - 1.0) / 2.0)));
                    double denominator = Math.PI * (k - ((L - 1.0) / 2.0));
                    result[k] = nominator / denominator;
                }
            }
            return result;
        }

        public static double[] FilterInTheTimeDomain(double[] samples, double[] filterValues)
        {
            int L = filterValues.Length;
            double[] result = new double[samples.Length + L - 1];

            for (int n = 0; n < result.Length; n++)
            {
                if (n < L)
                {
                    result[n] = 0;
                }
                else
                {
                    result[n] = samples[n - L];
                }
            }

            for (int n = L; n < result.Length; n++)
            {
                double sum = 0;
                for (int k = 0; k < L; k++)
                {
                    double x_n_k = result[n - k];
                    double h_k = filterValues[k];
                    sum += x_n_k * h_k;
                }
                result[n] = sum;
            }

            return result;
        }

        public static double[] GetResultInTheTimeDomain(double[] samples, int filterLength)
        {
            double[] result = new double[samples.Length - filterLength];
            Array.Copy(samples, filterLength - 1, result, 0, samples.Length - filterLength);
            return result;
        }

        #endregion
    }
}
