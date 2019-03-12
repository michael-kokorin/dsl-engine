namespace Workflow.GitHub.Extensions
{
	internal static class StringExtension
	{
		public static string GetLineSplitter(this string content)
		{
			for (var index = 0; index < content.Length; index++)
			{
				var sym = content[index];
				if ((sym != '\r') && (sym != '\n'))
					continue;

				if (sym == '\n')
					return "\n";

				if ((index + 1 < content.Length) && (content[index + 1] == '\n'))
					return "\r\n";

				return "\r";
			}

			return "\n";
		}
	}
}