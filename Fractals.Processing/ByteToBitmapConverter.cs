using System.Threading.Tasks;
using SkiaSharp;

namespace Fractals.Processing
{
    public class ByteArrayToBitmapConverter : IConvertByteArraysToBitmaps
    {
        public SKBitmap Convert(byte[] bytes, uint width, uint height)
        {
            var bitmap = new SKBitmap((int) width, (int) height);

            var pixelsAddress = bitmap.GetPixels();

            unsafe
            {
                var ptr = (byte*) pixelsAddress.ToPointer();

                var i = 0;
                for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    *ptr++ = bytes[i++]; // red
                    *ptr++ = bytes[i++]; // green
                    *ptr++ = bytes[i++]; // blue
                    *ptr++ = bytes[i++]; // alpha
                }
            }

            return bitmap;
        }

        public Task<SKBitmap> ConvertAsync(byte[] bytes, uint width, uint height)
        {
            return Task.Run(() => Convert(bytes, width, height));
        }
    }

    public interface IConvertByteArraysToBitmaps
    {
        SKBitmap Convert(byte[] bytes, uint width, uint height);
        Task<SKBitmap> ConvertAsync(byte[] bytes, uint width, uint height);
    }
}
