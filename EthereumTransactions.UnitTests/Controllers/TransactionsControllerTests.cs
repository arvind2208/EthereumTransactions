using EthereumTransactions.Controllers;
using EthereumTransactions.Models;
using EthereumTransactions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EthereumTransactions.UnitTests.Controllers
{
	public class TransactionsControllerTests
	{
		private readonly Mock<ITransactionSearchService> _mockTransactionSearchService;
		private readonly ILogger<TransactionsController> _logger;

		public TransactionsControllerTests()
		{
			_mockTransactionSearchService = new Mock<ITransactionSearchService>();

			_logger = new Mock<ILogger<TransactionsController>>().Object;
		}

		[Fact]
		public async Task WhenServiceReturnsValidResponseThenOkResponseIsReturned()
		{
			//Arrange
			var transactionSearchRequest = new TransactionSearchRequest
			{
				BlockNumber = 9148873,
				Address = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa"
			};

			_mockTransactionSearchService.Setup(m => m.ExecuteAsync(It.IsAny<TransactionSearchRequest>(), CancellationToken.None))
				.ReturnsAsync(new TransactionSearchResponse
				{
					Transactions = new List<Transaction>
					{
						new Transaction
						{
							BlockHash = "0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1",
							BlockNumber = 9148873,
							From = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa",
							Gas = 0.000000000000021M,
							Hash = "0x1fd509bc8a1f26351400f4ca206dbe7b8ebb8dcf3925ddf7201e8764e1bd3ea3",
							To = "0x59422595656a6b7c8917625607934d0e11fa0c40",
							Value = 80M
						},
						new Transaction
						{
							BlockHash = "0x6dbde4b224013c46537231c548bd6ff8e2a2c927c435993d351866d505c523f1",
							BlockNumber = 9148873,
							From = "0xc55eddadeeb47fcde0b3b6f25bd47d745ba7e7fa",
							Gas = 0.000000000000021M,
							Hash = "0xfcbbca93ff416601e5be95838fcfa2c534c48027b10172c12bf069784a4ec634",
							To = "0x15776a03ef5fdf2a54a1b3a991c1641d0bfa39e7",
							Value = 17.4M
						}
					}
				})
				.Verifiable();

			var service = new TransactionsController(_mockTransactionSearchService.Object, _logger);

			//Act
			var response = await service.SearchTransactionsAsync(transactionSearchRequest, CancellationToken.None) as OkObjectResult;
			var responseObject = response.Value as TransactionSearchResponse;

			//Assert
			Assert.Equal(200, response.StatusCode);
			Assert.NotNull(responseObject);
			Assert.Equal(2, responseObject.Transactions.Count());
			_mockTransactionSearchService.VerifyAll();
		}
	}
}
