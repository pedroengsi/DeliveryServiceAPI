namespace DeliveryServiceAPI
{
	/// <summary>
	/// </summary>
	/// ///
	/// <summary>
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// </summary>
		public static string ToCamelCase(this string input)
		{
			if (string.IsNullOrEmpty(input)) return input;

			var first = input.Substring(0, 1).ToLower();
			if (input.Length == 1) return first;

			return first + input.Substring(1);
		}
	}
}