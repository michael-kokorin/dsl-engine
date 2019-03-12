namespace Infrastructure.Security
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	using Authorities = Repository.Context.Authorities;

	[UsedImplicitly]
	internal sealed class AuthorityProvider : IAuthorityProvider
	{
		private readonly IAuthorityRepository _authorityRepository;

		private readonly IRoleAuthorityRepository _roleAuthorityRepository;

		public AuthorityProvider(
			[NotNull] IAuthorityRepository authorityRepository,
			[NotNull] IRoleAuthorityRepository roleAuthorityRepository)
		{
			_authorityRepository = authorityRepository;
			_roleAuthorityRepository = roleAuthorityRepository;
		}

		public Authority Create(string authorityKey, string displayName)
		{
			if (string.IsNullOrEmpty(authorityKey))
				throw new ArgumentNullException(nameof(authorityKey));

			var authority = _authorityRepository.GetByKey(authorityKey).SingleOrDefault();

			if (authority != null)
				throw new AuthorityAlreadyExistsException();

			authority = new Authorities
			{
				DisplayName = displayName,
				Key = authorityKey
			};

			_authorityRepository.Insert(authority);

			_authorityRepository.Save();

			return authority.ToModel();
		}

		public Authority Get(string authorityKey)
		{
			if (string.IsNullOrEmpty(authorityKey))
				throw new ArgumentNullException(nameof(authorityKey));

			var authority = _authorityRepository
				.GetByKey(authorityKey)
				.SingleOrDefault();

			return authority?.ToModel();
		}

		public IEnumerable<Authority> Get(long roleId, long? projectId = null)
		{
			var result = _authorityRepository.GetByProjectAndRole(roleId, projectId).ToArray();

			return result.Select(_ => _.ToModel());
		}

		public void Grant(long roleId, IEnumerable<string> authorityKeys)
		{
			if (authorityKeys == null)
				throw new ArgumentNullException(nameof(authorityKeys));

			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach (var authorityKey in authorityKeys)
			{
				var authority = Get(authorityKey) ?? Create(authorityKey, authorityKey);

				if (IsCan(roleId, authority.Id)) continue;

				_roleAuthorityRepository.Insert(new RoleAuthorities
				{
					AuthorityId = authority.Id,
					RoleId = roleId
				});

				_roleAuthorityRepository.Save();
			}
		}

		public bool IsCan(long roleId, long authorityId)
			=> _roleAuthorityRepository.GetByRoleAndAuthority(roleId, authorityId).Any();
	}
}