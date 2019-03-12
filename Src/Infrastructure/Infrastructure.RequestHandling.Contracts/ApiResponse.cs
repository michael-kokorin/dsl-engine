namespace Infrastructure.RequestHandling.Contracts
{
	using System.Runtime.Serialization;

	using Newtonsoft.Json;

	/// <summary>
	///   Represents response from API.
	/// </summary>
	[DataContract(Name = "output")]
	public sealed class ApiResponse
	{
		/// <summary>
		///   Gets or sets a value indicating whether this <see cref="ApiResponse"/> is success.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if success; otherwise, <see langword="false"/>.
		/// </value>
		[DataMember(Name = "result")]
		public bool Success { get; set; }

		/// <summary>
		///   Gets or sets the message.
		/// </summary>
		/// <value>
		///   The message.
		/// </value>
		[DataMember(Name = "message")]
		public string Message { get; set; }

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
		public void SetData(object data) => JsonConvert.SerializeObject(data);
	}
}