namespace DeliveryServiceAPI.Services
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using ApiDbContext;
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using Microsoft.EntityFrameworkCore;
	using Models;
	using Properties;
	using SearchAlgorithms;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Class responsible for all the operations over the routes
	/// </summary>
	public class RoutesService : IRoutesService
	{
		#region Fields

		private readonly DeliveryServiceApiContext _context;

		#endregion

		#region Constructor

		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="context"></param>
		public RoutesService(DeliveryServiceApiContext context)
		{
			_context = context;
		}

		#endregion

		#region Properties

		// This is needed to ensure that the related entities are loaded.
		private IQueryable<RouteEntity> RoutesQuery =>
			_context.Routes.Include(route => route.Origin).Include(route => route.Destination);

		#endregion

		/// <inheritdoc />
		/// <summary>
		///     Get a list of all the routes
		/// </summary>
		/// <param name="ct">Cancellation token to stop the retrieval of the routes</param>
		/// <returns>Returns a collection of routes</returns>
		public async Task<Results<Route>> GetRoutesAsync(CancellationToken ct)
		{
			var query = RoutesQuery;
			var size = await query.CountAsync(ct);
			var items = await query.ProjectTo<Route>().ToArrayAsync(ct);

			return new Results<Route>
			{
				Items = items,
				TotalSize = size
			};
		}

		/// <inheritdoc />
		/// <summary>
		///     Get the route with the given id
		/// </summary>
		/// <param name="id">The id of the route to retrieve</param>
		/// <param name="ct">Cancellation token to stop the retrieval of the route</param>
		/// <returns>Returns a route</returns>
		public async Task<Route> GetRouteByIdAsync(Guid id, CancellationToken ct)
		{
			var route = await RoutesQuery.SingleOrDefaultAsync(r => r.Id == id, ct);
			return route == null ? null : Mapper.Map<Route>(route);
		}

		/// <inheritdoc />
		/// <summary>
		///     Inserts a new Route
		/// </summary>
		/// <param name="newRouteToCreate">The fields of the new route to create</param>
		/// <param name="ct">Cancellation token to stop the insertion of the route</param>
		/// <returns>Returns the Id of the created route</returns>
		public async Task<Guid> CreateRouteAsync(RouteCreate newRouteToCreate, CancellationToken ct)
		{
			if (newRouteToCreate == null) return Guid.Empty;

			var id = Guid.NewGuid();
			var newRoute = _context.Routes.Add(new RouteEntity()
			{
				Id = id,
				Name = newRouteToCreate?.RouteName,
				Origin = _context.Points.FirstOrDefault(point => point.Id == newRouteToCreate.Origin),
				Destination = _context.Points.FirstOrDefault(point => point.Id == newRouteToCreate.Destination),
				Cost = newRouteToCreate.Cost,
				Time = newRouteToCreate.Time
			});

			var created = await _context.SaveChangesAsync(ct);

			if (created < 1) throw new InvalidOperationException(Resources.CouldNotCreateRoute);

			return id;
		}

		/// <inheritdoc />
		/// <summary>
		///     Updates an existing Route
		/// </summary>
		/// <param name="id">The id of the route to update</param>
		/// <param name="routeToUpdate">The route fields to update</param>
		/// <param name="ct">Cancellation token to stop the update of the route</param>
		/// <returns>Returns the updated route</returns>
		public async Task<Route> UpdateRouteByIdAsync(Guid id, RouteUpdate routeToUpdate, CancellationToken ct)
		{
			var route = await RoutesQuery.SingleOrDefaultAsync(r => r.Id == id, ct);
			if (route == null) throw new ArgumentException(Resources.InvalidRouteId);

			if (!string.IsNullOrEmpty(routeToUpdate?.RouteName))
			{
				route.Name = routeToUpdate.RouteName;
			}

			if (routeToUpdate?.Origin != Guid.Empty)
			{
				route.Origin = _context.Points.FirstOrDefault(point => point.Id == routeToUpdate.Origin);
			}

			if (routeToUpdate?.Destination != Guid.Empty)
			{
				route.Destination = _context.Points.FirstOrDefault(point => point.Id == routeToUpdate.Destination);
			}

			if (routeToUpdate != null)
			{
				route.Cost = routeToUpdate.Cost;
				route.Time = routeToUpdate.Time;
			}

			_context.Routes.Update(route);

			await _context.SaveChangesAsync(ct);

			return Mapper.Map<Route>(route);
		}

		/// <inheritdoc />
		/// <summary>
		///     Delete an existing Route
		/// </summary>
		/// <param name="id">The id of the route to delete</param>
		/// <param name="ct">Cancellation token to stop the delete of the route</param>
		public async Task DeleteRouteByIdAsync(Guid id, CancellationToken ct)
		{
			var route = await RoutesQuery.SingleOrDefaultAsync(b => b.Id == id, ct);
			if (route == null) return;

			_context.Routes.Remove(route);
			await _context.SaveChangesAsync(ct);
		}

		/// <summary>
		///     Performs a search to find the optimal path between two points according to the search option
		/// </summary>
		/// <param name="origin">The origin point</param>
		/// <param name="destination">The destination point</param>
		/// <param name="searchParameters">The options needed by the search algorithm</param>
		/// <param name="ct">Cancellation token to stop the delete of the route</param>
		/// <returns></returns>
		public async Task<Results<Point>> SearchRoutePointsAsync(Guid origin, Guid destination,
			SearchAlgorithmParameters searchParameters, CancellationToken ct)
		{
			if (origin == Guid.Empty)
			{
				throw new ArgumentException(Resources.InvalidOriginPointId);
			}

			if (destination == Guid.Empty)
			{
				throw new ArgumentException(Resources.InvalidDestinationPointId);
			}

			var taskResult = await Task.Run(() => SearchShortestPath(origin, destination, searchParameters), ct);

			if (taskResult != null)
			{
				var totalDistance = taskResult.TotalDistance;
				var items = new List<PointEntity>();

				if (taskResult.RouteVertex != null && taskResult.RouteVertex.Any())
				{
					items = taskResult.RouteVertex.Select(vertex =>
						_context.Points.FirstOrDefault(point => point.Id == vertex.NodeId)).ToList();
				}

				return new Results<Point>
				{
					Items = Mapper.Map<IEnumerable<Point>>(items),
					TotalSize = totalDistance
				};
			}

			return null;
		}

		private SearchAlgorithmResult SearchShortestPath(Guid origin, Guid destination, SearchAlgorithmParameters searchParameters)
		{
			var graph = new Graph(_context);
			graph.PopulateGraph(searchParameters.SearchOptions);

			var search = SearchAlgorithmFactory.GetSearchAlgorithm(searchParameters.SearchAlgorithm);
			return search.Search(graph, origin, destination, searchParameters.SearchOptionPath);
		}
	}
}