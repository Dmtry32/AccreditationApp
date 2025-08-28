using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AccreditationApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using AccreditationApp.Services;
using System;

namespace AccreditationApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _environment;
        private static readonly Dictionary<string, (string Code, DateTime Expiry)> _verificationCodes = new();

        public AuthController(IEmailService emailService, IWebHostEnvironment environment)
        {
            _emailService = emailService;
            _environment = environment;
        }

        // GET: /Auth/ClientLogin
        [HttpGet]
        public IActionResult ClientLogin()
        {
            return View(new ClientLoginModel());
        }

        // POST: /Auth/ClientLogin
        [HttpPost]
        public async Task<IActionResult> ClientLogin(ClientLoginModel model, string action)
        {
            if (action == "sendCode")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Générer un code de vérification
                        var verificationCode = GenerateVerificationCode();
                        var expiryTime = DateTime.Now.AddMinutes(10);

                        // Stocker le code en mémoire (en production, utilisez une base de données)
                        _verificationCodes[model.Email] = (verificationCode, expiryTime);

                        if (_environment.IsDevelopment())
                        {
                            // Mode développement: afficher le code dans la console
                            Console.WriteLine($"Code de vérification pour {model.Email}: {verificationCode}");
                            model.RequiresVerification = true;
                            TempData["Email"] = model.Email;
                            ViewBag.Message = $"Mode test: Utilisez le code {verificationCode}";
                            return View(model);
                        }
                        else
                        {
                            // Mode production: envoyer par email
                            await _emailService.SendVerificationCodeAsync(model.Email, verificationCode);
                            model.RequiresVerification = true;
                            TempData["Email"] = model.Email;
                            ViewBag.Message = "Code de vérification envoyé par email!";
                            return View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Erreur lors de l'envoi de l'email: " + ex.Message);
                    }
                }
            }
            else if (action == "verifyCode")
            {
                if (!string.IsNullOrEmpty(model.VerificationCode))
                {
                    try
                    {
                        var email = TempData["Email"]?.ToString() ?? model.Email;

                        if (_verificationCodes.TryGetValue(email, out var storedCode))
                        {
                            if (DateTime.Now > storedCode.Expiry)
                            {
                                ModelState.AddModelError("", "Le code a expiré. Veuillez en demander un nouveau.");
                                _verificationCodes.Remove(email);
                            }
                            else if (model.VerificationCode == storedCode.Code)
                            {
                                // Code valide
                                await SignInClient(email);
                                _verificationCodes.Remove(email); // Nettoyer après utilisation
                                return RedirectToAction("Create", "Accreditation");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Code de vérification invalide.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Aucun code de vérification trouvé. Veuillez en demander un nouveau.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Erreur lors de la vérification: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Veuillez entrer le code de vérification.");
                }
            }

            return View(model);
        }

        private string GenerateVerificationCode()
        {
            // Générer un code à 6 chiffres
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SignInClient(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, "Client")
            };

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
        }

        // POST: /Auth/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("ClientLogin");
        }
    }
}