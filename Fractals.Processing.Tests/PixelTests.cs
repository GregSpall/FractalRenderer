using NUnit.Framework;

namespace Fractals.Processing.Tests
{
    public class PixelTests
    {
        [TestFixture]
        public class WhenIterating
        {
            private MandelbrotPixel _sut;
            private MandelbrotPixel _expected;

            [SetUp]
            public void Setup()
            {
                _expected = new MandelbrotPixel
                {
                    Finished = false,
                    Iterations = 2,
                    Position = new Complex(0.5m,0),
                    Z = new Complex(0.75m, 0)
                };

                _sut = new MandelbrotPixel
                {
                    Finished = false,
                    Iterations = 1,
                    Position = new Complex(0.5m,0m),
                    Z = new Complex(0.5m, 0)
                };

                _sut.Iterate();
            }

            [Test]
            public void FinishedWasSet()
            {
                Assert.That(_sut.Finished, Is.EqualTo(_expected.Finished));
            }
            
            [Test]
            public void IterationsWasIncremented()
            {
                Assert.That(_sut.Iterations, Is.EqualTo(_expected.Iterations));
            }
            
            [Test]
            public void PositionWasNotChanged()
            {
                Assert.That(_sut.Position, Is.EqualTo(_expected.Position));
            }
            
            [Test]
            public void ZWasUpdatedAsExpected()
            {
                Assert.That(_sut.Z, Is.EqualTo(_expected.Z));
            }

        }

        [TestFixture]
        public class WhenIteratingForTheFirstTime
        {
            private MandelbrotPixel _sut;
            private MandelbrotPixel _expected;

            [SetUp]
            public void Setup()
            {
                _expected = new MandelbrotPixel
                {
                    Finished = false,
                    Iterations = 1,
                    Position = new Complex(0.5m,0.2m),
                    Z = new Complex(0.5m, 0.2m)
                };

                _sut = new MandelbrotPixel
                {
                    Finished = false,
                    Iterations = 0,
                    Position = new Complex(0.5m,0.2m),
                    Z = new Complex(0, 0)
                };

                _sut.Iterate();

                
            }

            [Test]
            public void FinishedWasSet()
            {
                Assert.That(_sut.Finished, Is.EqualTo(_expected.Finished));
            }
            
            [Test]
            public void IterationsWasIncremented()
            {
                Assert.That(_sut.Iterations, Is.EqualTo(_expected.Iterations));
            }
            
            [Test]
            public void PositionWasNotChanged()
            {
                Assert.That(_sut.Position, Is.EqualTo(_expected.Position));
            }
            
            [Test]
            public void ZWasUpdatedAsExpected()
            {
                Assert.That(_sut.Z, Is.EqualTo(_expected.Z));
            }

        }

        [TestFixture]
        public class WhenIteratingAndFinishing
        {
            private MandelbrotPixel _sut;
            private MandelbrotPixel _expected;

            [SetUp]
            public void Setup()
            {
                _expected = new MandelbrotPixel
                {
                    Finished = true,
                    Iterations = 2,
                    Position = new Complex(2,2),
                    Z = new Complex(2, -6)
                };

                _sut = new MandelbrotPixel
                {
                    Finished = false,
                    Iterations = 1,
                    Position = new Complex(2,2),
                    Z = new Complex(-2, 2)
                };

                _sut.Iterate();

                
            }

            [Test]
            public void FinishedWasSet()
            {
                Assert.That(_sut.Finished, Is.EqualTo(_expected.Finished));
            }
            
            [Test]
            public void IterationsWasIncremented()
            {
                Assert.That(_sut.Iterations, Is.EqualTo(_expected.Iterations));
            }
            
            [Test]
            public void PositionWasNotChanged()
            {
                Assert.That(_sut.Position, Is.EqualTo(_expected.Position));
            }
            
            [Test]
            public void ZWasUpdatedAsExpected()
            {
                Assert.That(_sut.Z, Is.EqualTo(_expected.Z));
            }

        }

    }
}