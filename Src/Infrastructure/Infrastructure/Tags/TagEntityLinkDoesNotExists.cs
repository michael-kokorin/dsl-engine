namespace Infrastructure.Tags
{
	using System;

	public sealed class TagEntityLinkDoesNotExists : Exception
	{
		public TagEntityLinkDoesNotExists(string tagName, string tableName, long entityId)
			: base($"Tag entity link  does not exists. Tag='{tagName}', Table='{tableName}', EntityId='{entityId}'")
		{
		}
	}
}