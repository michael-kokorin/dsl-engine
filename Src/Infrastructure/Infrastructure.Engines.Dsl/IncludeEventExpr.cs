namespace Infrastructure.Engines.Dsl
{
    using System;
    using System.Linq;

    partial class GroupExpr
    {
        /// <summary>
        ///     Group expression to include some items into rule.
        /// </summary>
        internal class IncludeGroupExpr : GroupExpr
        {
            private readonly string[] _items;

            public IncludeGroupExpr(string[] items)
            {
                _items = items;
            }

            public override string[] Dependent => _items;

            public override GroupActionType GroupAction => GroupActionType.Include;

            public override bool IsMatch(string itemName)
            {
                if (itemName == null)
                    throw new ArgumentNullException(nameof(itemName));

                return _items.Any(x => string.Equals(x, itemName, StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}