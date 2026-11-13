using System;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Resilience;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class ResilienceTests
    {
        [Fact]
        public async Task ExecuteAsync_RetriesUntilTheOperationSucceeds()
        {
            int attemptCount = 0;

            int result = await RetryExecutor.ExecuteAsync(
                token =>
                {
                    attemptCount++;

                    if (attemptCount < 3)
                    {
                        throw new InvalidOperationException("Not yet.");
                    }

                    return Task.FromResult(42);
                },
                maximumAttempts: 5,
                delayBetweenAttempts: TimeSpan.Zero);

            Assert.Equal(42, result);
            Assert.Equal(3, attemptCount);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsTheLastExceptionWhenAttemptsRunOut()
        {
            int attemptCount = 0;

            await Assert.ThrowsAsync<InvalidOperationException>(() => RetryExecutor.ExecuteAsync<int>(
                token =>
                {
                    attemptCount++;
                    throw new InvalidOperationException("Always fails.");
                },
                maximumAttempts: 3,
                delayBetweenAttempts: TimeSpan.Zero));

            Assert.Equal(3, attemptCount);
        }

        [Fact]
        public async Task ExecuteAsync_StopsWhenCancelled()
        {
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                source.Cancel();

                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => RetryExecutor.ExecuteAsync(
                    token => Task.FromResult(1),
                    maximumAttempts: 3,
                    delayBetweenAttempts: TimeSpan.Zero,
                    cancellationToken: source.Token));
            }
        }

        [Fact]
        public async Task ExecuteAsync_RunsActionsWithoutAResult()
        {
            int attemptCount = 0;

            await RetryExecutor.ExecuteAsync(
                token =>
                {
                    attemptCount++;
                    return Task.CompletedTask;
                },
                maximumAttempts: 2,
                delayBetweenAttempts: TimeSpan.Zero);

            Assert.Equal(1, attemptCount);
        }
    }
}
