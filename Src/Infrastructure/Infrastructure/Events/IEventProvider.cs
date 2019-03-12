namespace Infrastructure.Events
{
	public interface IEventProvider
	{
		void Publish(Event eventToPublish);

		Event GetNext();

		void RegisterType(string key, string description);
	}
}