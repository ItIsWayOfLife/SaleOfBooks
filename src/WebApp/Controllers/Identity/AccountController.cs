using Core.Constants;
using Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Account;

namespace WebApp.Controllers.Identity
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment appEnvironment,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/register]:[type:get]:[info:registeration]:[user:anonymous]");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

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

                // add user
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // setting cookies
                    await _signInManager.SignInAsync(user, false);

                    _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/register]:[type:post]:[info:registration successful]:[user:{user.Id}]");

                    return RedirectToAction("RegistrationSuccessful");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"[{DateTime.Now.ToString()}]:[account/register]:[type:post]:[error:{error}]:[user:{user.Id}]");

                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/login]:[type:get]:[info:login]:[user:anonymous]");

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = null;

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    user = _userManager.Users.FirstOrDefault(p => p.Email == model.Email);

                    if (user == null)
                    {
                        _logger.LogError($"[{DateTime.Now.ToString()}]:[account/login]:[type:post]:[error:user not found]:[user:anonymous]");

                        return RedirectToAction("Error", "Home", new { requestId = "400" });
                    }


                    _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/login]:[type:post]:[info:login successful]:[user:{user.Id}]");
                    // check if the URL belongs to the application
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}]:[account/login]:[type:post]:[error:wrong login or password]:[user:{user.Id}]");

                    ModelState.AddModelError("", "Wrong login or password");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = null;

                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = _userManager.Users.FirstOrDefault(p => p.Id == currentUserId);

                if (user == null)
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}]:[account/logout]:[type:post]:[error:user not found]:[user:anonymous]");

                    return RedirectToAction("Error", "Home", new { requestId = "400" });
                }

                _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/logout]:[type:post]:[info:logout]:[user:{user.Id}]");

                // delete authentication cookies
                await _signInManager.SignOutAsync();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/logout]:[type:post]:[error:user is not authenticated]:[user:anonymous]");

                return RedirectToAction("Error", "Home", new { requestId = "400" });
            }    
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = null;

                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = _userManager.Users.FirstOrDefault(p => p.Id == currentUserId);

                if (user == null)
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}]:[account/profile]:[type:get]:[error:user not found]:[user:anonymous]");

                    return RedirectToAction("Error", "Home", new { requestId = "400" });
                }

                if (user.Path == "" || user.Path == null)
                {
                    user.Path = PathConstants.PAPH_DEFAULT_PROFILE;
                }

                ProfileViewModel userView = new ProfileViewModel()
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Patronomic = user.Patronomic,
                    Email = user.Email,
                    Postcode = user.Postcode,
                    Address = user.Address,
                    Path = PathConstants.PAPH_USERS + user.Path
                };

                _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/profile]:[type:get]:[info:profile]:[user:{user.Id}]");

                return View(userView);
            }
            else
            {
                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/profile]:[type:get]:[error:user is not authenticated]:[user:anonymous]");

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Edit()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = null;

                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = _userManager.Users.ToList().FirstOrDefault(p => p.Id == currentUserId);

                if (user == null)
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}]:[account/edit]:[type:get]:[error:user not found]:[user:anonymous]");

                    return RedirectToAction("Error", "Home", new { requestId = "400" });
                }

                ProfileViewModel userView = new ProfileViewModel()
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Patronomic = user.Patronomic,
                    Email = user.Email,
                    Postcode = user.Postcode,
                    Address = user.Address
                };

                _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/edit]:[type:get]:[info:edit profile]:[user:{user.Id}]");

                return View(userView);
            }
            else
            {
                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/edit]:[type:get]:[error:user is not authenticated]:[user:anonymous]");

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (User.Identity.IsAuthenticated)
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
                            _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/edit]:[type:post]:[info:edit profile successful]:[user:{user.Id}]");

                            return RedirectToAction("Profile");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/edit]:[type:post]:[error:{error}]:[user:{user.Id}]");

                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
                return View(model);
            }
            else
            {
                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/edit]:[type:post]:[error:user is not authenticated]:[user:anonymous]");

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = null;

                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = _userManager.Users.ToList().FirstOrDefault(p => p.Id == currentUserId);

                if (user == null)
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:get]:[error:user not found]:[user:anonymous]");

                    return RedirectToAction("Error", "Home", new { requestId = "400" });
                }

                ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };

                _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:get]:[info:change password]:[user:{user.Id}]");

                return View(model);
            }
            else
            {
                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:get]:[error:user is not authenticated]:[user:anonymous]");

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:post]:[info:change password successful]:[user:{user.Id}]");

                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:post]:[error:{error}]:[user:{user.Id}]");

                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:post]:[error:user not found]:[user:anonymous]");

                    ModelState.AddModelError(string.Empty, "No such user found");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePath(IFormFile uploadedFile)
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = null;

                string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                user = _userManager.Users.ToList().FirstOrDefault(p => p.Id == currentUserId);

                if (user == null)
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}]:[account/changepath]:[type:post]:[error:user not found]:[user:anonymous]");

                    return RedirectToAction("Error", "Home", new { requestId = "400" });
                }

                string path;

                // save picture
                if (uploadedFile != null)
                {
                    path = uploadedFile.FileName;
                    // save the file to a folder files /provider/ in the catalog wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + PathConstants.PAPH_USERS + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    path = "";
                }

                user.Path = path;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:post]:[error:{error}]:[user:{user.Id}]");

                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/changepassword]:[type:post]:[info:change path successful]:[user:{user.Id}]");
            }

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult RegistrationSuccessful()
        {
            ApplicationUser user = null;

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user = _userManager.Users.FirstOrDefault(p => p.Id == currentUserId);

            if (user == null)
            {
                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/registrationsuccessful]:[type:get]:[error:user not found]:[user:anonymous]");

                _logger.LogError($"[{DateTime.Now.ToString()}]:[account/registrationsuccessful]:[type:get]:[error:user not found]:[user:anonymous]");

                return RedirectToAction("Error", "Home", new { requestId = "400" });
            }

            _logger.LogInformation($"[{DateTime.Now.ToString()}]:[account/registrationsuccessful]:[type:get]:[info:registration successful]:[user:{user.Id}]");

            ViewBag.Name = user.UserName;

            return View();
        }
    }
}
