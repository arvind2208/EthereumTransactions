using EthereumTransactions.Models;
using Microsoft.Extensions.Logging;
using System;

namespace EthereumTransactions.Common.Exceptions
{
	public class NotFoundException : BaseHttpException
	{
		public NotFoundException(ILogger logger, string message) : this(logger, message, null)
		{
		}

		public NotFoundException(ILogger logger, string message, Exception innerException) : base(logger, message, innerException)
		{
			logger.LogError(innerException, message);

			Result = new ApiResult<object>
			{
				HttpStatusCode = System.Net.HttpStatusCode.NotFound,
				Message = message
			};
		}
	}
}
