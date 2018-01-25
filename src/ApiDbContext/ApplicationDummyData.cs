namespace DeliveryServiceAPI.ApiDbContext
{
	#region Using Directives

	using System.Threading.Tasks;
	using Models;
	using Services;

	#endregion

	/// <summary>
	///     Class used to generate dummy data needed for the application
	/// </summary>
	public class ApplicationDummyData
	{
		#region Fields

		private static readonly string AdminRole = "AdminRole";
		private static readonly string UserRole = "UserRole";

		#endregion

		#region Add Dummy test data

		/// <summary>
		///     Method used to add the dummy user data to the service
		/// </summary>
		/// <param name="accountsService"></param>
		public static async Task AddDummyUsersAsync(IAccountsService accountsService)
		{
			// Add a test role - Admin / User
			await AddRoleAsync(accountsService, AdminRole);
			await AddRoleAsync(accountsService, UserRole);

			// Add an admin test user
			var adminUser = new UserCreate("localadmin", "admin@local.com", "Supersecret123!!");
			await AddUser(accountsService, adminUser, AdminRole);

			// Add a normal test user
			var normalUser = new UserCreate("localuser", "user@local.com", "Password!123");
			await AddUser(accountsService, normalUser, UserRole);
		}

		/// <summary>
		///		Add role to the system
		/// </summary>
		/// <param name="accountsService"></param>
		/// <param name="roleName"></param>
		/// <returns></returns>
		private static async Task AddRoleAsync(IAccountsService accountsService, string roleName)
		{
			await accountsService.CreateRoleAsync(roleName);
		}


		/// <summary>
		///		Add user to the system
		/// </summary>
		/// <param name="accountsService"></param>
		/// <param name="newUser"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		private static async Task AddUser(IAccountsService accountsService, UserCreate newUser, string role)
		{
			var createResult = await accountsService.CreateUserAsync(newUser);
			var createdUser = accountsService.GetUserByUserName(newUser.UserName);
			var addRoleResult = await accountsService.AddUserToRoleAsync(createdUser, role);
		}

		#endregion
	}
}