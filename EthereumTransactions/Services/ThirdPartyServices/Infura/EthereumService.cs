using EthereumTransactions.Options;
using EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura;
using EthereumTransactions.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EthereumTransactions.Services.ThirdPartyServices.Infura
{
	public class EthereumService : IEthereumService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<EthereumService> _logger;
		private readonly EthereumOptions _options;

		public EthereumService(HttpClient httpClient,
			IOptions<EthereumOptions> optionsAccessor,
			ILogger<EthereumService> logger)
		{
			_httpClient = httpClient;
			_options = optionsAccessor.Value;
			_logger = logger;
		}

		public async Task<GetBlockByNumberResponse> GetBlockByNumberAsync(GetBlockByNumberRequest request, CancellationToken token)
		{
			_logger.LogInformation($"Calling {Constants.EthGetBlockByNumber} Payload : {JsonConvert.SerializeObject(request)}");

			var httpRequest = new HttpRequestMessage(HttpMethod.Post,
			   $"{_options.BaseUrl}{_options.ProjectId}")
			{
				Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
				   "application/json")
			};

			var response = await _httpClient.SendAsync(httpRequest, token);

			response.EnsureSuccessStatusCode();

			_logger.LogInformation($"Received {Constants.EthGetBlockByNumber} response successfully");

			var apiResponse = JsonConvert.DeserializeObject<GetBlockByNumberResponse>(await response.Content.ReadAsStringAsync());

			_logger.LogInformation($"Deserialized {Constants.EthGetBlockByNumber} response successfully");

			return apiResponse;
		}
	}
}
