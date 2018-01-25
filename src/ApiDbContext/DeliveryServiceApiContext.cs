namespace DeliveryServiceAPI.ApiDbContext
{
	#region Using Directives

	using Microsoft.EntityFrameworkCore;
	using Models;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Class to store the points and routes
	/// </summary>
	public class DeliveryServiceApiContext : DbContext

	{
		#region Constructor

		/// <inheritdoc />
		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="options"></param>
		public DeliveryServiceApiContext(DbContextOptions<DeliveryServiceApiContext> options) : base(options)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		///     Representation of the Points
		/// </summary>
		public DbSet<PointEntity> Points { get; set; }

		/// <summary>
		///     Representation of the Routes
		/// </summary>
		public DbSet<RouteEntity> Routes { get; set; }

		#endregion
	}
}