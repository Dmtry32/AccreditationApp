//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using AccreditationApp.Models;

//namespace AccreditationApp.Controllers
//{
//    public class AccountController : Controller
//    {
//        [HttpGet]
//        public IActionResult Login(string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
//        {
//            if (ModelState.IsValid)
//            {
//                // For domain authentication, Windows Authentication is already configured
//                // This form is just for demonstration or alternative login
//                return RedirectToLocal(returnUrl);
//            }

//            ViewData["ReturnUrl"] = returnUrl;
//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync();
//            return RedirectToAction("Index", "Home");
//        }

//        private IActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            else
//            {
//                return RedirectToAction("Index", "Home");
//            }
//        }
//    }
//}

//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using System.DirectoryServices.AccountManagement;
//using AccreditationApp.Models;

//namespace AccreditationApp.Controllers
//{
//    [AllowAnonymous]
//    public class AccountController : Controller
//    {
//        private readonly ILogger<AccountController> _logger;

//        public AccountController(ILogger<AccountController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet]
//        public IActionResult Login(string returnUrl = null)
//        {
//            // If user is already authenticated, redirect to home
//            if (User.Identity.IsAuthenticated)
//            {
//                return RedirectToLocal(returnUrl);
//            }

//            ViewData["ReturnUrl"] = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;

//            if (ModelState.IsValid)
//            {
//                // Validate credentials against Windows Domain
//                if (ValidateCredentials(model.Username, model.Password))
//                {
//                    var claims = new List<Claim>
//                    {
//                        new Claim(ClaimTypes.Name, model.Username),
//                        new Claim(ClaimTypes.NameIdentifier, model.Username),
//                        new Claim(ClaimTypes.WindowsAccountName, model.Username)
//                    };

//                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                    var authProperties = new AuthenticationProperties
//                    {
//                        IsPersistent = true, // Remember me option
//                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
//                    };

//                    await HttpContext.SignInAsync(
//                        CookieAuthenticationDefaults.AuthenticationScheme,
//                        new ClaimsPrincipal(claimsIdentity),
//                        authProperties);

//                    _logger.LogInformation("User {Username} logged in at {Time}.", model.Username, DateTime.UtcNow);

//                    return RedirectToLocal(returnUrl);
//                }
//                else
//                {
//                    ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
//                    _logger.LogWarning("Failed login attempt for user {Username}.", model.Username);
//                }
//            }

//            return View(model);
//        }

//        // ===== ADD THE WINDOWS LOGIN ACTION RIGHT HERE =====
//        [HttpPost]
//        public async Task<IActionResult> WindowsLogin(string returnUrl = null)
//        {
//            try
//            {
//                // Try to authenticate using Windows authentication
//                var result = await HttpContext.AuthenticateAsync("Negotiate");

//                if (result.Succeeded && result.Principal.Identity.IsAuthenticated)
//                {
//                    // Create cookie for the Windows-authenticated user
//                    var claims = new List<Claim>
//                    {
//                        new Claim(ClaimTypes.Name, result.Principal.Identity.Name),
//                        new Claim(ClaimTypes.NameIdentifier, result.Principal.Identity.Name),
//                        new Claim(ClaimTypes.WindowsAccountName, result.Principal.Identity.Name)
//                    };

//                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                    var authProperties = new AuthenticationProperties
//                    {
//                        IsPersistent = true,
//                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
//                    };

//                    await HttpContext.SignInAsync(
//                        CookieAuthenticationDefaults.AuthenticationScheme,
//                        new ClaimsPrincipal(claimsIdentity),
//                        authProperties);

//                    _logger.LogInformation("User {Username} logged in via Windows auth at {Time}.",
//                        result.Principal.Identity.Name, DateTime.UtcNow);

//                    return RedirectToLocal(returnUrl);
//                }
//                else
//                {
//                    // Windows auth failed, redirect to login page
//                    return RedirectToAction("Login", new { returnUrl });
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error during Windows authentication");
//                return RedirectToAction("Login", new { returnUrl });
//            }
//        }
//        // ===== END OF WINDOWS LOGIN ACTION =====

//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            _logger.LogInformation("User logged out at {Time}.", DateTime.UtcNow);
//            return RedirectToAction("Login", "Account");
//        }

//        public IActionResult AccessDenied()
//        {
//            return View();
//        }

//        private bool ValidateCredentials(string username, string password)
//        {
//            try
//            {
//                using (var context = new PrincipalContext(ContextType.Domain))
//                {
//                    return context.ValidateCredentials(username, password);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error validating credentials for user {Username}", username);
//                return false;
//            }
//        }

//        private IActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            else
//            {
//                return RedirectToAction("Index", "Home");
//            }
//        }
//    }
//}


