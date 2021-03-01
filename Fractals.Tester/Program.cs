using Fractals.Processing;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Fractals.Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var prg = new AsyncProgram();
            try
            {
                prg.Run(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("StackTrace:");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }

    public class AsyncProgram
    {
        public async Task Run(string[] args)
        {
            Console.WriteLine("Greg's Fractal renderer test");
            Console.WriteLine("Setting up...");
            var timer = new Stopwatch();
            timer.Start();

            const uint width = 1000u;
            const uint height = 1000u;
            var centre = new Complex(0, -0.6m);
            var range = new Complex(3m, 3m);

            var arrayBuilder = new GraphArrayBuilder();
            var bitmapConverter = new ByteArrayToBitmapConverter();
            var iterator = new Iterator(arrayBuilder);
            await iterator.SetupAsync(width, height, centre, range);

            Console.WriteLine("Iterating...");
            var iterationTimer = new Stopwatch();
            for (var i = 0; i < 100; i++)
            {
                iterationTimer.Start();
                await iterator.IterateAsync();
                Console.WriteLine($"Iteration {i} complete taking {iterationTimer.Elapsed.TotalSeconds} seconds.");
                iterationTimer.Reset();
            }
            Console.WriteLine("Drawing...");

            // Get byte array of 32bpp colour data
            var imageBytes = await iterator.Get32BppImageByteArrayAsync();

            // Convert byte array to bitmap
            var image = await bitmapConverter.ConvertAsync(imageBytes, width, height);

            // Encode to png
            var imageData = image.Encode(SKEncodedImageFormat.Png, int.MaxValue);
            // Save to file system
            await using var fs = new FileStream($@"C:\FractalOutput\Mandelbrot{DateTime.Now:yyyyMMddHHmmss}.png", FileMode.CreateNew);
            imageData.SaveTo(fs);

            Console.WriteLine($"Finished, took {timer.Elapsed.TotalSeconds} seconds.");
        }
    }
}
