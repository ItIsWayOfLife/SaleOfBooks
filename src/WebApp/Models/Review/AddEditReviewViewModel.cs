
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Review
{
    public class AddEditReviewViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter review content")]
        [Display(Name = "Review content")]
        public string Content { get; set; }
    }
}
