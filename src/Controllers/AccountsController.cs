namespace DeliveryServiceAPI.Controllers
{
	#region Using Directives

	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Models;
	using Properties;
	using Services;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Accounts API Controller
	/// </summary>
	[Route("[controller]/[action]")]
	public class AccountsController : Controller
	{
		#region Fields

		private readonly IAccountsService _accountsService;

		#endregion

		#region Constructor

		/// <summary>
		///     Accounts Controller constructor
		/// </summary>
		/// <param name="accountsService">Service responsible for all the operations over the accounts</param>
		public AccountsController(IAccountsService accountsService)
		{
			_accountsService = accountsService;
		}

		#endregion

		/// <summary>
		///     Login a user and returns the Bearer Token
		/// </summary>
		/// <param name="login">User login info</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns Ok on sucess and BadRequest if it fauls</returns>
		//[Authorize]
		[HttpPost("", Name = nameof(LoginUser))]
		[ProducesResponseType(typeof(CreatedResult), 201)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		public async Task<IActionResult> LoginUser([FromBody] UserLogin login, CancellationToken ct)
		{
			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var result = await _accountsService.SignInUser(login.UserName, login.Password);

			if (!result.Succeeded) return Unauthorized();

			var appUser = _accountsService.GetUserByUserName(login.UserName);
			return Ok(_accountsService.GenerateJwtToken(appUser));
		}

		/// <summary>
		///     Verifies if a user has logged in
		/// </summary>
		/// <returns>Returns Ok on sucess and Unauthorized if it fauls</returns>
		[Authorize]
		[HttpGet(Name = nameof(VerifyLoginStatus))]
		[ProducesResponseType(typeof(CreatedResult), 201)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		public IActionResult VerifyLoginStatus()
		{
			if (!User.Identity.IsAuthenticated) return Unauthorized();

			var userLoginResult = _accountsService.RetrieveLoggedInUserInfo(User);
			return Ok(userLoginResult);
		}

		/// <summary>
		///     Inserts a new normal user
		/// </summary>
		/// <param name="newUser">The info of the new user to create</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns Ok on sucess and BadRequest if it fauls</returns>
		[Authorize]
		[HttpPost("", Name = nameof(CreateNewUserAsync))]
		[ProducesResponseType(typeof(CreatedResult), 201)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		public async Task<IActionResult> CreateNewUserAsync([FromBody] UserCreate newUser, CancellationToken ct)
		{
			if (!_accountsService.CanExecuteAdminReques(User)) return Unauthorized();

			if (string.IsNullOrEmpty(newUser?.UserName))
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.NameFieldRequired));
			if (string.IsNullOrEmpty(newUser.Email))
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.EmailFieldRequired));
			if (string.IsNullOrEmpty(newUser.Password))
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.PasswordFieldRequired));

			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var result = await _accountsService.CreateUserAsync(newUser);

			if (result.Succeeded) return Created(Url.Link(nameof(VerifyLoginStatus), null), null);

			return BadRequest(result.Errors);
		}
	}
}