namespace DeliveryServiceAPI.Services
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Security.Claims;
	using System.Text;
	using System.Threading.Tasks;
	using ApiDbContext;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Configuration;
	using Microsoft.IdentityModel.Tokens;
	using Models;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Class responsible for all the operations over the routes
	/// </summary>
	public class AccountsService : IAccountsService
	{
		#region Fields

		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		private readonly string AdminRole = "AdminRole";
		private readonly string UserRole = "UserRole";

		#endregion

		#region Constructor

		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="context"></param>
		/// <param name="userManager"></param>
		/// <param name="roleManager"></param>
		/// <param name="signInManager"></param>
		public AccountsService(IConfiguration configuration, ApplicationDbContext context, UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
		{
			_configuration = configuration;
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		#endregion

		/// <inheritdoc />
		/// <summary>
		///     Inserts a new Role
		/// </summary>
		/// <param name="roleName">The name of the new role to create</param>
		/// <returns></returns>
		public async Task<IdentityResult> CreateRoleAsync(string roleName)
		{
			IdentityResult result = null;

			if (!_roleManager.Roles.Any(role => role.Name == roleName))
			{
				result = await _roleManager.CreateAsync(new IdentityRole(roleName));
				await _context.SaveChangesAsync();
			}

			return result;
		}

		/// <inheritdoc />
		/// <summary>
		///     Inserts a new User
		/// </summary>
		/// <param name="newUser">Theinfo about the new normal user to create</param>
		/// <returns></returns>
		public async Task<IdentityResult> CreateUserAsync(UserCreate newUser)
		{
			var user = new IdentityUser
			{
				Id = Guid.NewGuid().ToString(),
				UserName = newUser.UserName,
				Email = newUser.Email,
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var result = await _userManager.CreateAsync(user, newUser.Password);
			await _context.SaveChangesAsync();
			return result;
		}

		/// <inheritdoc />
		/// <summary>
		///     Set the giver user with some role
		/// </summary>
		/// <param name="user"></param>
		/// <param name="newRole"></param>
		/// <returns></returns>
		public async Task<IdentityResult> AddUserToRoleAsync(IdentityUser user, string newRole)
		{
			// Put the user in the given role
			var result = await _userManager.AddToRoleAsync(user, newRole);

			if (result.Succeeded)
			{
				await _userManager.UpdateAsync(user);
				await _context.SaveChangesAsync();
			}

			return result;
		}

		/// <inheritdoc />
		/// <summary>
		///     Get the user with the given username
		/// </summary>
		/// <param name="userName">The username of the user to retrieve</param>
		/// <returns>Returns a user</returns>
		public IdentityUser GetUserByUserName(string userName)
		{
			return _userManager.Users.SingleOrDefault(r => r.UserName == userName);
		}

		/// <inheritdoc />
		/// <summary>
		///     Generates the authentication token for the user
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public string GenerateJwtToken(IdentityUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.Id),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, GetUserRole(user.Id)),

				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:JwtKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:JwtExpireDays"]));

			var token = new JwtSecurityToken(
				_configuration["JWT:JwtIssuer"],
				_configuration["JWT:JwtIssuer"],
				claims,
				expires: expires,
				signingCredentials: creds
			);

			return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
		}

		/// <inheritdoc />
		/// <summary>
		///     Signs the user in te system
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<SignInResult> SignInUser(string userName, string password)
		{
			return await _signInManager.PasswordSignInAsync(userName, password, false, false);
		}

		/// <summary>
		///     Chcks if a given user has the role of administator
		/// </summary>
		/// <param name="userId"></param>
		/// e
		/// <returns></returns>
		public bool IsAdminUser(string userId)
		{
			var currentUserRole = GetUserRole(userId);
			return currentUserRole == AdminRole;
		}

		/// <inheritdoc />
		/// <summary>
		///     Retrieves some information of the logged in user trying to execute the request
		/// </summary>
		/// <param name="user">The user who logged in into the system</param>
		/// <returns></returns>
		public UserLoginResult RetrieveLoggedInUserInfo(ClaimsPrincipal user)
		{
			if (user?.Identity == null || !user.Identity.IsAuthenticated) return null;

			var claimsIdentity = user.Identity as ClaimsIdentity;
			var userId = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
			var userName = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
			var userEmail = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
			var userRole = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

			return new UserLoginResult(userId, userName, userEmail, userRole);
		}

		/// <inheritdoc />
		/// <summary>
		///     Verifies if the user is logged in and if it is an administrator
		/// </summary>
		/// <param name="user">The user who logged in into the system</param>
		/// <returns></returns>
		public bool CanExecuteAdminReques(ClaimsPrincipal user)
		{
			if (user?.Identity == null || !user.Identity.IsAuthenticated) return false;

			var userInfo = RetrieveLoggedInUserInfo(user);
			return IsAdminUser(userInfo.Id);
		}

		private string GetUserRole(string userId)
		{
			var userRoleId = _context?.UserRoles?.FirstOrDefault(userRole => userRole.UserId == userId)?.RoleId;
			var currentUserRole = _context?.Roles?.FirstOrDefault(role => role.Id == userRoleId)?.Name;

			return currentUserRole;
		}
	}
}