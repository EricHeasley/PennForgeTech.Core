using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Http;
using PennForgeTech.Core.Results;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class HttpServiceTests
    {
        private sealed record Weather(string City, int Degrees);

        private sealed class StubHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode _statusCode;
            private readonly string _body;

            public StubHandler(HttpStatusCode statusCode, string body)
            {
                this._statusCode = statusCode;
                this._body = body;
            }

            public string? LastRequestBody { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request.Content is not null)
                {
                    this.LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
                }

                HttpResponseMessage response = new HttpResponseMessage(this._statusCode);
                response.Content = new StringContent(this._body, Encoding.UTF8, "application/json");
                return response;
            }
        }

        private static HttpService CreateService(StubHandler handler)
        {
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = new System.Uri("https://example.test");
            return new HttpService(client);
        }

        [Fact]
        public async Task GetJsonAsync_ReturnsTheDeserializedValue()
        {
            HttpService service = CreateService(new StubHandler(HttpStatusCode.OK, "{\"city\":\"Philadelphia\",\"degrees\":72}"));

            Result<Weather> result = await service.GetJsonAsync<Weather>("/weather");

            Assert.True(result.IsSuccess);
            Assert.Equal(new Weather("Philadelphia", 72), result.Value);
        }

        [Fact]
        public async Task GetJsonAsync_ReturnsFailureForErrorStatusCodes()
        {
            HttpService service = CreateService(new StubHandler(HttpStatusCode.NotFound, "{}"));

            Result<Weather> result = await service.GetJsonAsync<Weather>("/weather");

            Assert.True(result.IsFailure);
            Assert.Equal("Http.Status404", result.Error.Code);
        }

        [Fact]
        public async Task GetJsonAsync_ReturnsFailureForInvalidJson()
        {
            HttpService service = CreateService(new StubHandler(HttpStatusCode.OK, "{ nope"));

            Result<Weather> result = await service.GetJsonAsync<Weather>("/weather");

            Assert.Equal("Http.InvalidJson", result.Error.Code);
        }

        [Fact]
        public async Task GetJsonAsync_ReturnsFailureForNullJson()
        {
            HttpService service = CreateService(new StubHandler(HttpStatusCode.OK, "null"));

            Result<Weather> result = await service.GetJsonAsync<Weather>("/weather");

            Assert.Equal("Http.EmptyResponse", result.Error.Code);
        }

        [Fact]
        public async Task PostJsonAsync_SendsTheBodyAndReadsTheResponse()
        {
            StubHandler handler = new StubHandler(HttpStatusCode.OK, "{\"city\":\"Boston\",\"degrees\":60}");
            HttpService service = CreateService(handler);

            Result<Weather> result = await service.PostJsonAsync<Weather, Weather>("/weather", new Weather("Boston", 60));

            Assert.Equal("{\"city\":\"Boston\",\"degrees\":60}", handler.LastRequestBody);
            Assert.Equal(60, result.Value.Degrees);
        }
    }
}