// Update for DT.lan domain

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.DirectoryServices.Protocols;
using System.Net;
using AccreditationApp.Models;

namespace AccreditationApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: /Account/Login - This was missing!
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    if (ValidateCredentials(model.Username, model.Password))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(ClaimTypes.NameIdentifier, model.Username),
                            new Claim(ClaimTypes.WindowsAccountName, model.Username)
                        };

                        // Add user roles if available
                        try
                        {
                            var roles = GetUserRoles(model.Username);
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Could not retrieve roles for user {Username}", model.Username);
                        }

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        _logger.LogInformation("User {Username} logged in at {Time}.", model.Username, DateTime.UtcNow);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
                        _logger.LogWarning("Failed login attempt for user {Username}.", model.Username);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during authentication for user {Username}", model.Username);
                    ModelState.AddModelError(string.Empty, "Une erreur technique s'est produite lors de l'authentification.");
                }
            }

            return View(model);
        }

        // GET: /Account/WindowsLogin
        [HttpGet]
        public async Task<IActionResult> WindowsLogin(string returnUrl = null)
        {
            try
            {
                // Try to authenticate using Windows authentication
                var result = await HttpContext.AuthenticateAsync("Negotiate");

                if (result.Succeeded && result.Principal.Identity.IsAuthenticated)
                {
                    // Create cookie for the Windows-authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.Principal.Identity.Name),
                        new Claim(ClaimTypes.NameIdentifier, result.Principal.Identity.Name),
                        new Claim(ClaimTypes.WindowsAccountName, result.Principal.Identity.Name)
                    };

                    // Add roles
                    try
                    {
                        var roles = GetUserRoles(result.Principal.Identity.Name);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not retrieve roles for Windows user {Username}",
                            result.Principal.Identity.Name);
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger.LogInformation("User {Username} logged in via Windows auth at {Time}.",
                        result.Principal.Identity.Name, DateTime.UtcNow);

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    // Windows auth failed, redirect to login page with message
                    TempData["ErrorMessage"] = "L'authentification Windows a échoué. Veuillez saisir vos identifiants manuellement.";
                    return RedirectToAction("Login", new { returnUrl });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Windows authentication");
                TempData["ErrorMessage"] = "Erreur lors de l'authentification Windows. Veuillez saisir vos identifiants manuellement.";
                return RedirectToAction("Login", new { returnUrl });
            }
        }

        // POST: /Account/Logout - This was missing!
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out at {Time}.", DateTime.UtcNow);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied - This was missing!
        public IActionResult AccessDenied()
        {
            return View();
        }

        // ===== MISSING METHODS THAT NEED TO BE ADDED =====

        // ValidateCredentials method - This was missing!
        private bool ValidateCredentials(string username, string password)
        {
            try
            {
                // For development/testing without a domain controller
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    // Allow test credentials in development
                    if (username == "testuser@DT.lan" && password == "testpass")
                    {
                        return true;
                    }
                    if (username == "DT\\testuser" && password == "testpass")
                    {
                        return true;
                    }

                    _logger.LogWarning("Development mode: Using test authentication");
                    return false;
                }

                // For production - get domain from configuration
                var domain = _configuration["DomainSettings:Name"] ?? "DT.lan";

                // Parse username to handle different formats
                string parsedUsername = username;
                string parsedDomain = domain;

                // Handle DOMAIN\username format
                if (username.Contains("\\"))
                {
                    var parts = username.Split('\\');
                    if (parts.Length == 2)
                    {
                        parsedDomain = parts[0];
                        parsedUsername = parts[1];
                    }
                }
                // Handle username@domain format
                else if (username.Contains("@"))
                {
                    var parts = username.Split('@');
                    if (parts.Length == 2)
                    {
                        parsedUsername = parts[0];
                        parsedDomain = parts[1];
                    }
                }

                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(parsedDomain)))
                {
                    var networkCredential = new NetworkCredential(parsedUsername, password, parsedDomain);
                    connection.AuthType = AuthType.Negotiate;
                    connection.Bind(networkCredential);
                    return true;
                }
            }
            catch (LdapException ex)
            {
                _logger.LogError(ex, "LDAP authentication failed for user {Username}", username);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating credentials for user {Username}", username);
                return false;
            }
        }

        // RedirectToLocal method - This was missing!
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // Helper method to get user roles
        private List<string> GetUserRoles(string username)
        {
            var roles = new List<string>();

            try
            {
                // Basic role for all users
                roles.Add("User");

                // Example: if user is administrator in your domain
                if (username.StartsWith("admin", StringComparison.OrdinalIgnoreCase) ||
                    username.Contains("administrateur", StringComparison.OrdinalIgnoreCase))
                {
                    roles.Add("Administrator");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for user {Username}", username);
            }

            return roles;
        }
    }
}

