namespace Infrastructure.Engines.Dsl
{
    /// <summary>
    ///     Represents role expression.
    /// </summary>
    public sealed class RoleExpr
    {
        /// <summary>
        ///     Gets or sets the name of the role.
        /// </summary>
        /// <value>
        ///     The name of the role.
        /// </value>
        public string RoleName { get; set; }

        /// <summary>
        ///     Gets or sets the excluded persons.
        /// </summary>
        /// <value>
        ///     The excluded persons.
        /// </value>
        public string[] ExcludedPersons { get; set; }
    }
}