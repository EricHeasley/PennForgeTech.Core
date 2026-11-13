using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Core.Http
{
    public interface IHttpService
    {
        Task<Result<TResponse>> GetJsonAsync<TResponse>(string requestUri, CancellationToken cancellationToken = default);

        Task<Result<TResponse>> PostJsonAsync<TRequest, TResponse>(
            string requestUri,
            TRequest body,
            CancellationToken cancellationToken = default);
    }
}
