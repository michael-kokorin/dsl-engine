namespace Common.Licencing
{
	using System;

	internal sealed class EmptyLicenceIdException : Exception
	{
		public EmptyLicenceIdException() : base("Empty licence Id.")
		{

		}
	}
}
