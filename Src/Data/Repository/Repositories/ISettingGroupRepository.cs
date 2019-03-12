namespace Repository.Repositories
{
	using Repository.Context;

	/// <summary>
	///   Provides methods to manage setting groups.
	/// </summary>
	public interface ISettingGroupRepository: IWriteRepository<SettingGroups>
	{
		SettingGroups Get(long pluginId, string code);
	}
}