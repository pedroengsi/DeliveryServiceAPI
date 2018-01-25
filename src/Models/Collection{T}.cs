namespace DeliveryServiceAPI.Models
{
	/// <inheritdoc />
	/// <summary>
	///     Class used for collections
	/// </summary>
	/// <typeparam name="T">Type of the items of the collection</typeparam>
	public class Collection<T> : Resource
	{
		/// <summary>
		///     List of values of the collection
		/// </summary>
		public T[] Value { get; set; }

		/// <summary>
		///     Size of the items collection
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		///     Helper method to create result responses
		/// </summary>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="self"></param>
		/// <param name="items"></param>
		/// <param name="size"></param>
		/// <returns>The response result for the collection</returns>
		public static TResponse Create<TResponse>(Link self, T[] items, int size) where TResponse : Collection<T>, new()
		{
			return new TResponse
			{
				Self = self,
				Value = items,
				Size = size
			};
		}
	}
}