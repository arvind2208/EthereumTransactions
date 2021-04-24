using Newtonsoft.Json;
using System.Net;
using System.Runtime.Serialization;

namespace EthereumTransactions.Models
{
	[DataContract]
	public class ApiResult<T>
	{
		[JsonIgnore]
		public HttpStatusCode HttpStatusCode { get; set; }
		[DataMember]
		public string Message { get; set; }
		[DataMember]
		public int ExceptionCode { get; set; }

		[DataMember]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public T Data { get; set; }
	}
}
