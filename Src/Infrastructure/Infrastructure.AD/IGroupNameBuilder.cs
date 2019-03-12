// ReSharper disable MemberCanBeInternal
namespace Infrastructure.AD
{
	// ReSharper disable once MemberCanBeInternal
	public interface IGroupNameBuilder
	{
		GroupNameInfo Build(string roleAlias, string groupName, long? projectId = null);

		GroupNameInfo Build(long roleId, long? projectId = null);
	}

	public sealed class GroupNameInfo
	{
		public string Name { get; set; }

		public string Description { get; set; }
	}
}