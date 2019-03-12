namespace Infrastructure.Query
{
	using System;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Security;
	using Repository.Context;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class QueryAccessValidator : IQueryAccessValidator
	{
		private readonly IUserAuthorityValidator _userAuthorityValidator;

		public QueryAccessValidator([NotNull] IUserAuthorityValidator userAuthorityValidator)
		{
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));

			_userAuthorityValidator = userAuthorityValidator;
		}

		public bool IsCanView(Queries query, long userId)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			if (query.IsSystem && query.ProjectId == null)
				return true;

			var canViewAllQueriesInProject = _userAuthorityValidator.HasUserAuthorities(userId,
				new[] {Authorities.UI.Queries.ViewQueriesAll},
				query.ProjectId);

			if (canViewAllQueriesInProject)
				return true;

			var canViewQueriesInProject = _userAuthorityValidator.HasUserAuthorities(userId,
				new[] {Authorities.UI.Queries.ViewQuery},
				query.ProjectId);

			if (!canViewQueriesInProject)
				return false;

			return query.CreatedById == userId && query.Privacy == (int) QueryPrivacyType.Private
			       || query.Privacy == (int) QueryPrivacyType.PublicRead;
		}

		public bool IsCanEdit(Queries query, long userId)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			if (query.CreatedById == userId)
				return true;

			var canEditAllQueriesInProject = _userAuthorityValidator.HasUserAuthorities(userId,
				new[] {Authorities.UI.Queries.EditQueryAll},
				query.ProjectId);

			if (canEditAllQueriesInProject)
				return true;

			if (query.Privacy != (int) QueryPrivacyType.PublicWrite)
				return false;

			var canEditQueriesInProject = _userAuthorityValidator.HasUserAuthorities(userId,
				new[] {Authorities.UI.Queries.EditQuery},
				query.ProjectId);

			return canEditQueriesInProject;
		}
	}
}