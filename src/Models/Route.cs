namespace DeliveryServiceAPI.Models
{
	/// <inheritdoc />
	/// <summary>
	///     Route resource returned to the use
	/// </summary>
	public class Route : Resource
	{
		/// <summary>
		///     Route Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     Link to the origin point of the route
		/// </summary>
		public Link Origin { get; set; }

		/// <summary>
		///     Link to the destination point of the route
		/// </summary>
		public Link Destination { get; set; }

		/// <summary>
		///     The cost of the route
		/// </summary>
		public int Cost { get; set; }

		/// <summary>
		///     The time of the route
		/// </summary>
		public int Time { get; set; }
	}
}