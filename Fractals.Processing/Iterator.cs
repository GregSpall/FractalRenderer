using Fractals.Processing.Interface;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fractals.Processing
{
    public class Iterator : IIterate
    {
        public async Task IterateAsync(IPixel[,] data, CancellationToken ct = default)
        {
            var width = data.GetLength(0);
            var height = data.GetLength(1);

            var tasks = new List<Task>();

            for (var x = 0; x < width; x++)
            {
                var localX = x;
                tasks.Add(Task.Run(() =>
                {
                    for (var y = 0; y < height; y++)
                    {
                        data[localX,y].Iterate();

                        if (ct.IsCancellationRequested) return;
                    }
                }, ct));
            }

            await Task.WhenAll(tasks);
        }
    }
    
    public interface IIterate
    {
        Task IterateAsync(IPixel[,] data, CancellationToken ct = default);
    }
}