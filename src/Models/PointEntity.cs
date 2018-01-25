namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System;

	#endregion

	/// <summary>
	///     The Point Entity
	/// </summary>
	public class PointEntity
	{
		/// <summary>
		///     Unique point Id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		///     Point Name
		/// </summary>
		public string Name { get; set; }
	}
}