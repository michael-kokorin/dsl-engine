namespace Infrastructure.Engines.Dsl
{
	using System;

	/// <summary>
	///   Represents group of objects description in rule.
	/// </summary>
	public abstract partial class GroupExpr
	{
		/// <summary>
		///   Gets description to include all items from group.
		/// </summary>
		/// <value>
		///   Group expression.
		/// </value>
		internal static GroupExpr Any => new AnyGroupExpr();

		/// <summary>
		///   Gets the dependent group items.
		/// </summary>
		/// <value>
		///   The dependent items.
		/// </value>
		/// <remarks>Can't be <see langword="null" />, if empty then all items are used.</remarks>
		public abstract string[] Dependent { get; }

		/// <summary>
		///   Gets or sets a value indicating whether this instance has dependent.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this instance has dependent; otherwise, <see langword="false" />.
		/// </value>
		public bool HasDependent => Dependent.Length != 0;

		/// <summary>
		///   Gets the group action.
		/// </summary>
		/// <value>
		///   The group action.
		/// </value>
		public abstract GroupActionType GroupAction { get; }

		/// <summary>
		///   Determines whether the specified item name is match to current description.
		/// </summary>
		/// <param name="itemName">Name of the item.</param>
		/// <returns><see langword="true" /> if item matches; otherwise, <see langword="false" />.</returns>
		public abstract bool IsMatch(string itemName);

		/// <summary>
		///   Gets group description for exclude some group items.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <returns>Group expression.</returns>
		internal static GroupExpr Exclude(params string[] items)
		{
			if ((items == null) || (items.Length == 0))
				throw new ArgumentNullException(nameof(items));

			return new ExcludeGroupExpr(items);
		}

		/// <summary>
		///   Gets group description to include some items.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <returns>Group expression.</returns>
		internal static GroupExpr Include(params string[] items)
		{
			if ((items == null) || (items.Length == 0))
				throw new ArgumentNullException(nameof(items));

			return new IncludeGroupExpr(items);
		}
	}
}