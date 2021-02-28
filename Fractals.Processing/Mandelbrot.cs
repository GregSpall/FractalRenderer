using System.Threading;
using System.Threading.Tasks;

namespace Fractals.Processing
{
    public class Mandelbrot
    {
        private readonly Pixel[,] _data;
        private readonly Pixel[,] _nextData;
        private readonly uint _width;
        private readonly uint _height;
        
        public uint Iteration { get; private set; }
        public bool CanRender { get; private set; }


        public Mandelbrot(uint width, uint height)
        {
            _width = width;
            _height = height;
            _data = new Pixel[width, height];
            _nextData = new Pixel[width, height];
            ResetIteration();
        }

        public Task SetArea(decimal centreReal, decimal centreImaginary, decimal rangeReal, decimal rangeImaginary, CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                ResetIteration();

                var xMin = centreReal - (rangeReal / 2);
                var yMin = centreImaginary - (rangeImaginary / 2);
                for (var x = 0; x < _width; x++)
                {
                    var real = xMin + ((x / (decimal) _width) * rangeReal);

                    for (var y = 0; x < _height; x++)
                    {
                        if (ct.IsCancellationRequested) return;

                        var imaginary = yMin + ((y / (decimal) _height) * rangeImaginary);

                        _data[x, y] = new Pixel(real, imaginary);
                    }
                }
            }, ct);
        }

        private void ResetIteration()
        {
            Iteration = 0;
            CanRender = false;
        }

        public async Task<uint[,]> GetCurrentValuesAsync(CancellationToken ct = default)
        {
            var output = new uint[_width,_height];

            await Task.Run(() =>
            {
                for (var x = 0; x < _width; x++)
                {
                    for (var y = 0; x < _height; x++)
                    {
                        if (ct.IsCancellationRequested) return;
                        output[x, y] = _data[x, y].Iterations;
                    }
                }
            }, ct);

            return output;
        }

        public Task Iterate(CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                for (var x = 0; x < _data.Length; x++)
                {
                    for (var y = 0; y < _data.Length; y++)
                    {
                        _nextData[x,y] = _data[x,y]; 
                        _nextData[x,y].Iterate();

                        if (ct.IsCancellationRequested) return;
                    }
                }

                Iteration++;
                _nextData.CopyTo(_data, 0);
                CanRender = true;
            }, ct);
        }
    }
}
