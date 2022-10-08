using Microsoft.OpenApi.Models;
using ZeroFramework.API.Infrastructure.Swagger;
using ZeroFramework.DeviceCenter.API.Extensions.Hosting;
using ZeroFramework.DeviceCenter.API.Extensions.Tenants;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddCustomExtensions();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Device Center API", Version = "v1" });

                c.SupportNonNullableReferenceTypes();
                c.UseAllOfToExtendReferenceSchemas();

                c.CustomOperationIds(api =>
                {
                    string? actionName = api.ActionDescriptor.RouteValues["action"];
                    if (actionName is not null)
                    {
                        return $"{System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(actionName)}";
                    }
                    return api.ActionDescriptor.Id;
                });

                string identityServer = Configuration.GetValue<string>("IdentityServer:AuthorizationUrl");

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{identityServer}/connect/authorize"),
                            TokenUrl = new Uri($"{identityServer}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Your user identifier" },
                                { "devicecenter", "Device Center API" }
                            }
                        }
                    }
                });

                c.MapType<IEnumerable<SortingDescriptor>>(() => new OpenApiSchema { Type = "string", Format = "json" });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<CamelCaseNamingOperationFilter>();
            });

            services.AddTenantMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Device Center API v1");
                c.DocumentTitle = "Device Center API Document";
                c.IndexStream = () => GetType().Assembly.GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Infrastructure.Swagger.Index.html");

                c.OAuthClientId("devicecenterswagger");
                c.OAuthClientSecret("secret");
                c.OAuthAppName("Device Center Swagger");
                c.OAuthUsePkce();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseTenantMiddleware();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
