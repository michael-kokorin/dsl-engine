using Infrastructure.Engines.Dsl.Query;

namespace Infrastructure.Engines.Dsl
{
	/// <summary>
	///   Represents comment expression.
	/// </summary>
	public sealed class CommentExprBlock : IDslQueryBlock
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="CommentExprBlock"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public CommentExprBlock(string message)
		{
			Message = message;
		}

		/// <summary>
		///   Gets the message.
		/// </summary>
		/// <value>
		///   The message.
		/// </value>
		public string Message { get; private set; }
	}
}