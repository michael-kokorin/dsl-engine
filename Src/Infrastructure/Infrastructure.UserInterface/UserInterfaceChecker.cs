namespace Infrastructure.UserInterface
{
	using System;
	using System.Linq;
	using System.Reflection;
	using System.ServiceModel;

	using JetBrains.Annotations;

	using Common.Exceptions;
	using Common.Time;
	using Infrastructure.UserInterface.Extensions;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class UserInterfaceChecker: IUserInterfaceChecker
	{
		private readonly ITimeService _timeService;

		private readonly IUserInterfaceRepository _userInterfacesRepository;

		public UserInterfaceChecker(
			[NotNull] ITimeService timeService,
			[NotNull] IUserInterfaceRepository userInterfacesRepository)
		{
			if(timeService == null) throw new ArgumentNullException(nameof(timeService));
			if(userInterfacesRepository == null) throw new ArgumentNullException(nameof(userInterfacesRepository));

			_timeService = timeService;
			_userInterfacesRepository = userInterfacesRepository;
		}

		/// <summary>
		///   Checks the specified version.
		/// </summary>
		/// <param name="host">User interface host address</param>
		/// <param name="version">The User interface module version.</param>
		/// <exception cref="System.ArgumentNullException">
		///   <paramref name="host"/> or <paramref name="version"/> is <see langword="null"/>.
		/// </exception>
		public void Check(string host, string version)
		{
			if(string.IsNullOrEmpty(host))
				throw new ArgumentNullException(nameof(host));

			if(string.IsNullOrEmpty(version))
				throw new ArgumentNullException(nameof(version));

			var hostUrl = new Uri(host);

			var hostUri = hostUrl.AbsoluteUri;

			var context = OperationContext.Current;

			var remoteIp = context.GetRemoteIp();

			var remotePort = context.GetRemotePort();

			var userInterfaceModule = _userInterfacesRepository.Get(hostUri).SingleOrDefault();

			if(userInterfaceModule != null)
			{
				userInterfaceModule.LastCheckedUtc = _timeService.GetUtc();
				userInterfaceModule.RemoteIp = remoteIp;
				userInterfaceModule.RemotePort = remotePort;
				userInterfaceModule.Version = version;
			}
			else
			{
				userInterfaceModule =
					new UserInterfaces
					{
						Host = hostUri,
						LastCheckedUtc = _timeService.GetUtc(),
						RegisteredUtc = _timeService.GetUtc(),
						RemoteIp = remoteIp,
						RemotePort = remotePort,
						Version = version
					};

				_userInterfacesRepository.Insert(userInterfaceModule);
			}

			_userInterfacesRepository.Save();

			ValidateVersion(version);
		}

		private void ValidateVersion([NotNull] string version)
		{
			if(version == null) throw new ArgumentNullException(nameof(version));

			var assembly = Assembly.GetExecutingAssembly();

			var currentAssemblyVersion = assembly.GetName().Version.ToString();

			if(!currentAssemblyVersion.Equals(version))
				throw new ModuleVersionIsNotSupportedException();
		}
	}
}