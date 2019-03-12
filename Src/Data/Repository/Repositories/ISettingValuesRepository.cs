namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Repository.Context;

	/// <summary>
	///     Provides methods to manage setting values.
	/// </summary>
	public interface ISettingValuesRepository: IWriteRepository<SettingValues>
	{
		[NotNull]
		IQueryable<SettingValues> Get(PluginType pluginType, int settingOwner, long entityId);

		[CanBeNull]
		SettingValues Get(PluginType pluginType, int settingOwner, long entityId, string settingKey);
	}
}