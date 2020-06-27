using System;
using System.Collections.Generic;
using System.Linq;

namespace poid.Models
{
    public static class FilterWithFiniteImpulseResponse
    {
        #region Static methods

        public static double[] GetFilterValues(double fc, double fs, int L)
        {
            double[] result = new double[L];
            double half = (L - 1) / 2.0;

            for (int i = 0; i < L; i++)
            {
                if (i == half)
                {
                    result[i] = 2 * fc / fs;
                }
                else
                {
                    result[i] = Math.Sin(2 * Math.PI * fc / fs * (i - half)) / (Math.PI * (i - half));
                }
            }

            return result;
        }

        public static double[] FilterInTheTimeDomain(double[] samples, double[] filterValues, int L)
        {
            double[] result = new double[samples.Length + L - 1];

            List<double> data = samples.ToList();
            double[] zeros = new double[L - 1];

            data.InsertRange(0, zeros);
            data.AddRange(zeros);

            for (int i = L - 1; i < data.Count; i++)
            {

                for (int j = 0; j < filterValues.Length; j++)
                {
                    result[i - L + 1] += data[i - j] * filterValues[j];
                }
            }

            return result;
        }

        #endregion
    }
}
