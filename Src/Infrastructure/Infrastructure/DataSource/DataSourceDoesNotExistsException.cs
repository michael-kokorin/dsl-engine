namespace Infrastructure.DataSource
{
	using System;

	using Common.Extensions;
	using Infrastructure.Resources;

	internal sealed class DataSourceDoesNotExistsException : Exception
	{
		public DataSourceDoesNotExistsException(string dataSourceName)
			: base(Resources.DataSourceDoesNotExistsExceptionMessageWithDataSourceName.FormatWith(dataSourceName))
		{

		}
	}
}