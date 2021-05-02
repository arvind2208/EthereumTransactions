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

		public static string RemoveHexPrefix(this string value)
		{
			return value.Substring(value.StartsWith("0x") ? 2 : 0);
		}

		public static bool IsHex(this string value)
		{
			bool isHex;
			foreach (var c in value.RemoveHexPrefix())
			{
				isHex = ((c >= '0' && c <= '9') ||
						 (c >= 'a' && c <= 'f') ||
						 (c >= 'A' && c <= 'F'));

				if (!isHex)
					return false;
			}
			return true;
		}
	}
}
