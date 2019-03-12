namespace Infrastructure.Engines
{
    using Common.Enums;
    using Repository.Context;

    /// <summary>
    ///     Requests data from user.
    /// </summary>
    public interface IUserDataProvider
    {
        /// <summary>
        ///     Gets the delivery contacts.
        /// </summary>
        /// <param name="protocol">The protocol.</param>
        /// <param name="user">The user.</param>
        /// <returns>Delivery contacts.</returns>
        string GetDeliveryContacts(NotificationProtocolType protocol, Users user);
    }
}