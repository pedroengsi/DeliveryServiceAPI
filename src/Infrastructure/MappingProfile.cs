namespace DeliveryServiceAPI.Infrastructure
{
	#region Using Directives

	using AutoMapper;
	using Controllers;
	using Models;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     The AutoMapper class to help mapp from an entity in the context to the corresponding resource
	/// </summary>
	public class MappingProfile : Profile
	{
		/// <summary>
		///     The Mapping constructor for the entities
		/// </summary>
		public MappingProfile()
		{
			CreateMap<PointEntity, Point>()
				.ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
					Link.To(nameof(PointsController.GetPointByIdAsync), new {pointId = src.Id})))
				.ForMember(dest => dest.Delete, opt => opt.MapFrom(src =>
					new Link
					{
						RouteName = nameof(PointsController.DeletePointByIdAsync),
						RouteValues = new {pointId = src.Id},
						Method = Link.DeleteMethod
					}))
				.ForMember(dest => dest.Update, opt => opt.MapFrom(src =>
					new Link
					{
						RouteName = nameof(PointsController.UpdatePointByIdAsync),
						RouteValues = new {pointId = src.Id},
						Method = Link.PutMethod
					}));

			CreateMap<RouteEntity, Route>()
				.ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
					Link.To(nameof(RoutesController.GetRouteByIdAsync), new {routeId = src.Id})))
				.ForMember(dest => dest.Origin, opt => opt.MapFrom(src =>
					Link.To(nameof(PointsController.GetPointByIdAsync),
						new {pointId = src.Origin.Id})))
				.ForMember(dest => dest.Destination, opt => opt.MapFrom(src =>
					Link.To(nameof(PointsController.GetPointByIdAsync),
						new {pointId = src.Destination.Id})))
				.ForMember(dest => dest.Delete, opt => opt.MapFrom(src =>
					new Link
					{
						RouteName = nameof(RoutesController.DeleteRouteByIdAsync),
						RouteValues = new {routeId = src.Id},
						Method = Link.DeleteMethod
					}));
		}
	}
}