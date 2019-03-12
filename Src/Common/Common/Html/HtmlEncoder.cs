namespace Common.Html
{
	using System.Web;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class HtmlEncoder :IHtmlEncoder
	{
		public string Encode(string source) => HttpUtility.HtmlEncode(source);
	}
}