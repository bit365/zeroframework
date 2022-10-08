using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Roles
{
    public class RoleCreateRequestModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [RegularExpression("^[a-z]+$", ErrorMessage = "The {0} must be lowercase letter.")]
        [AllowNull]
        public string RoleName { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        public string? DisplayName { get; set; }
    }
}