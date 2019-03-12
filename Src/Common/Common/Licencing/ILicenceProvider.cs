namespace Common.Licencing
{
	public interface ILicenceProvider
	{
		ILicence GetCurrent();
	}
}