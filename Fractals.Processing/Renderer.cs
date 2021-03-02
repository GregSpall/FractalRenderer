using Fractals.Processing.Interface;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;

namespace Fractals.Processing
{
    public class Renderer<T> where T : IPixel, new()
    {
        private readonly IBuildTheComplexPlane _arrayBuilder;
        private readonly IIterate _iterator;

        private IPixel[,] _data;
        private int _width;
        private int _height;
        
        public uint Iteration { get; private set; }
        public bool CanRender { get; private set; }


        public Renderer(IBuildTheComplexPlane arrayBuilder, IIterate iterator)
        {
            _arrayBuilder = arrayBuilder;
            _iterator = iterator;
        }

        public async Task SetupAsync(int width, int height, Complex centre, Complex range, CancellationToken ct = default)
        {
            _width = width;
            _height = height;
            _data = new IPixel[width, height];
            ResetIteration();

            var space = await _arrayBuilder.BuildArrayAsync(width, height, centre, range, ct);
            await PopulateDataAsync(space, ct);
        }

        private void ResetIteration()
        {
            Iteration = 0;
            CanRender = false;
        }

        public async Task PopulateDataAsync(Complex[,] space, CancellationToken ct = default)
        {
            var tasks = new List<Task>();

            for (var x = 0; x < _width; x++)
            {
                var localX = x;
                tasks.Add(Task.Run(() =>
                {
                    for (var y = 0; y < _height; y++)
                    {
                        if (ct.IsCancellationRequested) return;

                        var position = space[localX, y];
                        var pixel = GetPixelFromPosition(position);

                        _data[localX, y] = (T)pixel;
                    }
                }, ct));
            }

            await Task.WhenAll(tasks);
        }

        protected virtual IPixel GetPixelFromPosition(Complex position)
        {
            return new T().SetPosition(position);
        }

        public async Task<byte[]> Get32BppImageByteArrayAsync(ColourPalette palette, SKColorType pixelFormat = SKColorType.Rgba8888, CancellationToken ct = default)
        {
            var output = new byte[_width * _height * 4];

            var tasks = new List<Task>();

            for (var y = 0; y < _height; y++)
            {
                var localY = y;
                var yRowStart = y * _width;
                tasks.Add(Task.Run(() =>
                {
                    for (var x = 0; x < _width; x++)
                    {
                        _data[x, localY].ToColour(palette).ToBytes(pixelFormat).CopyTo(output, (yRowStart + x) * 4);
                    }
                }, ct));
            }

            await Task.WhenAll(tasks);

            return output;
        }

        public async Task IterateAsync(CancellationToken ct = default)
        {
            await _iterator.IterateAsync(_data, ct);

            Iteration++;
            CanRender = true;
        }
    }
}
