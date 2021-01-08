using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Genre
{
    public class AddGenreViewModel
    {
        [Required(ErrorMessage = "Enter name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
