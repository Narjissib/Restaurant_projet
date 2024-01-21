// Sous licence de la .NET Foundation en vertu d'un ou de plusieurs accords.
// La .NET Foundation vous octroie une licence pour ce fichier en vertu de la licence MIT.
#nullable disable

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace myapp.Areas.Identity.Pages.Account.Manage
{
    public class GenerateRecoveryCodesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> _logger;

        public GenerateRecoveryCodesModel(
            UserManager<IdentityUser> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure d'interface utilisateur par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [TempData]
        public string[] RecoveryCodes { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure d'interface utilisateur par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
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

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException($"Impossible de générer des codes de récupération pour l'utilisateur car ils n'ont pas activé la double authentification (2FA).");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!isTwoFactorEnabled)
            {
                throw new InvalidOperationException($"Impossible de générer des codes de récupération pour l'utilisateur car ils n'ont pas activé la double authentification (2FA).");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            RecoveryCodes = recoveryCodes.ToArray();

            _logger.LogInformation("L'utilisateur avec l'ID '{UserId}' a généré de nouveaux codes de récupération 2FA.", userId);
            StatusMessage = "Vous avez généré de nouveaux codes de récupération.";
            return RedirectToPage("./ShowRecoveryCodes");
        }
    }
}
