namespace DeliveryServiceAPI.SearchAlgorithms
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Models;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Base Search Algorithm
	/// </summary>
	internal abstract class BaseSearchAlgorithm : ISearchAlgorithm
	{
		/// <inheritdoc />
		/// <summary>
		///     Search the optimal path according to the parameters
		/// </summary>
		/// <param name="graph">The route graph to search</param>
		/// <param name="origin">The origin point to search</param>
		/// <param name="destination">The destination point</param>
		/// <param name="searchOptionsPath">
		///     Defines if the search can be direct or needs at least one hop between origin and
		///     destination
		/// </param>
		public SearchAlgorithmResult Search(Graph graph, Guid origin, Guid destination,
			SearchOptions.SearchOptionPath searchOptionsPath)
		{
			if (graph?.Vertices == null || origin == Guid.Empty || destination == Guid.Empty || origin == destination) return null;

			SearchAlgorithmResult result = null;

			if (searchOptionsPath == SearchOptions.SearchOptionPath.Direct)
			{
				result = SearchShortestPaths(graph, origin, destination);
			}
			else
			{
				var neighbors = graph.Neighbors(origin);

				if (neighbors != null && neighbors.Any())
				{
					foreach (var neighbor in neighbors)
					{
						if (neighbor.NodeId != destination)
						{
							var neighborResult = SearchShortestPaths(graph, neighbor.NodeId, destination, origin);

							if (neighborResult == null) continue;

							neighborResult.RouteVertex.Insert(0, new Vertex(origin, 0));
							neighborResult.TotalDistance += neighbor.DistanceToNode;

							if (result == null || result.TotalDistance > neighborResult.TotalDistance)
							{
								result = neighborResult;
							}
						}
					}
				}
			}

			return result;
		}

		private SearchAlgorithmResult SearchShortestPaths(Graph graph, Guid origin, Guid destination)
		{
			return SearchShortestPaths(graph, origin, destination, Guid.Empty);
		}

		private SearchAlgorithmResult SearchShortestPaths(Graph graph, Guid origin, Guid destination, Guid nodeToNotInclude)
		{
			SearchAlgorithmResult result = null;
			var visitedNodes = new List<Guid>();
			var unvisitedNodes = new List<Guid>();
			var searchVertices = new List<SearchVertex>();

			graph.Vertices.ForEach(id =>
			{
				if (!unvisitedNodes.Contains(id) && id != nodeToNotInclude)
				{
					var distance = int.MaxValue;
					if (id == origin) distance = 0;

					unvisitedNodes.Add(id);
					searchVertices.Add(new SearchVertex(id, distance));
				}
			});

			DepthFirstSearch(graph, visitedNodes, unvisitedNodes, searchVertices);

			// Search from back to front ir order to build de path
			var lastVertex = searchVertices.FirstOrDefault(vertex => vertex.VertexId == destination);

			if (lastVertex != null && lastVertex.PreviousVertexId != Guid.Empty)
			{
				// The destination node is reachable from the origin
				var stack = new Stack<SearchVertex>();

				stack.Push(lastVertex);

				while (lastVertex != null && lastVertex.VertexId != origin && lastVertex.PreviousVertexId != Guid.Empty)
				{
					var previousVertexId = lastVertex.PreviousVertexId;

					lastVertex = searchVertices.FirstOrDefault(vertex => vertex.VertexId == previousVertexId);

					if (lastVertex != null)
					{
						stack.Push(lastVertex);
					}
				}

				result = new SearchAlgorithmResult();

				foreach (var searchVertex in stack)
				{
					result.RouteVertex.Add(new Vertex(searchVertex.VertexId, searchVertex.DistanceToOrigin));
					result.TotalDistance += searchVertex.DistanceToOrigin;
				}
			}

			return result;
		}

		protected SearchVertex GetSmallKnownUnvisitedVertex(List<Guid> unvisitedNodes, List<SearchVertex> searchVertices)
		{
			return searchVertices?.OrderBy(search => search.DistanceToOrigin)
				.FirstOrDefault(search => unvisitedNodes.Contains(search.VertexId) && search.DistanceToOrigin != int.MaxValue);
		}

		protected abstract void DepthFirstSearch(Graph graph, List<Guid> visitedNodes, List<Guid> unvisitedNodes,
			List<SearchVertex> searchVertices);
	}

	internal class SearchVertex
	{
		public SearchVertex(Guid vertexId, int distanceToOrigin) : this(vertexId, distanceToOrigin, Guid.Empty)
		{
		}

		public SearchVertex(Guid vertexId, int distanceToOrigin, Guid previousVertexId)
		{
			VertexId = vertexId;
			DistanceToOrigin = distanceToOrigin;
			PreviousVertexId = previousVertexId;
		}

		public Guid VertexId { get; }

		public int DistanceToOrigin { get; set; }

		public Guid PreviousVertexId { get; set; }
	}
}