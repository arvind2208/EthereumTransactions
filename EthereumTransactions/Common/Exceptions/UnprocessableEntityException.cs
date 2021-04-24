using EthereumTransactions.Models;
using Microsoft.Extensions.Logging;
using System;

namespace EthereumTransactions.Common.Exceptions
{
	public class UnprocessableEntityException : BaseHttpException
	{
		public UnprocessableEntityException(ILogger logger, string message, int exceptionCode) : this(logger, message, exceptionCode, null)
		{
		}

		public UnprocessableEntityException(ILogger logger, string message, int exceptionCode, Exception innerException) : base(logger, message, innerException)
		{
			if (innerException != null)
				logger.LogInformation(innerException, message);
			else
				logger.LogInformation(message);

			Result = new ApiResult<object>
			{
				HttpStatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
				Message = message,
				ExceptionCode = exceptionCode
			};
		}
	}
}
