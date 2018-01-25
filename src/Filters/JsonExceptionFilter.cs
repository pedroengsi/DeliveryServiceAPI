namespace DeliveryServiceAPI.Filters
{
	#region Using Directives

	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using Models;
	using Properties;

	#endregion

	/// <inheritdoc />
	/// <summary>
	///     Class responsible to treat the exceptions
	/// </summary>
	public class JsonExceptionFilter : IExceptionFilter
	{
		#region Fields

		private readonly IHostingEnvironment _env;

		#endregion

		#region Constructor

		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="env"></param>
		public JsonExceptionFilter(IHostingEnvironment env)
		{
			_env = env;
		}

		#endregion

		/// <inheritdoc />
		/// <summary>
		///     Method responsible to treat the exceptions and return the corresponding server error code to the user
		/// </summary>
		/// <param name="context"></param>
		public void OnException(ExceptionContext context)
		{
			var error = new ApiError();

			if (_env.IsDevelopment())
			{
				error.Message = context.Exception.Message;
				error.Detail = context.Exception.StackTrace;
			}
			else
			{
				error.Message = Resources.ServerError;
				error.Detail = context.Exception.Message;
			}

			context.Result = new ObjectResult(error)
			{
				StatusCode = 500
			};
		}
	}
}