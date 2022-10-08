using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Users
{
    public class UserCreateRequestModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "User Name", Prompt = "User Name")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [RegularExpression("^[a-z]+$", ErrorMessage = "The {0} must be lowercase letter.")]
        [AllowNull]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        [AllowNull]
        public string Password { get; set; }

        [Required(ErrorMessage = "The {0} field is required."), Phone]
        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "The {0} must be 11 digits.")]
        [AllowNull]
        public string PhoneNumber { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        public string? DisplayName { get; set; }
    }
}