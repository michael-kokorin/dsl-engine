namespace Common.Transaction
{
	using System;

	/// <summary>
	///   Provides methods to make some operations transactional.
	/// </summary>
	public interface IUnitOfWork: IDisposable
	{
		/// <summary>
		///   Begins the transaction.
		/// </summary>
		/// <returns>Instance of transaction.</returns>
		ITransaction BeginTransaction();

		/// <summary>
		///   Applies transaction.
		/// </summary>
		void Commit();

		/// <summary>
		///   Resets this instance.
		/// </summary>
		void Reset();
	}
}