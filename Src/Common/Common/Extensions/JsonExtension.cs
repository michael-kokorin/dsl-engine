namespace Common.Extensions
{
	using Newtonsoft.Json;

	/// <summary>
	///   Provides extension methods for json serialization of entity.
	/// </summary>
	public static class JsonExtension
	{
		public static string ToJson<T>(this T entity, bool enableTypeNaming = true) =>
			JsonConvert.SerializeObject(entity, Getsettings(enableTypeNaming));

		public static T FromJson<T>(this string jsonString, bool enableTypeNaming = true) =>
			JsonConvert.DeserializeObject<T>(jsonString, Getsettings(enableTypeNaming));

		private static JsonSerializerSettings Getsettings(bool enableTypeNames) =>
			new JsonSerializerSettings
			{
				TypeNameHandling = enableTypeNames ? TypeNameHandling.All : TypeNameHandling.None
			};
	}
}