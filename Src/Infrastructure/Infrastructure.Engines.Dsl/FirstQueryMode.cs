namespace Infrastructure.Engines.Dsl
{
    /// <summary>
    /// Indicates way to take first element from collection.
    /// </summary>
    public enum FirstQueryMode
    {
        /// <summary>
        /// Take first.
        /// </summary>
        First = 1,

        /// <summary>
        /// Take first or default.
        /// </summary>
        FirstOrDefault = 2
    }
}