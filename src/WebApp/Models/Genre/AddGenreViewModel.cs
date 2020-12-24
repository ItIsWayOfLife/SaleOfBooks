using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Genre
{
    public class AddGenreViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
