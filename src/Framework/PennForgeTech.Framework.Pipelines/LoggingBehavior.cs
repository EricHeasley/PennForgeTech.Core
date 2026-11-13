using System;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Primitives;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Framework.Pipelines
{
    public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IAppLogger _logger;
        private readonly IClock _clock;

        public LoggingBehavior(IAppLogger logger, IClock clock)
        {
            this._logger = Guard.AgainstNull(logger, nameof(logger));
            this._clock = Guard.AgainstNull(clock, nameof(clock));
        }

        public async Task<Result<TResponse>> HandleAsync(TRequest request, NextStep<TResponse> nextStep, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            DateTimeOffset startedAt = this._clock.UtcNow;
            this._logger.LogInformation($"Handling {requestName}.");

            Result<TResponse> result;

            try
            {
                result = await nextStep(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception, $"{requestName} threw after {this.ElapsedMilliseconds(startedAt)} ms.");
                throw;
            }

            if (result.IsFailure)
            {
                this._logger.LogWarning($"{requestName} failed with {result.Error.Code}: {result.Error.Message} ({this.ElapsedMilliseconds(startedAt)} ms).");
            }
            else
            {
                this._logger.LogInformation($"{requestName} completed in {this.ElapsedMilliseconds(startedAt)} ms.");
            }

            return result;
        }

        private double ElapsedMilliseconds(DateTimeOffset startedAt)
        {
            return (this._clock.UtcNow - startedAt).TotalMilliseconds;
        }
    }
}
