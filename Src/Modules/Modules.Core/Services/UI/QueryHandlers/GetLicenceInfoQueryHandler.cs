namespace Modules.Core.Services.UI.QueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Licencing;
	using Common.Query;
	using Modules.Core.Contracts.UI.Dto.Admin;
	using Modules.Core.Services.UI.Queries;

	internal sealed class GetLicenceInfoQueryHandler : IDataQueryHandler<GetLicenceInfoQuery, LicenceInfoDto>
	{
		private readonly ILicenceProvider _licenceProvider;

		public GetLicenceInfoQueryHandler([NotNull] ILicenceProvider licenceProvider)
		{
			if (licenceProvider == null) throw new ArgumentNullException(nameof(licenceProvider));

			_licenceProvider = licenceProvider;
		}

		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		public LicenceInfoDto Execute(GetLicenceInfoQuery dataQuery)
		{
			var currentLicence = _licenceProvider.GetCurrent();

			return new LicenceInfoDto
			{
				Description = currentLicence.Description,
				Id = currentLicence.Id
			};
		}
	}
}