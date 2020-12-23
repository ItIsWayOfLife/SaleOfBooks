using Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Users;


namespace Web.Controllers.Identity
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string searchSelectionString, string seacrhString = "")
        {
            var listUsers = _userManager.Users;
            var listViewUsers = new List<UserViewModel>();

            foreach (var listUser in listUsers)
            {
                listViewUsers.Add(
                    new UserViewModel()
                    {
                        Id = listUser.Id,
                        Email = listUser.Email,
                        FLP = $"{listUser.Lastname} {listUser.Firstname} {listUser.Patronomic}",
                        Address = listUser.Address,
                        Postcode = listUser.Postcode
                    });
            }

            List<string> searchSelection = new List<string>() { "Search by", "Id", "Email", "Full name" };

            if (seacrhString == null)
            {
                seacrhString = "";
            }

            if (searchSelectionString == searchSelection[1])
            {
                listViewUsers = listViewUsers.Where(e => e.Id.ToLower().Contains(seacrhString.ToLower())).ToList();
            }
            else if (searchSelectionString == searchSelection[2])
            {
                listViewUsers = listViewUsers.Where(e => e.Email.ToLower().Contains(seacrhString.ToLower())).ToList();
            }
            else if (searchSelectionString == searchSelection[3])
            {
                listViewUsers = listViewUsers.Where(e => e.FLP.ToLower().Contains(seacrhString.ToLower())).ToList();
            }

            return View(new UserFilterListViewModel()
            {
                ListUsers = new ListUserViewModel { Users = listViewUsers },
                SearchSelection = new SelectList(searchSelection),
                SeacrhString = seacrhString,
                SearchSelectionString = searchSelectionString
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Patronomic = model.Patronomic,
                    Postcode = model.Postcode,
                    Address = model.Address
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { requestId = "400" });
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Patronomic = user.Patronomic,
                Address = user.Address,
                Postcode = user.Postcode
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.Firstname = model.Firstname;
                    user.Lastname = model.Lastname;
                    user.Patronomic = model.Patronomic;
                    user.Address = model.Address;
                    user.Postcode = model.Postcode;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { requestId = "400" });
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(model.Id);

                    if (user != null)
                    {
                        IdentityResult result =
                            await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (result.Succeeded)
                        {
                            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User is not found");
                    }
                }
            }
            catch (Core.Exceptions.ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }

            return View(model);
        }
    }
}
