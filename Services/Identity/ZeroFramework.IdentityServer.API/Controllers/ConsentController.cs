using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.IdentityServer.API.Models.Consents;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    public class ConsentController(ILogger<ConsentController> logger, IIdentityServerInteractionService interactionService) : Controller
    {
        private readonly ILogger<ConsentController> _logger = logger;

        private readonly IIdentityServerInteractionService _interactionService = interactionService;

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            return View(await BuildViewModelAsync(returnUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            ProcessConsentResult processConsentResult = await ProcessConsent(model);

            if (processConsentResult.IsRedirect && processConsentResult.RedirectUri is not null)
            {
                return Redirect(processConsentResult.RedirectUri);
            }

            if (processConsentResult.HasValidationError && processConsentResult.ValidationError is not null)
            {
                ModelState.AddModelError(string.Empty, processConsentResult.ValidationError);
            }

            return View("Index", processConsentResult.ViewModel);
        }

        private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            ProcessConsentResult processConsentResult = new();

            // validate return url is still valid
            AuthorizationRequest? authorizationRequest = await _interactionService.GetAuthorizationContextAsync(model.ReturnUrl);

            if (authorizationRequest is null)
            {
                return processConsentResult;
            }

            ConsentResponse? grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model?.Button?.Equals("no", StringComparison.OrdinalIgnoreCase) == true)
            {
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };
            }
            // user clicked 'yes' - validate the data
            else if (model?.Button?.Equals("yes", StringComparison.OrdinalIgnoreCase) == true)
            {
                // if the user consented to some scope, build the response model
                if (model.ScopesConsented is not null && model.ScopesConsented.Any())
                {
                    IEnumerable<string> scopesConsented = model.ScopesConsented;

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesValuesConsented = scopesConsented,
                        Description = model.Description
                    };
                }
                else
                {
                    processConsentResult.ValidationError = "You must pick at least one permission";
                }
            }
            else
            {
                processConsentResult.ValidationError = "Invalid selection";
            }

            if (grantedConsent is not null)
            {
                // communicate outcome of consent back to identityserver
                await _interactionService.GrantConsentAsync(authorizationRequest, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                processConsentResult.RedirectUri = model?.ReturnUrl ?? string.Empty;
                processConsentResult.Client = authorizationRequest.Client;
            }
            else
            {
                // we need to redisplay the consent UI
                processConsentResult.ViewModel = await BuildViewModelAsync(model?.ReturnUrl ?? string.Empty, model);
            }

            return processConsentResult;
        }

        private async Task<ConsentViewModel?> BuildViewModelAsync(string returnUrl, ConsentInputModel? model = null)
        {
            AuthorizationRequest? authorizationRequest = await _interactionService.GetAuthorizationContextAsync(returnUrl);

            if (authorizationRequest is not null)
            {
                return CreateConsentViewModel(model, returnUrl, authorizationRequest);
            }
            else
            {
                _logger.LogError("No consent request matching request: {returnUrl}", returnUrl);
            }

            return null;
        }

        private ConsentViewModel CreateConsentViewModel(ConsentInputModel? model, string returnUrl, AuthorizationRequest request)
        {
            ConsentViewModel consentViewModel = new()
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),
                Description = model?.Description,

                ReturnUrl = returnUrl,

                ClientName = request.Client.ClientName ?? request.Client.ClientId,
                ClientUrl = request.Client.ClientUri,
                ClientLogoUrl = request.Client.LogoUri,
                AllowRememberConsent = request.Client.AllowRememberConsent
            };

            consentViewModel.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, consentViewModel.ScopesConsented.Contains(x.Name) || model == null)).ToArray();

            List<ScopeViewModel> scopeViewModels = [];

            foreach (ParsedScopeValue parsedScope in request.ValidatedResources.ParsedScopes)
            {
                var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);

                if (apiScope is not null)
                {
                    ScopeViewModel scopeViewModel = CreateScopeViewModel(parsedScope, apiScope, consentViewModel.ScopesConsented.Contains(parsedScope.RawValue) || model == null);
                    scopeViewModels.Add(scopeViewModel);
                }
            }
            if (request.ValidatedResources.Resources.OfflineAccess)
            {
                scopeViewModels.Add(GetOfflineAccessScope(consentViewModel.ScopesConsented.Contains(Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null));
            }
            consentViewModel.ApiScopes = scopeViewModels;

            return consentViewModel;
        }

        private static ScopeViewModel CreateScopeViewModel(IdentityResource identityResource, bool @checked)
        {
            return new ScopeViewModel
            {
                Value = identityResource.Name,
                DisplayName = identityResource.DisplayName ?? identityResource.Name,
                Description = identityResource.Description,
                Emphasize = identityResource.Emphasize,
                Required = identityResource.Required,
                Checked = @checked || identityResource.Required
            };
        }

        public ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool @checked)
        {
            string displayName = apiScope.DisplayName ?? apiScope.Name;

            if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
            {
                displayName += $":{parsedScopeValue.ParsedParameter}";
            }

            return new ScopeViewModel
            {
                Value = parsedScopeValue.RawValue,
                DisplayName = displayName,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = @checked || apiScope.Required
            };
        }

        private static ScopeViewModel GetOfflineAccessScope(bool @checked)
        {
            return new ScopeViewModel
            {
                Value = Duende.IdentityServer.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access",
                Description = "Access to your applications and resources, even when you are offline",
                Emphasize = true,
                Checked = @checked
            };
        }
    }
}
