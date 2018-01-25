namespace DeliveryServiceAPI.ApiDbContext
{
	#region Using Directives

	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Class to store the users and roles
	/// </summary>
	public class ApplicationDbContext : IdentityDbContext
	{
		#region Constructor

		/// <inheritdoc />
		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="options"></param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		#endregion

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	//optionsBuilder.UseMySql(GetConnectionString());
		//}

		//private static string GetConnectionString()
		//{
		//	const string databaseName = "webapijwt";
		//	const string databaseUser = "";
		//	const string databasePass = "";

		//	return $"Server=localhost;" +
		//	       $"database={databaseName};" +
		//	       $"uid={databaseUser};" +
		//	       $"pwd={databasePass};" +
		//	       $"pooling=true;";
		//}
	}
}