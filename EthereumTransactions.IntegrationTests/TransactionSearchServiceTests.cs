using EthereumTransactions.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EthereumTransactions.IntegrationTests
{
	public class TransactionSearchServiceTests : IClassFixture<CustomWebApplicationFactory<TestStartup>>
	{
        private HttpClient _client { get; }

        public TransactionSearchServiceTests(CustomWebApplicationFactory<TestStartup> fixture)
        {
            _client = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:52864/")
            });
        }

        [Fact]
        public async Task WhenValidResponseReturnsOk()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_client.BaseAddress}api/transactions?blockNumber=9148873&address=0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"),
                Method = HttpMethod.Get
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var transactionSearchResponse = JsonConvert.DeserializeObject<TransactionSearchResponse>(await response.Content.ReadAsStringAsync());

            Assert.Equal(2, transactionSearchResponse.Transactions.Count());
        }

        [Fact]
        public async Task WhenEthereumThrowsExceptionsReturnsInternalServerError()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_client.BaseAddress}api/transactions?blockNumber=0000000&address=0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"),
                Method = HttpMethod.Get
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task WhenMissingPayloadReturnsBadRequest()
        {
            var requestContent = new TransactionSearchRequest
            {
                Address = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"
            };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_client.BaseAddress}api/transactions?address=0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"),
                Method = HttpMethod.Get
            };

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
