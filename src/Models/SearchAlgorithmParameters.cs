namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using SearchAlgorithms;

	#endregion

	/// <summary>
	///     Parameters needed by the Search Algorithms
	/// </summary>
	public class SearchAlgorithmParameters
	{
		/// <summary>
		///     Defines if the search is by Time or Cost
		/// </summary>
		public SearchOptions.SearchOption SearchOptions { get; set; }

		/// <summary>
		///     Defines with Search Algorithm to use
		/// </summary>
		public SearchOptions.SearchAlgorithm SearchAlgorithm { get; set; }

		/// <summary>
		///     Defines if the path can be direct or at least one hop
		/// </summary>
		public SearchOptions.SearchOptionPath SearchOptionPath { get; set; }
	}
}