namespace Infrastructure.Engines.Dsl
{
    /// <summary>
    ///     Represents group of subjects.
    /// </summary>
    public sealed class SubjectGroupExpr
    {
        /// <summary>
        ///     Gets or sets the name of the group.
        /// </summary>
        /// <value>
        ///     The name of the group.
        /// </value>
        public string GroupName { get; set; }

        /// <summary>
        ///     Gets or sets the excluded persons.
        /// </summary>
        /// <value>
        ///     The excluded persons.
        /// </value>
        public string[] ExcludedPersons { get; set; }
    }
}