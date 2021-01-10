using Microsoft.AspNetCore.Identity;

namespace Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Patronomic { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Path { get; set; }

        public string LFP()
        {
            string lastname = "";
            string firstname = "";
            string patronomic = "";

            if (Lastname != null)
                lastname = Lastname;

            if (Firstname != null)
                firstname = Firstname;

            if (Patronomic != null)
                patronomic = Patronomic;

            return $"{lastname} {firstname[0]}. {patronomic[0]}.";
        }
    }
}
