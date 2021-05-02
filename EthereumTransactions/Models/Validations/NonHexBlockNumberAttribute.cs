using System;
using System.ComponentModel.DataAnnotations;

namespace EthereumTransactions.Models.Validations
{
	public class NonHexBlockNumberAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var blockNumber = Convert.ToInt64(value);
			if ($"{blockNumber:X2}".Length == 6)
				return true;

			return false;
		}
	}
}
