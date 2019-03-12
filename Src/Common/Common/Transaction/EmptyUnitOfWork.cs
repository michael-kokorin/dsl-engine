namespace Common.Transaction
{
	/// <summary>
	///   Represents empty implementation of IUnitOfWork.
	/// </summary>
	public sealed class EmptyUnitOfWork: IUnitOfWork
	{
		/// <summary>
		///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		///   Begins the transaction.
		/// </summary>
		/// <returns>
		///   Instance of transaction.
		/// </returns>
		public ITransaction BeginTransaction() => new EmptyTransaction();

		/// <summary>
		///   Applies transaction.
		/// </summary>
		public void Commit()
		{
		}

		/// <summary>
		///   Resets this instance.
		/// </summary>
		public void Reset()
		{
		}
	}
}