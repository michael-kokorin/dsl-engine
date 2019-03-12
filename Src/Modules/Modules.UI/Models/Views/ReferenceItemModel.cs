namespace Modules.UI.Models.Views
{
	using System;

	using JetBrains.Annotations;

	public sealed class ReferenceItemModel
	{
		public string Text { get; set; }

		public long Value { get; set; }

		public ReferenceItemModel(long value, [NotNull] string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			Value = value;

			Text = text;
		}
	}
}