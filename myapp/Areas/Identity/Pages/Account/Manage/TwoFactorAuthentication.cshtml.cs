// Sous licence .NET Foundation, conformément à un ou plusieurs accords.
// La .NET Foundation vous octroie une licence pour ce fichier en vertu de la licence MIT.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace myapp.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<TwoFactorAuthenticationModel> _logger;

        public TwoFactorAuthenticationModel(
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<TwoFactorAuthenticationModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut être modifiée ou supprimée dans les versions futures.
        /// </summary>
        public bool HasAuthenticator { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut être modifiée ou supprimée dans les versions futures.
        /// </summary>
        public int RecoveryCodesLeft { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut être modifiée ou supprimée dans les versions futures.
        /// </summary>
        [BindProperty]
        public bool Is2faEnabled { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut être modifiée ou supprimée dans les versions futures.
        /// </summary>
        public bool IsMachineRemembered { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut être modifiée ou supprimée dans les versions futures.
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

            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            await _signInManager.ForgetTwoFactorClientAsync();
            StatusMessage = "Le navigateur actuel a été oublié. Lorsque vous vous reconnecterez à partir de ce navigateur, on vous demandera votre code 2FA.";
            return RedirectToPage();
        }
    }
}
