using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace CITI.EVO.Tools.Helpers
{
    public class BigFloat
    {
        private readonly bool _sign;

        private readonly int _fractions;

        private readonly BigInteger _exponent;
        private readonly BigInteger _fraction;


        public static readonly BigFloat Zero;
        public static readonly String Separator;

        private static readonly BigInteger _ten = new BigInteger(10);

        static BigFloat()
        {
            Zero = new BigFloat();
            Separator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        }

        private BigFloat()
        {
            _sign = true;
            _fractions = 128;
            _exponent = BigInteger.Zero;
            _fraction = BigInteger.Zero;

        }

        public BigFloat(bool sign, int fractions) : this(sign, fractions, BigInteger.Zero, BigInteger.Zero)
        {
        }

        public BigFloat(bool sign, BigInteger exponent, BigInteger fraction) : this(sign, 128, exponent, fraction)
        {
        }

        public BigFloat(bool sign, int fractions, BigInteger exponent, BigInteger fraction)
        {
            _sign = sign;
            _fractions = fractions;
            _exponent = exponent;
            _fraction = fraction;
        }

        public BigFloat Add(BigFloat x)
        {
            var sign = !_sign && !x._sign;
            //TODO: dasaxvewia

            var fractions = Math.Max(_fractions, x._fractions);

            var denominator = _fraction * x._fraction;

            var xExponent = _exponent * x._fraction;
            var yExponent = x._exponent * _fraction;

            var nominator = xExponent + yExponent;

            return new BigFloat(sign, fractions, nominator, denominator);
        }

        public BigFloat Subtract(BigFloat x)
        {
            var sign = !_sign && !x._sign;
            //TODO: dasaxvewia

            var fractions = Math.Max(_fractions, x._fractions);

            var denominator = _fraction * x._fraction;

            var xExponent = _exponent * x._fraction;
            var yExponent = x._exponent * _fraction;

            var nominator = xExponent - yExponent;

            return new BigFloat(sign, fractions, nominator, denominator);
        }

        public BigFloat Multiply(BigFloat x)
        {
            var sign = !_sign && !x._sign;
            var fractions = Math.Max(_fractions, x._fractions);

            var exponent = _exponent * x._exponent;
            var fraction = _fraction * x._fraction;

            return new BigFloat(sign, fractions, exponent, fraction);
        }

        public BigFloat Divide(BigFloat x)
        {
            var sign = !_sign && !x._sign;
            var fractions = Math.Max(_fractions, x._fractions);

            var exponent = _exponent * x._fraction;
            var fraction = _fraction * x._exponent;

            return new BigFloat(sign, fractions, exponent, fraction);
        }

        public BigFloat Pow(BigInteger @base)
        {
            var exponent = _exponent * @base;
            var fraction = _fraction * @base;

            return new BigFloat(_sign, _fractions, exponent, fraction);
        }

        public override String ToString()
        {
            if (_exponent == BigInteger.Zero && _fraction == BigInteger.Zero)
                return String.Format("0{0}0", Separator);

            var sb = new StringBuilder();

            var dividend = _exponent;

            var lenOfDigits = (int)(Math.Floor(BigInteger.Log10(dividend)) + 1D);
            var powerOfTen = BigInteger.Pow(_ten, lenOfDigits);


            if (_exponent < _fraction)
            {
                sb.AppendFormat("0{0}", Separator);
            }
            else
            {
                var res = BigInteger.DivRem(_exponent, _fraction, out dividend);

                sb.AppendFormat("{0}{1}", res, Separator);
            }

            dividend *= powerOfTen;

            int count = 0;

            while (dividend > 0 || count++ < _fractions)
            {
                BigInteger rem;
                var res = BigInteger.DivRem(dividend, _fraction, out rem);

                sb.Append(res);

                dividend = rem * powerOfTen;
            }

            return sb.ToString();
        }
    }
}
