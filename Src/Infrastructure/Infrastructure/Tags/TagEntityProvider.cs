namespace Infrastructure.Tags
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class TagEntityProvider : ITagEntityProvider
	{
		private readonly ITagEntityRepository _tagEntityRepository;

		private readonly ITagDataSourceProvider _tagDataSourceProvider;

		public TagEntityProvider([NotNull] ITagEntityRepository tagEntityRepository,
			[NotNull] ITagDataSourceProvider tagDataSourceProvider)
		{
			if (tagEntityRepository == null) throw new ArgumentNullException(nameof(tagEntityRepository));
			if (tagDataSourceProvider == null) throw new ArgumentNullException(nameof(tagDataSourceProvider));

			_tagEntityRepository = tagEntityRepository;
			_tagDataSourceProvider = tagDataSourceProvider;
		}

		public void Add<T>([NotNull] T entity, [NotNull] Tags tag) where T : class, IEntity
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			if (tag == null) throw new ArgumentNullException(nameof(tag));

			var dataSource = _tagDataSourceProvider.Get(entity);

			var tagEntity = new TagEntities
			{
				EntityId = entity.Id,
				TableId = dataSource.Id,
				TagId = tag.Id
			};

			_tagEntityRepository.Insert(tagEntity);

			_tagEntityRepository.Save();
		}

		public bool Exists([NotNull] Tags tag)
		{
			if (tag == null) throw new ArgumentNullException(nameof(tag));

			return _tagEntityRepository.Get(tag.Id).Any();
		}

		public void Remove<T>([NotNull] T entity, [NotNull] Tags tag) where T : class, IEntity
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			if (tag == null) throw new ArgumentNullException(nameof(tag));

			var dataSource = _tagDataSourceProvider.Get(entity);

			var tagEntity = _tagEntityRepository.Get(tag.Id, dataSource.Id, entity.Id).SingleOrDefault();

			if (tagEntity == null)
				throw new TagEntityLinkDoesNotExists(tag.Name, dataSource.Name, entity.Id);

			_tagEntityRepository.Delete(tagEntity);

			_tagEntityRepository.Save();
		}
	}
}