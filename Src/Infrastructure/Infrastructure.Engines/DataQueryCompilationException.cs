namespace Infrastructure.Engines
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	/// <summary>
	///     Describes error while compilation of data query.
	/// </summary>
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public sealed class DataQueryCompilationException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="DataQueryCompilationException" /> class.
		/// </summary>
		/// <param name="errors">The errors.</param>
		public DataQueryCompilationException(IEnumerable<Tuple<string, string>> errors)
		{
			Errors = errors;
		}

		/// <summary>
		///     Gets the errors.
		/// </summary>
		/// <value>
		///     The errors.
		/// </value>
		public IEnumerable<Tuple<string, string>> Errors { get; }
	}
}