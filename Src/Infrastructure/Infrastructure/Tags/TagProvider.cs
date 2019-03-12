namespace Infrastructure.Tags
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class TagProvider : ITagProvider
	{
		private readonly ITagRepository _tagRepository;

		private readonly ITagValidator _tagValidator;

		public TagProvider([NotNull] ITagRepository tagRepository, [NotNull] ITagValidator tagValidator)
		{
			if (tagRepository == null) throw new ArgumentNullException(nameof(tagRepository));
			if (tagValidator == null) throw new ArgumentNullException(nameof(tagValidator));

			_tagRepository = tagRepository;
			_tagValidator = tagValidator;
		}

		public Tags Add(string tagName)
		{
			_tagValidator.Validate(tagName);

			var tag = new Tags
			{
				Name = tagName
			};

			_tagRepository.Insert(tag);

			_tagRepository.Save();

			return tag;
		}

		public Tags Get(string tagName)
		{
			_tagValidator.Validate(tagName);

			var tag = _tagRepository.Get(tagName).SingleOrDefault();

			if (tag == null)
				throw new TagDoesNotExistsException(tagName);

			return tag;
		}

		public void Remove(Tags tag)
		{
			_tagRepository.Delete(tag);

			_tagRepository.Save();
		}
	}
}