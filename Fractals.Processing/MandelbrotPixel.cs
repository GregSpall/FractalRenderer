namespace Fractals.Processing
{
    public struct MandelbrotPixel
    {
        private uint _totalIterations;
        public uint Iterations { get; set; }
        public bool Finished { get; set; }

        public Complex Position { get; set; }
        public Complex Z { get; set; }

        public MandelbrotPixel(Complex position)
        {
            _totalIterations = 0;
            Iterations = 0;
            Finished = false;
            Position = position;
            Z = new Complex();
        }

        public void Iterate()
        {
            _totalIterations++;
            if (Finished) return;
            Z = (Z * Z) + Position;
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
