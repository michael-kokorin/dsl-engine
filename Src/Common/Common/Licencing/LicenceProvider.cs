namespace Common.Licencing
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Licencing.Licences;

	[UsedImplicitly]
	internal sealed class LicenceProvider : ILicenceProvider
	{
		private static IDictionary<string, ILicence> _licences;

		private readonly IInstallationLicenceIdProvider _installationLicenceIdProvider;

		public LicenceProvider([NotNull] IInstallationLicenceIdProvider installationLicenceIdProvider)
		{
			if (installationLicenceIdProvider == null) throw new ArgumentNullException(nameof(installationLicenceIdProvider));

			_installationLicenceIdProvider = installationLicenceIdProvider;
		}

		public ILicence GetCurrent()
		{
			var currentLicenceId = _installationLicenceIdProvider.GetInstallationLicenceId();

			if (string.IsNullOrEmpty(currentLicenceId))
			{
				throw new EmptyLicenceIdException();
			}

			CollectSolutionLicences();

			if (!_licences.ContainsKey(currentLicenceId))
			{
				throw new IncorrectLicenceIdException(currentLicenceId);
			}

			return _licences[currentLicenceId];
		}

		private static readonly object Lock = new object();

		private static void CollectSolutionLicences()
		{
			lock (Lock)
			{
				if (_licences != null) return;

				_licences = new Dictionary<string, ILicence>();

				var ftpLicence = new FtpLicence();

				_licences.Add(ftpLicence.Id, ftpLicence);

				var sdlLicence = new SdlLicence();

				_licences.Add(sdlLicence.Id, sdlLicence);
			}
		}
	}
}