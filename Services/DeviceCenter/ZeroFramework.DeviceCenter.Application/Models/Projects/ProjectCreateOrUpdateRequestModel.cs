using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Projects
{
    /// <summary>
    /// https://github.com/dotnet/runtime/blob/master/src/libraries/System.ComponentModel.Annotations/src/Resources/Strings.resx
    /// https://github.com/dotnet/runtime/issues/24084
    /// https://stackoverflow.com/questions/48769199/localization-of-requiredattribute-in-asp-net-core-2-0
    /// https://github.com/Der-Kraken/AspNetCore.Docs/tree/238bfe13e0dc83d1b68102894a6753f71fb4d628/aspnetcore/tutorials/first-mvc-app/start-mvc/sample/MvcMovie22
    /// </summary>
    public class ProjectCreateOrUpdateRequestModel
    {
        public int Id { get; set; }

        [StringLength(8, ErrorMessage = "The {0} must be at least {2} characte long.", MinimumLength = 6)]
        [System.ComponentModel.DisplayName("ProjectName")]
        [AllowNull]
        public string Name { get; set; }

        [Display(Name = nameof(CreationTime))]
        public DateTimeOffset CreationTime { get; set; }
    }
}