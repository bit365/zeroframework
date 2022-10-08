using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ZeroFramework.API.Infrastructure.Swagger
{
    public class CamelCaseNamingOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters?.ToList()?.ForEach(op =>
            {
                op.Name = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(op.Name);
            });

            operation.Tags?.ToList()?.ForEach(tag =>
            {
                tag.Name = tag.Name;
            });
        }
    }
}