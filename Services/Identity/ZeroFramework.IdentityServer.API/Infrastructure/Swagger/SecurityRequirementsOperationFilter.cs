using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ZeroFramework.API.Infrastructure.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Add Security Definitions and Requirements
        /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore#add-security-definitions-and-requirements
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
            bool hasAllowAnonymous = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() == true || context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            if (hasAuthorize && !hasAllowAnonymous)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                OpenApiSecurityScheme oAuthScheme = new()
                {
                    Reference = new() { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new() {
                        [oAuthScheme] =new []{ "openapi" }
                    }
                };
            }
        }
    }
}