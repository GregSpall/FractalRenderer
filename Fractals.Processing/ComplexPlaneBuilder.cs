using System;
using Fractals.Processing.Interface;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fractals.Processing
{
    public class ComplexPlaneBuilder : IBuildTheComplexPlane
    {
        public async Task<Complex[,]> BuildArrayAsync(int width, int height, Complex centre, Complex range, CancellationToken ct = default)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Dimensions must be > 0.");
            }

            var output = new Complex[width, height];

            var xMin = centre.Real - (range.Real / 2);
            var yMin = centre.Imaginary - (range.Imaginary / 2);

            var tasks = new List<Task>();
            for (var x = 0; x < width; x++)
            {
                var real = xMin + ((x / (decimal) (width - 1)) * range.Real);
                var localX = x;
                tasks.Add(Task.Run(() =>
                {
                    for (var y = 0; y < height; y++)
                    {
                        var imaginary = yMin + ((y / (decimal) (height - 1)) * range.Imaginary);
                        output[localX, y] = new Complex(real, imaginary);
                    }
                }, ct));
            }
            await Task.WhenAll(tasks);

            return output;
        }
    }
}
