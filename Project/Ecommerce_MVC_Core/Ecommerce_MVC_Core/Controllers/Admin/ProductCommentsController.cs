using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VaderSharp;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class ProductCommentsController : Controller
    {
        

        private readonly IRepository<Product> _repoProduct;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IRepository<ProductComments> _repoProductComments;

        public ProductCommentsController(IRepository<Product> repoProduct, UserManager<ApplicationUsers> userManager, IRepository<ProductComments> repoProductComments)
        {
            _repoProduct = repoProduct;
            _userManager = userManager;
            _repoProductComments = repoProductComments;
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
            _repoProductComments.GetAll().ToList().ForEach(x =>
            {
                ProductCommentsListViewModel productComments = new ProductCommentsListViewModel
                {
                    Id = x.Id,
                    ModifiedDate = x.ModifiedDate,
                    ProductId = x.ProductId,
                    AddedDate = x.AddedDate,
                    UserId = x.UserId,
                    Comment = x.Comment,
                    ProductName = _repoProduct.GetAll().First(p => p.Id == x.ProductId).Name,
                    
                };
                ApplicationUsers user = _userManager.FindByIdAsync(x.UserId).Result;
                productComments.UserName = user.Name;
                productComments.UserReaction = GetReactionOfComment(x.Comment);
                commentsList.Add(productComments);
            });
            return commentsList;
        }

        [HttpGet]
        public IActionResult DeleteComment(int id)
        {
            ProductComments comments = _repoProductComments.GetById(id);

            return PartialView("_DeleteComment", comments?.Comment);
        }

        [HttpPost]
        public IActionResult DeleteUnit(int id, IFormCollection form)
        {
            ProductComments comments = _repoProductComments.GetById(id);
            if (comments != null)
            {
                _repoProductComments.Delete(comments);
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