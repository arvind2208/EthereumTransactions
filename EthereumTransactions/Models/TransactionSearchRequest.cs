using System;
using System.ComponentModel.DataAnnotations;

namespace EthereumTransactions.Models
{
	[Serializable]
	public class TransactionSearchRequest
	{
		[Required]
		public string Address { get; set; }

		[Required]
		public int? BlockNumber { get; set; }
	}
}
