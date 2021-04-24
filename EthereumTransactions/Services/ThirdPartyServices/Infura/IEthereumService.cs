using EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumTransactions.Services.ThirdPartyServices.Infura
{
	public interface IEthereumService
	{
		Task<GetBlockByNumberResponse> GetBlockByNumberAsync(GetBlockByNumberRequest request, CancellationToken token);
	}
}