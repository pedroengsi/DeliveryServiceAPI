namespace DeliveryServiceAPI
{
	#region Using Directives

	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;

	#endregion

	/// <summary>
	/// </summary>
	public class Program
	{
		/// <summary>
		/// </summary>
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		/// <summary>
		/// </summary>
		public static IWebHost BuildWebHost(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseDefaultServiceProvider(options =>
					options.ValidateScopes = false)
				.Build();
		}
	}
}