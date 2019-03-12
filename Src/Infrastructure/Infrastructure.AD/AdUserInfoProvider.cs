namespace Infrastructure.AD
{
	using System;
	using System.DirectoryServices.AccountManagement;

	internal sealed class AdUserInfoProvider : IAdUserInfoProvider
	{
		public AdUserInfo Get(string userSid)
		{
			if (string.IsNullOrEmpty(userSid))
				throw new ArgumentNullException(nameof(userSid));

			AdUserInfo adUserInfo2;

			return GetUserInfoDromLocalmachine(userSid, out adUserInfo2)
				? adUserInfo2
				: GetUserInfoFromDomain(userSid);
		}

		private static AdUserInfo GetUserInfoFromDomain(string userSid)
		{
			using (var ctx = GetContext(ContextType.Domain))
			{
				return GetUserInfoFromContext(userSid, ctx);
			}
		}

		private static bool GetUserInfoDromLocalmachine(string userSid, out AdUserInfo adUserInfo2)
		{
			adUserInfo2 = null;

			using (var ctx = GetContext(ContextType.Machine))
			{
				var adUserInfo = GetUserInfoFromContext(userSid, ctx);

				if (adUserInfo == null)
					return false;

				adUserInfo2 = adUserInfo;
				return true;
			}
		}

		private static AdUserInfo GetUserInfoFromContext(string sid, PrincipalContext ctx)
		{
			using (var user = UserPrincipal.FindByIdentity(ctx, sid))
			{
				if (user != null)
				{
					return new AdUserInfo
					{
						DisplayName = user.DisplayName ?? user.SamAccountName ?? user.Name,
						Email = user.EmailAddress,
						IsActive = !user.IsAccountLockedOut(),
						Login = user.SamAccountName ?? user.Name
					};
				}
			}
			return null;
		}

		private static PrincipalContext GetContext(ContextType type) => new PrincipalContext(type);
	}
}