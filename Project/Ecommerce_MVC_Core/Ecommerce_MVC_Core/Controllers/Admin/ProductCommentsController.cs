using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VaderSharp;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class ProductCommentsController : Controller
    {


        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCommentsController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUsers> userManager
        )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index(string serach="")
        {
            List<ProductCommentsListViewModel> model;
            if (String.IsNullOrEmpty(serach))
            {
                model = GetAllComments();
            }
            else
            {
                
                model=(List<ProductCommentsListViewModel>) GetAllComments().Where(x => x.UserName.ToLower().Contains(serach.ToLower()) ||
                                                         x.ProductName.ToLower().Contains(serach.ToLower()) ||
                                                         x.Comment.ToLower().Contains(serach.ToLower())) ;
            }
            return View(model);
        }

        public List<ProductCommentsListViewModel> GetAllComments()
        {
            List<ProductCommentsListViewModel> commentsList=new List<ProductCommentsListViewModel>();
            _unitOfWork.Repository<ProductComments>().GetAllInclude(x=>x.Product).ToList().ForEach(x =>
            {
                ProductCommentsListViewModel productComments = new ProductCommentsListViewModel
                {
                    Id = x.Id,
                    ModifiedDate = x.ModifiedDate,
                    ProductId = x.ProductId,
                    AddedDate = x.AddedDate,
                    UserId = x.UserId,
                    Comment = x.Comment,
                    ProductName = x.Product.Name,
                    
                };
                ApplicationUsers user = _userManager.FindByIdAsync(x.UserId).Result;
                productComments.UserName = user.Name;
                productComments.UserReaction = GetReactionOfComment(x.Comment);
                commentsList.Add(productComments);
            });
            return commentsList;
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id)
        {
            ProductComments comments =await _unitOfWork.Repository<ProductComments>().GetByIdAsync(id);

            return PartialView("_DeleteComment", comments?.Comment);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUnit(int id, IFormCollection form)
        {
            ProductComments comments = await _unitOfWork.Repository<ProductComments>().GetByIdAsync(id);
            if (comments != null)
            {
                await _unitOfWork.Repository<ProductComments>().DeleteAsync(comments);
            }
            return RedirectToAction(nameof(Index));
        }

        public string GetReactionOfComment(string comment)
        {
            SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();

            var results = analyzer.PolarityScores(comment);
            string reaction = "";
            if (results.Positive>results.Compound && results.Positive >= results.Neutral && results.Positive > results.Negative)
            {
                reaction = "Positive " + results.Positive*100 + "%";
            }



            else if (results.Negative > results.Compound && results.Negative > results.Neutral && results.Negative > results.Positive)
            {
                reaction = "Negative ";
            }
            else
            {
                reaction = "Neutral Reaction";
            }
            return reaction;
        }
    }
}