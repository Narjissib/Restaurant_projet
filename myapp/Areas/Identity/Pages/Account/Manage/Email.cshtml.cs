// Sous licence de la .NET Foundation en vertu d'un ou plusieurs accords.
// La .NET Foundation vous accorde une licence MIT pour ce fichier.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace myapp.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

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
            [EmailAddress]
            [Display(Name = "Nouvel e-mail")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Input.NewEmail,
                    "Confirmez votre e-mail",
                    $"Veuillez confirmer votre compte en <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>cliquant ici</a>.");

                StatusMessage = "Lien de confirmation pour changer l'e-mail envoyé. Veuillez vérifier votre e-mail.";
                return RedirectToPage();
            }

            StatusMessage = "Votre e-mail n'a pas changé.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirmez votre e-mail",
                $"Veuillez confirmer votre compte en <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>cliquant ici</a>.");

            StatusMessage = "E-mail de vérification envoyé. Veuillez vérifier votre e-mail.";
            return RedirectToPage();
        }
    }
}
