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
    public class PaymentMethodController : Controller
    {
        private readonly IRepository<PaymentMethod> _repoPaymentMethod;

        public PaymentMethodController( IRepository<PaymentMethod> repoPaymentMethod)
        {
            
            _repoPaymentMethod = repoPaymentMethod;
        }

        public IActionResult Index(string search="")
        {

            List<PaymentMethodListViewModel> model = new List<PaymentMethodListViewModel>();

            if (!String.IsNullOrEmpty(search))
            {
                _repoPaymentMethod.GetAll().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList().ForEach(b =>
                {
                    PaymentMethodListViewModel payment = new PaymentMethodListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        Processor = b.Processor,
                    };

                    model.Add(payment);
                });
                ViewBag.SearchString = search;
            }
            else
            {
                _repoPaymentMethod.GetAll().ToList().ForEach(b =>
                {
                    PaymentMethodListViewModel payment = new PaymentMethodListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        Processor = b.Processor,
                    };

                    model.Add(payment);
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditPaymentMethod(int id)
        {
            PaymentMethodViewModel model = new PaymentMethodViewModel();
            if (id > 0)
            {
                PaymentMethod payment = _repoPaymentMethod.GetById(id);
                model.Name = payment.Name;
                model.Description = payment.Description;
                model.Processor = payment.Processor;
            }

            return PartialView("_AddEditPaymentMethod", model);
        }

        [HttpPost]
        public IActionResult AddEditPaymentMethod(int id, PaymentMethodViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditPaymentMethod", model);
            }

            if (id > 0)
            {
                PaymentMethod payment = _repoPaymentMethod.GetById(id);
                if (payment != null)
                {
                    payment.Name = model.Name;
                    payment.ModifiedDate = DateTime.Now;
                    payment.Description = model.Description;
                    payment.Processor = model.Processor;
                    _repoPaymentMethod.Update(payment);
                }
            }
            else
            {
                PaymentMethod payment = new PaymentMethod
                {
                    Name = model.Name,
                    ModifiedDate = DateTime.Now,
                    AddedDate = DateTime.Now,
                    Description = model.Description,
                    Processor = model.Processor
                };
                _repoPaymentMethod.Insert(payment);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DeletePaymentMethod(int id)
        {
            PaymentMethod payment = _repoPaymentMethod.GetById(id);

            return PartialView("_DeletePaymentMethod", payment?.Name);
        }

        [HttpPost]
        public IActionResult DeletePaymentMethod(int id, IFormCollection form)
        {
            PaymentMethod payment = _repoPaymentMethod.GetById(id);
            if (payment != null)
            {
                _repoPaymentMethod.Delete(payment);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}