using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FLP { get; set; }

        [Required(ErrorMessage = "Email not specified")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
    }
}
