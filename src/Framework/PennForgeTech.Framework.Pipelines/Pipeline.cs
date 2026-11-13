using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Primitives;
using PennForgeTech.Core.Results;
using PennForgeTech.Framework.Application;

namespace PennForgeTech.Framework.Pipelines
{
    public sealed class Pipeline<TRequest, TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _handler;
        private readonly IReadOnlyList<IPipelineBehavior<TRequest, TResponse>> _behaviors;

        public Pipeline(
            IRequestHandler<TRequest, TResponse> handler,
            IEnumerable<IPipelineBehavior<TRequest, TResponse>> behaviors)
        {
            this._handler = Guard.AgainstNull(handler, nameof(handler));
            this._behaviors = Guard.AgainstNull(behaviors, nameof(behaviors)).ToList();
        }

        public Task<Result<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            NextStep<TResponse> next = token => this._handler.HandleAsync(request, token);

            foreach (IPipelineBehavior<TRequest, TResponse> behavior in this._behaviors.Reverse())
            {
                next = CreateStep(behavior, request, next);
            }

            return next(cancellationToken);
        }

        private static NextStep<TResponse> CreateStep(
            IPipelineBehavior<TRequest, TResponse> behavior,
            TRequest request,
            NextStep<TResponse> following)
        {
            return token => behavior.HandleAsync(request, following, token);
        }
    }
}
