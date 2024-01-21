using System.ComponentModel.DataAnnotations;

namespace myapp.Models
{
    public class PlatSignature
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du plat est requis.")]
        [Display(Name = "Nom du Plat")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "La description du plat est requise.")]
        [Display(Name = "Description")]
        public string Description { get; set; }


    }

}
