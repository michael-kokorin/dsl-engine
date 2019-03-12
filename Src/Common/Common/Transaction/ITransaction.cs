namespace Common.Transaction
{
	using System;

	/// <summary>
	///   Transaction object. Rollbacks on dispose.
	/// </summary>
	public interface ITransaction: IDisposable
	{
		/// <summary>
		///   Commits all changes made during transaction.
		/// </summary>
		void Commit();
	}
}