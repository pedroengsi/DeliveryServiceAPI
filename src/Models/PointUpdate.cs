namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.ComponentModel.DataAnnotations;

	#endregion

	/// <summary>
	///     This entity is used to update an existing Point
	/// </summary>
	public class PointUpdate
	{
		/// <summary>
		///     Point Name to update, it can't be empty
		/// </summary>
		[Required]
		public string PointName { get; set; }
	}
}