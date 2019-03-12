namespace Infrastructure.RequestHandling.Contracts
{
	using System.Runtime.Serialization;

	using Newtonsoft.Json;

	/// <summary>
	///   Represents request from API
	/// </summary>
	[DataContract(Name = "data")]
	public sealed class ApiRequest
	{
		/// <summary>
		///   Gets or sets the source identifier.
		/// </summary>
		/// <value>
		///   The source identifier.
		/// </value>
		[DataMember(Name = "sourceid")]
		public string SourceId { get; set; }

		/// <summary>
		///   Gets or sets the type of the source.
		/// </summary>
		/// <value>
		///   The type of the source.
		/// </value>
		[DataMember(Name = "sourcetype")]
		public string SourceType { get; set; }

		/// <summary>
		///   Gets or sets the request method.
		/// </summary>
		/// <value>
		///   The request method.
		/// </value>
		[DataMember(Name = "method")]
		public string RequestMethod { get; set; }

		/// <summary>
		///   Gets or sets the json data.
		/// </summary>
		/// <value>
		///   The json data.
		/// </value>
		[DataMember(Name = "content")]
		public string JsonData { get; set; }

		/// <summary>
		///   Gets the data.
		/// </summary>
		/// <typeparam name="T">Type of data.</typeparam>
		/// <returns>Deserialized data.</returns>
		public T GetData<T>() => JsonConvert.DeserializeObject<T>(JsonData);

		/// <summary>
		///   Sets the data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void SetData(object data) => JsonData = JsonConvert.SerializeObject(data);
	}
}