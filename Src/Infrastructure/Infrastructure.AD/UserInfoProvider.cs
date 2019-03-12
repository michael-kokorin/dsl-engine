namespace Infrastructure.AD
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Security;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class UserInfoProvider : IUserInfoProvider
	{
		private readonly IUserRepository _userRepository;

		public UserInfoProvider([NotNull] IUserRepository userRepository)
		{
			if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

			_userRepository = userRepository;
		}

		public UserInfo Create(UserInfo userInfo)
		{
			if (userInfo == null)
				throw new ArgumentNullException(nameof(userInfo));

			var user = new Users
			{
				DisplayName = userInfo.DisplayName,
				Email = userInfo.Email,
				IsActive = true,
				Login = userInfo.Login,
				Sid = userInfo.Sid
			};

			_userRepository.Insert(user);

			_userRepository.Save();

			userInfo.Id = user.Id;

			return userInfo;
		}

		public UserInfo Get(string userSid)
		{
			if (string.IsNullOrEmpty(userSid))
				throw new ArgumentNullException(nameof(userSid));

			var userDb = _userRepository.GetBySid(userSid).SingleOrDefault();

			if (userDb != null)
				return new UserInfo
				{
					DisplayName = userDb.DisplayName,
					Email = userDb.Email,
					Id = userDb.Id,
					Login = userDb.Login,
					Sid = userSid
				};

			return null;
		}

		public void Update(long userId, [NotNull] string login, string displayName, string emal)
		{
			if (login == null) throw new ArgumentNullException(nameof(login));

			var user = _userRepository.GetById(userId);

			user.DisplayName = displayName;
			user.Email = emal;
			user.Login = login;

			_userRepository.Save();
		}
	}
}