namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.Collections.Generic;

	#endregion

	/// <summary>
	///     Result class to store returned lists
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Results<T>
	{
		/// <summary>
		///     Items list returned
		/// </summary>
		public IEnumerable<T> Items { get; set; }

		/// <summary>
		///     Total size of the items list
		/// </summary>
		public int TotalSize { get; set; }
	}
}