using Core.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using WebApp.Interfaces;

namespace WebApp.Helper
{
    public class CartHelper : ICartHelper
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartHelper(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public int GetCount(string name)
        {
            var user = _userManager.Users.Where(p => p.UserName == name).FirstOrDefault();

            if (user == null)
            {
                return 0;
            }
            else
            {
                return _cartService.GetCount(user.Id);
            }
        }
    }
}
