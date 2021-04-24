using System.Globalization;
using System.Numerics;

namespace EthereumTransactions.Extensions
{
	public static class HexConverterExtensions
	{
		public static BigInteger HexToBigInteger(this string value)
		{
			if (BigInteger.TryParse(value.Replace("x", string.Empty), NumberStyles.AllowHexSpecifier, null, out var x))
				return x;

			return default;
		}
	}
}
