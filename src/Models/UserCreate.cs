namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.ComponentModel.DataAnnotations;

	#endregion

	/// <summary>
	///     This entity is used to create a new User
	/// </summary>
	public class UserCreate
	{
		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="email"></param>
		/// <param name="password"></param>
		public UserCreate(string userName, string email, string password)
		{
			UserName = userName;
			Email = email;
			Password = password;
		}

		/// <summary>
		///     The user UserName
		/// </summary>
		[Required]
		public string UserName { get; set; }

		/// <summary>
		///     The user Email
		/// </summary>
		[Required]
		public string Email { get; set; }

		/// <summary>
		///     The user Pasword
		/// </summary>
		/// <remarks>
		///     The pretended password must have:
		///     - At least one digit;
		///     - At least one lower case letter;
		///     - At least one upper case letter;
		///     - At least one non alphanumeric symbol;
		///     - At least six characters.
		/// </remarks>
		[Required]
		public string Password { get; set; }
	}
}