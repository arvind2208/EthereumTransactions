using EthereumTransactions.Options;
using EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EthereumTransactions.Services.ThirdPartyServices.Infura
{
	public class EthereumServiceTests 
	{
        private readonly IOptions<EthereumOptions> _ethereumOptions;
        private readonly ILogger<EthereumService> _logger;
        public EthereumServiceTests()
		{
            var ethereumOptions = new Mock<IOptions<EthereumOptions>>();

            ethereumOptions.Setup(x => x.Value).Returns(new EthereumOptions
            {
                BaseUrl = "https://dummy.infura.io/v3/",
                ProjectId = "dummyProject"
            });

            _ethereumOptions = ethereumOptions.Object;

            _logger = new Mock<ILogger<EthereumService>>().Object;
        }

        [Fact]
		public async Task WhenPassedValidSearchQueryThenValidTransactionsReturned()
		{
			var mockResponse = "{\"jsonrpc\": \"2.0\",\"id\": 1,\"result\": {\"difficulty\": \"0x92acba0bc9ee9\",\"extraData\": \"0x5050594520737061726b706f6f6c2d6574682d636e2d687a34\",\"gasLimit\": \"0x9870b0\",\"gasUsed\": \"0x9861c4\",\"hash\": \"0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1\",\"logsBloom\": \"0x26de480d43833fa8a58c064ea30993953c0a73009c3a5214150b1310a313632b644d002ffd84e95642a6dc7212b1c94790441e180d4a498b428cd28d34ecc660572405819c878da0e01cbe695a0ae88926c27eae9505239a201acb2588cd3d1366d749157bd5c8136d205011600b3f307f8162d5d4b24c365263073687e1700b0e08e31d56470048e7888c752220485b10765bc949ca384cfd1a119ac8b204609a4e2d4ecd0817dd0f89e788ecf9aaa4049260ce6780615ef8d280a066f480e52e90b0632998222db4d940c42104ad55fc3d12a0c60b218c0a9393e029c639b4b0b6543c10625408141ad4a49d803e8c0a44c90ab95183f7045a109849342847\",\"miner\": \"0x5a0b54d5dc17e0aadc383d2db43b0a0d3e029c4c\",\"mixHash\": \"0x15cb91e75a426171bc165baa20045d9012ed2c35f04c67998c647b438be93f33\",\"nonce\": \"0xe1709a0011208e95\",\"number\": \"0x8b99c9\",\"parentHash\": \"0x260e32bca150f18b32863cfe6490945376e7d3a9790e61e85c091d0d48c6cd81\",\"receiptsRoot\": \"0x42cf423349e2c5d9ad2ead1a98f3d571e426752f39f51edb197ea3e3d64aba67\",\"sha3Uncles\": \"0x1dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d49347\",\"size\": \"0x98d4\",\"stateRoot\": \"0xd330eff4f82be0732f867ec1d1f630d254a2aa42be23c40d0c0b10942e3c781f\",\"timestamp\": \"0x5e003833\",\"totalDifficulty\": \"0x2d6401524274158d1b9\",\"transactions\": [{                \"blockHash\": \"0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1\",                \"blockNumber\": \"0x8b99c9\",                \"from\": \"0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa\",                \"gas\": \"0x5208\",                \"gasPrice\": \"0x4a817c800\",                \"hash\": \"0x1fd509bc8a1f26351400f4ca206dbe7b8ebb8dcf3925ddf7201e8764e1bd3ea3\",                \"input\": \"0x\",                \"nonce\": \"0x95e7\",                \"r\": \"0x8e0bd4787e3396dc1697ef278960f9d9743323d3e2b8d6a67f773f305385fe89\",                \"s\": \"0x7fe255386e1bb617c630a0ff8afdb2cc1affab7367df48232bfaddb7bd5b9d22\",                \"to\": \"0x59422595656a6b7c8917625607934d0e11fa0c40\",                \"transactionIndex\": \"0x3e\",                \"type\": \"0x0\",                \"v\": \"0x1c\",                \"value\": \"0x4563918244f400000\"            },            {                \"blockHash\": \"0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1\",                \"blockNumber\": \"0x8b99c9\",                \"from\": \"0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa\",                \"gas\": \"0x5208\",                \"gasPrice\": \"0x4a817c800\",                \"hash\": \"0xfcbbca93ff416601e5be95838fcfa2c534c48027b10172c12bf069784a4ec634\",                \"input\": \"0x\",                \"nonce\": \"0x95e8\",                \"r\": \"0x1c79013f8efbb2e4dce3d29e3626f08df16247b5069e58a88584878235d89f03\",                \"s\": \"0x168a66cf4819a5663e32eb09535b005211c9daae7bd25bde58e0e7f43f02adbf\",                \"to\": \"0x15776a03ef5fdf2a54a1b3a991c1641d0bfa39e7\",                \"transactionIndex\": \"0x3f\",                \"type\": \"0x0\",                \"v\": \"0x1c\",                \"value\": \"0xf17937cf93cc0000\"            }]}}";
			var httpClient = GetMockHttpClient(HttpStatusCode.OK, mockResponse);

			var service = new EthereumService(httpClient, _ethereumOptions, _logger);

			var request = new GetBlockByNumberRequest
			{
				Id = "1",
				Jsonrpc = "2.0",
				Method = "eth_getBlockByNumber",
				Params = new object[] { "0X8B99C9", true }
			};

			var response = await service.GetBlockByNumberAsync(request, CancellationToken.None);

			Assert.NotNull(response);
			
			Assert.Equal(2, response.Result.Transactions.Where(x =>
					x.BlockNumber.Equals("0x8b99c9", StringComparison.OrdinalIgnoreCase)).Count());

			Assert.Equal(2, response.Result.Transactions.Where(x =>
					x.From.Equals("0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa", StringComparison.OrdinalIgnoreCase)).Count());

		}

		[Fact]
		public async Task WhenNoTransactionsExistsThenEmptyArrayReturned()
		{
			var mockResponse = "{\"jsonrpc\": \"2.0\",\"id\": 1,\"result\": {\"difficulty\": \"0x92acba0bc9ee9\",\"extraData\": \"0x5050594520737061726b706f6f6c2d6574682d636e2d687a34\",\"gasLimit\": \"0x9870b0\",\"gasUsed\": \"0x9861c4\",\"hash\": \"0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1\",\"logsBloom\": \"0x26de480d43833fa8a58c064ea30993953c0a73009c3a5214150b1310a313632b644d002ffd84e95642a6dc7212b1c94790441e180d4a498b428cd28d34ecc660572405819c878da0e01cbe695a0ae88926c27eae9505239a201acb2588cd3d1366d749157bd5c8136d205011600b3f307f8162d5d4b24c365263073687e1700b0e08e31d56470048e7888c752220485b10765bc949ca384cfd1a119ac8b204609a4e2d4ecd0817dd0f89e788ecf9aaa4049260ce6780615ef8d280a066f480e52e90b0632998222db4d940c42104ad55fc3d12a0c60b218c0a9393e029c639b4b0b6543c10625408141ad4a49d803e8c0a44c90ab95183f7045a109849342847\",\"miner\": \"0x5a0b54d5dc17e0aadc383d2db43b0a0d3e029c4c\",\"mixHash\": \"0x15cb91e75a426171bc165baa20045d9012ed2c35f04c67998c647b438be93f33\",\"nonce\": \"0xe1709a0011208e95\",\"number\": \"0x8b99c9\",\"parentHash\": \"0x260e32bca150f18b32863cfe6490945376e7d3a9790e61e85c091d0d48c6cd81\",\"receiptsRoot\": \"0x42cf423349e2c5d9ad2ead1a98f3d571e426752f39f51edb197ea3e3d64aba67\",\"sha3Uncles\": \"0x1dcc4de8dec75d7aab85b567b6ccd41ad312451b948a7413f0a142fd40d49347\",\"size\": \"0x98d4\",\"stateRoot\": \"0xd330eff4f82be0732f867ec1d1f630d254a2aa42be23c40d0c0b10942e3c781f\",\"timestamp\": \"0x5e003833\",\"totalDifficulty\": \"0x2d6401524274158d1b9\",\"transactions\": []}}";
			var httpClient = GetMockHttpClient(HttpStatusCode.OK, mockResponse);

			var service = new EthereumService(httpClient, _ethereumOptions, _logger);

			var request = new GetBlockByNumberRequest
			{
				Id = "1",
				Jsonrpc = "2.0",
				Method = "eth_getBlockByNumber",
				Params = new object[] { "0X8B99C9", true }
			};

			var response = await service.GetBlockByNumberAsync(request, CancellationToken.None);

			Assert.NotNull(response);

			Assert.Empty(response.Result.Transactions);
		}

		[Fact]
		public async Task WhenErrorStatusCodeReturnedThenHttpRequestExceptionIsThrown()
		{
			var mockResponse = "{}";
			var httpClient = GetMockHttpClient(HttpStatusCode.InternalServerError, mockResponse);

			var service = new EthereumService(httpClient, _ethereumOptions, _logger);

			var request = new GetBlockByNumberRequest
			{
				Id = "1",
				Jsonrpc = "2.0",
				Method = "eth_getBlockByNumber",
				Params = new object[] { "0X8B99C9", true }
			};

			var exception = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetBlockByNumberAsync(request, CancellationToken.None));

			Assert.NotNull(exception);
		}

		private HttpClient GetMockHttpClient(HttpStatusCode httpStatusCode, string mockResponse)
		{
			var handlerMock = new Mock<HttpMessageHandler>();
			var response = new HttpResponseMessage
			{
				StatusCode = httpStatusCode,
				Content = new StringContent(mockResponse),
			};

			handlerMock
			   .Protected()
			   .Setup<Task<HttpResponseMessage>>(
				  "SendAsync",
				  ItExpr.IsAny<HttpRequestMessage>(),
				  ItExpr.IsAny<CancellationToken>())
			   .ReturnsAsync(response);

			var httpClient = new HttpClient(handlerMock.Object);
			return httpClient;
		}
	}
}
