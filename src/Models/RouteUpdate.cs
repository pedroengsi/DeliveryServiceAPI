namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System;

	#endregion

	/// <summary>
	///     This entity is used to update an existing Route
	/// </summary>
	public class RouteUpdate
	{
		/// <summary>
		///     Route Name to update, it can't be empty
		/// </summary>
		public string RouteName { get; set; }

		/// <summary>
		///     The id of the origin point
		/// </summary>
		public Guid Origin { get; set; }

		/// <summary>
		///     The id of the destination point
		/// </summary>
		public Guid Destination { get; set; }

		/// <summary>
		///     The new cost of the route
		/// </summary>
		public int Cost { get; set; }

		/// <summary>
		///     The new time of the route
		/// </summary>
		public int Time { get; set; }
	}
}