namespace Modules.Core.Controllers
{
	using System.Web.Mvc;

	/// <summary>
	///     Provides methods to get home pages.
	/// </summary>
	/// <seealso cref="System.Web.Mvc.Controller"/>
	public sealed class HomeController: Controller
	{
		/// <summary>
		///     Gets the index page.
		/// </summary>
		public ActionResult Index() => View();
	}
}