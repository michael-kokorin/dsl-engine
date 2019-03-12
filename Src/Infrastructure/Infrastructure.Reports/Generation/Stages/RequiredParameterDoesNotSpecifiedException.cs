namespace Infrastructure.Reports.Generation.Stages
{
	using System;

	internal sealed class RequiredParameterDoesNotSpecifiedException : Exception
	{
		public RequiredParameterDoesNotSpecifiedException(string parameterName)
			: base($"Required parameter does not specified. Parameter name='{parameterName}'")
		{
		}
	}
}