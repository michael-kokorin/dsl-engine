namespace Infrastructure.AD
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class UserProvider : IUserProvider
	{
		private readonly IUserRepository _userRepository;

		public UserProvider([NotNull] IUserRepository userRepository)
		{
			if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

			_userRepository = userRepository;
		}

		public Users Get(long userId) => _userRepository.GetById(userId);
	}
}