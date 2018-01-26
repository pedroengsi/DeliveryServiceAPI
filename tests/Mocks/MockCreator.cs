using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using DeliveryServiceAPI.Models;
using DeliveryServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeliveryServiceAPI.Tests.Mocks
{
	public static class MockCreator
	{
		#region Fields

		public const string PointAId = "DB49AFE9-5907-4E89-94C4-240208D25987";
		private const string PointBId = "C19301BD-F496-4EB7-9D96-0DBDC3E205F3";
		private const string PointCId = "A63D8451-2F2E-4BE6-A4E7-825AAEAD7D3E";
		private const string PointDId = "365E8C9B-F404-4E4F-A9B3-AB84013EBFD9";
		private const string PointEId = "F070FA06-E975-404D-8C9C-F03DCFF3C16C";
		private const string PointFId = "989BAB29-5EDE-4C12-8CE8-DBDEC68E917C";
		private const string PointGId = "B90B29E0-8ECF-443F-8B1C-E355D9C2F7F5";
		private const string PointHId = "B61D4384-C8E1-4316-9375-E64C25286FB4";
		private const string PointIId = "CBE134EE-361D-4056-8043-D3EF4441761A";

		#endregion

		public static IPointsService GetMockPointsService(string pointId = PointAId)
		{
			var pointsService = new Mock<IPointsService>();

			pointsService.Setup(service => service.GetPointsAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(GetPoints()));

			pointsService.Setup(service => service.GetPointByIdAsync(Guid.Parse(pointId), It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(GetPointA(pointId)));





			return pointsService.Object;
		}

		public static void AddUserToController(Controller controller, string userName, string role)
		{
			var user = new Mock<IPrincipal>();
			user.Setup(p => p.IsInRole("AdminRole")).Returns(true);
			user.SetupGet(x => x.Identity.Name).Returns("localadmin");
			//controller.HttpContext.User = new ClaimsPrincipal(user.Object);
		}


		public static IAccountsService GetMockAccountsService(bool allowAdminToLogIn=false)
		{
			var accountsService = new Moq.Mock<IAccountsService>();
			accountsService.Setup(service => service.CanExecuteAdminRequest(It.IsAny<ClaimsPrincipal>()))
				.Returns(allowAdminToLogIn);



			return accountsService.Object;
		}



		private static Results<Point> GetPoints()
		{
			IList<Point> dummyPoints = new List<Point>();

			AddPointEntity(dummyPoints, PointAId, "Point A");
			AddPointEntity(dummyPoints, PointBId, "Point B");
			AddPointEntity(dummyPoints, PointCId, "Point C");
			AddPointEntity(dummyPoints, PointDId, "Point D");
			AddPointEntity(dummyPoints, PointEId, "Point E");
			AddPointEntity(dummyPoints, PointFId, "Point F");
			AddPointEntity(dummyPoints, PointGId, "Point G");
			AddPointEntity(dummyPoints, PointHId, "Point H");
			AddPointEntity(dummyPoints, PointIId, "Point I");

			return new Results<Point>() { Items = dummyPoints.ToArray(), TotalSize = dummyPoints.Count };
		}

		private static void AddPointEntity(IList<Point> dummyPoints, string pointId, string pointName)
		{
			dummyPoints.Add(new Point
			{
				Name = pointName
			});
		}


		private static Point GetPointA(string pointId)
		{
			if (pointId == PointAId)
			{
				return new Point
				{
					Name = "Point A"
				};
			}
			return null;
		}
		//public async Task<Point> GetPointByIdAsync(Guid id, CancellationToken ct)
		//{
		//	var point = await _context.Points.SingleOrDefaultAsync(r => r.Id == id, ct);
		//	return point == null ? null : Mapper.Map<Point>(point);
		//}

		///// <inheritdoc />
		///// <summary>
		/////     Inserts a new Point
		///// </summary>
		///// <param name="newPointName">The name of the new point to create</param>
		///// <param name="ct">Cancellation token to stop the insertion of the points</param>
		///// <returns>Returns the Id of the created point</returns>
		//public async Task<Guid> CreatePointAsync(string newPointName, CancellationToken ct)
		//{
		//	var id = Guid.NewGuid();
		//	var newPoint = _context.Points.Add(new PointEntity
		//	{
		//		Id = id,
		//		Name = newPointName
		//	});

		//	var created = await _context.SaveChangesAsync(ct);

		//	if (created < 1) throw new InvalidOperationException(Resources.CouldNotCreatePoint);

		//	return id;
		//}

		///// <inheritdoc />
		///// <summary>
		/////     Updates an existing
		///// </summary>
		///// <param name="id">The id of the point to update</param>
		///// <param name="newPointName">The name of the new point to create</param>
		///// <param name="ct">Cancellation token to stop the insertion of the points</param>
		///// <returns>Returns the updated point</returns>
		//public async Task<Point> UpdatePointByIdAsync(Guid id, string newPointName, CancellationToken ct)
		//{
		//	var point = await _context.Points.SingleOrDefaultAsync(r => r.Id == id, ct);
		//	if (point == null) throw new ArgumentException(Resources.InvalidPointId);

		//	point.Name = newPointName;
		//	_context.Points.Update(point);

		//	await _context.SaveChangesAsync(ct);

		//	return Mapper.Map<Point>(point);
		//}

		///// <inheritdoc />
		///// <summary>
		/////     Delete an existing Point
		///// </summary>
		///// <param name="id">The id of the point to delete</param>
		///// <param name="ct">Cancellation token to stop the delete of the points</param>
		//public async Task DeletePointByIdAsync(Guid id, CancellationToken ct)
		//{
		//	var point = await _context.Points.SingleOrDefaultAsync(b => b.Id == id, ct);
		//	if (point == null) return;

		//	_context.Points.Remove(point);
		//	await _context.SaveChangesAsync(ct);
		//}
	}
}