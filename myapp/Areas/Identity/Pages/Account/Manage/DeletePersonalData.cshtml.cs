// Sous licence de la .NET Foundation en vertu d'un ou plusieurs accords.
// La .NET Foundation accorde une licence de ce fichier en vertu de la licence MIT.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace myapp.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
            ///     directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        ///     Cette API prend en charge l'infrastructure UI par défaut d'ASP.NET Core Identity et n'est pas destinée à être utilisée
        ///     directement à partir de votre code. Cette API peut changer ou être supprimée dans les versions futures.
        /// </summary>
        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de charger l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Mot de passe incorrect.");
                    return Page();
                }
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Une erreur inattendue s'est produite lors de la suppression de l'utilisateur.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("L'utilisateur avec l'ID '{UserId}' s'est supprimé.", userId);

            return Redirect("~/");
        }
    }
}
