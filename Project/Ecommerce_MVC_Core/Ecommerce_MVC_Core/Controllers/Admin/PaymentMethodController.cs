using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class PaymentMethodController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodController(
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string search="")
        {

            List<PaymentMethodListViewModel> model = new List<PaymentMethodListViewModel>();
            var dbData = await _unitOfWork.Repository<PaymentMethod>().GetAllAsync();
            if (!String.IsNullOrEmpty(search))
            {
                dbData = dbData.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchString = search;
            }

            foreach (var b in dbData)
            {
                PaymentMethodListViewModel payment = new PaymentMethodListViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Processor = b.Processor,
                };

                model.Add(payment);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditPaymentMethod(int id)
        {
            PaymentMethodViewModel model = new PaymentMethodViewModel();
            if (id > 0)
            {
                PaymentMethod payment = await _unitOfWork.Repository<PaymentMethod>().GetByIdAsync(id);
                model.Name = payment.Name;
                model.Description = payment.Description;
                model.Processor = payment.Processor;
            }

            return PartialView("_AddEditPaymentMethod", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditPaymentMethod(int id, PaymentMethodViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditPaymentMethod", model);
            }

            if (id > 0)
            {
                PaymentMethod payment = await _unitOfWork.Repository<PaymentMethod>().GetByIdAsync(id);
                if (payment != null)
                {
                    payment.Name = model.Name;
                    payment.ModifiedDate = DateTime.Now;
                    payment.Description = model.Description;
                    payment.Processor = model.Processor;
                    await _unitOfWork.Repository<PaymentMethod>().UpdateAsync(payment);
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
                await _unitOfWork.Repository<PaymentMethod>().InsertAsync(payment);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            PaymentMethod payment = await _unitOfWork.Repository<PaymentMethod>().GetByIdAsync(id);

            return PartialView("_DeletePaymentMethod", payment?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePaymentMethod(int id, IFormCollection form)
        {
            PaymentMethod payment = await _unitOfWork.Repository<PaymentMethod>().GetByIdAsync(id);
            if (payment != null)
            {
                await _unitOfWork.Repository<PaymentMethod>().DeleteAsync(payment);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}