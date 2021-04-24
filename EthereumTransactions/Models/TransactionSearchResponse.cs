using System.Collections.Generic;

namespace EthereumTransactions.Models
{
	public class TransactionSearchResponse
	{
		public IEnumerable<Transaction> Transactions { get; set; }
	}
}
