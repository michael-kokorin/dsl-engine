namespace Modules.UI.Controllers
{
	using System;
	using System.Security.Principal;
	using System.Web.Mvc;

	public abstract class BaseController : Controller
	{
		protected WindowsImpersonationContext Impersonate()
		{
			var windowsIdentity = HttpContext.User?.Identity as WindowsIdentity;

			if (windowsIdentity == null)
				throw new UnauthorizedAccessException();

			return windowsIdentity.Impersonate();
		}
	}
}