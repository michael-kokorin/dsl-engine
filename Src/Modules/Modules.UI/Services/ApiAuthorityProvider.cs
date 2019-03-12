namespace Modules.UI.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Modules.Core.Contracts.UI;
	using Modules.Core.Contracts.UI.Dto;

	[UsedImplicitly]
	internal sealed class ApiAuthorityProvider : IAuthorityProvider
	{
		private readonly IApiService _apiService;

		public ApiAuthorityProvider([NotNull] IApiService apiService)
		{
			if (apiService == null) throw new ArgumentNullException(nameof(apiService));

			_apiService = apiService;
		}

		public bool IsCan(IEnumerable<string> authorityNames, long? entityId)
		{
			if (authorityNames == null)
				throw new ArgumentNullException(nameof(authorityNames));

			return _apiService.HaveAuthority(new AuthorityRequestDto
			{
				AuthorityNames = authorityNames.ToArray(),
				EntityId = entityId
			});
		}

		public IEnumerable<ProjectDto> GetProjects(IEnumerable<string> authorityNames)
		{
			if (authorityNames == null)
				throw new ArgumentNullException(nameof(authorityNames));

			return _apiService.GetProjectsByAuthority(authorityNames);
		}
	}
}