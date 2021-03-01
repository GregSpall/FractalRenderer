using System;

namespace Fractals.Processing
{
    public struct Complex
    {
        public decimal Real { get; }
        public decimal Imaginary { get; }

        public Complex(decimal real, decimal imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public double QuickAbs()
        {
            return Math.Sqrt((double)(Real * Real + Imaginary * Imaginary));
        }

        public decimal AccurateAbs()
        {
            return DecimalExtensions.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.Real * b.Real - a.Imaginary * b.Imaginary, a.Real * b.Imaginary + a.Imaginary * b.Real);
        }

        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
        }
        

        public static bool operator ==(Complex a, Complex b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return !(a.Equals(b));
        }

        public bool Equals(Complex a)
        {
            return Real == a.Real && Imaginary == a.Imaginary;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Complex complex)
            {
                return Equals(complex);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Real.GetHashCode() * 397) ^ Imaginary.GetHashCode();
            }
        }
    }
}
