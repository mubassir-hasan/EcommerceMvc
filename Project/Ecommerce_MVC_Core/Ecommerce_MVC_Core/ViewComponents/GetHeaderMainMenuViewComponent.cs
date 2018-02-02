using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Ecommerce_MVC_Core.ViewComponents
{
    public class GetHeaderMainMenuViewComponent : ViewComponent
    {
        private readonly IRepository<Category> _repoCategory;
        private readonly IRepository<Product> _repoProduct;
        private ApplicationDbContext _context;

        public GetHeaderMainMenuViewComponent(IRepository<Category> repoCategory, IRepository<Product> repoProduct)
        {
            _repoProduct = repoProduct;
            _repoCategory = repoCategory;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId=0)
        {
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            if (categoryId>0)
            {
                categories = await GetLists(categoryId);
                return View("SubMenu", categories.OrderBy(x=>x.Name).ToList());
            }
            else
            {
                categories = await GetLists();
                return View("MainMenu", categories.OrderBy(x => x.Name).ToList());
            }
            
        }

        private Task<List<CategoryViewModel>> GetLists(int categoryId = 0)
        {
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            if (categoryId>0)
            {
                _repoCategory.GetAll().Where(x => x.CategoryId == categoryId).ToList().ForEach(c =>
                {
                    CategoryViewModel category = new CategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                    };
                   
                        categories.Add(category);
                   
                });
                return Task.Run(() => categories);
            }
            else
            {
                _repoCategory.GetAll().Where(x => x.CategoryId == null).ToList().ForEach(c =>
                {
                    CategoryViewModel category = new CategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                    };
                    
                        categories.Add(category);
                    
                });
                return Task.Run(() => categories);
            }
        }
    }
}