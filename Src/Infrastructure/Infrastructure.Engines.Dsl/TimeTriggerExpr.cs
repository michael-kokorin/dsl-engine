namespace Infrastructure.Engines.Dsl
{
    using System;

    /// <summary>
    ///     Represents time trigger expression.
    /// </summary>
    public sealed class TimeTriggerExpr
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TimeTriggerExpr" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="repeat">The repeat.</param>
        internal TimeTriggerExpr(DateTime start, TimeSpan? repeat)
        {
            Start = start;
            Repeat = repeat;
        }

        /// <summary>
        ///     Gets the start.
        /// </summary>
        /// <value>
        ///     The start.
        /// </value>
        public DateTime Start { get; }

        /// <summary>
        ///     Gets or sets the repeat.
        /// </summary>
        /// <value>
        ///     The repeat.
        /// </value>
        public TimeSpan? Repeat { get; }
    }
}