// Sous licence de la .NET Foundation en vertu d'un ou plusieurs accords.
// La .NET Foundation vous accorde une licence MIT pour ce fichier.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace myapp.Areas.Identity.Pages.Account.Manage
{
    public class EnableAuthenticatorModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<EnableAuthenticatorModel> _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public EnableAuthenticatorModel(
            UserManager<IdentityUser> userManager,
            ILogger<EnableAuthenticatorModel> logger,
            UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public string SharedKey { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public string AuthenticatorUri { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [TempData]
        public string[] RecoveryCodes { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
            ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
            /// </summary>
            [Required]
            [StringLength(7, ErrorMessage = "Le {0} doit comporter au moins {2} caractères et au maximum {1} caractères.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Code de vérification")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadSharedKeyAndQrCodeUriAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            // Supprimez les espaces et les tirets
            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Le code de vérification est invalide.");
                await LoadSharedKeyAndQrCodeUriAsync(user);
                return Page();
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            var userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("L'utilisateur avec l'ID '{UserId}' a activé l'authentification à deux facteurs avec une application d'authentification.", userId);

            StatusMessage = "Votre application d'authentification a été vérifiée.";

            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                RecoveryCodes = recoveryCodes.ToArray();
                return RedirectToPage("./ShowRecoveryCodes");
            }
            else
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(IdentityUser user)
        {
            // Chargez la clé de l'authentificateur et l'URI du code QR à afficher sur le formulaire
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            SharedKey = FormatKey(unformattedKey);

            var email = await _userManager.GetEmailAsync(user);
            AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Microsoft.AspNetCore.Identity.UI"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}
