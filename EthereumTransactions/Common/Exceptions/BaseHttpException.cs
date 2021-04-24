using EthereumTransactions.Models;
using Microsoft.Extensions.Logging;
using System;

namespace EthereumTransactions.Common.Exceptions
{
	public abstract class BaseHttpException : Exception
	{
		public ApiResult<object> Result { get; set; }

		protected BaseHttpException(ILogger logger, string message) : this(logger, message, null)
		{
		}

		protected BaseHttpException(ILogger logger, string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
