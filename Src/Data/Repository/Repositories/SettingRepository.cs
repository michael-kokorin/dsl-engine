namespace Repository.Repositories
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;

	[UsedImplicitly]
	internal sealed class SettingRepository: Repository<Settings>, ISettingRepository
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Repository{T}"/> class.
		/// </summary>
		/// <param name="dbContextProvider">The database context provider.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dbContextProvider"/> is <see langword="null"/>.</exception>
		public SettingRepository(
			[NotNull] IDbContextProvider dbContextProvider)
			: base(dbContextProvider)
		{
		}

		public Settings Get(long pluginId, int settingOwner, string code) =>
			Query()
				.FirstOrDefault(
					item => (item.OwnerPluginId == pluginId) && (item.Code == code) && (item.SettingOwner == settingOwner) && !item.IsArchived);

		public Settings Get(PluginType pluginType, int settingOwner, string code) =>
			Get(pluginType, settingOwner).FirstOrDefault(item => item.Code == code);

		public IQueryable<Settings> Get(PluginType pluginType, int settingOwner) =>
			Query()
				.Where(
					_ =>
							(_.SettingOwner == settingOwner) && _.OwnerPluginId.HasValue && (_.Plugins.Type == (int)pluginType) && !_.IsArchived);
	}
}