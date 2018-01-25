namespace DeliveryServiceAPI.Models
{
	/// <summary>
	///     User info returned to the user
	/// </summary>
	public class UserLoginResult
	{
		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="name"></param>
		/// <param name="email"></param>
		/// <param name="role"></param>
		public UserLoginResult(string userId, string name, string email, string role)
		{
			Id = userId;
			Name = name;
			Email = email;
			Role = role;
		}

		/// <summary>
		///     The user Id
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		///     The user username
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     The user email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		///     The user role in the system
		/// </summary>
		public string Role { get; set; }
	}
}