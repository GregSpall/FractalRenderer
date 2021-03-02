using Fractals.Processing.Interface;

namespace Fractals.Processing
{
    public struct JuliaPixel : IPixel
    {
        private uint _totalIterations;
        public uint Iterations { get; set; }
        public bool Finished { get; set; }

        public Complex C { get; set; }
        public Complex Z { get; set; }

        public IPixel SetPosition(Complex position)
        {
            _totalIterations = 0;
            Iterations = 0;
            Finished = false;
            Z = position;

            return this;
        }

        public void Iterate()
        {
            _totalIterations++;
            if (Finished) return;
            Z = (Z * Z) + C;
            Iterations++;
            Finished = HasEscaped();
        }

        public bool HasEscaped()
        {
            return Z.QuickAbs() > 2;
        }

        public Colour ToColour(ColourPalette palette)
        {
            return palette.GetColour(100m * Iterations / _totalIterations);
        }
    }
}
