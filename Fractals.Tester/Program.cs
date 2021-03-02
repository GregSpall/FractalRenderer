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

            const int width = 1000; // Real Axis
            const int height = 1000; // Imaginary Axis
            var centre = new Complex(0m, 0m);
            var range = new Complex(3m, 3m);

            var arrayBuilder = new ComplexPlaneBuilder();
            var bitmapConverter = new ByteArrayToBitmapConverter();
            var iterator = new Iterator();
            var renderer = new JuliaRenderer(arrayBuilder, iterator);
            renderer.C = new Complex(0.285m, 0.01m);
            await renderer.SetupAsync(width, height, centre, range);

            Console.WriteLine("Iterating...");
            var iterationTimer = new Stopwatch();
            for (var i = 0; i < 200; i++)
            {
                iterationTimer.Start();
                await renderer.IterateAsync();
                Console.WriteLine($"Iteration {i} complete taking {iterationTimer.Elapsed.TotalSeconds} seconds.");
                iterationTimer.Reset();
            }
            Console.WriteLine("Drawing...");

            var palette = new ColourPalette();
            palette.AddMarker(Colour.Black(), 0);
            palette.AddMarker(Colour.White(), 99.999m);
            palette.AddMarker(Colour.Black(), 100);

            // Get byte array of 32bpp colour data
            var imageBytes = await renderer.Get32BppImageByteArrayAsync(palette);

            // Convert byte array to bitmap
            var image = await bitmapConverter.ConvertAsync(imageBytes, width, height);

            // Encode to png
            var imageData = image.Encode(SKEncodedImageFormat.Png, int.MaxValue);
            // Save to file system
            await using var fs = new FileStream($@"C:\FractalOutput\Julia{DateTime.Now:yyyyMMddHHmmss}.png", FileMode.CreateNew);
            imageData.SaveTo(fs);

            Console.WriteLine($"Finished, took {timer.Elapsed.TotalSeconds} seconds.");
        }
    }
}
