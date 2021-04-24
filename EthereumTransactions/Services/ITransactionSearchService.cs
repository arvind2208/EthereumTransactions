using EthereumTransactions.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumTransactions.Services
{
	public interface ITransactionSearchService
	{
		Task<TransactionSearchResponse> ExecuteAsync(TransactionSearchRequest request, CancellationToken token);
	}
}