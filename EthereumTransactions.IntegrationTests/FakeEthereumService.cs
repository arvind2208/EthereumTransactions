using EthereumTransactions.Options;
using EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura;
using EthereumTransactions.Services.ThirdPartyServices.Infura;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumTransactions.IntegrationTests
{
	public class FakeEthereumService : IEthereumService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<FakeEthereumService> _logger;
		private readonly EthereumOptions _options;

		public FakeEthereumService(HttpClient httpClient,
			IOptions<EthereumOptions> optionsAccessor,
			ILogger<FakeEthereumService> logger)
		{
			_httpClient = httpClient;
			_options = optionsAccessor.Value;
			_logger = logger;
		}

		public async Task<GetBlockByNumberResponse> GetBlockByNumberAsync(GetBlockByNumberRequest request, CancellationToken token)
		{
			if((string)request.Params[0] == "0X8B99CA")
			{
				throw new HttpRequestException("some error");
			}

			return new GetBlockByNumberResponse
			{
				Result = new Result
				{
					Transactions = new List<EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura.Transaction>
					{
						new EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura.Transaction
						{
							BlockHash = "0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1",
							BlockNumber = "0x8b99c9",
							From = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa",
							Gas = "0x5208",
							GasPrice = "0x4a817c800",
							Hash = "0x1fd509bc8a1f26351400f4ca206dbe7b8ebb8dcf3925ddf7201e8764e1bd3ea3",
							Input = "0x",
							Nonce = "0x95e7",
							R = "0x8e0bd4787e3396dc1697ef278960f9d9743323d3e2b8d6a67f773f305385fe89",
							S = "0x7fe255386e1bb617c630a0ff8afdb2cc1affab7367df48232bfaddb7bd5b9d22",
							To = "0x59422595656a6b7c8917625607934d0e11fa0c40",
							TransactionIndex = "0x3e",
							Type = "0x0",
							V = "0x1c",
							Value = "0x4563918244f400000"
						},
						new EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura.Transaction
						{
							BlockHash = "0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1",
							BlockNumber = "0x8b99c9",
							From = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa",
							Gas = "0x5208",
							GasPrice = "0x4a817c800",
							Hash = "0xfcbbca93ff416601e5be95838fcfa2c534c48027b10172c12bf069784a4ec634",
							Input = "0x",
							Nonce = "0x95e7",
							R = "0x1c79013f8efbb2e4dce3d29e3626f08df16247b5069e58a88584878235d89f03",
							S = "0x168a66cf4819a5663e32eb09535b005211c9daae7bd25bde58e0e7f43f02adbf",
							To = "0x15776a03ef5fdf2a54a1b3a991c1641d0bfa39e7",
							TransactionIndex = "0x3f",
							Type = "0x0",
							V = "0x1c",
							Value = "0xf17937cf93cc0000"
						}
					}
				}
			};
		}
	}
}
