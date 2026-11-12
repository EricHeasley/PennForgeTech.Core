using System;
using PennForgeTech.Core.Results;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class ResultTests
    {
        private static readonly ResultError NotFoundError = new ResultError("User.NotFound", "The user does not exist.");

        [Fact]
        public void Success_HasNoError()
        {
            Result result = Result.Success();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Throws<InvalidOperationException>(() => result.Error);
        }

        [Fact]
        public void Failure_CarriesTheError()
        {
            Result result = Result.Failure(NotFoundError);

            Assert.True(result.IsFailure);
            Assert.Equal(NotFoundError, result.Error);
        }

        [Fact]
        public void FailureWithValue_ThrowsWhenValueIsRead()
        {
            Result<int> result = Result.Failure<int>(NotFoundError);

            Assert.Throws<InvalidOperationException>(() => result.Value);
        }

        [Fact]
        public void Map_TransformsValueOnSuccess()
        {
            Result<int> result = Result.Success(2);

            Result<string> mapped = result.Map(number => $"number {number}");

            Assert.Equal("number 2", mapped.Value);
        }

        [Fact]
        public void Map_PassesFailureThrough()
        {
            Result<int> result = Result.Failure<int>(NotFoundError);

            Result<string> mapped = result.Map(number => $"number {number}");

            Assert.Equal(NotFoundError, mapped.Error);
        }

        [Fact]
        public void Then_ChainsOnSuccess()
        {
            Result<int> result = Result.Success(2);

            Result<int> chained = result.Then(number => Result.Success(number * 10));

            Assert.Equal(20, chained.Value);
        }

        [Fact]
        public void ValueOr_ReturnsFallbackOnFailure()
        {
            Result<int> result = Result.Failure<int>(NotFoundError);

            Assert.Equal(7, result.ValueOr(7));
        }
    }
}
