namespace Plugins.Rtc.It.Client
{
	using System.IO;

	using Newtonsoft.Json;

	using RestSharp.Serializers;

	/// <summary>
	/// Default JSON serializer for request bodies
	/// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
	/// </summary>
	internal sealed class RestSharpJsonNetSerializer : ISerializer
	{
		private readonly Newtonsoft.Json.JsonSerializer _serializer;

		/// <summary>
		/// Default serializer
		/// </summary>
		public RestSharpJsonNetSerializer()
		{
			ContentType = "application/json";
			_serializer = new Newtonsoft.Json.JsonSerializer
			{
				MissingMemberHandling = MissingMemberHandling.Ignore,
				NullValueHandling = NullValueHandling.Include,
				DefaultValueHandling = DefaultValueHandling.Include
			};
		}

		/// <summary>
		/// Serialize the object as JSON
		/// </summary>
		/// <param name="obj"/>Object to serialize
		/// <returns>JSON as String</returns>
		public string Serialize(object obj)
		{
			using (var stringWriter = new StringWriter())
			{
				using (var jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.Formatting = Formatting.Indented;
					jsonTextWriter.QuoteChar = '"';

					_serializer.Serialize(jsonTextWriter, obj);

					var result = stringWriter.ToString();
					return result;
				}
			}
		}

		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string DateFormat { get; set; }
		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string RootElement { get; set; }
		/// <summary>
		/// Unused for JSON Serialization
		/// </summary>
		public string Namespace { get; set; }
		/// <summary>
		/// Content type for serialized content
		/// </summary>
		public string ContentType { get; set; }
	}
}
