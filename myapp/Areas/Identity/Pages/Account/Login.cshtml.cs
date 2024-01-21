// Sous licence de la .NET Foundation en vertu d'un ou plusieurs accords.
// La .NET Foundation accorde une licence de ce fichier en vertu de la licence MIT.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace myapp.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
            /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
            /// </summary>
            [Required(ErrorMessage = "Le champ {0} est requis.")]
            [EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide.")]
            [Display(Name = "Email")]

            public string Email { get; set; }

            /// <summary>
            /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
            /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
            /// </summary>
            [Required(ErrorMessage = "Le champ {0} est requis.")]
            [DataType(DataType.Password, ErrorMessage = "Veuillez entrer un mot de passe valide.")]
            [Display(Name = "Mot de passe")]

            public string Password { get; set; }

            /// <summary>
            /// Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
            /// directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
            /// </summary>
            [Display(Name = "Se souvenir de moi ?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Effacer le cookie externe existant pour assurer un processus de connexion propre
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilisateur connecté.");

                    // Remplacez "NomDeVotreAction" et "NomDeVotreController" par les noms réels de votre action et contrôleur de catégories
                    return RedirectToAction("Index", "Categories");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Compte utilisateur verrouillé.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentative de connexion invalide.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
