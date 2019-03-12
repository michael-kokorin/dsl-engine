namespace Common.Licencing
{
	using System;

	internal sealed class IncorrectLicenceIdException : Exception
	{
		public IncorrectLicenceIdException(string key) : base($"Incorrect licence Id. Id='{key}'")
		{

		}
	}
}