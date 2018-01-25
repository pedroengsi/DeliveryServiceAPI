namespace DeliveryServiceAPI.SearchAlgorithms
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using ApiDbContext;
	using Microsoft.EntityFrameworkCore;

	#endregion

	internal class Graph
	{
		private readonly DeliveryServiceApiContext _context;

		private readonly Dictionary<Guid, List<Vertex>> _graph = new Dictionary<Guid, List<Vertex>>();

		public Graph(DeliveryServiceApiContext context)
		{
			_context = context;
		}

		public List<Guid> Vertices { get; private set; } = new List<Guid>();

		public void PopulateGraph(SearchOptions.SearchOption searchOptions)
		{
			ResetGraph();
			AddRoutes(searchOptions);
			AddPoints();
		}

		public List<Vertex> Neighbors(Guid origin)
		{
			if (origin != Guid.Empty && _graph.ContainsKey(origin))
			{
				return _graph[origin];
			}

			return null;
		}

		private void ResetGraph()
		{
			_graph.Clear();
			Vertices.Clear();
		}

		private void AddRoutes(SearchOptions.SearchOption searchOptions)
		{
			var routesQuery = _context.Routes.Include(route => route.Origin).Include(route => route.Destination);

			if (routesQuery == null || !routesQuery.Any()) return;

			foreach (var routeEntity in routesQuery)
			{
				if (routeEntity.Origin == null || routeEntity.Destination == null) continue;

				var origin = routeEntity.Origin.Id;
				var destination = routeEntity.Destination.Id;
				var weight = searchOptions == SearchOptions.SearchOption.Cost ? routeEntity.Cost : routeEntity.Time;

				AddRoute(origin, destination, weight);
			}
		}

		private void AddRoute(Guid originNode, Guid destinationNode, int distance)
		{
			if (!_graph.ContainsKey(originNode))
			{
				_graph.Add(originNode, new List<Vertex>());
			}

			var neighbors = _graph[originNode];
			neighbors.Add(new Vertex(destinationNode, distance));
			_graph[originNode] = neighbors;
		}

		private void AddPoints()
		{
			var pointIdsQuery = _context.Points.Select(point => point.Id);

			if (!pointIdsQuery.Any()) return;

			Vertices = pointIdsQuery.ToList();
		}
	}

	internal class Vertex
	{
		public Vertex(Guid nodeId, int distanceToNode)
		{
			NodeId = nodeId;
			DistanceToNode = distanceToNode;
		}

		public Guid NodeId { get; }

		public int DistanceToNode { get; }
	}
}