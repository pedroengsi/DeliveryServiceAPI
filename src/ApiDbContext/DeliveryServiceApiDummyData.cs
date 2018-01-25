namespace DeliveryServiceAPI.ApiDbContext
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Models;

	#endregion

	/// <summary>
	///     Class used to generate dummy data for the Delivery Service API
	/// </summary>
	public static class DeliveryServiceApiDummyData
	{
		#region Fields

		private static readonly IList<PointEntity> DummyPoints = new List<PointEntity>();
		private static readonly IList<RouteEntity> DummyRoutes = new List<RouteEntity>();

		private const string PointAId = "DB49AFE9-5907-4E89-94C4-240208D25987";
		private const string PointBId = "C19301BD-F496-4EB7-9D96-0DBDC3E205F3";
		private const string PointCId = "A63D8451-2F2E-4BE6-A4E7-825AAEAD7D3E";
		private const string PointDId = "365E8C9B-F404-4E4F-A9B3-AB84013EBFD9";
		private const string PointEId = "F070FA06-E975-404D-8C9C-F03DCFF3C16C";
		private const string PointFId = "989BAB29-5EDE-4C12-8CE8-DBDEC68E917C";
		private const string PointGId = "B90B29E0-8ECF-443F-8B1C-E355D9C2F7F5";
		private const string PointHId = "B61D4384-C8E1-4316-9375-E64C25286FB4";
		private const string PointIId = "CBE134EE-361D-4056-8043-D3EF4441761A";

		private const string RouteAtoCId = "472B66B7-7DFE-4820-9CD0-6CBA836F287E";
		private const string RouteAtoEId = "1AB63127-E468-4BC7-9099-50662067B951";
		private const string RouteAtoHId = "5272CDE6-84E2-4BC6-A87D-365D49A98D2D";
		private const string RouteCtoBId = "674DF1C3-A0AA-4DE2-8479-B14FB6EBB5A0";
		private const string RouteDtoFId = "910AD925-4AA2-4794-B01A-D5AF0CC3B91C";
		private const string RouteEtoDId = "6E2ADECC-70F8-4DFB-AD99-6B8EC5FB78CA";
		private const string RouteFtoIId = "E930DAA8-3F5A-4EB0-ADDB-968267B7BBBC";
		private const string RouteFtoGId = "77F2AC5F-CD2C-4604-A4A5-CE500D814E8D";
		private const string RouteGtoBId = "C240F282-CA02-4F4E-B723-9B6333C44189";
		private const string RouteHtoEId = "A081FA42-1415-4983-A88F-F3CA20BB71F9";
		private const string RouteItoBId = "02F26B5D-AACE-46C7-BABA-18F8FA987906";

		#endregion

		#region Add Dummy test data

		/// <summary>
		///     Method used to add the dummy data to the context
		/// </summary>
		/// <param name="context"></param>
		public static void AddDummyData(DeliveryServiceApiContext context)
		{
			if (context == null) return;

			CreateDummyPoints();
			CreateDummyRoutes();

			context.Points.AddRangeAsync(DummyPoints);
			context.Routes.AddRangeAsync(DummyRoutes);

			context.SaveChanges();
		}

		/// <summary>
		///     Creates the dummy points for the service
		/// </summary>
		private static void CreateDummyPoints()
		{
			AddPointEntity(PointAId, "Point A");
			AddPointEntity(PointBId, "Point B");
			AddPointEntity(PointCId, "Point C");
			AddPointEntity(PointDId, "Point D");
			AddPointEntity(PointEId, "Point E");
			AddPointEntity(PointFId, "Point F");
			AddPointEntity(PointGId, "Point G");
			AddPointEntity(PointHId, "Point H");
			AddPointEntity(PointIId, "Point I");
		}

		/// <summary>
		///     Creates the dummy routes for the service
		/// </summary>
		private static void CreateDummyRoutes()
		{
			AddRouteEntity(RouteAtoCId, "Route From A To C", PointAId, PointCId, 1, 20);
			//AddRouteEntity(RouteAtoEId, "Route From A To E", PointAId, PointEId, 30, 5);
			AddRouteEntity(RouteAtoEId, "Route From A To E", PointAId, PointEId, 30, 2);
			AddRouteEntity(RouteAtoHId, "Route From A To H", PointAId, PointHId, 10, 1);
			AddRouteEntity(RouteCtoBId, "Route From C To B", PointCId, PointBId, 1, 12);
			AddRouteEntity(RouteDtoFId, "Route From D To F", PointDId, PointFId, 4, 50);
			AddRouteEntity(RouteEtoDId, "Route From E To D", PointEId, PointDId, 3, 5);
			AddRouteEntity(RouteFtoIId, "Route From F To I", PointFId, PointIId, 45, 50);
			AddRouteEntity(RouteFtoGId, "Route From F To G", PointFId, PointGId, 40, 50);
			AddRouteEntity(RouteGtoBId, "Route From G To B", PointGId, PointBId, 64, 73);
			AddRouteEntity(RouteHtoEId, "Route From H To E", PointHId, PointEId, 30, 1);
			AddRouteEntity(RouteItoBId, "Route From I To B", PointIId, PointBId, 65, 5);
		}

		/// <summary>
		///     Method to add a new point entity
		/// </summary>
		/// <param name="pointId"></param>
		/// <param name="pointName"></param>
		private static void AddPointEntity(string pointId, string pointName)
		{
			DummyPoints.Add(new PointEntity
			{
				Id = Guid.Parse(pointId),
				Name = pointName
			});
		}

		/// <summary>
		///     Method to add a new route entity
		/// </summary>
		/// <param name="routeId"></param>
		/// <param name="routeName"></param>
		/// <param name="originId"></param>
		/// <param name="destinationId"></param>
		/// <param name="time"></param>
		/// <param name="cost"></param>
		private static void AddRouteEntity(string routeId, string routeName, string originId, string destinationId, int time,
			int cost)
		{
			var originPoint = DummyPoints.FirstOrDefault(point => point.Id == Guid.Parse(originId));
			var destinationPoint =
				DummyPoints.FirstOrDefault(point => point.Id == Guid.Parse(destinationId));

			DummyRoutes.Add(new RouteEntity
			{
				Id = Guid.Parse(routeId),
				Name = routeName,
				Origin = originPoint,
				Destination = destinationPoint,
				Cost = cost,
				Time = time
			});
		}

		#endregion
	}
}