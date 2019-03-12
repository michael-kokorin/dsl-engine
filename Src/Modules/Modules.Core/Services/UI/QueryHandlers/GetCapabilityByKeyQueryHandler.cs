namespace Modules.Core.Services.UI.QueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Licencing;
	using Common.Query;
	using Modules.Core.Services.UI.Queries;

	internal sealed class GetCapabilityByKeyQueryHandler : IDataQueryHandler<GetCapabilityByKeyQuery, string>
	{
		private readonly ILicenceProvider _licenceProvider;

		public GetCapabilityByKeyQueryHandler([NotNull] ILicenceProvider licenceProvider)
		{
			if (licenceProvider == null) throw new ArgumentNullException(nameof(licenceProvider));

			_licenceProvider = licenceProvider;
		}

		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		public string Execute([NotNull] GetCapabilityByKeyQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			var currentLicence = _licenceProvider.GetCurrent();

			var userInterfaceLicenceComponent = currentLicence.Get<UserInterfaceLicenceComponent>();

			return userInterfaceLicenceComponent.Get(dataQuery.CapabilityKey);
		}
	}
}