namespace Infrastructure.Engines.Dsl
{
    using System;

    partial class GroupExpr
    {
        /// <summary>
        ///     Represents all items from the group.
        /// </summary>
        internal sealed class AnyGroupExpr : GroupExpr
        {
            public override string[] Dependent => new string[0];

            public override GroupActionType GroupAction => GroupActionType.Include;

            public override bool IsMatch(string itemName)
            {
                if (itemName == null)
                    throw new ArgumentNullException(nameof(itemName));

                return true;
            }
        }
    }
}