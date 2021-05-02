using EthereumTransactions.Models;
using EthereumTransactions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumTransactions.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionsController : ControllerBase
	{
		private readonly ITransactionSearchService _transactionSearchService;
		private readonly ILogger<TransactionsController> _logger;

		public TransactionsController(ITransactionSearchService transactionSearchService, 
			ILogger<TransactionsController> logger)
		{
			_transactionSearchService = transactionSearchService;
			_logger = logger;
		}

		/// <summary>
		/// Searches transactions with the address in the given block
		/// </summary>
		/// <param name="request"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(TransactionSearchResponse), 200)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> SearchTransactionsAsync([FromQuery] TransactionSearchRequest request, CancellationToken token)
		{
			HttpContext.Items.TryGetValue("CorrelationId", out var correlationId);

			_logger.LogInformation($"Search request for CorrelationId : {correlationId} Address: {request.Address} BlockNumber: {request.BlockNumber}");

			var response = await _transactionSearchService.ExecuteAsync(request, token);

			return Ok(response);
		}
	}
}
