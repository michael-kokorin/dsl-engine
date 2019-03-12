namespace Infrastructure.Telemetry
{
	using JetBrains.Annotations;

	using Repository;

	public interface ITelemetryQueue
	{
		bool AutoCommit { get; set; }

		void Commit();

		void Send<T>([NotNull] T telemetryData) where T: class, ITelemetry;
	}
}