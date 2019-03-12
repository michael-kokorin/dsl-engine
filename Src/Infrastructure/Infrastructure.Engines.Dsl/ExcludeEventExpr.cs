namespace Infrastructure.Engines.Dsl
{
    using System;

    partial class GroupExpr
    {
        /// <summary>
        ///     Group expression to exclude some items from rule.
        /// </summary>
        internal sealed class ExcludeGroupExpr : IncludeGroupExpr
        {
            public ExcludeGroupExpr(string[] items) : base(items)
            {
            }

            public override GroupActionType GroupAction => GroupActionType.Exclude;

            public override bool IsMatch(string itemName)
            {
                if (itemName == null)
                    throw new ArgumentNullException(nameof(itemName));

                return !string.IsNullOrWhiteSpace(itemName) && !base.IsMatch(itemName);
            }
        }
    }
}