namespace Infrastructure.Tags
{
	using System;

	internal sealed class IncorrectTagLengthException : Exception
	{
		public IncorrectTagLengthException(int tagNameLength)
			: base($"Incorrect tag name length. Length='{tagNameLength}'")
		{

		}

	}
}