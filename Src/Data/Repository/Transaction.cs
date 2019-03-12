namespace Repository
{
	using System;
	using System.Data.Entity;

	using JetBrains.Annotations;

	using Common.Transaction;

	internal sealed class Transaction: ITransaction
	{
		private readonly DbContextTransaction _transaction;

		private bool _isDisposed;

		public Transaction([NotNull] IDbContext dbContext)
		{
			if(dbContext == null) throw new ArgumentNullException(nameof(dbContext));

			_transaction = dbContext.CreateTransaction();
		}

		/// <summary>
		///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() => Dispose(true);

		/// <summary>
		///   Commits all changes made during transaction.
		/// </summary>
		public void Commit() => _transaction.Commit();

		private void Dispose(bool isDisposing)
		{
			if(_isDisposed)
				return;

			if(isDisposing)
			{
				if(_transaction.UnderlyingTransaction.Connection != null)
					_transaction.Rollback();

				_transaction.Dispose();
			}

			_isDisposed = true;
		}
	}
}