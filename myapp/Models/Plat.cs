using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace myapp.Models
{
    public class Plat
    {
        [Key]
        public int PlatId { get; set; }

        [Required(ErrorMessage = "Le champ description est requis.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le champ prix est requis.")]
        public double Prix { get; set; }

        public string ImgUrl { get; set; }

        [Required(ErrorMessage = "Le champ Image est requis.")]
        [NotMapped]
        public IFormFile Image { get; set; }

        public int CategorieId { get; set; }
        [ForeignKey("CategorieId")]
        public Categorie Categorie { get; set; }


    }
}
