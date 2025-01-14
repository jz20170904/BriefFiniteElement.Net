﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BriefFiniteElementNet.Mathh
{
    [Serializable]
    public class Polynomial
    {
        public Polynomial(params double[] coefs)
        {
            Coefficients = (double[])coefs.Clone();
        }

        double[] Coefficients;

        public Polynomial Clone()
        {
            var c2 = Coefficients == null ? (double[])null : (double[])Coefficients.Clone();

            var buf = new Polynomial(c2);

            return buf;
        }

        /// <summary>
        /// Evaluates the value o polynomial at specified <see cref="x"/>
        /// </summary>
        /// <param name="x">evaluation point</param>
        /// <returns>P(<see cref="x"/>)</returns>
        public double Evaluate(double x)
        {
            var buf = 0.0;

            for (var i = 0; i < Coefficients.Length; i++)
            {
                var origPow = Coefficients.Length - 1 - i;

                buf += Coefficients[i] * Math.Pow(x, origPow);
            }

            return buf;
        }

        /// <summary>
        /// Gets the derivative of polynomial as another polynomial
        /// </summary>
        /// <param name="deg">derivation degree</param>
        /// <returns>derivation</returns>
        public Polynomial GetDerivative(int deg)
        {
            if (deg == 0)
                return (Polynomial)this.MemberwiseClone();

            if (Coefficients.Length == 0)
                return this.Clone();

            if (deg >= Coefficients.Length)
                return new Polynomial();

            var dic = new Dictionary<int, double>();

            var buf = new double[Coefficients.Length - deg];

            for (var i = 0; i < Coefficients.Length; i++)
            {
                var origPow = Coefficients.Length - 1 - i;

                var n = origPow;
                var m = deg;

                if (n - m >= 0)
                {
                    var newPow = n - m;

                    //dic[newPow] = Factorial(n)/Factorial(n - m) * this.Coefficients[i];
                    dic[newPow] = Factorial(n, n - m) * this.Coefficients[i];

                    //var d = Factorial(n) / Factorial(n - m) - Factorial(n, n - m);
                }
            }

            for (var i = 0; i < buf.Length; i++)
            {
                var origPow = buf.Length - 1 - i;

                buf[i] = dic[origPow];
            }

            var pBuf = new Polynomial(buf);
            return pBuf;
            /**/
        }

        /// <summary>
        /// Evaluates the <see cref="n"/>'th derivation of current polynomial at specified <see cref="x"/>.
        /// </summary>
        /// <param name="n">derivation degree</param>
        /// <param name="x">evaluation location</param>
        /// <returns>evaluation of derivation</returns>
        public double EvaluateDerivative(double x, int n)
        {
            var retVal = 0.0;

            for (var i = 0; i < Coefficients.Length; i++)
            {
                var origPow = Coefficients.Length - 1 - i;

                var nn = origPow;
                var mm = n;

                if (nn - mm >= 0)
                {
                    var cff = Factorial(nn, nn - mm);

                    var cf2 = Coefficients[i];
                    var vari = Math.Pow(x, nn - mm);
                    retVal += vari * cff * cf2;
                }
            }

            return retVal;
        }

        /*
        /// <summary>
        /// returns value of n'th derivative of F where F(x) = x ^ n
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        static double NthDer(double x, int n, int m)
        {
            var pval = Math.Pow(x, n - m);

            if (m > n) return 0;

            return Factorial(n) / Factorial(n - m) * pval;
        }
        */

        static int Factorial(int x)
        {
            if (x < 0)
            {
                return -1;
            }
            else if (x == 1 || x == 0)
            {
                return 1;
            }
            else
            {
                return x * Factorial(x - 1);
            }
        }

        /// <summary>
        /// return x!/y! where y < x
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static long Factorial(int x,int y)
        {
            if (x < y)
                throw new NotImplementedException();

            if (x == y)
                return 1;

            if (y > 1)
            {
                var buf = 1l;

                for (var i = y + 1; i <= x; i++)
                    buf *= i;

                return buf;
            }

            return Factorial(x) / Factorial(y);
        }

        public bool TryFindRoot(double y,out double x)
        {
            var cnt = 0;

            var loc = -1.0;

            while(cnt++<100)
            {
                var yi = this.Evaluate(loc);
                var d =  yi -y;

                if (Math.Abs(d) < 1e-6)
                {
                    x = loc;
                    return true;
                }

                loc = loc - d / this.EvaluateDerivative(loc, 1);
            }

            x = default(double);

            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            var parts = new List<String>();

            for (var i = 0; i < Coefficients.Length; i++)
            {
                var origPow = Coefficients.Length - 1 - i;

                parts.Add(string.Format("{0} * x^{1}", Coefficients[i], origPow));
            }

            return string.Join(" + ", parts);
        }

        protected bool Equals(Polynomial other)
        {
            if (ReferenceEquals(Coefficients, null) && ReferenceEquals(other.Coefficients, null))
                return true;

            if (ReferenceEquals(Coefficients, null) || ReferenceEquals(other.Coefficients, null))
                return false;


            if (Coefficients.Length != other.Coefficients.Length)
                return false;

            for (int i = 0; i < Coefficients.Length; i++)
            {
                var d = Math.Abs(Coefficients[i] - other.Coefficients[i]);

                if (d > 1e-10*Math.Abs(Coefficients[i]))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Polynomial) obj);
        }

        public override int GetHashCode()
        {
            return (Coefficients != null ? Coefficients.GetHashCode() : 0);
        }
    }
}
