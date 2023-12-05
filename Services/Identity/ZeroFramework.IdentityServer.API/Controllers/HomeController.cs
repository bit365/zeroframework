using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.IdentityServer.API.Models.Generics;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    public class HomeController(IIdentityServerInteractionService interactionService, IWebHostEnvironment environment) : Controller
    {
        private readonly IIdentityServerInteractionService _interactionService = interactionService;

        private readonly IWebHostEnvironment _environment = environment;

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(string errorId)
        {
            ErrorViewModel errorViewModel = new();

            // retrieve error details from identityserver
            ErrorMessage? errorMessage = await _interactionService.GetErrorContextAsync(errorId);

            if (errorMessage is not null)
            {
                errorViewModel.Error = errorMessage;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    errorMessage.ErrorDescription = null;
                }
            }

            return View("Error", errorViewModel);
        }
    }
}
