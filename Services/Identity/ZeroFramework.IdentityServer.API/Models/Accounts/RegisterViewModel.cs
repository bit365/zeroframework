using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "User Name", Prompt = "User Name")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [RegularExpression("^[a-z]+$", ErrorMessage = "The {0} must be lowercase letter.")]
        [Remote(action: "VerifyUserName", controller: "Account")]
        [AllowNull]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        [AllowNull]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [AllowNull]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The {0} field is required."), Phone]
        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "The {0} must be 11 digits.")]
        [Remote(action: "VerifyPhoneNumber", controller: "Account")]
        [AllowNull]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Confirmed Code", Prompt = "Confirmed Code")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "The {0} must be 6 digits.")]
        [AllowNull]
        public string ConfirmedCode { get; set; }
    }
}