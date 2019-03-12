namespace Infrastructure.AD.Security
{
	using System;
	using System.Linq;
	using System.Security.Principal;
	using System.Web;

	using JetBrains.Annotations;

	using Common.Security;

	[UsedImplicitly]
	internal sealed class WindowsIdentityUserPrincipalProvider : IUserPrincipalProvider
	{
		public UserPrincipalInfo Get()
		{
			var currentUser = GetCurrentIdentity();

			if (!currentUser.IsAuthenticated || currentUser.User == null)
				throw new UnauthorizedAccessException();

			var nameParts = currentUser.Name.Split('\\');

			return new UserPrincipalInfo(currentUser.User.Value, nameParts[1], nameParts[0]);
		}

		public bool IsCurrentUserInRole(string roleSid)
		{
			var groups = GetCurrentIdentity().Groups;

			return (groups != null) && groups.Any(_ => _.Value.Contains(roleSid));
		}

		private static WindowsIdentity GetCurrentIdentity()
		{
			var idenity = HttpContext.Current?.User?.Identity as WindowsIdentity;

			return idenity ?? WindowsIdentity.GetCurrent();
		}
	}
}