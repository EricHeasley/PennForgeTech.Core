using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Framework.Pipelines
{
    public delegate Task<Result<TResponse>> NextStep<TResponse>(CancellationToken cancellationToken);
}
