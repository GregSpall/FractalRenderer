namespace Fractals.Processing.Interface
{
    public interface IPixel
    {
        IPixel SetPosition(Complex position);
        void Iterate();
        Colour ToColour(ColourPalette palette);
    }
}
