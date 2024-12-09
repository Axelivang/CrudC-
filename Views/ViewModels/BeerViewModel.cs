using System.ComponentModel.DataAnnotations;
namespace IntroAsp.Views.ViewModels
{
    public class BeerViewModel
    {

        [Display(Name = "Id")]
        public int BeerId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Marca")]
        public int BrandId { get; set; }




    }
}
