// Sous licence de la .NET Foundation en vertu d'un ou plusieurs accords.
// La .NET Foundation vous accorde une licence MIT pour ce fichier.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myapp.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserStore<IdentityUser> _userStore;

        public ExternalLoginsModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            string passwordHash = null;
            if (_userStore is IUserPasswordStore<IdentityUser> userPasswordStore)
            {
                passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
            }

            ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                StatusMessage = "La connexion externe n'a pas été supprimée.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "La connexion externe a été supprimée.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Effacez le cookie externe existant pour garantir un processus de connexion propre
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Demandez une redirection vers le fournisseur de connexion externe pour lier une connexion pour l'utilisateur actuel
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            {
                throw new InvalidOperationException($"Une erreur inattendue s'est produite lors du chargement des informations de connexion externe.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                StatusMessage = "La connexion externe n'a pas été ajoutée. Les connexions externes ne peuvent être associées qu'à un compte.";
                return RedirectToPage();
            }

            // Effacez le cookie externe existant pour garantir un processus de connexion propre
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "La connexion externe a été ajoutée.";
            return RedirectToPage();
        }
    }
}
