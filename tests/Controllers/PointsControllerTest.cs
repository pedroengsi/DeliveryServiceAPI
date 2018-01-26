using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryServiceAPI.Controllers;
using DeliveryServiceAPI.Models;
using DeliveryServiceAPI.Services;
using DeliveryServiceAPI.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeliveryServiceAPI.Tests.Controllers
{
	[TestClass]
	public class PointsControllerTest
	{
		[TestMethod, TestCategory("HttpGet")]
		[Owner("Pedro Martins")]
		public async Task ShouldGetAllPoints_Ok()
		{
			// Arrange
			var controller = new PointsController(MockCreator.GetMockPointsService(), MockCreator.GetMockAccountsService());
			
			// Act
			var result = await controller.GetPointsAsync(new CancellationToken());

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkObjectResult));

			var okResult = result as OkObjectResult;
			Assert.IsInstanceOfType(okResult.Value, typeof(PointsResponse));

			var pointsResponse = okResult.Value as PointsResponse;

			Assert.AreEqual(pointsResponse.Size, 9);
		}

		[TestMethod, TestCategory("HttpGet")]
		[Owner("Pedro Martins")]
		public async Task ShouldGetAllPoints_BadRequestWithInvalidModel()
		{
			// Arrange
			var controller = new PointsController(MockCreator.GetMockPointsService(), MockCreator.GetMockAccountsService());
			controller.ModelState.AddModelError("Unknown error", "There was an unknown error");

			// Act
			var result = await controller.GetPointsAsync(new CancellationToken());

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
		}

		[TestMethod, TestCategory("HttpGet")]
		[Owner("Pedro Martins")]
		public async Task ShouldGetPoint_A()
		{
			// Arrange
			var controller = new PointsController(MockCreator.GetMockPointsService(), MockCreator.GetMockAccountsService());

			// Act
			var result = await controller.GetPointByIdAsync(Guid.Parse(MockCreator.PointAId), new CancellationToken());

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(OkObjectResult));

			var okResult = result as OkObjectResult;
			Assert.IsInstanceOfType(okResult.Value, typeof(Point));

			var point = okResult.Value as Point;

			Assert.AreEqual(point.Name, "Point A");
		}

		[TestMethod, TestCategory("HttpGet")]
		[Owner("Pedro Martins")]
		public async Task ShouldGetPoint_A_BadRequestWithInvalidModel()
		{
			// Arrange
			var controller = new PointsController(MockCreator.GetMockPointsService(Guid.Empty.ToString()), MockCreator.GetMockAccountsService());

			// Act
			var result = await controller.GetPointByIdAsync(Guid.Empty, new CancellationToken());

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
		}

		[TestMethod, TestCategory("HttpPost")]
		[Owner("Pedro Martins")]
		public async Task ShouldNotCreateNewPointAndReturnUnAuthorized()
		{
			// Arrange
			var controller = new PointsController(MockCreator.GetMockPointsService(), MockCreator.GetMockAccountsService());

			// Act
			var result = await controller.CreateNewPointAsync(new PointCreate(){PointName = "Test Point"}, new CancellationToken());

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
		}

		[TestMethod, TestCategory("HttpPost")]
		[Owner("Pedro Martins")]
		public async Task ShouldCreateNewPointForAdminUser()
		{
		//	// Arrange
		//	var controller = new PointsController(MockCreator.GetMockPointsService(), MockCreator.GetMockAccountsService(true));
		//	MockCreator.AddUserToController(controller, "localadmin", "AdminRole");
		//	// Act
		//	var result = await controller.CreateNewPointAsync(new PointCreate() { PointName = "Test Point" }, new CancellationToken());

		//	// Assert
		//	Assert.IsNotNull(result);
		//	Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
		}

	}
}