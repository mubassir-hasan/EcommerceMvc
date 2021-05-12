using Ecommerce_MVC_Core.Controllers.Admin;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.Repository;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.UnitTest.Controllers
{
    
    public class BrandControllerTest
    {
        private Mock<IRepository<Brand>> mockBrandRepo;
        private Mock<IUnitOfWork> mockUOW;
        public BrandControllerTest()
        {
            mockBrandRepo = new Mock<IRepository<Brand>>();
            mockUOW = new Mock<IUnitOfWork>();

        }

        private IList<Brand> GetListOfBrands()
        {
            var brands= new List<Brand>
            {
                new Brand{Id=1,Name="HP",Description="Test Description",Products=new List<Product>()},
                new Brand{Id=2,Name="ACER",Description="Test Description",Products=new List<Product>()}
            };
            return brands;
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithListOfBrands()
        {
            // Arrange
            var brands = GetListOfBrands();
            
            mockBrandRepo.Setup(x => x.GetAllIncludeAsync(It.IsAny<Expression<Func<Brand, object>>[]>())).ReturnsAsync(brands);
            
            mockUOW.Setup(x => x.Repository<Brand>()).Returns(mockBrandRepo.Object);
            
            var controller = new BrandController(mockUOW.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<BrandListViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);

        }

        [Fact]
        public async Task AddEditBrand_ReturnsPartialView_WithSingleBrand()
        {
            // Arrange
            var brand = GetListOfBrands().First(x => x.Id == 1);

            mockBrandRepo.Setup(x => x.GetByIdAsync(It.IsAny<int?>())).ReturnsAsync(brand);

            mockUOW.Setup(x => x.Repository<Brand>()).Returns(mockBrandRepo.Object);

            var controller = new BrandController(mockUOW.Object);

            // Act
            var result = await controller.AddEditBrand(1);

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            var model = Assert.IsAssignableFrom<BrandViewModel>(viewResult.ViewData.Model);
            Assert.Equal(brand.Name, model.Name);

        }

        [Fact]
        public async Task AddEditBrandPOST_ReturnsEmptyModelWithError_WhenModelStateInvalid()
        {
            // Arrange
            var brand = new BrandViewModel { };
            
            //mockBrandRepo.Setup(x => x.GetByIdAsync(It.IsAny<int?>())).ReturnsAsync(brand);

            mockUOW.Setup(x => x.Repository<Brand>()).Returns(mockBrandRepo.Object);

            var controller = new BrandController(mockUOW.Object);
            controller.ModelState.AddModelError("Name", "Required");
            // Act
            var result = await controller.AddEditBrand(0,brand);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BrandViewModel>(viewResult.ViewData.Model);
            Assert.Null(model.Name);
            Assert.False(viewResult.ViewData.ModelState.IsValid);

        }

        [Fact]
        public async Task AddEditBrandPost_ReturnsARedirectAndAddBrand_WhenBrandIsValid()
        {
            // Arrange
            var brand = new BrandViewModel {Name="Apple" };

            mockBrandRepo.Setup(x => x.InsertAsync(It.IsAny<Brand>())).Returns(Task.FromResult<Brand>(new Brand())).Verifiable();

            mockUOW.Setup(x => x.Repository<Brand>()).Returns(mockBrandRepo.Object);

            var controller = new BrandController(mockUOW.Object);

            // Act
            var result = await controller.AddEditBrand(0,brand);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
            mockBrandRepo.Verify();
        }

        [Fact]
        public async Task AddEditBrandPost_ReturnsARedirectAndUpdateBrand_WhenBrandIsValid()
        {
            // Arrange
            var brandId = 1;
            var brand = new BrandViewModel {Id=brandId, Name = "Apple" };
            mockBrandRepo.Setup(x => x.Update(It.IsAny<Brand>())).Verifiable();

            mockUOW.Setup(x => x.Repository<Brand>()).Returns(mockBrandRepo.Object);

            var controller = new BrandController(mockUOW.Object);

            // Act
            var result = await controller.AddEditBrand(brandId, brand);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
            mockBrandRepo.Verify();
        }

        [Fact]
        public async Task Delete_ReturnPartialViewAndBrandName_ByPassingBrandId()
        {
            // Arrange
            var brandId = 1;
            var brand = GetListOfBrands().First(x=>x.Id==brandId);
            mockBrandRepo.Setup(x => x.GetByIdAsync(It.IsAny<int?>())).ReturnsAsync(brand);

            mockUOW.Setup(x => x.Repository<Brand>()).Returns(mockBrandRepo.Object);

            var controller = new BrandController(mockUOW.Object);

            // Act
            var result = await controller.Delete(brandId);

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result); 
            var model = Assert.IsAssignableFrom<string>(viewResult.ViewData.Model);
            Assert.Equal(brand.Name, model);
        }


        [Fact]
        public async Task DeletePost_RedirectAndDeleteBrand_ByPassingBrandId()
        {
            // Arrange
            var brandId = 1;
            var brand = GetListOfBrands().First(x => x.Id == brandId);
            mockBrandRepo.Setup(x => x.GetByIdAsync(It.IsAny<int?>())).ReturnsAsync(brand);
            mockBrandRepo.Setup(x => x.DeleteAsync(It.IsAny<Brand>())).Returns(Task.FromResult(It.IsAny<int>())).Verifiable();
            
            mockUOW.Setup(x => x.Repository<Brand>()).Returns(mockBrandRepo.Object);

            var controller = new BrandController(mockUOW.Object);

            // Act
            var result = await controller.Delete(brandId,It.IsAny<IFormCollection>());

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
            mockBrandRepo.Verify();
        }
    }
}
