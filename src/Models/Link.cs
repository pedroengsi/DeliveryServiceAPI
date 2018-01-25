namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.ComponentModel;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	///     Helper class to automatically generate the url to a given resource / operation
	/// </summary>
	public class Link
	{
		#region Fields

		internal const string GetMethod = "GET";
		internal const string PostMethod = "POST";
		internal const string DeleteMethod = "DELETE";
		internal const string PutMethod = "PUT";

		#endregion

		/// <summary>
		///     Generate link To resource
		/// </summary>
		/// <param name="routeName"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		public static Link To(string routeName, object routeValues = null)
			=> new Link
			{
				RouteName = routeName,
				RouteValues = routeValues,
				Method = GetMethod,
				Relations = null
			};

		/// <summary>
		///     Generate link To Collectionresource
		/// </summary>
		/// <param name="routeName"></param>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		public static Link ToCollection(string routeName, object routeValues = null)
			=> new Link
			{
				RouteName = routeName,
				RouteValues = routeValues,
				Method = GetMethod,
				Relations = new[] {"collection"}
			};

		/// <summary>
		///     Link to self resource
		/// </summary>
		[JsonProperty(Order = -4)]
		public string Href { get; set; }

		/// <summary>
		///     Method needed to use in the self url
		/// </summary>
		[JsonProperty(Order = -3, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DefaultValue(GetMethod)]
		public string Method { get; set; }

		/// <summary>
		///     Type of relations for the self url
		/// </summary>
		[JsonProperty(Order = -2, PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore)]
		public string[] Relations { get; set; }

		/// <summary>
		///     Stores the route name before being rewritten
		/// </summary>
		[JsonIgnore]
		public string RouteName { get; set; }

		/// <summary>
		///     Stores the route parameters before being rewritten
		/// </summary>
		[JsonIgnore]
		public object RouteValues { get; set; }
	}
}