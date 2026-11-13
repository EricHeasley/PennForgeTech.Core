using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Primitives;
using PennForgeTech.Core.Results;

namespace PennForgeTech.Core.Http
{
    public sealed class HttpService : IHttpService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public HttpService(HttpClient client, JsonSerializerOptions? options = null)
        {
            this._client = Guard.AgainstNull(client, nameof(client));
            this._options = options ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }

        public async Task<Result<TResponse>> GetJsonAsync<TResponse>(string requestUri, CancellationToken cancellationToken = default)
        {
            Guard.AgainstNullOrWhiteSpace(requestUri, nameof(requestUri));

            using (HttpResponseMessage response = await this._client
                .GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false))
            {
                return await this.ReadResponseAsync<TResponse>(response, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<TResponse>> PostJsonAsync<TRequest, TResponse>(
            string requestUri,
            TRequest body,
            CancellationToken cancellationToken = default)
        {
            Guard.AgainstNullOrWhiteSpace(requestUri, nameof(requestUri));

            using (HttpResponseMessage response = await this._client
                .PostAsJsonAsync(requestUri, body, this._options, cancellationToken)
                .ConfigureAwait(false))
            {
                return await this.ReadResponseAsync<TResponse>(response, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task<Result<TResponse>> ReadResponseAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                int statusCode = (int)response.StatusCode;
                string reason = response.ReasonPhrase ?? "The HTTP request failed.";
                return Result.Failure<TResponse>(new ResultError("Http.Status" + statusCode, reason));
            }

            try
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
                {
                    TResponse? value = await JsonSerializer
                        .DeserializeAsync<TResponse>(stream, this._options, cancellationToken)
                        .ConfigureAwait(false);

                    if (value is null)
                    {
                        return Result.Failure<TResponse>(new ResultError("Http.EmptyResponse", "The response contained no JSON value."));
                    }

                    return Result.Success(value);
                }
            }
            catch (JsonException exception)
            {
                return Result.Failure<TResponse>(new ResultError("Http.InvalidJson", exception.Message));
            }
        }
    }
}
