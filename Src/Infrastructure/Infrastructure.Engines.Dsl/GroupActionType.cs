namespace Infrastructure.Engines.Dsl
{
    /// <summary>
    ///     Indicates type of grouping action in expression.
    /// </summary>
    public enum GroupActionType
    {
        /// <summary>
        ///     No actions specified.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Include action is specified.
        /// </summary>
        Include = 1,

        /// <summary>
        ///     Exclude action is specified.
        /// </summary>
        Exclude = 2
    }
}