using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PennForgeTech.Core.Abstractions;
using PennForgeTech.Core.Logging;
using PennForgeTech.Core.Logging.MicrosoftExtensions;
using PennForgeTech.Core.Results;
using PennForgeTech.Framework.Application;
using PennForgeTech.Framework.Pipelines;
using Xunit;
using MicrosoftLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace PennForgeTech.Core.Tests
{
    public sealed class LoggingTests
    {
        private readonly FixedClock _clock = new FixedClock(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        private sealed class RecordingMicrosoftLogger : Microsoft.Extensions.Logging.ILogger
        {
            public List<(MicrosoftLogLevel Level, string Message, Exception? Exception)> Records { get; } =
                new List<(MicrosoftLogLevel, string, Exception?)>();

            public MicrosoftLogLevel MinimumLevel { get; set; } = MicrosoftLogLevel.Trace;

            public IDisposable? BeginScope<TState>(TState state)
                where TState : notnull
            {
                return null;
            }

            public bool IsEnabled(MicrosoftLogLevel logLevel)
            {
                return logLevel >= this.MinimumLevel;
            }

            public void Log<TState>(
                MicrosoftLogLevel logLevel,
                Microsoft.Extensions.Logging.EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                this.Records.Add((logLevel, formatter(state, exception), exception));
            }
        }

        private sealed class FailingHandler : IRequestHandler<string, int>
        {
            public Task<Result<int>> HandleAsync(string request, System.Threading.CancellationToken cancellationToken = default)
            {
                return Task.FromResult(Result.Failure<int>(new ResultError("Request.Rejected", "Not today.")));
            }
        }

        private sealed class ThrowingHandler : IRequestHandler<string, int>
        {
            public Task<Result<int>> HandleAsync(string request, System.Threading.CancellationToken cancellationToken = default)
            {
                throw new InvalidOperationException("Boom.");
            }
        }

        [Fact]
        public void InMemoryAppLogger_RecordsEntriesWithTimestampLevelAndException()
        {
            InMemoryAppLogger logger = new InMemoryAppLogger(this._clock);
            InvalidOperationException exception = new InvalidOperationException("Boom.");

            logger.LogInformation("Started.");
            logger.LogError(exception, "Failed.");

            Assert.Equal(2, logger.Entries.Count);
            Assert.Equal(LogLevel.Information, logger.Entries[0].Level);
            Assert.Equal("Started.", logger.Entries[0].Message);
            Assert.Equal(this._clock.UtcNow, logger.Entries[0].Timestamp);
            Assert.Same(exception, logger.Entries[1].Exception);
        }

        [Fact]
        public void ConsoleAppLogger_WritesOnlyMessagesAtOrAboveTheMinimumLevel()
        {
            StringWriter writer = new StringWriter();
            ConsoleAppLogger logger = new ConsoleAppLogger(this._clock, LogLevel.Warning, writer);

            logger.LogInformation("Hidden.");
            logger.LogWarning("Shown.");

            string output = writer.ToString();
            Assert.DoesNotContain("Hidden.", output);
            Assert.Contains("2026-01-01T00:00:00.0000000+00:00 [Warning] Shown.", output);
        }

        [Fact]
        public void ConsoleAppLogger_WritesTheExceptionDetails()
        {
            StringWriter writer = new StringWriter();
            ConsoleAppLogger logger = new ConsoleAppLogger(this._clock, LogLevel.Information, writer);

            logger.LogError(new InvalidOperationException("Boom."), "Failed.");

            Assert.Contains("InvalidOperationException: Boom.", writer.ToString());
        }

        [Fact]
        public void NullAppLogger_IgnoresEverything()
        {
            NullAppLogger.Instance.LogCritical(new InvalidOperationException("Boom."), "Ignored.");

            Assert.False(NullAppLogger.Instance.IsEnabled(LogLevel.Critical));
        }

        [Theory]
        [InlineData(LogLevel.Trace, MicrosoftLogLevel.Trace)]
        [InlineData(LogLevel.Debug, MicrosoftLogLevel.Debug)]
        [InlineData(LogLevel.Information, MicrosoftLogLevel.Information)]
        [InlineData(LogLevel.Warning, MicrosoftLogLevel.Warning)]
        [InlineData(LogLevel.Error, MicrosoftLogLevel.Error)]
        [InlineData(LogLevel.Critical, MicrosoftLogLevel.Critical)]
        public void MicrosoftAppLogger_MapsLevelsMessagesAndExceptions(LogLevel level, MicrosoftLogLevel expectedLevel)
        {
            RecordingMicrosoftLogger recorder = new RecordingMicrosoftLogger();
            MicrosoftAppLogger logger = new MicrosoftAppLogger(recorder);
            InvalidOperationException exception = new InvalidOperationException("Boom.");

            logger.Log(level, "Hello.", exception);

            Assert.Single(recorder.Records);
            Assert.Equal(expectedLevel, recorder.Records[0].Level);
            Assert.Equal("Hello.", recorder.Records[0].Message);
            Assert.Same(exception, recorder.Records[0].Exception);
        }

        [Fact]
        public void MicrosoftAppLogger_ReportsWhetherALevelIsEnabled()
        {
            RecordingMicrosoftLogger recorder = new RecordingMicrosoftLogger { MinimumLevel = MicrosoftLogLevel.Error };
            MicrosoftAppLogger logger = new MicrosoftAppLogger(recorder);

            Assert.False(logger.IsEnabled(LogLevel.Information));
            Assert.True(logger.IsEnabled(LogLevel.Critical));
        }

        [Fact]
        public async Task LoggingBehavior_LogsSuccessfulRequests()
        {
            InMemoryAppLogger logger = new InMemoryAppLogger(this._clock);
            Pipeline<string, int> pipeline = new Pipeline<string, int>(
                new LengthHandler(),
                new IPipelineBehavior<string, int>[] { new LoggingBehavior<string, int>(logger, this._clock) });

            await pipeline.ExecuteAsync("hello");

            Assert.Equal("Handling String.", logger.Entries[0].Message);
            Assert.StartsWith("String completed in", logger.Entries[1].Message);
        }

        [Fact]
        public async Task LoggingBehavior_LogsAWarningForFailedResults()
        {
            InMemoryAppLogger logger = new InMemoryAppLogger(this._clock);
            Pipeline<string, int> pipeline = new Pipeline<string, int>(
                new FailingHandler(),
                new IPipelineBehavior<string, int>[] { new LoggingBehavior<string, int>(logger, this._clock) });

            await pipeline.ExecuteAsync("hello");

            Assert.Equal(LogLevel.Warning, logger.Entries[1].Level);
            Assert.Contains("Request.Rejected", logger.Entries[1].Message);
        }

        [Fact]
        public async Task LoggingBehavior_LogsAnErrorAndRethrowsWhenTheHandlerThrows()
        {
            InMemoryAppLogger logger = new InMemoryAppLogger(this._clock);
            Pipeline<string, int> pipeline = new Pipeline<string, int>(
                new ThrowingHandler(),
                new IPipelineBehavior<string, int>[] { new LoggingBehavior<string, int>(logger, this._clock) });

            await Assert.ThrowsAsync<InvalidOperationException>(() => pipeline.ExecuteAsync("hello"));

            Assert.Equal(LogLevel.Error, logger.Entries[1].Level);
            Assert.IsType<InvalidOperationException>(logger.Entries[1].Exception);
        }

        private sealed class LengthHandler : IRequestHandler<string, int>
        {
            public Task<Result<int>> HandleAsync(string request, System.Threading.CancellationToken cancellationToken = default)
            {
                return Task.FromResult(Result.Success(request.Length));
            }
        }
    }
}
