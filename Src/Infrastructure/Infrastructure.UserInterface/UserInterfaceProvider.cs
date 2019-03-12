namespace Infrastructure.UserInterface
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class UserInterfaceProvider : IUserInterfaceProvider
	{
		private readonly IUserInterfaceRepository _userInterfaceRepository;

		public UserInterfaceProvider([NotNull] IUserInterfaceRepository userInterfaceRepository)
		{
			if (userInterfaceRepository == null) throw new ArgumentNullException(nameof(userInterfaceRepository));

			_userInterfaceRepository = userInterfaceRepository;
		}

		public UserInterfaces GetLatest() =>
			_userInterfaceRepository.Query()
				.OrderByDescending(_ => _.LastCheckedUtc)
				.FirstOrDefault();
	}
}