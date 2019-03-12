namespace Infrastructure.Tags
{
	using System;
	using System.Text.RegularExpressions;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class TagValidator : ITagValidator
	{
		private const string TagNameRegEx = @"^[\w ]+$";

		private const int TagMinLength = 3;

		private const int TagMaxLenght = 16;

		public void Validate([NotNull] string tagName)
		{
			if (tagName == null) throw new ArgumentNullException(nameof(tagName));

			var regExp = new Regex(TagNameRegEx);

			var isMatch = regExp.IsMatch(tagName);

			if (!isMatch)
				throw new IncorrectTagNameException(tagName);

			var tagNameLength = tagName.Length;

			if (tagNameLength < TagMinLength)
				throw new IncorrectTagLengthException(tagNameLength);

			if (tagName.Length > TagMaxLenght)
				throw new IncorrectTagLengthException(tagNameLength);
		}
	}
}