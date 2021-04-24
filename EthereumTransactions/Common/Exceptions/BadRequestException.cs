using EthereumTransactions.Models;
using Microsoft.Extensions.Logging;
using System;

namespace EthereumTransactions.Common.Exceptions
{
	public class BadRequestException : BaseHttpException
	{
		public BadRequestException(ILogger logger, string message) : this(logger, message, null)
		{
		}

		public BadRequestException(ILogger logger, string message, Exception innerException) : base(logger, message, innerException)
		{
			logger.LogError(innerException, message);

			Result = new ApiResult<object>
			{
				HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
				Message = message
			};
		}
	}
}
