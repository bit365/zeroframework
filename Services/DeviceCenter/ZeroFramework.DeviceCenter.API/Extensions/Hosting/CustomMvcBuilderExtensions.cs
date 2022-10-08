using Microsoft.AspNetCore.Mvc.Razor;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZeroFramework.DeviceCenter.API.Extensions.Hosting
{
    public static class CustomMvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomExtensions(this IMvcBuilder builder)
        {
            builder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(type));

            builder.AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });

            return builder;
        }
    }
}