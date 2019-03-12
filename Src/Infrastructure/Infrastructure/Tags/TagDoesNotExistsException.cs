namespace Infrastructure.Tags
{
	using System;

	using JetBrains.Annotations;

	[UsedImplicitly]
	public sealed class TagDoesNotExistsException : Exception
	{
		public TagDoesNotExistsException(string tagName)
			: base($"Tag does not exists. Tag name='{tagName}'")
		{
		}
	}
}