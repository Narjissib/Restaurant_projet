// Autorisé à la .NET Foundation en vertu d'une ou plusieurs ententes.
// La .NET Foundation vous accorde une licence pour ce fichier en vertu de la licence MIT.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myapp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure par défaut de l'interface utilisateur ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement depuis votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public string Username { get; set; }

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
            [Phone]
            [Display(Name = "Numéro de téléphone")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
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

        public async Task<IActionResult> OnPostAsync()
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

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Erreur inattendue lors de la tentative de configuration du numéro de téléphone.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Votre profil a été mis à jour";
            return RedirectToPage();
        }
    }
}
