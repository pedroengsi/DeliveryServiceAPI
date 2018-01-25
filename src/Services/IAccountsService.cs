namespace DeliveryServiceAPI.Services
{
	#region Using Directives

	using System.Security.Claims;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Identity;
	using Models;

	#endregion

	/// <summary>
	///     Interface responsible for all the operations over the accounts
	/// </summary>
	public interface IAccountsService
	{
		/// <summary>
		///     Inserts a new Role
		/// </summary>
		/// <param name="roleName">The name of the new role to create</param>
		/// <returns></returns>
		Task<IdentityResult> CreateRoleAsync(string roleName);

		/// <summary>
		///     Inserts a new User
		/// </summary>
		/// <param name="newUser">Theinfo about the new normal user to create</param>
		/// <returns></returns>
		Task<IdentityResult> CreateUserAsync(UserCreate newUser);

		/// <summary>
		///     Set the giver user with some role
		/// </summary>
		/// <param name="user"></param>
		/// <param name="newRole"></param>
		/// <returns></returns>
		Task<IdentityResult> AddUserToRoleAsync(IdentityUser user, string newRole);

		/// <summary>
		///     Get the user with the given username
		/// </summary>
		/// <param name="userName">The username of the user to retrieve</param>
		/// <returns>Returns a user</returns>
		IdentityUser GetUserByUserName(string userName);

		/// <summary>
		///		Generates the authentication token for the user
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		string GenerateJwtToken(IdentityUser user);

		/// <summary>
		///		Signs the user in te system
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		Task<SignInResult> SignInUser(string userName, string password);

		/// <summary>
		///		Chcks if a given user has the role of administator
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		bool IsAdminUser(string userId);

		/// <summary>
		///     Retrieves some information of the logged in user trying to execute the request
		/// </summary>
		/// <param name="user">The user who logged in into the system</param>
		/// <returns></returns>
		UserLoginResult RetrieveLoggedInUserInfo(ClaimsPrincipal user);

		/// <summary>
		///     Verifies if the user is logged in and if it is an administrator
		/// </summary>
		/// <param name="user">The user who logged in into the system</param>
		/// <returns></returns>
		bool CanExecuteAdminReques(ClaimsPrincipal user);
	}
}