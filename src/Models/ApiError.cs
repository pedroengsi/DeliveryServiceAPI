namespace DeliveryServiceAPI.Models
{
	#region Using Directives

	using System.ComponentModel;
	using System.Linq;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using Newtonsoft.Json;
	using Properties;

	#endregion

	/// <summary>
	///     Class used to report any error to the user
	/// </summary>
	public class ApiError
	{
		/// <summary>
		///     ApiError constructor
		/// </summary>
		public ApiError()
		{
		}

		/// <summary>
		///     ApiError constructor
		/// </summary>
		/// <param name="message">Message to add to the error.</param>
		public ApiError(string message)
		{
			Message = message;
		}

		/// <summary>
		///     ApiError constructor
		/// </summary>
		/// <param name="message">Message to add to the error.</param>
		/// <param name="detail">Detail to add to the error.</param>
		public ApiError(string message, string detail)
		{
			Message = message;
			Detail = detail;
		}

		/// <summary>
		///     ApiError constructor
		/// </summary>
		/// <param name="modelState">Model containing the parameters and their corresponding errors</param>
		public ApiError(ModelStateDictionary modelState)
		{
			Message = Resources.InvalidParameters;

			if (modelState != null)
				Detail = modelState.FirstOrDefault(x => x.Value.Errors.Any()).Value.Errors.FirstOrDefault()?.ErrorMessage;
		}

		/// <summary>
		///     Error Message
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		///     Full detail of the errors
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string Detail { get; set; }

		/// <summary>
		///     Stack trace of the error, specially for debugging purposes
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DefaultValue("")]
		public string StackTrace { get; set; }
	}
}