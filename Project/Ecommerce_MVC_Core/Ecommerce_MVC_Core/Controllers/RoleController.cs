using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRoles> _roleManager;
        private readonly UserManager<ApplicationUsers> _userManager;

        public RoleController(UserManager<ApplicationUsers> userManager, RoleManager<ApplicationRoles> roleManager)
        {
            this._roleManager = roleManager;

            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<RolesListViewModel> model = new List<RolesListViewModel>();
            model = _roleManager.Roles.Select(r => new RolesListViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                NumberOfUser = 0
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditApplicationRole(string id)
        {
            RolesViewModel model = new RolesViewModel();
           
            
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRoles applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.Name = applicationRole.Name;
                    model.Description = applicationRole.Description;
                    
                }
            }
            return PartialView("_AddEditApplicationRole", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditApplicationRole(string id, RolesViewModel model)
        {

            if (ModelState.IsValid)
            {
                bool isExist = !String.IsNullOrEmpty(id);
                ApplicationRoles applicationRole =
                    isExist ? await _roleManager.FindByIdAsync(id) : new ApplicationRoles
                    {
                        CreatedDate = DateTime.UtcNow
                    };
                
                applicationRole.Name = model.Name;
                applicationRole.Description = model.Description;
                applicationRole.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                IdentityResult roleResult = isExist
                    ? await _roleManager.UpdateAsync(applicationRole)
                    : await _roleManager.CreateAsync(applicationRole);

                string header = String.IsNullOrEmpty(id) ? "Add" : "Edit";
                ViewBag.header = header;

                if (roleResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteApplicationRole(string id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRoles applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    name = applicationRole.Name;
                }
            }
            return PartialView("_DeleteApplicationRole", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApplicationRole(string id, IFormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRoles applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    IdentityResult result = _roleManager.DeleteAsync(applicationRole).Result;
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
    }
}