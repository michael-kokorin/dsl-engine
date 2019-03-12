namespace Infrastructure.Engines.Dsl
{
    /// <summary>
    ///     Represents subjects expression.
    /// </summary>
    public sealed class SubjectsExpr
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this instance is all.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if this instance is all; otherwise, <see langword="false" />.
        /// </value>
        public bool IsAll { get; set; }

        /// <summary>
        ///     Gets or sets the roles.
        /// </summary>
        /// <value>
        ///     The roles.
        /// </value>
        public RoleExpr[] Roles { get; set; }

        /// <summary>
        ///     Gets or sets the subject groups.
        /// </summary>
        /// <value>
        ///     The subject groups.
        /// </value>
        public SubjectGroupExpr[] SubjectGroups { get; set; }

        /// <summary>
        ///     Gets or sets the persons.
        /// </summary>
        /// <value>
        ///     The persons.
        /// </value>
        public string[] Persons { get; set; }
    }
}