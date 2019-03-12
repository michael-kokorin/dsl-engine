namespace Infrastructure.Engines.Dsl
{
    using System.Data.SqlClient;

    /// <summary>
    ///     Describes order item expression.
    /// </summary>
    public sealed class OrderBlockItem
    {
        /// <summary>
        ///     Gets or sets the name of the order field.
        /// </summary>
        /// <value>
        ///     The name of the order field.
        /// </value>
        public string OrderFieldName { get; set; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>
        ///     The sort order.
        /// </value>
        public SortOrder SortOrder { get; set; }
    }
}