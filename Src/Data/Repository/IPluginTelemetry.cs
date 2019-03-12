namespace Repository
{
	public interface IPluginTelemetry : ITelemetry
	{
		string AssemblyName { get; set; }

		string TypeFullName { get; set; }

		string DisplayName { get; set; }
	}
}