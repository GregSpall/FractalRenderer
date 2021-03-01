using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fractals.Processing
{
    public class Iterator
    {
        private readonly IBuildGraphArrays _arrayBuilder;

        private MandelbrotPixel[,] _data;
        private ColourPalette _palette;
        private uint _width;
        private uint _height;
        
        public uint Iteration { get; private set; }
        public bool CanRender { get; private set; }


        public Iterator(IBuildGraphArrays arrayBuilder)
        {
            _arrayBuilder = arrayBuilder;
        }

        public async Task SetupAsync(uint width, uint height, Complex centre, Complex range, CancellationToken ct = default)
        {
            _width = width;
            _height = height;
            _data = new MandelbrotPixel[width, height];
            _palette = new ColourPalette();
            _palette.AddMarker(Colour.Black(), 0);
            _palette.AddMarker(Colour.White(), 100);
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

                        _data[localX, y] = new MandelbrotPixel(space[localX, y]);
                    }
                }, ct));
            };

            await Task.WhenAll(tasks);
        }

        public async Task<byte[]> Get32BppImageByteArrayAsync(CancellationToken ct = default)
        {
            var output = new byte[_width * _height * 4];

            var tasks = new List<Task>();

            for (var x = 0; x < _width; x++)
            {
                var localX = x;
                tasks.Add(Task.Run(() =>
                {
                    for (var y = 0; y < _height; y++)
                    {
                        _data[localX, y].ToColour(_palette).ToBytes().CopyTo(output, (localX * _width + y) * 4);
                    }
                }, ct));
            }

            await Task.WhenAll(tasks);

            return output;
        }

        public async Task IterateAsync(CancellationToken ct = default)
        {
            var tasks = new List<Task>();

            for (var x = 0; x < _width; x++)
            {
                var localX = x;
                tasks.Add(Task.Run(() =>
                {
                        for (var y = 0; y < _height; y++)
                        {
                            _data[localX,y].Iterate();

                            if (ct.IsCancellationRequested) return;
                        }
                }, ct));
            }

            await Task.WhenAll(tasks);

            Iteration++;
            CanRender = true;
        }
    }
}
