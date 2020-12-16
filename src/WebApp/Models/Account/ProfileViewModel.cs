using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Account
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Second Name")]
        public string Lastname { get; set; }

        [Display(Name = "Middle Name")]
        public string Patronomic { get; set; }

        [Required(ErrorMessage = "Email address not specified")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the address")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter postal code")]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }
        public string Path { get; set; }
    }
}
