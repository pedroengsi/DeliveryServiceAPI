using DeliveryServiceAPI.Controllers;
using DeliveryServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeliveryServiceAPI.Tests.Controllers
{
	[TestClass]
	public class RootControllerTest
	{
		[TestMethod, TestCategory("HttpGet")]
		[Owner("Pedro Martins")]
		public void ShouldGetLinksToSelfRouteAndPiints()
		{
			// Arrange
			var controller = new RootController();

			// Act
			var result = controller.GetRoot();

			// Assert
			var isOk = result is OkObjectResult;
			var rootResponse = (result as OkObjectResult)?.Value;
			var isRootResponse = rootResponse is RootResponse;

			Assert.IsNotNull(result);
			Assert.IsTrue(isOk);
			Assert.IsNotNull(rootResponse);
			Assert.IsTrue(isRootResponse);
		}
	}
}