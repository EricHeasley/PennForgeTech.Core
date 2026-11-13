using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PennForgeTech.Core.Diagnostics;
using PennForgeTech.Core.Results;
using PennForgeTech.Framework.Application;
using PennForgeTech.Framework.Pipelines;
using PennForgeTech.Framework.WebApi;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class FrameworkTests
    {
        private sealed class DoubleHandler : IRequestHandler<int, int>
        {
            public Task<Result<int>> HandleAsync(int request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(Result.Success(request * 2));
            }
        }

        private sealed class RecordingBehavior : IPipelineBehavior<int, int>
        {
            private readonly string _name;
            private readonly List<string> _log;

            public RecordingBehavior(string name, List<string> log)
            {
                this._name = name;
                this._log = log;
            }

            public async Task<Result<int>> HandleAsync(int request, NextStep<int> nextStep, CancellationToken cancellationToken)
            {
                this._log.Add(this._name + " before");
                Result<int> result = await nextStep(cancellationToken);
                this._log.Add(this._name + " after");
                return result;
            }
        }

        private sealed class BlockingBehavior : IPipelineBehavior<int, int>
        {
            public Task<Result<int>> HandleAsync(int request, NextStep<int> nextStep, CancellationToken cancellationToken)
            {
                return Task.FromResult(Result.Failure<int>(new ResultError("Blocked", "Not allowed.")));
            }
        }

        [Fact]
        public async Task Pipeline_RunsBehaviorsInOrderAroundTheHandler()
        {
            List<string> log = new List<string>();
            Pipeline<int, int> pipeline = new Pipeline<int, int>(
                new DoubleHandler(),
                new IPipelineBehavior<int, int>[] { new RecordingBehavior("outer", log), new RecordingBehavior("inner", log) });

            Result<int> result = await pipeline.ExecuteAsync(21);

            Assert.Equal(42, result.Value);
            Assert.Equal(new[] { "outer before", "inner before", "inner after", "outer after" }, log);
        }

        [Fact]
        public async Task Pipeline_LetsABehaviorShortCircuit()
        {
            Pipeline<int, int> pipeline = new Pipeline<int, int>(
                new DoubleHandler(),
                new IPipelineBehavior<int, int>[] { new BlockingBehavior() });

            Result<int> result = await pipeline.ExecuteAsync(21);

            Assert.Equal("Blocked", result.Error.Code);
        }

        [Fact]
        public void ToHttpResult_MapsSuccessToOk()
        {
            IResult httpResult = Result.Success(5).ToHttpResult();

            Ok<int> ok = Assert.IsType<Ok<int>>(httpResult);
            Assert.Equal(5, ok.Value);
        }

        [Fact]
        public void ToHttpResult_MapsFailureToProblemWithBadRequestByDefault()
        {
            IResult httpResult = Result.Failure<int>(new ResultError("Bad", "Nope.")).ToHttpResult();

            ProblemHttpResult problem = Assert.IsType<ProblemHttpResult>(httpResult);
            Assert.Equal(StatusCodes.Status400BadRequest, problem.StatusCode);
        }

        [Fact]
        public void ToHttpResult_UsesTheStatusCodeSelector()
        {
            Result<int> failure = Result.Failure<int>(new ResultError("Item.NotFound", "Missing."));

            IResult httpResult = failure.ToHttpResult(error => StatusCodes.Status404NotFound);

            ProblemHttpResult problem = Assert.IsType<ProblemHttpResult>(httpResult);
            Assert.Equal(StatusCodes.Status404NotFound, problem.StatusCode);
        }

        [Fact]
        public void ToHttpResult_MapsSuccessWithoutValueToNoContent()
        {
            Assert.IsType<NoContent>(Result.Success().ToHttpResult());
        }

        [Fact]
        public void Diagnostics_ActivitySourceUsesTheSharedName()
        {
            Assert.Equal(PennForgeDiagnostics.SourceName, PennForgeDiagnostics.ActivitySource.Name);
        }
    }
}
