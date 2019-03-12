namespace Common.Command
{
	using System;
	using System.Runtime.Serialization;

	using JetBrains.Annotations;

	/// <summary>
	///   Thrown when command is unknown.
	/// </summary>
	[UsedImplicitly]
	[Serializable]
	public sealed class UnknownCommandException: Exception
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownCommandException"/> class.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <param name="context">The context.</param>
		private UnknownCommandException([NotNull] SerializationInfo info, StreamingContext context): base(info, context)
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownCommandException"/> class.
		/// </summary>
		public UnknownCommandException()
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownCommandException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public UnknownCommandException(string message): base(message)
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="UnknownCommandException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public UnknownCommandException(string message, Exception innerException): base(message, innerException)
		{
		}
	}
}