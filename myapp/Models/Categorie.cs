using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace myapp.Models
{
    public class Categorie
    {
        [Key]
        public int CategorieId { get; set; }

        [Required(ErrorMessage = "Le champ Nom est requis.")]
        public string Nom { get; set; }
        public string ImgUrl { get; set; }
        [Required(ErrorMessage = "Le champ Image est requis.")]
        [NotMapped]
        public IFormFile Image { get; set; }
        public List<Plat> Plats { get; set; }
    }
}
