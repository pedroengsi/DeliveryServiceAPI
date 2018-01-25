namespace DeliveryServiceAPI.Controllers
{
	#region Using Directives

	using System;
	using System.Linq;
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
	///     Routes API Controller
	/// </summary>
	[ApiVersion("1")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class RoutesController : Controller
	{
		#region Fields

		private readonly IRoutesService _routesService;
		private readonly IPointsService _pointsService;
		private readonly IAccountsService _accountsService;

		#endregion

		#region Constructor

		/// <summary>
		///     Root Controller constructor
		/// </summary>
		/// <param name="routesService">Service responsible for all the operations over the Routes</param>
		/// <param name="pointsService">Service responsible for all the operations over the Points</param>
		/// <param name="accountsService">Service responsible for all the operations over the accounts</param>
		public RoutesController(IRoutesService routesService, IPointsService pointsService, IAccountsService accountsService)
		{
			_accountsService = accountsService;
			_routesService = routesService;
			_pointsService = pointsService;
		}

		#endregion

		/// <summary>
		///     Get a list of all available routes
		/// </summary>
		/// <param name="ct">Cancellation token to stop the retrieval of the routes</param>
		/// <returns>Returns a collection of routes</returns>
		[HttpGet(Name = nameof(GetRoutesAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		public async Task<IActionResult> GetRoutesAsync(CancellationToken ct)
		{
			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var routes = await _routesService.GetRoutesAsync(ct);

			var collection = Collection<Route>.Create<RoutesResponse>(Link.ToCollection(nameof(GetRoutesAsync)),
				routes.Items.ToArray(), routes.TotalSize);

			return Ok(collection);
		}

		/// <summary>
		///     Get the route with the given id
		/// </summary>
		/// <param name="routeId">The id of the route to retrieve</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the route</param>
		/// <returns>Returns a route</returns>
		[HttpGet("{routeId}", Name = nameof(GetRouteByIdAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(NotFoundResult), 404)]
		public async Task<IActionResult> GetRouteByIdAsync(Guid routeId, CancellationToken ct)
		{
			if (routeId == Guid.Empty) return BadRequest(new ApiError(Resources.InvalidParameters, Resources.IdFieldRequired));

			var route = await _routesService.GetRouteByIdAsync(routeId, ct);
			if (route == null) return NotFound();

			return Ok(route);
		}

		/// <summary>
		///     Insert a new route
		/// </summary>
		/// <param name="newRoute">The information relative to the route to create</param>
		/// <param name="ct">Cancellation token to stop the creation of the route</param>
		/// <returns>Returns Ok on sucess and BadRequest if it fauls</returns>
		[Authorize]
		[HttpPost("", Name = nameof(CreateNewRouteAsync))]
		[ProducesResponseType(typeof(CreatedResult), 201)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		public async Task<IActionResult> CreateNewRouteAsync([FromBody] RouteCreate newRoute, CancellationToken ct)
		{
			if (!_accountsService.CanExecuteAdminReques(User)) return Unauthorized();

			if (string.IsNullOrEmpty(newRoute?.RouteName))
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.NameFieldRequired));
			if (newRoute.Origin == Guid.Empty)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidOriginPointId));
			if (newRoute.Destination == Guid.Empty)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidDestinationPointId));
			if (newRoute.Cost <= 0)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidCostMustBeGreaterThanZero));
			if (newRoute.Time <= 0)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidTimeMustBeGreaterThanZero));

			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var routeId = await _routesService.CreateRouteAsync(newRoute, ct);

			return Created(Url.Link(nameof(GetRouteByIdAsync), new {routeId}), null);
		}

		/// <summary>
		///     Update the route with the given id
		/// </summary>
		/// <param name="routeId">The id of the route to update</param>
		/// <param name="routeToUpdate">The route fields to update</param>
		/// <param name="ct">Cancellation token to stop the update of the route</param>
		/// <returns>Returns a route</returns>
		[Authorize]
		[HttpPut("{routeId}", Name = nameof(UpdateRouteByIdAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(NotFoundResult), 404)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		public async Task<IActionResult> UpdateRouteByIdAsync(Guid routeId, RouteUpdate routeToUpdate, CancellationToken ct)
		{
			if (!_accountsService.CanExecuteAdminReques(User)) return Unauthorized();

			if (routeId == Guid.Empty) return BadRequest(new ApiError(Resources.InvalidParameters, Resources.IdFieldRequired));

			if (string.IsNullOrEmpty(routeToUpdate?.RouteName))
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.NameFieldRequired));
			if (routeToUpdate.Origin == Guid.Empty)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidOriginPointId));
			if (routeToUpdate.Destination == Guid.Empty)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidDestinationPointId));
			if (routeToUpdate.Cost <= 0)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidCostMustBeGreaterThanZero));
			if (routeToUpdate.Time <= 0)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidTimeMustBeGreaterThanZero));

			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var route = await _routesService.GetRouteByIdAsync(routeId, ct);
			if (route == null) return NotFound();

			route = await _routesService.UpdateRouteByIdAsync(routeId, routeToUpdate, ct);

			return Ok(route);
		}

		/// <summary>
		///     Delete the route with the given id
		/// </summary>
		/// <param name="routeId">The id of the route to Delete</param>
		/// <param name="ct">Cancellation token to stop the delete of the route</param>
		/// <returns>Returns NoContent</returns>
		[Authorize]
		[HttpDelete("{routeId}", Name = nameof(DeleteRouteByIdAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		[ProducesResponseType(typeof(NotFoundResult), 404)]
		public async Task<IActionResult> DeleteRouteByIdAsync(Guid routeId, CancellationToken ct)
		{
			if (!_accountsService.CanExecuteAdminReques(User)) return Unauthorized();

			if (routeId == Guid.Empty) return BadRequest(new ApiError(Resources.InvalidParameters, Resources.IdFieldRequired));

			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var point = await _routesService.GetRouteByIdAsync(routeId, ct);
			if (point == null) return NotFound();

			await _routesService.DeleteRouteByIdAsync(routeId, ct);

			return Ok();
		}

		// /routes/{originPointId}/{destinationPointId}
		/// <summary>
		///     Get the route with the optimal path
		/// </summary>
		/// <param name="originPointId">The id of the point to start the path</param>
		/// <param name="destinationPointId">The id of the point to end the path</param>
		/// <param name="searchParameters">The parameters needed to perform the search</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the route</param>
		/// <returns>Returns a route</returns>
		[HttpGet("{originPointId}/{destinationPointId}", Name = nameof(SearchPathRouteAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(NotFoundResult), 404)]
		public async Task<IActionResult> SearchPathRouteAsync(Guid originPointId, Guid destinationPointId,
			SearchAlgorithmParameters searchParameters, CancellationToken ct)
		{
			if (originPointId == Guid.Empty)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidOriginPointId));
			if (destinationPointId == Guid.Empty)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidDestinationPointId));

			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var originPoint = await _pointsService.GetPointByIdAsync(originPointId, ct);
			if (originPoint == null)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidOriginPointId));

			var destinationPoint = await _pointsService.GetPointByIdAsync(destinationPointId, ct);
			if (destinationPoint == null)
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.InvalidDestinationPointId));

			var points = await _routesService.SearchRoutePointsAsync(originPointId, destinationPointId, searchParameters, ct);

			if (points?.Items == null || !points.Items.Any())
			{
				return NotFound();
			}

			var collection = Collection<Point>.Create<PointsResponse>(Link.ToCollection(nameof(PointsController.GetPointsAsync)),
				points.Items.ToArray(), points.TotalSize);

			return Ok(collection);
		}
	}
}