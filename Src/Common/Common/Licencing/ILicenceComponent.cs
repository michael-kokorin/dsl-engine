namespace Common.Licencing
{
	using System.Collections.Generic;

	public interface ILicenceComponent
	{
		IDictionary<string, string> GetCapabilities();
	}
}