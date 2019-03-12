namespace Repository
{
	public interface IEntityWithProject : IEntity
	{
		long? ProjectId { get; }
	}
}