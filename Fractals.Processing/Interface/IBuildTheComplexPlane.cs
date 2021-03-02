using System.Threading;
using System.Threading.Tasks;

namespace Fractals.Processing.Interface
{
    public interface IBuildTheComplexPlane
    {
        Task<Complex[,]> BuildArrayAsync(int width, int height, Complex centre, Complex range, CancellationToken ct = default);
    }
}