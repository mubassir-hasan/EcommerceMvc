using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_MVC_Core.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<ApplicationRoles> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUsers> _signInManager;

        public UsersController(UserManager<ApplicationUsers> userManager,
            RoleManager<ApplicationRoles> roleManager,
            IUnitOfWork unitOfWork,
            SignInManager<ApplicationUsers> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }

        public IActionResult Index(string search = "")
        {
            IEnumerable<UserListViewModel> model=new List<UserListViewModel>();
            if (!String.IsNullOrEmpty(search))
            {
                model =  GetAllUsers().Where(x => 
                    x.Name.ToLower().Contains(search.ToLower()) ||
                    x.CityName.ToLower().Contains(search.ToLower()) ||
                    x.Email.ToLower().Contains(search.ToLower()) ||
                    x.Contact.ToLower().Contains(search.ToLower()) 
                );
                ViewBag.SearchString = search;
            }

            else
            {
                model = GetAllUsers();
            }
            return View(model);
        }

        public List<UserListViewModel> GetAllUsers()
        {
           List<UserListViewModel> userList = new List<UserListViewModel>();

            userList=  _userManager.Users.Select(u =>
            new UserListViewModel
            {
                Id = u.Id,
                Name = u.Name,
                CityId = u.CityId,
                CityName ="null", //_repoCity.GetAll().First(x => x.Id == u.CityId).Name,
                Contact = u.Contact,
                Gender = u.Gender,
                Email = u.Email
            }).ToList();
            return userList;
        }

        #region SignUp
   
        [HttpGet]
        public IActionResult SignUp()
        {
            UsersViewModel model = new UsersViewModel
            {
                Countries = _unitOfWork.Repository<Country>().GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };
            model.Countries.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            if (model.CityId <= 0)
            {
                model.Cities = new List<SelectListItem>();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UsersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something wrong");
                return View(model);
            }
            ApplicationUsers user=new ApplicationUsers
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                Address = model.Address,
                Refference = model.Refference,
                CityId = model.CityId,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                JoinIp = "1234",
                Contact = model.Contact,
                EmailConfirmed = false,
                
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                ApplicationRoles applicationRoles = await _roleManager.FindByNameAsync("User");
                if (applicationRoles!=null)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRoles.Name);
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                }
            }
            return View(model);
        }
        #endregion

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            LoginViewModel model=new LoginViewModel();
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (model.Email.IndexOf('@') > -1)
            {
                //Validate email format
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            else
            {
                //validate Username format
                string emailRegex = @"^[a-zA-Z0-9]*$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "Username is not valid");
                }
            }

            if (ModelState.IsValid)
            {
                var userName = model.Email;
                if (userName.IndexOf('@') > -1)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                        userName = user.UserName;
                    
                }
                var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    ApplicationUsers user = _userManager.Users.FirstOrDefault(x => x.UserName == userName|| x.Email==userName);

                    if (user != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            if (returnUrl==null)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            return RedirectToLocal(returnUrl);
                        }
                        if (await _userManager.IsInRoleAsync(user, "User"))
                        {
                            return RedirectToLocal(returnUrl);
                        }
                    }

                }
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

        #region Delete User

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUsers applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    name = applicationUser.Name;
                }
            }
            return PartialView("_DeleteUser", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id, IFormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUsers applicationUser = await _userManager.FindByIdAsync(id);

                if (applicationUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }

        #endregion

        #region Logout

        [AllowAnonymous, HttpGet]
        public IActionResult LogOut()
        {
            LogOutViewModel model = new LogOutViewModel
            {
                Error = "Are you Want to Logout?"
            };
            return PartialView("_Logout", model);
        }

        //
        //POST: Account/LogOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut(LogOutViewModel model)
        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Users");
        }

        public class LogOutViewModel
        {
            public string Error { get; set; }
        }

        #endregion
        [HttpGet]
        public ActionResult GetCity(int id)
        {
            var ddlCity = _unitOfWork.Repository<City>().GetAll().Where(x => x.CountryId == id).OrderBy(x => x.Name).ToList();
            List<SelectListItem> cities = new List<SelectListItem>
            {
                new SelectListItem { Text = "--Select State--", Value = "0" }
            };

            foreach (var state in ddlCity)
            {
                cities.Add(new SelectListItem { Text = state.Name, Value = state.Id.ToString() });
            }
            return Json(new SelectList(cities, "Value", "Text"));
        }
        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            ApplicationUsers usr = await GetCurrentUserAsync();
            return usr?.Id;
        }

        private Task<ApplicationUsers> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

    }
}