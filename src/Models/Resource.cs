namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using Newtonsoft.Json;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     The base resource class
	/// </summary>
	public abstract class Resource : Link
	{
		/// <summary>
		///     The Url to access the current resource
		/// </summary>
		[JsonIgnore]
		public Link Self { get; set; }

		/// <summary>
		///     The Url to delete the current resource
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Link Delete { get; set; }

		/// <summary>
		///     The Url to update the current resource
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public Link Update { get; set; }
	}
}