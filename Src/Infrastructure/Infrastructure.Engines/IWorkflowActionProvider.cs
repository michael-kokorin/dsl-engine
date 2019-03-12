namespace Infrastructure.Engines
{
    /// <summary>
    ///     Provides methods to get workflow action.
    /// </summary>
    public interface IWorkflowActionProvider
    {
        /// <summary>
        ///     Gets the workflow action by specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Workflow action.</returns>
        IWorkflowAction Get(string key);
    }
}