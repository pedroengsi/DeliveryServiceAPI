namespace DeliveryServiceAPI.SearchAlgorithms
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;

	#endregion

	/// <inheritdoc>
	///     <cref></cref>
	/// </inheritdoc>
	/// <summary>
	///     Dijkstra Search Algorithm
	/// </summary>
	internal class DijkstraSearchAlgorithm : BaseSearchAlgorithm
	{
		protected override void DepthFirstSearch(Graph graph, List<Guid> visitedNodes, List<Guid> unvisitedNodes,
			List<SearchVertex> searchVertices)
		{
			var canContinue = unvisitedNodes != null && unvisitedNodes.Any();

			while (canContinue)
			{
				var startVertex = GetSmallKnownUnvisitedVertex(unvisitedNodes, searchVertices);

				if (startVertex != null)
				{
					var startVertexId = startVertex.VertexId;
					var neighbors = graph.Neighbors(startVertex.VertexId);

					if (neighbors != null && neighbors.Any())
					{
						foreach (var neighbor in neighbors)
						{
							var currentDistance = startVertex.DistanceToOrigin + neighbor.DistanceToNode;
							var searchVertex = searchVertices.FirstOrDefault(vertex => vertex.VertexId == neighbor.NodeId);

							if (searchVertex == null || searchVertex.DistanceToOrigin <= currentDistance) continue;

							searchVertex.DistanceToOrigin = currentDistance;
							searchVertex.PreviousVertexId = startVertexId;
						}
					}

					unvisitedNodes.Remove(startVertexId);
					visitedNodes.Add(startVertexId);
				}
				else
				{
					canContinue = false;
				}

				canContinue = canContinue && unvisitedNodes.Any();
			}
		}
	}
}