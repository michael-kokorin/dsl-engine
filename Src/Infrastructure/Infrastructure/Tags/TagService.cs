namespace Infrastructure.Tags
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Transaction;
	using Repository;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class TagService : ITagService
	{
		private readonly ITagDataSourceProvider _tagDataSourceProvider;

		private readonly ITagEntityProvider _tagEntityProvider;

		private readonly ITagEntityRepositoryProvider _tagEntityRepositoryProvider;

		private readonly ITagProvider _tagProvider;

		private readonly IUnitOfWork _unitOfWork;

		public TagService([NotNull] ITagDataSourceProvider tagDataSourceProvider,
			[NotNull] ITagEntityProvider tagEntityProvider,
			[NotNull] ITagEntityRepositoryProvider tagEntityRepositoryProvider,
			[NotNull] ITagProvider tagProvider,
			[NotNull] IUnitOfWork unitOfWork)
		{
			if (tagDataSourceProvider == null) throw new ArgumentNullException(nameof(tagDataSourceProvider));
			if (tagEntityProvider == null) throw new ArgumentNullException(nameof(tagEntityProvider));
			if (tagEntityRepositoryProvider == null) throw new ArgumentNullException(nameof(tagEntityRepositoryProvider));
			if (tagProvider == null) throw new ArgumentNullException(nameof(tagProvider));
			if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

			_tagDataSourceProvider = tagDataSourceProvider;
			_tagEntityProvider = tagEntityProvider;
			_tagEntityRepositoryProvider = tagEntityRepositoryProvider;
			_tagProvider = tagProvider;
			_unitOfWork = unitOfWork;
		}

		public void Add<T>([NotNull] T entity, [NotNull] string tagName)
			where T : class, IEntity
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			using (var transaction = _unitOfWork.BeginTransaction())
			{
				Tags tag;

				try
				{
					tag = _tagProvider.Get(tagName);
				}
				catch (TagDoesNotExistsException)
				{
					tag = _tagProvider.Add(tagName);
				}

				_tagEntityProvider.Add(entity, tag);

				transaction.Commit();
			}
		}

		public void Remove<T>([NotNull] T entity, [NotNull] string tagName)
			where T : class, IEntity
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			using (var transaction = _unitOfWork.BeginTransaction())
			{
				var tag = _tagProvider.Get(tagName);

				_tagEntityProvider.Remove(entity, tag);

				var entityLinkExists = _tagEntityProvider.Exists(tag);

				if (!entityLinkExists)
				{
					_tagProvider.Remove(tag);
				}

				transaction.Commit();
			}
		}

		public IQueryable<T> GetByTag<T>([NotNull] string tagName, long? projectId)
			where T : class, IEntity
		{
			var tag = _tagProvider.Get(tagName);

			var dataSource = _tagDataSourceProvider.Get<T>(projectId);

			var tagEntities = tag.TagEntities
				.Where(_ => _.TableId == dataSource.Id)
				.Select(_ => _.EntityId);

			var entityRepository = _tagEntityRepositoryProvider.Get<T>();

			return entityRepository.Query().Where(_ => tagEntities.Contains(_.Id));
		}
	}
}