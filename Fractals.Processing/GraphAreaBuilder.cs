using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fractals.Processing
{
    public class GraphArrayBuilder : IBuildGraphArrays
    {
        public async Task<Complex[,]> BuildArrayAsync(uint width, uint height, Complex centre, Complex range, CancellationToken ct = default)
        {
            var output = new Complex[width, height];

            var xMin = centre.Real - (range.Real / 2);
            var yMin = centre.Imaginary - (range.Imaginary / 2);

            var tasks = new List<Task>();
            for (var x = 0; x < width; x++)
            {
                var imaginary = xMin + ((x / (decimal) width) * range.Real);
                var localX = x;
                tasks.Add(Task.Run(() =>
                {
                    for (var y = 0; y < height; y++)
                    {
                        var real = yMin + ((y / (decimal) height) * range.Imaginary);
                        output[localX, y] = new Complex(real, imaginary);
                    }
                }, ct));
            }
            await Task.WhenAll(tasks);

            return output;
        }
    }

    public interface IBuildGraphArrays
    {
        Task<Complex[,]> BuildArrayAsync(uint width, uint height, Complex centre, Complex range, CancellationToken ct = default);
    }
}
