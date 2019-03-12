namespace Infrastructure.Security
{
	using Repository.Context;

	internal static class AuthorityExtension
	{
		public static Authority ToModel(this Authorities authorities)
			=> new Authority
			{
				DisplayName = authorities.DisplayName,
				Id = authorities.Id,
				Key = authorities.Key
			};
	}
}