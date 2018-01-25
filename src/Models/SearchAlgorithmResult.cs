namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.Collections.Generic;
	using SearchAlgorithms;

	#endregion

	/// <summary>
	///     The result of performing a search for an optimal path
	/// </summary>
	internal class SearchAlgorithmResult
	{
		/// <summary>
		///     Class constructor
		/// </summary>
		public SearchAlgorithmResult()
		{
			RouteVertex = new List<Vertex>();
			TotalDistance = 0;
		}

		/// <summary>
		///     The points included in the path
		/// </summary>
		public IList<Vertex> RouteVertex { get; set; }

		/// <summary>
		///     The total cost of the found path
		/// </summary>
		public int TotalDistance { get; set; }
	}
}