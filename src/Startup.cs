namespace DeliveryServiceAPI
{
	#region Using Directives

	using System;
	using System.IdentityModel.Tokens.Jwt;
	using System.IO;
	using System.Text;
	using ApiDbContext;
	using AutoMapper;
	using Filters;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Versioning;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.PlatformAbstractions;
	using Microsoft.IdentityModel.Tokens;
	using Newtonsoft.Json;
	using Services;
	using Swashbuckle.AspNetCore.Swagger;

	#endregion

	/// <summary>
	///     Startup class for the ASP.Net Core Web Application
	/// </summary>
	public class Startup
	{
		#region Fields

		private readonly int? _httpsPort;

		#endregion

		#region Constructor

		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="configuration">ASP.Net Core Web Application Configuration</param>
		/// <param name="env"></param>
		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;

			// Get the HTTPS port (only in development)
			if (env.IsDevelopment())
			{
				var launchJsonConfig = new ConfigurationBuilder()
					.SetBasePath(env.ContentRootPath)
					.AddJsonFile("Properties\\launchSettings.json")
					.Build();
				_httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
			}
		}

		#endregion

		#region Properties

		/// <summary>
		///     ASP.Net Core Web Application Configuration
		/// </summary>
		public IConfiguration Configuration { get; }

		#endregion

		/// <summary>
		///     This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			// Use an in-memory database for quick dev and testing
			// TODO: Swap out with a real database in production
			// ===== Add our DbContext ========
			services.AddDbContext<DeliveryServiceApiContext>(opt => { opt.UseInMemoryDatabase("DeliveryServiceApiDatabase"); });

			services.AddDbContext<ApplicationDbContext>(opt => { opt.UseInMemoryDatabase("ApplicationDbContext"); });

			// ===== Add Identity ========
			services.AddIdentity<IdentityUser, IdentityRole>(opt =>
				{
					opt.Password.RequireDigit = true;
					opt.Password.RequireLowercase = true;
					opt.Password.RequireUppercase = true;
					opt.Password.RequireNonAlphanumeric = true;
					opt.Password.RequiredLength = 6;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			// ===== Add Jwt Authentication ========
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(cfg =>
				{
					cfg.RequireHttpsMetadata = false;
					cfg.SaveToken = true;
					cfg.TokenValidationParameters = new TokenValidationParameters
					{
						ValidIssuer = Configuration["JWT:JwtIssuer"],
						ValidAudience = Configuration["JWT:JwtIssuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:JwtKey"])),
						ClockSkew = TimeSpan.Zero // remove delay of token when expire
					};
				});

			// ===== Add AutoMapper ========
			services.AddAutoMapper();

			// ===== Add MVC ========
			services.AddMvc(opt =>
				{
					opt.Filters.Add(typeof(JsonExceptionFilter));
					opt.Filters.Add(typeof(LinkRewritingFilter));

					// Require HTTPS for all controllers
					opt.SslPort = _httpsPort;
					opt.Filters.Add(typeof(RequireHttpsAttribute));
				})
				.AddJsonOptions(opt =>
				{
					// These should be the defaults, but we can be explicit:
					opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
					opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
					opt.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
				});

			// ===== Add Routing options ========
			services.AddRouting(opt => opt.LowercaseUrls = true);

			// ===== Add API Versioning ========
			services.AddApiVersioning(opt =>
			{
				opt.ApiVersionReader = new MediaTypeApiVersionReader();
				opt.AssumeDefaultVersionWhenUnspecified = true;
				opt.ReportApiVersions = true;
				opt.DefaultApiVersion = new ApiVersion(1, 0);
				opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
			});

			// ===== Add CustomServices ========
			services.AddScoped<IPointsService, PointsService>();
			services.AddScoped<IRoutesService, RoutesService>();
			services.AddScoped<IAccountsService, AccountsService>();

			// ===== Register the Swagger generator, defining one or more Swagger documents ========
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info {Title = "Deliver Service API", Version = "v1"});

				var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "DeliveryService.xml");
				c.IncludeXmlComments(filePath);

				c.DescribeAllEnumsAsStrings();
				c.DescribeStringEnumsInCamelCase();

				c.AddSecurityDefinition("JWTBearer",
					new ApiKeyScheme
					{
						Description = "JWT Bearer Authentication",
						In = "header",
						Type = "apiKey",
						Name = "Authorization"
					});
			});
		}

		/// <summary>
		///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="loggerFactory"></param>
		/// <param name="accountsService"></param>
		/// <param name="applicationDbContext"></param>
		/// <param name="deliveryServiceApiContext"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
			IAccountsService accountsService, ApplicationDbContext applicationDbContext,
			DeliveryServiceApiContext deliveryServiceApiContext)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			// Add some test data in development
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				// ===== Add test roles and users ======
				ApplicationDummyData.AddDummyUsersAsync(accountsService).Wait();

				// ===== Add some test data in development ======
				DeliveryServiceApiDummyData.AddDummyData(deliveryServiceApiContext);
			}

			app.Use(async (context, next) =>
			{
				if (context.Request.IsHttps)
				{
					await next();
				}
				else
				{
					var sslPortStr = string.Empty;
					if (_httpsPort != null && _httpsPort != 0 && _httpsPort != 443) sslPortStr = $":{_httpsPort}";
					var httpsUrl = $"https://{context.Request.Host.Host}{sslPortStr}{context.Request.Path}";
					context.Response.Redirect(httpsUrl);
				}
			});

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Deliver Service API V1"); });

			// ===== Use Authentication ======
			app.UseAuthentication();

			app.UseMvc();

			// ===== Create tables ======
			applicationDbContext.Database.EnsureCreated();
		}
	}
}