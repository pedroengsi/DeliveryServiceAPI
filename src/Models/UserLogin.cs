namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.ComponentModel.DataAnnotations;

	#endregion

	/// <summary>
	///     The class with the information needed to login the system
	/// </summary>
	public class UserLogin
	{
		/// <summary>
		///     The user username
		/// </summary>
		[Required]
		public string UserName { get; set; }

		/// <summary>
		///     The user password
		/// </summary>
		[Required]
		public string Password { get; set; }
	}
}