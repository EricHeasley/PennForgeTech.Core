using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Framework.Pipelines
{
    public interface IPipelineBehavior<in TRequest, TResponse>
    {
        Task<Result<TResponse>> HandleAsync(TRequest request, NextStep<TResponse> nextStep, CancellationToken cancellationToken);
    }
}
