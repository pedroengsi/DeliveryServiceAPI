namespace DeliveryServiceAPI.SearchAlgorithms
{
	/// <summary>
	///     Enums needed by the Search Algorithms
	/// </summary>
	public static class SearchOptions
	{
		/// <summary>
		///     Defines with Search Algorithm to use
		/// </summary>
		public enum SearchAlgorithm
		{
			/// <summary>
			///     Dijkstra search algorithm
			/// </summary>
			Dijkstra,

			/// <summary>
			///     A* search algorithm
			/// </summary>
			/// <remarks>To be implemented in version 2</remarks>
			AStar
		}

		/// <summary>
		///     Defines if the search is by Time or Cost
		/// </summary>
		public enum SearchOption
		{
			/// <summary>
			///     Use the cost to search the optimal path
			/// </summary>
			Cost,

			/// <summary>
			///     Use the time to search the optimal path
			/// </summary>
			Time
		}

		/// <summary>
		///     Defines if the search is by Direct or at least one hop
		/// </summary>
		public enum SearchOptionPath
		{
			/// <summary>
			///     Direct search for the shortestPath
			/// </summary>
			Direct,

			/// <summary>
			///     At least one hop to the destination
			/// </summary>
			AtLeastOneHop
		}
	}
}