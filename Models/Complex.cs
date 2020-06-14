using System;

namespace poid.Models
{
    public class Complex
    {
        #region Properties

        public double Re { get; set; }

        public double Im { get; set; }

        #endregion

        #region Constructors

        public Complex()
        {
        }

        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }

        #endregion

        #region Methods

        public static Complex Add(Complex a, Complex b)
        {
            Complex result = new Complex();
            result.Re = a.Re + b.Re;
            result.Im = a.Im + b.Im;
            return result;
        }

        public Complex GetAbsouluteValue()
        {
            Complex result = GetZero();
            result.Re = Math.Sqrt(Re * Re + Im * Im);
            return result;
        }

        #endregion

        #region Static Methods

        public static Complex Subtract(Complex a, Complex b)
        {
            Complex result = new Complex();
            result.Re = a.Re - b.Re;
            result.Im = a.Im - b.Im;
            return result;
        }

        public static Complex Multiply(Complex a, Complex b)
        {
            Complex result = new Complex();
            result.Re = a.Re * b.Re - a.Im * b.Im;
            result.Im = a.Re * b.Im + a.Im * b.Re;
            return result;
        }

        public static Complex Divide(Complex a, Complex b)
        {
            Complex result;
            result = Multiply(a, b);
            double divisor = b.Re * b.Re + b.Im * b.Im;
            if (divisor >= 0.001 || divisor <= -0.001)
            {
                result.Re /= divisor;
                result.Im /= divisor;
            }
            else
            {
                result.Re /= 0.001;
                result.Im /= 0.001;
            }
            return result;
        }

        public static Complex GetZero()
        {
            Complex result = new Complex
            {
                Re = 0,
                Im = 0
            };
            return result;
        }

        #endregion
    }
}
