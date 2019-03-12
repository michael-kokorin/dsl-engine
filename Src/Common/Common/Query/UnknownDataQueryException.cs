namespace Common.Query
{
	using System;
	using System.Runtime.Serialization;

	using JetBrains.Annotations;

	/// <summary>
	///   Occurs when requested data query of unknown type.
	/// </summary>
	/// <seealso cref="System.Exception"/>
	[Serializable]
	internal sealed class UnknownDataQueryException: Exception
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownDataQueryException"/> class.
		/// </summary>
		public UnknownDataQueryException()
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownDataQueryException"/> class.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <param name="context">The context.</param>
		private UnknownDataQueryException([NotNull] SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownDataQueryException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		[UsedImplicitly]
		public UnknownDataQueryException(string message): base(message)
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownDataQueryException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		[UsedImplicitly]
		public UnknownDataQueryException(string message, Exception innerException): base(message, innerException)
		{
		}
	}
}