namespace Infrastructure.AD
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Common.Security;
	using Infrastructure.AD.Security;

	public sealed class AdContainerModule : IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<IActiveDirectoryClient, ActiveDirectoryClient>(reuseScope)
			.RegisterType<IAdUserInfoProvider, AdUserInfoProvider>(reuseScope)
			.RegisterType<IUserRoleProvider, AdUserRoleProvider>(reuseScope)
			.RegisterType<IUserAuthorityValidator, AdUserAuthorityValidator>(reuseScope)
			.RegisterType<IGroupNameBuilder, GroupNameBuilder>(reuseScope)
			.RegisterType<ISolutionGroupManager, SolutionGroupManager>(reuseScope)
			.RegisterType<IRoleProvider, RoleProvider>(reuseScope)
			.RegisterType<IUserPrincipalProvider, WindowsIdentityUserPrincipalProvider>(reuseScope)
			.RegisterType<ICurrentUserDataProvider, CurrentUserDataProvider>(reuseScope)
			.RegisterType<IActiveDirectoryPathProvider, ActiveDirectoryPathProvider>(reuseScope)
			.RegisterType<IUserPrincipal, UserPrincipal>(reuseScope)
			.RegisterType<IUserInfoProvider, UserInfoProvider>(reuseScope)
			.RegisterType<IUserGroupMembershipProvider, UserGroupMembershipProvider>(reuseScope)
			.RegisterType<IUserProvider, UserProvider>(reuseScope);
	}
}