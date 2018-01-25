namespace DeliveryServiceAPI.Models
{
	/// <inheritdoc />
	/// <summary>
	///     Root resource returned to the use
	/// </summary>
	public class RootResponse : Resource
	{
		/// <summary>
		///     Link to the Points list
		/// </summary>
		public Link Points { get; set; }

		/// <summary>
		///     Link to the Routes list
		/// </summary>
		public Link Routes { get; set; }
	}
}