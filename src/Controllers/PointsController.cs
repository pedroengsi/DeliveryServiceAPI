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
	///     Points API Controller
	/// </summary>
	[ApiVersion("1")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class PointsController : Controller
	{
		#region Fields

		private readonly IPointsService _pointsService;
		private readonly IAccountsService _accountsService;

		#endregion

		#region Constructor

		/// <summary>
		/// </summary>
		/// <param name="pointsService">Service responsible for all the operations over the Points</param>
		/// <param name="accountsService">Service responsible for all the operations over the accounts</param>
		public PointsController(IPointsService pointsService, IAccountsService accountsService)
		{
			_pointsService = pointsService;
			_accountsService = accountsService;
		}

		#endregion

		/// <summary>
		///     Get a list of all available points
		/// </summary>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns a collection of points</returns>
		[HttpGet(Name = nameof(GetPointsAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		public async Task<IActionResult> GetPointsAsync(CancellationToken ct)
		{
			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var points = await _pointsService.GetPointsAsync(ct);

			var collection = Collection<Point>.Create<PointsResponse>(Link.ToCollection(nameof(GetPointsAsync)),
				points.Items.ToArray(), points.TotalSize);

			return Ok(collection);
		}

		/// <summary>
		///     Get the point with the given id
		/// </summary>
		/// <param name="pointId">The id of the point to retrieve</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns a point</returns>
		[HttpGet("{pointId}", Name = nameof(GetPointByIdAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(NotFoundResult), 404)]
		public async Task<IActionResult> GetPointByIdAsync(Guid pointId, CancellationToken ct)
		{
			if (pointId == Guid.Empty) return BadRequest(new ApiError(Resources.InvalidParameters, Resources.IdFieldRequired));

			var point = await _pointsService.GetPointByIdAsync(pointId, ct);
			if (point == null) return NotFound();

			return Ok(point);
		}

		/// <summary>
		///     Insert a new point
		/// </summary>
		/// <param name="newPoint">The name of the new point to create</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns Ok on sucess and BadRequest if it fauls</returns>
		[Authorize]
		[HttpPost("", Name = nameof(CreateNewPointAsync))]
		[ProducesResponseType(typeof(CreatedResult), 201)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		public async Task<IActionResult> CreateNewPointAsync([FromBody] PointCreate newPoint, CancellationToken ct)
		{
			if (!_accountsService.CanExecuteAdminRequest(User)) return Unauthorized();

			if (string.IsNullOrEmpty(newPoint?.PointName))
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.NameFieldRequired));
			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var pointId = await _pointsService.CreatePointAsync(newPoint.PointName, ct);

			return Created(Url.Link(nameof(GetPointByIdAsync), new {pointId}), null);
		}

		/// <summary>
		///     Update the point with the given id
		/// </summary>
		/// <param name="pointId">The id of the point to update</param>
		/// <param name="pointToUpdate">The point name to update</param>
		/// <param name="ct">Cancellation token to stop the update of the point</param>
		/// <returns>Returns a point</returns>
		[Authorize]
		[HttpPut("{pointId}", Name = nameof(UpdatePointByIdAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		[ProducesResponseType(typeof(NotFoundResult), 404)]
		public async Task<IActionResult> UpdatePointByIdAsync(Guid pointId, PointUpdate pointToUpdate, CancellationToken ct)
		{
			if (!_accountsService.CanExecuteAdminRequest(User)) return Unauthorized();

			if (pointId == Guid.Empty) return BadRequest(new ApiError(Resources.InvalidParameters, Resources.IdFieldRequired));

			if (string.IsNullOrEmpty(pointToUpdate?.PointName))
				return BadRequest(new ApiError(Resources.InvalidParameters, Resources.NameFieldRequired));

			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var point = await _pointsService.GetPointByIdAsync(pointId, ct);
			if (point == null) return NotFound();

			point = await _pointsService.UpdatePointByIdAsync(pointId, pointToUpdate.PointName, ct);

			return Ok(point);
		}

		/// <summary>
		///     Delete the point with the given id
		/// </summary>
		/// <param name="pointId">The id of the point to Delete</param>
		/// <param name="ct">Cancellation token to stop the deletion of the point</param>
		/// <returns>Returns NoContent</returns>
		[Authorize]
		[HttpDelete("{pointId}", Name = nameof(DeletePointByIdAsync))]
		[ProducesResponseType(typeof(OkResult), 200)]
		[ProducesResponseType(typeof(BadRequestResult), 400)]
		[ProducesResponseType(typeof(UnauthorizedResult), 401)]
		[ProducesResponseType(typeof(NotFoundResult), 404)]
		public async Task<IActionResult> DeletePointByIdAsync(Guid pointId, CancellationToken ct)
		{
			if (!_accountsService.CanExecuteAdminRequest(User)) return Unauthorized();

			if (pointId == Guid.Empty) return BadRequest(new ApiError(Resources.InvalidParameters, Resources.IdFieldRequired));

			if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

			var point = await _pointsService.GetPointByIdAsync(pointId, ct);
			if (point == null) return NotFound();

			await _pointsService.DeletePointByIdAsync(pointId, ct);

			return Ok();
		}
	}
}