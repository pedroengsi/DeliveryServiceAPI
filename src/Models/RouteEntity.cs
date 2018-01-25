namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System;

	#endregion

	/// <summary>
	///     This entity is used to store the routes
	/// </summary>
	public class RouteEntity
	{
		/// <summary>
		///     Unique route Id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		///     Route Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     The origin point of the route
		/// </summary>
		public PointEntity Origin { get; set; }

		/// <summary>
		///     The destination point of the route
		/// </summary>
		public PointEntity Destination { get; set; }

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