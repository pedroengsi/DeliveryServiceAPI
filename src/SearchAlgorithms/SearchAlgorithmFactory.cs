namespace DeliveryServiceAPI.SearchAlgorithms
{
	/// <summary>
	///     Factory used to create an instance of the Search Algorithm
	/// </summary>
	internal static class SearchAlgorithmFactory
	{
		/// <summary>
		///     Method used to create an instance of the Search Algorithm
		/// </summary>
		/// <param name="searchAlgorithm">Search algorithm to create</param>
		/// <returns></returns>
		public static ISearchAlgorithm GetSearchAlgorithm(SearchOptions.SearchAlgorithm searchAlgorithm)
		{
			if (searchAlgorithm == SearchOptions.SearchAlgorithm.AStar)
			{
				return new AStarSearchAlgorithm();
			}

			return new DijkstraSearchAlgorithm();
		}
	}
}