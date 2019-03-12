namespace Repository
{
	using System;

	using JetBrains.Annotations;

	using Common.Transaction;

	internal sealed class UnitOfWork: IUnitOfWork, IDbContextProvider
	{
		private readonly IDbContextFactory _dbContextFactory;

		private IDbContext _context;

		private bool _disposed;

		public UnitOfWork([NotNull] IDbContextFactory factory)
		{
			if(factory == null) throw new ArgumentNullException(nameof(factory));

			_context = factory.GetContext();
			_dbContextFactory = factory;
		}

		/// <summary>
		///   Gets the context.
		/// </summary>
		/// <returns></returns>
		public IDbContext GetContext() => _context;

		/// <summary>
		///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() => Dispose(true);

		/// <summary>
		///   Begins the transaction.
		/// </summary>
		/// <returns>Instance of transaction.</returns>
		public ITransaction BeginTransaction() => new Transaction(GetContext());

		/// <summary>
		///   Applies transaction.
		/// </summary>
		public void Commit() => _context.ApplyUpdates();

		/// <summary>
		///   Resets this instance.
		/// </summary>
		public void Reset()
		{
			_context.Dispose();

			_context = _dbContextFactory.GetContext();
		}

		private void Dispose(bool disposing)
		{
			if(!_disposed)
			{
				if(disposing)
					_context.Dispose();
			}

			_disposed = true;
		}
	}
}