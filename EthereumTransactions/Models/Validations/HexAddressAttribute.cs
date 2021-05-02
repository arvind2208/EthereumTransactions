using EthereumTransactions.Extensions;
using System.ComponentModel.DataAnnotations;

namespace EthereumTransactions.Models.Validations
{
	public class HexAddressAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var address = value as string;

			if (!string.IsNullOrEmpty(address) 
				&& address.StartsWith("0x") 
				&& address.RemoveHexPrefix().Length == 40 
				&& address.IsHex())
				return true;

			return false;
		}
	}
}
