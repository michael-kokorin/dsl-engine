namespace Infrastructure.Tags
{
	using System;

	using JetBrains.Annotations;

	[UsedImplicitly]
	public sealed class IncorrectTagNameException : Exception
	{
		public IncorrectTagNameException(string tagName)
			: base($"Incorrect Tag name. Tag name='{tagName}'")
		{

		}
	}
}