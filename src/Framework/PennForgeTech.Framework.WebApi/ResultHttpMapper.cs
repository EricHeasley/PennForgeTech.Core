using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PennForgeTech.Core.Primitives;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Framework.WebApi
{
    public static class ResultHttpMapper
    {
        public static IResult ToHttpResult<TValue>(this Result<TValue> result, Func<ResultError, int>? statusCodeSelector = null)
        {
            Guard.AgainstNull(result, nameof(result));

            if (result.IsSuccess)
            {
                return TypedResults.Ok(result.Value);
            }

            return CreateProblem(result.Error, statusCodeSelector);
        }

        public static IResult ToHttpResult(this Result result, Func<ResultError, int>? statusCodeSelector = null)
        {
            Guard.AgainstNull(result, nameof(result));

            if (result.IsSuccess)
            {
                return TypedResults.NoContent();
            }

            return CreateProblem(result.Error, statusCodeSelector);
        }

        private static ProblemHttpResult CreateProblem(ResultError error, Func<ResultError, int>? statusCodeSelector)
        {
            int statusCode = StatusCodes.Status400BadRequest;

            if (statusCodeSelector is not null)
            {
                statusCode = statusCodeSelector(error);
            }

            return TypedResults.Problem(title: error.Code, detail: error.Message, statusCode: statusCode);
        }
    }
}
