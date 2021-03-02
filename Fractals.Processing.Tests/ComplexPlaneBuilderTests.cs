using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fractals.Processing.Tests
{
    public class ComplexPlaneBuilderTests
    {
        [TestFixture]
        public class WhenTryingToBuildAPlaneWithInvalidDimensions
        {

            [Test]
            [TestCase(0,0)]
            [TestCase(0,1)]
            [TestCase(1,0)]
            [TestCase(-1,1)]
            [TestCase(1,-1)]
            [TestCase(-1,-1)]
            [TestCase(1000,-1000)]
            public void ThrowsArgumentException(int width, int height)
            {
                var centre = new Complex(0, 0);
                var range = new Complex(2, 2);
                var sut = new ComplexPlaneBuilder();

                Assert.ThrowsAsync<ArgumentException>(async () => await sut.BuildArrayAsync(width, height, centre, range), "Dimensions must be > 0.");
            }
        }

        [TestFixture]
        public class WhenBuildingASmallComplexPlaneAroundTheOrigin
        {
            private Complex[,] _result;
            private Complex[,] _expected;

            [SetUp]
            public async Task Setup()
            {
                var centre = new Complex(0, 0);
                var range = new Complex(2, 2);
                var sut = new ComplexPlaneBuilder();

                _result = await sut.BuildArrayAsync(3, 3, centre, range);

                _expected = new Complex[,]
                {
                    { new Complex(-1, -1), new Complex(-1, 0), new Complex(-1, 1) },
                    { new Complex(0, -1), new Complex(0, 0), new Complex(0, 1) },
                    { new Complex(1, -1), new Complex(1, 0), new Complex(1, 1) },
                };
            }

            [Test]
            public void ResultEqualsExpected()
            {
                Assert.That(_result.GetLength(0), Is.EqualTo(_expected.GetLength(0)));
                Assert.That(_result.GetLength(1), Is.EqualTo(_expected.GetLength(1)));

                for(var y = 0; y < _expected.GetLength(0); y++)
                for (var x = 0; x < _expected.GetLength(1); x++)
                {
                    Assert.That(_result[x,y], Is.EqualTo(_expected[x,y]));
                }
            }
        }

        [TestFixture]
        public class WhenBuildingALargeComplexPlaneInThePositiveQuadrant
        {
            private Complex[,] _result;

            [SetUp]
            public async Task Setup()
            {
                var centre = new Complex(5, 10);
                var range = new Complex(10, 20);
                var sut = new ComplexPlaneBuilder();

                _result = await sut.BuildArrayAsync(51, 101, centre, range);
            }

            [Test]
            public void ResultIsTheSizeExpected()
            {
                Assert.That(_result.GetLength(0), Is.EqualTo(51));
                Assert.That(_result.GetLength(1), Is.EqualTo(101));
            }

            [Test]
            // Corners
            [TestCase(0, 0,  0, 0)]
            [TestCase(50, 100,  10, 20)]
            [TestCase(0, 100,  0, 20)]
            [TestCase(50, 0,  10, 0)]
            // Centre
            [TestCase(25, 50,  5, 10)]
            public void ResultEqualsExpected(int x, int y, decimal expectedReal, decimal expectedImaginary)
            {
                Assert.That(_result[x,y], Is.EqualTo(new Complex(expectedReal, expectedImaginary)));
            }
        }


    }
}