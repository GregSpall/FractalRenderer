using Fractals.Processing.Interface;

namespace Fractals.Processing
{
    public class JuliaRenderer : Renderer<JuliaPixel>
    {
        public Complex C { get; set; }

        public JuliaRenderer(IBuildTheComplexPlane arrayBuilder, IIterate iterator)
            : base(arrayBuilder, iterator)
        { }

        protected override IPixel GetPixelFromPosition(Complex position)
        {
            var pixel = (JuliaPixel)base.GetPixelFromPosition(position);
            pixel.C = C;
            return pixel;
        }
    }
}
