namespace DbMigrations
{
	internal sealed class ForeignKeyConstraintMapper
	{
		public static string SqlForConstraint(ForeignKeyConstraint constraint)
		{
			switch(constraint)
			{
				case ForeignKeyConstraint.Cascade:
				{
					return "CASCADE";
				}
				case ForeignKeyConstraint.SetNull:
				{
					return "SET NULL";
				}
				case ForeignKeyConstraint.NoAction:
				{
					return "NO ACTION";
				}
				case ForeignKeyConstraint.Restrict:
				{
					return "RESTRICT";
				}
				case ForeignKeyConstraint.SetDefault:
				{
					return "SET DEFAULT";
				}
				default:
				{
					return "NO ACTION";
				}
			}
		}
	}
}