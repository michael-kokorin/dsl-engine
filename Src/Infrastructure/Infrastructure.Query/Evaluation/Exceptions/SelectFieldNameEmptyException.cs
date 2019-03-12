namespace Infrastructure.Query.Evaluation.Exceptions
{
	using System;

	using Common.Extensions;
	using Infrastructure.Query.Resources;

	internal sealed class SelectFieldNameEmptyException : Exception
	{
		public SelectFieldNameEmptyException(string fieldValue)
			: base(Resources.SelectFieldNameEmptyExceptionMessage.FormatWith(fieldValue))
		{
		}
	}
}