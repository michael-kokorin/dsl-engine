namespace Repository.Repositories
{
	using Repository.Context;

	/// <summary>
	///   Provides methods to manage templates.
	/// </summary>
	public interface ITemplateRepository: IWriteRepository<Templates>
	{
		/// <summary>
		///   Gets template by key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Template.</returns>
		Templates GetByKey(string key);
	}
}