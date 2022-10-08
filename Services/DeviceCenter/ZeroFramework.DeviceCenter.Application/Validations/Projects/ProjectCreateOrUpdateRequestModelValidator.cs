using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Models.Projects;

namespace ZeroFramework.DeviceCenter.Application.Validations.Projects
{
    public class ProjectCreateOrUpdateRequestModelValidator : AbstractValidator<ProjectCreateOrUpdateRequestModel>
    {
        public ProjectCreateOrUpdateRequestModelValidator(IStringLocalizerFactory factory)
        {
            IStringLocalizer _localizer1 = factory.Create("Models.Projects.ProjectCreateOrUpdateRequestModel", Assembly.GetExecutingAssembly().ToString());
            IStringLocalizer _localizer2 = factory.Create(typeof(ProjectCreateOrUpdateRequestModel));

            RuleFor(m => m.Name).NotNull().NotEmpty();
            RuleFor(m => m.Name).Length(10, 20).WithMessage((m, p) => _localizer1["LengthValidator", _localizer1["ProjectName"], 5, 20, p.Length]);
            RuleFor(m => m.Name).Length(5, 20).WithName(_localizer2["ProjectName"]);
        }
    }
}