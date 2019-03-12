namespace Infrastructure.Notifications
{
	using System;
	using System.Linq;

	using Common.Validation;

	internal sealed class NotificationValidator: IValidator<Notification>
	{
		/// <summary>
		///   Validates the specified entity and throws appropriate exception if it doesn't valid.
		/// </summary>
		/// <param name="entity">The entity to validate.</param>
		/// <exception cref="ArgumentNullException">
		///   <paramref name="entity"/> is <see langword="null"/> or message is <see langword="null"/> or empty or targets is
		///   <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException"><paramref name="entity"/> targets is empty.</exception>
		public void Validate(Notification entity)
		{
			if(entity == null)
				throw new ArgumentNullException(nameof(entity));

			if(string.IsNullOrEmpty(entity.Message))
				throw new ArgumentNullException(nameof(entity.Message));

			if(entity.Targets == null)
				throw new ArgumentNullException(nameof(entity.Targets));

			if(!entity.Targets.Any())
				throw new ArgumentException(nameof(entity.Targets));
		}
	}
}