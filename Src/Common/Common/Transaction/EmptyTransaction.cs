namespace Common.Transaction
{
	internal sealed class EmptyTransaction: ITransaction
	{
		/// <summary>
		///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		///   Commits this transaction instance.
		/// </summary>
		public void Commit()
		{
		}
	}
}