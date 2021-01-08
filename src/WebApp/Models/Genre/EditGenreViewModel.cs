using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Genre
{
    public class EditGenreViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
