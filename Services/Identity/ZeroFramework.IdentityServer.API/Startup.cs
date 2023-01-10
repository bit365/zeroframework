using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using ZeroFramework.API.Infrastructure.Swagger;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.Extensions;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Infrastructure.Aliyun;
using ZeroFramework.IdentityServer.API.Infrastructure.Authentication.Microsoft;
using ZeroFramework.IdentityServer.API.Models.Generics;
using ZeroFramework.IdentityServer.API.Services;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                options.Filters.Add<HttpResponseExceptionFilter>();
            }).AddViewLocalization().AddDataAnnotationsLocalization();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()).AddFluentValidationAutoValidation();

            bool isDemoMode = Convert.ToBoolean(Configuration.GetRequiredSection("UseDemoLaunchMode").Value);

            services.AddDbContext<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });

                if (isDemoMode)
                {
                    optionsBuilder.AddInterceptors(new DisalbeModifiedDeletedSaveChangesInterceptor());
                }

                ICurrentTenant currentTenant = serviceProvider.GetRequiredService<ICurrentTenant>();
                optionsBuilder.AddInterceptors(new CustomSaveChangesInterceptor(currentTenant), new CustomDbCommandInterceptor());
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz@01234567890";
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders().AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();

            services.AddIdentityServer().AddAspNetIdentity<ApplicationUser>()
                .AddSigningCredential(Certificates.Certificate.Get())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString("Default"), options =>
                    {
                        options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString("Default"), options =>
                    {
                        options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                }).AddProfileService<IdentityProfileService>();

            services.AddTransient<IEmailSender, AuthMessageSender>().AddTransient<ISmsSender, AuthMessageSender>();

            services.AddScoped<CustomCookieAuthenticationEvents>().AddScoped<CustomOAuthAuthenticationEvents>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.TryAdd(JwtClaimTypes.Name, ClaimTypes.Name);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.Authority = Configuration["IdentityServer:AuthorizationUrl"];
                o.TokenValidationParameters.ValidateAudience = false;
                o.TokenValidationParameters.NameClaimType = ClaimTypes.Name;
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"]!;
                microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"]!;
                microsoftOptions.EventsType = typeof(CustomOAuthAuthenticationEvents);
                //.AspNetCore.Correlation. state property not found
                microsoftOptions.RemoteAuthenticationTimeout = TimeSpan.FromDays(15);
                microsoftOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
            }).AddQQ(qqOptions =>
            {
                qqOptions.ClientId = Configuration["Authentication:TencentQQ:AppID"]!;
                qqOptions.ClientSecret = Configuration["Authentication:TencentQQ:AppKey"]!;
                qqOptions.EventsType = typeof(CustomOAuthAuthenticationEvents);
                qqOptions.RemoteAuthenticationTimeout = TimeSpan.FromDays(15);
                qqOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
            }).AddGitHub(gitHubOptions =>
            {
                gitHubOptions.ClientId = Configuration["Authentication:GitHub:ClientID"]!;
                gitHubOptions.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"]!;
                gitHubOptions.EventsType = typeof(CustomOAuthAuthenticationEvents);
                gitHubOptions.RemoteAuthenticationTimeout = TimeSpan.FromDays(15);
                gitHubOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
            }).AddWeibo("Weibo", "Î¢²©", weiboOptions =>
            {
                weiboOptions.ClientId = Configuration["Authentication:Weibo:AppKey"]!;
                weiboOptions.ClientSecret = Configuration["Authentication:Weibo:AppSecret"]!;
                weiboOptions.EventsType = typeof(CustomOAuthAuthenticationEvents);
                weiboOptions.UserEmailsEndpoint = string.Empty;
                weiboOptions.RemoteAuthenticationTimeout = TimeSpan.FromDays(15);
                weiboOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
            }).AddWeixin("WeChat", "Î¢ÐÅ", weChatOptions =>
            {
                weChatOptions.ClientId = Configuration["Authentication:WeChat:AppID"]!;
                weChatOptions.ClientSecret = Configuration["Authentication:WeChat:AppSecret"]!;
                weChatOptions.EventsType = typeof(CustomOAuthAuthenticationEvents);
                weChatOptions.RemoteAuthenticationTimeout = TimeSpan.FromDays(15);
                weChatOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
            });

            services.Configure<AlibabaCloudOptions>(Configuration.GetSection("AlibabaCloud"));
            services.AddTransient<AliyunAuthHandler>();
            services.AddHttpClient("aliyun").AddHttpMessageHandler<AliyunAuthHandler>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer API", Version = "v1" });

                c.SupportNonNullableReferenceTypes();

                c.CustomOperationIds(api =>
                {
                    string? actionName = api.ActionDescriptor.RouteValues["action"];
                    if (actionName is not null)
                    {
                        return $"{System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(actionName)}";
                    }
                    return api.ActionDescriptor.Id;
                });

                string? identityServer = Configuration.GetValue<string>("IdentityServer:AuthorizationUrl");

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
                                { "identityserver", "Identity server api" }
                            }
                        }
                    }
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.OperationFilter<CamelCaseNamingOperationFilter>();

                c.MapType<IEnumerable<SortingDescriptor>>(() => new OpenApiSchema { Type = "string", Format = "json" });
            });

            services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizeConstants.TenantOwnerPolicyName, policy => policy.AddRequirements(new OperationAuthorizationRequirement()));
            });

            services.AddCors(options =>
            {
                string[]? allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<string[]>();
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
                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                {
                    if (context.Request.Headers.TryGetValue("Culture", out var values))
                    {
                        return await Task.FromResult(new ProviderCultureResult(values.First()));
                    }
                    return null;
                }));
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            ValidatorOptions.Global.DisplayNameResolver = (t, m, l) => m.GetCustomAttribute<DisplayAttribute>()?.Name ?? m.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? m.Name;

            services.AddTenantMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            //Configure ASP.NET Core to work with proxy servers and load balancers
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

            app.UseAuthentication();
            app.UseTenantMiddleware();

            app.UseRequestLocalization();

            SampleDataSeed.SeedAsync(app).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer API v1");
                c.DocumentTitle = "IdentityServer API Document";
                c.IndexStream = () => GetType().Assembly.GetManifestResourceStream($"{GetType().Assembly.GetName().Name}.Infrastructure.Swagger.Index.html");

                c.OAuthClientId("identityserverswagger");
                c.OAuthClientSecret("secret");
                c.OAuthAppName("Identity Server Swagger");
                c.OAuthUsePkce();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseIdentityServer();
            // Fix a problem with chrome. Chrome enabled a new feature "Cookies without SameSite must be secure", 
            // the coockies shold be expided from https, but in eShop, the internal comunicacion in aks and docker compose is http.
            // To avoid this problem, the policy of cookies shold be in Lax mode.
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
            app.UseRouting();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();
        }
    }
}