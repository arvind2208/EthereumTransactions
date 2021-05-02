using EthereumTransactions.Models.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace EthereumTransactions.Models
{
	[Serializable]
	public class TransactionSearchRequest
	{
		[Required]
		[HexAddress]
		public string Address { get; set; }

		[Required]
		[NonHexBlockNumber]
		public int? BlockNumber { get; set; }
	}
}
