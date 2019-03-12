namespace Plugins.Rtc.It
{
	using System;

	internal sealed class IncorrectProjectResourceException : Exception
	{
		public IncorrectProjectResourceException(string resourceLink)
			: base($"Incorrect project resource link. Resource='{resourceLink}'")
		{
		}
	}
}