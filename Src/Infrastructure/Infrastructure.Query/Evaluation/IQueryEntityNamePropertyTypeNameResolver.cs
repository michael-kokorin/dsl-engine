namespace Infrastructure.Query.Evaluation
{
	// ReSharper disable once MemberCanBeInternal
	public interface IQueryEntityNamePropertyTypeNameResolver
	{
		string ResolvePropertyTypeName(string entityTypeName, string propertyName);
	}
}