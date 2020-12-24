using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Genre
{
    public class EditGenreViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
