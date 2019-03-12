namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;

	/// <summary>
	///   Provides methods to manage settings.
	/// </summary>
	public interface ISettingRepository: IWriteRepository<Settings>
	{
		[CanBeNull]
		Settings Get(long pluginId, int settingOwner, string code);

		[CanBeNull]
		Settings Get(PluginType pluginType, int settingOwner, string code);

		[NotNull]
		IQueryable<Settings> Get(PluginType pluginType, int settingOwner);
	}
}