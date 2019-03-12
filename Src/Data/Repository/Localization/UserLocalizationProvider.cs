namespace Repository.Localization
{
	using System;
	using System.Linq;
	using System.Threading;

	using JetBrains.Annotations;

	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class UserLocalizationProvider: IUserLocalizationProvider
	{
		private readonly ICultureRepository _cultureRepository;

		public UserLocalizationProvider([NotNull] ICultureRepository cultureRepository)
		{
			if(cultureRepository == null) throw new ArgumentNullException(nameof(cultureRepository));

			_cultureRepository = cultureRepository;
		}

		/// <summary>
		///   Gets the culture identifier from dictionary.
		/// </summary>
		/// <returns></returns>
		public long GetCultureId()
		{
			var threadCultureCode = GetThreadCultureCode();

			// returns culture Id or 0 for default data culture
			return GetCultureId(threadCultureCode);
		}

		private long GetCultureId(string cultureCode)
		{
			var cultureId = _cultureRepository
				.Get(cultureCode)
				.Select(_ => _.Id)
				.SingleOrDefault();

			return cultureId;
		}

		private static string GetThreadCultureCode() =>
			Thread.CurrentThread.CurrentCulture.Name.ToLower();
	}
}