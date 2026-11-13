using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Framework.Application
{
    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
