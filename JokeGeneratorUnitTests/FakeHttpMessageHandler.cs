using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JokeGeneratorUnitTests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage RequestMessage { get; private set; }
        public int Called { get; private set; }
        public HttpRequestException ThrowException { get; }
        public string ResponseContent { get; }

        private FakeHttpMessageHandler()
        {
            this.Called = 0;
        }

        public FakeHttpMessageHandler(HttpRequestException throwException) : this()
        {
            this.ThrowException = throwException;
        }

        public FakeHttpMessageHandler(string responseContent) : this()
        {
            this.ResponseContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.RequestMessage = request;
            this.Called++;

            if (this.ThrowException != null)
            {
                throw ThrowException;
            }
            else
            {
                var responseContent = new StringContent(this.ResponseContent);
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = responseContent,
                };
                return Task.FromResult(responseMessage);
            }
        }
    }
}
