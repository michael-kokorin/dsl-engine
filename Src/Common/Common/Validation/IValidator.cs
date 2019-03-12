namespace Common.Validation
{
	/// <summary>
	///   Checks entity on validity and throws appropriate exception if it doesn't.
	/// </summary>
	/// <typeparam name="T">Type of entity to validate.</typeparam>
	public interface IValidator<in T>
	{
		/// <summary>
		///   Validates the specified entity and throws appropriate exception if it doesn't valid.
		/// </summary>
		/// <param name="entity">The entity to validate.</param>
		void Validate(T entity);
	}
}