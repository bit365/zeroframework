using System.ComponentModel.DataAnnotations;

namespace ZeroFramework.IdentityServer.API.Models.Roles
{
    public class RoleUpdateRequestModel
    {
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        public string? DisplayName { get; set; }
    }
}
