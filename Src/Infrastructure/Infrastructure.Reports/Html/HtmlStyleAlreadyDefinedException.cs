namespace Infrastructure.Reports.Html
{
	using System;

	internal sealed class HtmlStyleAlreadyDefinedException : Exception
	{
		public HtmlStyleAlreadyDefinedException(string key) : base($"Html style already defined. Key='{key}'")
		{

		}
	}
}