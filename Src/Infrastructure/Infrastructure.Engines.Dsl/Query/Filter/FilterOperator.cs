namespace Infrastructure.Engines.Dsl.Query.Filter
{
	using System.ComponentModel;

	/// <summary>
	/// Filter block available operators
	/// </summary>
	public enum FilterOperator
	{
		[Description(@"[Cc][Oo][Nn][Tt][Aa][Ii][Nn][Ss]")]
		Contains,

		[Description(@"/")]
		Divide,

		[Description(@"==")]
		Equal,

		[Description(@">")]
		Greather,

		[Description(@">=")]
		GreatherOrEqual,

		[Description(@"[Ii][Ss]")]
		Is,

		[Description(@"[Ii][Nn]")]
		In,

		[Description(@"<")]
		Less,

		[Description(@"<=")]
		LessOrEqual,

		[Description(@"-")]
		Minus,

		[Description(@"\*")]
		Multiple,

		[Description(@"[Nn][Oo][Tt]")]
		Not,

		[Description(@"\+")]
		Plus
	}
}