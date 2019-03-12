namespace Infrastructure.Telemetry
{
	using System;

	using JetBrains.Annotations;

	public interface ITelemetryScope<in T> : IDisposable
		where T : class
	{
		void SetEntity([NotNull] T entity);

		void SetHResult(int? hResult);

		void WriteException(Exception exception = null);

		void WriteSuccess();
	}
}