namespace DeliveryServiceAPI.Services
{
	#region Using Directives

	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Models;

	#endregion

	/// <summary>
	///     Interface responsible for all the operations over the rputes
	/// </summary>
	public interface IRoutesService
	{
		/// <summary>
		///     Get a list of all the routes
		/// </summary>
		/// <param name="ct">Cancellation token to stop the retrieval of the routes</param>
		/// <returns>Returns a collection of routes</returns>
		Task<Results<Route>> GetRoutesAsync(CancellationToken ct);

		/// <summary>
		///     Get the route with the given id
		/// </summary>
		/// <param name="id">The id of the route to retrieve</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the route</param>
		/// <returns>Returns a route</returns>
		Task<Route> GetRouteByIdAsync(Guid id, CancellationToken ct);

		/// <summary>
		///     Inserts a new Route
		/// </summary>
		/// <param name="newRouteToCreate">The fields of the new route to create</param>
		/// <param name="ct">Cancellation token to stop the insertion of the route</param>
		/// <returns>Returns the Id of the created route</returns>
		Task<Guid> CreateRouteAsync(RouteCreate newRouteToCreate, CancellationToken ct);

		/// <summary>
		///     Updates an existing Route
		/// </summary>
		/// <param name="id">The id of the route to update</param>
		/// <param name="routeToUpdate">The route fields to update</param>
		/// <param name="ct">Cancellation token to stop the update of the route</param>
		/// <returns>Returns the updated route</returns>
		Task<Route> UpdateRouteByIdAsync(Guid id, RouteUpdate routeToUpdate, CancellationToken ct);

		/// <summary>
		///     Delete an existing Route
		/// </summary>
		/// <param name="id">The id of the route to delete</param>
		/// <param name="ct">Cancellation token to stop the delete of the route</param>
		Task DeleteRouteByIdAsync(Guid id, CancellationToken ct);

		/// <summary>
		///     Performs a search to find the optimal path between two points according to the search option
		/// </summary>
		/// <param name="origin">The origin point</param>
		/// <param name="destination">The destination point</param>
		/// <param name="searchParameters">The options needed by the search algorithm</param>
		/// <param name="ct">Cancellation token to stop the delete of the route</param>
		/// <returns></returns>
		Task<Results<Point>> SearchRoutePointsAsync(Guid origin, Guid destination, SearchAlgorithmParameters searchParameters,
			CancellationToken ct);
	}
}