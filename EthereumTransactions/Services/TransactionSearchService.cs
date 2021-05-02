using AutoMapper;
using EthereumTransactions.Models;
using EthereumTransactions.Services.ThirdPartyServices.Infura;
using Microsoft.Extensions.Logging;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumTransactions.Services
{
	public class TransactionSearchService : ITransactionSearchService
	{
		private readonly IEthereumService _ethereumService;
		private readonly IMapper _mapper;
		private readonly ILogger<TransactionSearchService> _logger;

		public TransactionSearchService(
			IEthereumService ethereumService,
			IMapper mapper,
			ILogger<TransactionSearchService> logger)
		{
			_ethereumService = ethereumService;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<TransactionSearchResponse> ExecuteAsync(TransactionSearchRequest request, CancellationToken token)
		{
			_logger.LogInformation($"Searching transactions for address {request.Address} in block {request.BlockNumber}");

			var getBlockByNumberRequest = _mapper.Map<ThirdPartyServices.Dtos.Infura.GetBlockByNumberRequest>(request);

			var getBlockByNumberResponse = await _ethereumService.GetBlockByNumberAsync(getBlockByNumberRequest, token);

			var ethereumTransactions = getBlockByNumberResponse.Result.Transactions.Where(x =>
											x.From.Equals(request.Address, StringComparison.OrdinalIgnoreCase) ||
											x.To.Equals(request.Address, StringComparison.OrdinalIgnoreCase));

			_logger.LogInformation($"Found {ethereumTransactions.Count()} for {request.Address} in block {request.BlockNumber}");

			var transactions = _mapper.Map<IEnumerable<Transaction>>(ethereumTransactions);

			_logger.LogInformation($"Search results mapped successfully");

			return new TransactionSearchResponse { Transactions = transactions ?? new List<Transaction>() };
		}
	}
}
