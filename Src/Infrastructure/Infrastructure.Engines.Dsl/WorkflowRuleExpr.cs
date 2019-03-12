namespace Infrastructure.Engines.Dsl
{
    /// <summary>
    ///     Represents expression for workflow rule.
    /// </summary>
    public sealed class WorkflowRuleExpr
    {
        /// <summary>
        ///     Gets or sets the event.
        /// </summary>
        /// <value>
        ///     The event.
        /// </value>
        public GroupExpr Event { get; internal set; }

        /// <summary>
        ///     Gets or sets the trigger.
        /// </summary>
        /// <value>
        ///     The trigger.
        /// </value>
        public TimeTriggerExpr Trigger { get; internal set; }

        /// <summary>
        ///     Gets or sets the action.
        /// </summary>
        /// <value>
        ///     The action.
        /// </value>
        public ParameterExpr Action { get; set; }
    }
}