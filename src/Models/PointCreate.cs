namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.ComponentModel.DataAnnotations;

	#endregion

	/// <summary>
	///     This entity is used to add a new Point
	/// </summary>
	public class PointCreate
	{
		/// <summary>
		///     Point Name to create, it can't be empty
		/// </summary>
		[Required]
		public string PointName { get; set; }
	}
}