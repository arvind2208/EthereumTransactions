namespace EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura
{
	public class GetBlockByNumberRequest
	{
		public string Jsonrpc { get; set; }
		public string Method { get; set; }
		public object[] Params { get; set; }

		public string Id { get; set; }
	}
}
