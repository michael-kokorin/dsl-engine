namespace Workflow.Notification
{
    /// <summary>
    ///     Contract of processor for notifications in queue.
    /// </summary>
    internal interface INotificationProcessor
    {
        /// <summary>
        ///     Handles the next notification in queue.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the next notification is processed; <see langword="false" /> if there is no more
        ///     notifications in queue.
        /// </returns>
        bool ProcessNext();
    }
}