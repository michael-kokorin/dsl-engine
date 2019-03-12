namespace Common.Licencing
{
	public interface ILicence
	{
		string Description { get; }

		string Id { get; }

		TComponent Get<TComponent>() where TComponent : ILicenceComponent;
	}
}