using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Accounts;
using ZeroFramework.IdentityServer.API.Services;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IDistributedCache _distributedCache;

        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IStringLocalizerFactory _localizerFactory;
        private readonly ITenantProvider _tenantProvider;
        private readonly ICurrentTenant _currentTenant;

        private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISmsSender smsSender, ILoggerFactory loggerFactory, IIdentityServerInteractionService interactionService, IDistributedCache distributedCache, IStringLocalizerFactory localizerFactory, ITenantProvider tenantProvider, ICurrentTenant currentTenant, IAuthenticationHandlerProvider authenticationHandlerProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _interactionService = interactionService;
            _distributedCache = distributedCache;
            _localizerFactory = localizerFactory;
            _tenantProvider = tenantProvider;
            _currentTenant = currentTenant;
            _authenticationHandlerProvider = authenticationHandlerProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            var authorizationRequest = await _interactionService.GetAuthorizationContextAsync(returnUrl);

            _logger.LogInformation("The current tenant id or name {Tenant}", authorizationRequest?.Tenant);

            LoginViewModel? loginViewModel = null;

            if (authorizationRequest?.Client?.ClientId is not null)
            {
                loginViewModel = new LoginViewModel { UserName = authorizationRequest.LoginHint };
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View(loginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                IdentityTenant? identityTenant = await _tenantProvider.GetTenantAsync();

                string currentUserName = model.UserName;

                string pattern = @"^(?<tenantUserName>[a-zA-Z0-9]+)@(?<tenantName>[a-zA-Z0-9]+)$";
                Match match = Regex.Match(model.UserName, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string tenantName = match.Groups["tenantName"].Value;
                    identityTenant = await _tenantProvider.FindTenantAsync(tenantName);
                }

                if (!match.Success && identityTenant is not null)
                {
                    currentUserName += $"@{identityTenant.Name}";
                }

                using (_currentTenant.Change(identityTenant?.Id, identityTenant?.Name))
                {
                    ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.UserName == currentUserName || u.PhoneNumber == currentUserName);

                    var _localizer = _localizerFactory.Create(typeof(LoginViewModel));

                    if (user is null)
                    {
                        ModelState.AddModelError(nameof(user.UserName), _localizer["Invalid userid or password."].Value);
                        return View(model);
                    }

                    var loginResult = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (loginResult.Succeeded)
                    {
                        // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                        if (_interactionService.IsValidReturnUrl(returnUrl))
                        {
                            return Redirect(returnUrl ?? string.Empty);
                        }

                        return RedirectToLocal(returnUrl);
                    }

                    if (loginResult.RequiresTwoFactor)
                    {
                        return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, model.RememberMe });
                    }

                    if (loginResult.IsLockedOut)
                    {
                        return View("Lockout");
                    }

                    ModelState.AddModelError(string.Empty, _localizer["Invalid userid or password."].Value);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.ConfirmedCode) || await _distributedCache.GetStringAsync(model.PhoneNumber) != model.ConfirmedCode)
                {
                    var _localizer = _localizerFactory.Create(typeof(RegisterViewModel));
                    ModelState.AddModelError(nameof(model.ConfirmedCode), _localizer["Invalid confirmed code."].Value);
                    return View(model);
                }

                IdentityTenant? identityTenant = await _tenantProvider.GetTenantAsync();

                var user = new ApplicationUser { UserName = model.UserName, PhoneNumber = model.PhoneNumber, TenantId = identityTenant?.Id };
                var registerResult = await _userManager.CreateAsync(user, model.Password);

                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);

                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();

                if (registerResult.Succeeded && info is not null)
                {
                    registerResult = await _userManager.AddLoginAsync(user, info);
                    if (registerResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);

                        // Update any authentication tokens as well
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                        return RedirectToLocal(returnUrl);
                    }
                }

                registerResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            //Test for Xamarin. 
            LogoutRequest logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            if (logoutRequest?.ShowSignoutPrompt == false)
            {
                //it's safe to automatically sign-out
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            LogoutViewModel logoutViewModel = new() { LogoutId = logoutId };

            return View(logoutViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            string? idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp is not null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                // if there's no current logout context, we need to create one
                // this captures necessary info from the current logged in user
                // before we signout and redirect away to the external IdP for signout
                model.LogoutId ??= await _interactionService.CreateLogoutContextAsync();

                string? url = Url.Action("Logout", new { model.LogoutId });

                if (await _authenticationHandlerProvider.GetHandlerAsync(HttpContext, idp) is IAuthenticationSignOutHandler)
                {
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties { RedirectUri = url });
                }
            }

            // delete authentication cookie
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            LogoutRequest logoutRequest = await _interactionService.GetLogoutContextAsync(model.LogoutId);

            return logoutRequest?.PostLogoutRedirectUri is null ? RedirectToLocal(null) : Redirect(logoutRequest?.PostLogoutRedirectUri ?? string.Empty);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = "~/")
        {
            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && !_interactionService.IsValidReturnUrl(returnUrl))
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return url");
            }

            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                return RedirectToAction(nameof(Login));
            }

            ApplicationUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user is not null)
            {
                await _signInManager.SignInAsync(user, true);
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded && returnUrl is not null && user is not null)
            {
                // Update any authentication tokens if login succeeded
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);

                return RedirectToLocal(returnUrl);
            }

            return View(viewName: nameof(Register));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Phone number does not exist.");
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.ConfirmedCode) || await _distributedCache.GetStringAsync(model.PhoneNumber) != model.ConfirmedCode)
                {
                    ModelState.AddModelError(nameof(model.ConfirmedCode), "Invalid confirmed code.");
                    return View(model);
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var identityResult = await _userManager.ResetPasswordAsync(user, code, model.Password);

                if (identityResult.Succeeded)
                {
                    return RedirectToLocal(null);
                }

                identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendCode([System.ComponentModel.DataAnnotations.Phone] string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Generate the token and send it

            string? code = await _distributedCache.GetStringAsync(phoneNumber);

            if (string.IsNullOrWhiteSpace(code))
            {
                Random random = new((int)DateTime.Now.Ticks);
                code = random.Next(100000, 999999).ToString();
            }

            await _distributedCache.SetStringAsync(phoneNumber, code, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            await _smsSender.SendSmsAsync(phoneNumber, code);

            return Ok();
        }

        #region Helpers

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion

        [AcceptVerbs("GET", "POST"), AllowAnonymous]
        public IActionResult VerifyPhoneNumber(string phoneNumber)
        {
            var _localizer = _localizerFactory.Create(typeof(RegisterViewModel));

            if (_userManager.Users.Any(u => u.PhoneNumber == phoneNumber))
            {
                return Json(_localizer["Phone number is already in use."].Value);
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST"), AllowAnonymous]
        public IActionResult VerifyUserName(string userName)
        {
            var _localizer = _localizerFactory.Create(typeof(RegisterViewModel));

            if (_userManager.Users.Any(u => u.UserName == userName))
            {
                return Json(_localizer["User name is already in use."].Value);
            }

            return Json(true);
        }
    }
}