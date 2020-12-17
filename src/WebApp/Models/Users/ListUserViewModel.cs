using System.Collections.Generic;

namespace WebApp.Models.Users
{
    public class ListUserViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
