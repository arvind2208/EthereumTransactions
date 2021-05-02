using AutoMapper;
using EthereumTransactions.Models;
using EthereumTransactions.Services;
using EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura;
using EthereumTransactions.Services.ThirdPartyServices.Infura;
using EthereumTransactions.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EthereumTransactions.UnitTests.Services
{
	public class TransactionSearchServiceTests
	{
		private readonly IMapper _mapper;
		private readonly ILogger<TransactionSearchService> _logger;
        public TransactionSearchServiceTests()
        {
			var mappingProfile = new MappingProfile();
			var config = new MapperConfiguration(mappingProfile);

			_mapper = config.CreateMapper();
			_logger = new Mock<ILogger<TransactionSearchService>>().Object;
        }

		[Fact]
		public async Task WhenPassedValidSearchQueryThenValidTransactionsReturned()
		{
			//Arrange
			var srcResponse = new GetBlockByNumberResponse
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

			var mockEthereumService = new Mock<IEthereumService>();
			mockEthereumService.Setup(x => x.GetBlockByNumberAsync(It.IsAny<GetBlockByNumberRequest>(), CancellationToken.None))
				.ReturnsAsync(srcResponse).Verifiable();

			var service = new TransactionSearchService(mockEthereumService.Object, _mapper, _logger);

			var request = new TransactionSearchRequest
			{
				BlockNumber = 9148873,
				Address = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"
			};

			//Act
			var response = await service.ExecuteAsync(request, CancellationToken.None);

			//Assert
			Assert.NotNull(response);
			var transactions = response.Transactions.ToList();
			Assert.Equal(2, transactions.Count);
			mockEthereumService.VerifyAll();
		}

		[Fact]
		public async Task WhenPassedInvalidSearchQueryThenEmptyArrayOfTransactionsReturned()
		{
			//Arrange
			var srcResponse = new GetBlockByNumberResponse
			{
				Result = new Result
				{
					Transactions = new List<EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura.Transaction>()
				}
			};

			var mockEthereumService = new Mock<IEthereumService>();
			mockEthereumService.Setup(x => x.GetBlockByNumberAsync(It.IsAny<GetBlockByNumberRequest>(), CancellationToken.None))
				.ReturnsAsync(srcResponse).Verifiable();

			var service = new TransactionSearchService(mockEthereumService.Object, _mapper, _logger);

			var request = new TransactionSearchRequest
			{
				BlockNumber = 9148873,
				Address = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"
			};

			//Act
			var response = await service.ExecuteAsync(request, CancellationToken.None);

			//Assert
			Assert.NotNull(response);
			var transactions = response.Transactions;
			Assert.Empty(transactions);
			mockEthereumService.VerifyAll();
		}

		[Fact]
		public async Task WhenGetBlockByNumberServiceThrowsAnexceptionThenExceptionIsRethrown()
		{
			//Arrange
			var mockEthereumService = new Mock<IEthereumService>();
			mockEthereumService.Setup(x => x.GetBlockByNumberAsync(It.IsAny<GetBlockByNumberRequest>(), CancellationToken.None))
				.Throws(new NullReferenceException("some exception")).Verifiable();

			var service = new TransactionSearchService(mockEthereumService.Object, _mapper, _logger);

			var request = new TransactionSearchRequest
			{
				BlockNumber = 9148873,
				Address = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"
			};

			//Act
			var exception = await Assert.ThrowsAsync<NullReferenceException>(() => service.ExecuteAsync(request, CancellationToken.None));

			//Assert
			Assert.Equal("some exception", exception.Message);
			mockEthereumService.VerifyAll();
		}
	}
}
