namespace Common.Exceptions
{
	using System;
	using System.Runtime.Serialization;
	using System.Web;

	using JetBrains.Annotations;

	using Common.Properties;

	/// <summary>
	///   Exception for incompatible module version
	/// </summary>
	[Serializable]
	public sealed class ModuleVersionIsNotSupportedException: HttpException
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="ModuleVersionIsNotSupportedException"/> class.
		/// </summary>
		public ModuleVersionIsNotSupportedException()
			: base(
				426,
				Resources.ModuleVersionIsNotSupportedException_ModuleVersionIsNotSupportedException_Module_version_is_not_supported)
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="ModuleVersionIsNotSupportedException"/> class.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <param name="context">The context.</param>
		private ModuleVersionIsNotSupportedException([NotNull] SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="ModuleVersionIsNotSupportedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		[UsedImplicitly]
		public ModuleVersionIsNotSupportedException(string message): base(message)
		{
		}

		/// <summary>
		///   Initializes a new instance of the <see cref="ModuleVersionIsNotSupportedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		[UsedImplicitly]
		public ModuleVersionIsNotSupportedException(string message, Exception innerException): base(message, innerException)
		{
		}
	}
}