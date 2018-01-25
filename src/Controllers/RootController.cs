namespace DeliveryServiceAPI.Controllers
{
	#region Using Directives

	using Microsoft.AspNetCore.Mvc;
	using Models;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Root API Controller to show the user the available links
	/// </summary>
	[Route("/")]
	[ApiVersion("1")]
	public class RootController : Controller
	{
		/// <summary>
		///     Root entry point to the Delivery Service API
		/// </summary>
		/// <remarks>
		///     This Request lists the main links of the API
		/// </remarks>
		/// <returns>
		///     The Delivery Service API main links
		/// </returns>
		[HttpGet(Name = nameof(GetRoot))]
		public IActionResult GetRoot()
		{
			var response = new RootResponse
			{
				Self = Link.To(nameof(GetRoot)),
				Points = Link.To(nameof(PointsController.GetPointsAsync)),
				Routes = Link.To(nameof(RoutesController.GetRoutesAsync))
			};

			return Ok(response);
		}
	}
}