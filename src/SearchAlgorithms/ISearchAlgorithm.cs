namespace DeliveryServiceAPI.SearchAlgorithms
{
	#region Using Directives

	using System;
	using Models;

	#endregion

	/// <summary>
	///     Search Algorithm interface
	/// </summary>
	internal interface ISearchAlgorithm
	{
		/// <summary>
		///     Search the optimal path according to the parameters
		/// </summary>
		/// <param name="graph">The route graph to search</param>
		/// <param name="origin">The origin point to search</param>
		/// <param name="destination">The destination point</param>
		/// <param name="searchOptionsPath">Defines if the search can be direct or needs at least one hop between origin and destination</param>
		SearchAlgorithmResult Search(Graph graph, Guid origin, Guid destination, SearchOptions.SearchOptionPath searchOptionsPath);
	}
}