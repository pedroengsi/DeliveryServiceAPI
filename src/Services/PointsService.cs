namespace DeliveryServiceAPI.Services
{
	#region Using Directives

	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using ApiDbContext;
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using Microsoft.EntityFrameworkCore;
	using Models;
	using Properties;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Class responsible for all the operations over the points
	/// </summary>
	public class PointsService : IPointsService
	{
		#region Fields

		private readonly DeliveryServiceApiContext _context;

		#endregion

		#region Constructor

		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="context"></param>
		public PointsService(DeliveryServiceApiContext context)
		{
			_context = context;
		}

		#endregion

		/// <inheritdoc />
		/// <summary>
		///     Get a list of all the points
		/// </summary>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns a collection of points</returns>
		public async Task<Results<Point>> GetPointsAsync(CancellationToken ct)
		{
			IQueryable<PointEntity> query = _context.Points;
			var size = await query.CountAsync(ct);

			var items = await query.ProjectTo<Point>().ToArrayAsync(ct);

			return new Results<Point>
			{
				Items = items,
				TotalSize = size
			};
		}

		/// <inheritdoc />
		/// <summary>
		///     Get the point with the given id
		/// </summary>
		/// <param name="id">The id of the point to retrieve</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the points</param>
		/// <returns>Returns a point</returns>
		public async Task<Point> GetPointByIdAsync(Guid id, CancellationToken ct)
		{
			var point = await _context.Points.SingleOrDefaultAsync(r => r.Id == id, ct);
			return point == null ? null : Mapper.Map<Point>(point);
		}

		/// <inheritdoc />
		/// <summary>
		///     Inserts a new Point
		/// </summary>
		/// <param name="newPointName">The name of the new point to create</param>
		/// <param name="ct">Cancellation token to stop the insertion of the points</param>
		/// <returns>Returns the Id of the created point</returns>
		public async Task<Guid> CreatePointAsync(string newPointName, CancellationToken ct)
		{
			var id = Guid.NewGuid();
			var newPoint = _context.Points.Add(new PointEntity
			{
				Id = id,
				Name = newPointName
			});

			var created = await _context.SaveChangesAsync(ct);

			if (created < 1) throw new InvalidOperationException(Resources.CouldNotCreatePoint);

			return id;
		}

		/// <inheritdoc />
		/// <summary>
		///     Updates an existing
		/// </summary>
		/// <param name="id">The id of the point to update</param>
		/// <param name="newPointName">The name of the new point to create</param>
		/// <param name="ct">Cancellation token to stop the insertion of the points</param>
		/// <returns>Returns the updated point</returns>
		public async Task<Point> UpdatePointByIdAsync(Guid id, string newPointName, CancellationToken ct)
		{
			var point = await _context.Points.SingleOrDefaultAsync(r => r.Id == id, ct);
			if (point == null) throw new ArgumentException(Resources.InvalidPointId);

			point.Name = newPointName;
			_context.Points.Update(point);

			await _context.SaveChangesAsync(ct);

			return Mapper.Map<Point>(point);
		}

		/// <inheritdoc />
		/// <summary>
		///     Delete an existing Point
		/// </summary>
		/// <param name="id">The id of the point to delete</param>
		/// <param name="ct">Cancellation token to stop the delete of the points</param>
		public async Task DeletePointByIdAsync(Guid id, CancellationToken ct)
		{
			var point = await _context.Points.SingleOrDefaultAsync(b => b.Id == id, ct);
			if (point == null) return;

			_context.Points.Remove(point);
			await _context.SaveChangesAsync(ct);
		}
	}
}