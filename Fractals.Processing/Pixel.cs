namespace Fractals.Processing
{
    public struct Pixel
    {
        public uint Iterations { get; set; }
        public bool Finished { get; set; }

        public Complex Position { get; set; }
        public Complex Z { get; set; }

        public Pixel(decimal real, decimal imaginary)
        {
            Iterations = 0;
            Finished = false;
            Position = new Complex(real, imaginary);
            Z = new Complex();
        }

        public void Iterate()
        {
            if (Finished) return;
            Z = (Z * Z) + Position;
            Iterations++;
            Finished = HasEscaped();
        }

        public bool HasEscaped()
        {
            return Z.Abs() > 2;
        }
    }
}
