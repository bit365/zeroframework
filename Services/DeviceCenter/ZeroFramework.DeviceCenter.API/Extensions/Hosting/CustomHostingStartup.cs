using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ZeroFramework.DeviceCenter.Application;
using ZeroFramework.DeviceCenter.Domain;
using ZeroFramework.DeviceCenter.Infrastructure;
[assembly: HostingStartup(typeof(ZeroFramework.DeviceCenter.API.Extensions.Hosting.CustomHostingStartup))]
namespace ZeroFramework.DeviceCenter.API.Extensions.Hosting
{
    public class CustomHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddFluentValidationAutoValidation();

                services.AddDomainLayer();
                services.AddInfrastructureLayer(context.Configuration);
                services.AddApplicationLayer(context.Configuration);
                services.AddWebApiLayer();

                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.TryAdd(nameof(ClaimTypes.Name).ToLower(), ClaimTypes.Name);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options =>
                {
                    options.Authority = context.Configuration.GetValue<string>("IdentityServer:AuthorizationUrl");
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.NameClaimType = ClaimTypes.Name;
                });

                services.AddCors(options =>
                {
                    string[]? allowedOrigins = context.Configuration.GetSection("AllowedOrigins").Get<string[]>();

                    if (allowedOrigins is not null)
                    {
                        options.AddDefaultPolicy(builder => builder.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                    }
                });

                string[] supportedCultures = new[] { "zh-CN", "en-US" };
                services.AddRequestLocalization(options =>
                {
                    options.ApplyCurrentCultureToResponseHeaders = false;
                    options.SetDefaultCulture(supportedCultures.First());
                    options.AddSupportedCultures(supportedCultures);
                    options.AddSupportedUICultures(supportedCultures);
                });

                services.Configure<Infrastructure.ConnectionStrings.TenantStoreOptions>(context.Configuration);

                services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
                {
                    options.ModelBinderProviders.Add(new ModelBinding.SortingBinderProvider());
                });
            });
        }
    }
}
