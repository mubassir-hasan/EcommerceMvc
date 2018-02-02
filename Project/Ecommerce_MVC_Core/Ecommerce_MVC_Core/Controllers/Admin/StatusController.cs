using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class StatusController : Controller
    {

        private IRepository<Status> _repoStatus { get; set; }
        public StatusController(IRepository<Status> repoStatus)
        {
            _repoStatus = repoStatus;
        }

        public IActionResult Index()
        {
            List<StatusViewModel> model=new List<StatusViewModel>();
            _repoStatus.GetAll().ToList().ForEach(s =>
            {
                StatusViewModel status=new StatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ModifiedDate = s.ModifiedDate,
                    AddedDate = s.AddedDate,
                    Description = s.Description,
                    Level = s.Level

                };
                model.Add(status);
            });

            return View(model);
        }

        public IActionResult AddEditStatus(int id=0)
        {
            StatusViewModel model=new StatusViewModel();
            if (id>0)
            {
                Status status = _repoStatus.GetById(id);
                model.Id = status.Id;
                model.Name = status.Name;
                model.Description = status.Description;
                model.Level = status.Level;
            }
            return PartialView("_AddEditStatus",model);
        }

        [HttpPost]
        public IActionResult AddEditStatus(int id, StatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Something Wrong");
                return View(model);
            }
            if (id>0)
            {
                Status status = _repoStatus.GetById(id);
                if (status!=null)
                {
                    status.Name = model.Name;
                    status.Description = model.Description;
                    status.Level = model.Level;
                    status.ModifiedDate=DateTime.Now;
                    _repoStatus.Update(status);
                }
            }
            else
            {
                Status statusUp = new Status();

                statusUp.Name = model.Name;
                statusUp.Description = model.Description;
                statusUp.Level = model.Level;
                statusUp.ModifiedDate = DateTime.Now;
                statusUp.AddedDate = DateTime.Now;
                _repoStatus.Insert(statusUp);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Status status = _repoStatus.GetById(id);

            return PartialView("_DeleteStatus", status?.Name);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection form)
        {
            Status status = _repoStatus.GetById(id); 
            if (status != null)
            {
                _repoStatus.Delete(status);

            }
            return RedirectToAction("Index");
        }

    }
}