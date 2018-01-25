namespace DeliveryServiceAPI.SearchAlgorithms
{
	#region Using Directives

	using System;
	using System.Collections.Generic;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     A* Search Algorithm
	/// </summary>
	internal class AStarSearchAlgorithm : BaseSearchAlgorithm
	{
		protected override void DepthFirstSearch(Graph graph, List<Guid> visitedNodes, List<Guid> unvisitedNodes,
			List<SearchVertex> searchVertices)
		{
			throw new NotImplementedException();
		}
	}
}