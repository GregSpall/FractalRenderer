using NUnit.Framework;

namespace Fractals.Processing.Tests
{
    public class ComplexTests
    {
        [Test]
        [TestCase(3, 2, 1, 7, -11, 23)]
        [TestCase(1, 1, 1, 1, 0, 2)]
        [TestCase(3, 4, 0, 1, -4, 3)]
        public void WhenMultiplyingComplexNumbers(decimal aReal, decimal aImaginary, decimal bReal, decimal bImaginary, decimal expectedReal, decimal expectedImaginary)
        {
            var a = new Complex(aReal, aImaginary);
            var b = new Complex(bReal, bImaginary);

            var result = a * b;

            Assert.That(result.Real, Is.EqualTo(expectedReal));
            Assert.That(result.Imaginary, Is.EqualTo(expectedImaginary));
        }

        [Test]
        [TestCase(3, 2, 1, 7, 4, 9)]
        [TestCase(1, 1, 1, 1, 2, 2)]
        [TestCase(3, 4, 0, 1, 3, 5)]
        public void WhenAddingComplexNumbers(decimal aReal, decimal aImaginary, decimal bReal, decimal bImaginary, decimal expectedReal, decimal expectedImaginary)
        {
            var a = new Complex(aReal, aImaginary);
            var b = new Complex(bReal, bImaginary);

            var result = a + b;

            Assert.That(result.Real, Is.EqualTo(expectedReal));
            Assert.That(result.Imaginary, Is.EqualTo(expectedImaginary));
        }

        [Test]
        [TestCase(3, 2, 1, 7, 2, -5)]
        [TestCase(1, 1, 1, 1, 0, 0)]
        [TestCase(3, 4, 0, 1, 3, 3)]
        public void WhenSubtractingComplexNumbers(decimal aReal, decimal aImaginary, decimal bReal, decimal bImaginary, decimal expectedReal, decimal expectedImaginary)
        {
            var a = new Complex(aReal, aImaginary);
            var b = new Complex(bReal, bImaginary);

            var result = a + b;

            Assert.That(result.Real, Is.EqualTo(expectedReal));
            Assert.That(result.Imaginary, Is.EqualTo(expectedImaginary));
        }

        [Test]
        [TestCase(3, 4, 5)]
        public void WhenGettingTheAbsoluteValueOfAComplexNumber(decimal real, decimal imaginary, decimal expected)
        {
            var sut = new Complex(real, imaginary);

            Assert.That(sut.QuickAbs(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(3, 2, 1, 7, false)]
        [TestCase(1, 1, 1, 1, true)]
        [TestCase(3, 4, 0, 1, false)]
        [TestCase(3, 11, 3, 11, true)]
        public void WhenComparingComplexNumbers(decimal aReal, decimal aImaginary, decimal bReal, decimal bImaginary, bool expected)
        {
            var a = new Complex(aReal, aImaginary);
            var b = new Complex(bReal, bImaginary);

            var result = a == b;

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}