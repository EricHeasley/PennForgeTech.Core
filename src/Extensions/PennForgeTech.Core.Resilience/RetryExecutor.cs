using System;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Resilience
{
    public static class RetryExecutor
    {
        public static async Task<TResult> ExecuteAsync<TResult>(
            Func<CancellationToken, Task<TResult>> operation,
            int maximumAttempts,
            TimeSpan delayBetweenAttempts,
            CancellationToken cancellationToken = default)
        {
            Guard.AgainstNull(operation, nameof(operation));
            Guard.AgainstBelowMinimum(maximumAttempts, 1, nameof(maximumAttempts));
            Guard.AgainstBelowMinimum(delayBetweenAttempts, TimeSpan.Zero, nameof(delayBetweenAttempts));

            int attemptNumber = 1;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    return await operation(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception) when (attemptNumber < maximumAttempts && !cancellationToken.IsCancellationRequested)
                {
                    attemptNumber++;
                    await Task.Delay(delayBetweenAttempts, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public static async Task ExecuteAsync(
            Func<CancellationToken, Task> operation,
            int maximumAttempts,
            TimeSpan delayBetweenAttempts,
            CancellationToken cancellationToken = default)
        {
            Guard.AgainstNull(operation, nameof(operation));

            await ExecuteAsync(
                async token =>
                {
                    await operation(token).ConfigureAwait(false);
                    return true;
                },
                maximumAttempts,
                delayBetweenAttempts,
                cancellationToken).ConfigureAwait(false);
        }
    }
}
