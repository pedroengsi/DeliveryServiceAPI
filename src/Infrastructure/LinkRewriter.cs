namespace DeliveryServiceAPI.Infrastructure
{
	#region Using Directives

	using Microsoft.AspNetCore.Mvc;
	using Models;

	#endregion

	/// <summary>
	///     Helper class to rewrite all the links before sending the resource back to the user
	/// </summary>
	public class LinkRewriter
	{
		#region Fields

		private readonly IUrlHelper _urlHelper;

		#endregion

		#region Constructos

		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="urlHelper"></param>
		public LinkRewriter(IUrlHelper urlHelper)
		{
			_urlHelper = urlHelper;
		}

		#endregion

		/// <summary>
		///     Rewrites the original link
		/// </summary>
		/// <param name="original">The original link</param>
		/// <returns></returns>
		public Link Rewrite(Link original)
		{
			if (original == null) return null;

			return new Link
			{
				Href = _urlHelper.Link(original.RouteName, original.RouteValues),
				Method = original.Method,
				Relations = original.Relations
			};
		}
	}
}