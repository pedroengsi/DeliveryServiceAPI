namespace DeliveryServiceAPI.Services
{
	#region Using Directives

	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Models;

	#endregion

	/// <summary>
	///     Interface responsible for all the operations over the points
	/// </summary>
	public interface IPointsService
	{
		/// <summary>
		///     Get a list of all the points
		/// </summary>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns a collection of points</returns>
		Task<Results<Point>> GetPointsAsync(CancellationToken ct);

		/// <summary>
		///     Get the point with the given id
		/// </summary>
		/// <param name="id">The id of the point to retrieve</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns a point</returns>
		Task<Point> GetPointByIdAsync(Guid id, CancellationToken ct);

		/// <summary>
		///     Inserts a new Point
		/// </summary>
		/// <param name="newPointName">The name of the new point to create</param>
		/// <param name="ct">Cancellation token to stop the insertion of the points</param>
		/// <returns>Returns the Id of the created point</returns>
		Task<Guid> CreatePointAsync(string newPointName, CancellationToken ct);

		/// <summary>
		///     Updates an existing Point
		/// </summary>
		/// <param name="id">The id of the point to update</param>
		/// <param name="newPointName">The name of the new point to create</param>
		/// <param name="ct">Cancellation token to stop the update of the points</param>
		/// <returns>Returns the updated point</returns>
		Task<Point> UpdatePointByIdAsync(Guid id, string newPointName, CancellationToken ct);

		/// <summary>
		///     Delete an existing Point
		/// </summary>
		/// <param name="id">The id of the point to delete</param>
		/// <param name="ct">Cancellation token to stop the delete of the points</param>
		Task DeletePointByIdAsync(Guid id, CancellationToken ct);
	}
}