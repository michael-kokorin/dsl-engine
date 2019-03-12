namespace Infrastructure.Engines.Dsl
{
    using JetBrains.Annotations;

    /// <summary>
    ///     Represents parameter expression.
    /// </summary>
    public sealed class ParameterExpr
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParameterExpr" /> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="value">The value.</param>
        internal ParameterExpr(string parameter, string value)
        {
            Parameter = parameter;
            Value = value;
        }

        [UsedImplicitly]
        public string Parameter { get; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public string Value { get; }
    }
}