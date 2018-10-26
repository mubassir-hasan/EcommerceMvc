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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Country

        

        
        public async Task<IActionResult> CountryIndex(string search="")
        {
            List<CountryListViewModel> model=new List<CountryListViewModel>();
            var dbData = await _unitOfWork.Repository<Country>().GetAllIncludeAsync(x => x.Cities);
            if (!String.IsNullOrEmpty(search))
            {
                dbData = dbData.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchString = search;
            }

            foreach (var b in dbData)
            {
                CountryListViewModel country = new CountryListViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    TotalCities = b.Cities.Count
                };
                model.Add(country);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditCountry(int id)
        {
            CountryViewModel model=new CountryViewModel();
            if (id>0)
            {
                Country country = await _unitOfWork.Repository<Country>().GetByIdAsync(id);
                model.Name = country.Name;
            }

            return PartialView("_AddEditCountry",model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCountry(int id, CountryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditCountry", model);
            }

            if (id>0)
            {
                Country country = await _unitOfWork.Repository<Country>().GetByIdAsync(id);
                if (country!=null)
                {
                    country.Name = model.Name;
                    country.ModifiedDate=DateTime.Now;
                    await _unitOfWork.Repository<Country>().UpdateAsync(country);
                }
            }
            else
            {
                Country country = new Country
                {
                    Name = model.Name,
                    ModifiedDate = DateTime.Now,
                    AddedDate = DateTime.Now
                };
                await _unitOfWork.Repository<Country>().InsertAsync(country);
            }
            return RedirectToAction(nameof(CountryIndex));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            Country country = await _unitOfWork.Repository<Country>().GetByIdAsync(id);
            
            return PartialView("_DeleteCountry",country?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCountry(int id, IFormCollection form)
        {
            Country country = await _unitOfWork.Repository<Country>().GetByIdAsync(id);
            if (country!=null)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(country);
            }
            return RedirectToAction(nameof(CountryIndex));
        }
        #endregion
        ////
        //City Methods
        ////

        #region City

        

        
        public async Task<IActionResult> CityIndex(string search = "")
        {
            List<CityListViewModel> model = new List<CityListViewModel>();
            var dbData = await _unitOfWork.Repository<City>().GetAllIncludeAsync(x => x.Country,l=>l.Locations);
            if (!String.IsNullOrEmpty(search))
            {
                dbData = dbData.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchString = search;
            }

            foreach (var b in dbData)
            {
                CityListViewModel city = new CityListViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    TotalLocation = b.Locations?.Count ?? 0,
                    CountryId = b.CountryId,
                    CountryName = b.Country.Name,

                };
                model.Add(city);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditCity(int id)
        {
            CityViewModel model = new CityViewModel
            {
                Countries =  _unitOfWork.Repository<Country>().GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };
            if (id <= 0) return PartialView("_AddEditCity", model);
            City city = await _unitOfWork.Repository<City>().GetByIdAsync(id);
            model.Name = city.Name;
            model.CountryId = city.CountryId;

            return PartialView("_AddEditCity", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCity(int id, CityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditCity", model);
            }

            if (id > 0)
            {
                City city = await _unitOfWork.Repository<City>().GetByIdAsync(id);
                if (city != null)
                {
                    city.Name = model.Name;
                    city.ModifiedDate = DateTime.Now;
                    city.CountryId = model.CountryId;
                    await _unitOfWork.Repository<City>().UpdateAsync(city);
                }
            }
            else
            {
                City city = new City()
                {
                    Name = model.Name,
                    ModifiedDate = DateTime.Now,
                    AddedDate = DateTime.Now,
                    CountryId = model.CountryId
                };
                await _unitOfWork.Repository<City>().InsertAsync(city);
            }
            return RedirectToAction(nameof(CityIndex));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCity(int id)
        {
            City city = await _unitOfWork.Repository<City>().GetByIdAsync(id);

            return PartialView("_DeleteCity", city?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCity(int id, IFormCollection form)
        {
            City city = await _unitOfWork.Repository<City>().GetByIdAsync(id);
            if (city != null)
            {
                await _unitOfWork.Repository<City>().DeleteAsync(city);
            }
            return RedirectToAction(nameof(CityIndex));
        }
        #endregion
        /*
         * 
         Location Methods
         * 
        */

        #region Location

        

        
        public async Task<IActionResult> LocationIndex(string search = "")
        {
            List<LocationListViewModel> model = new List<LocationListViewModel>();

            var dbData = await _unitOfWork.Repository<Location>().GetAllIncludeAsync(c => c.City);
            if (!String.IsNullOrEmpty(search))
            {
                dbData = dbData.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.SearchString = search;
            }
            model=dbData.Select(b=> new LocationListViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Latitude = b.Latitude,
                    Longditude = b.Longditude,
                    Pram = b.Pram,
                    Charge = b.Charge,
                    CityId = b.CityId,
                    CityName = b.City.Name,
                }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditLocation(int id)
        {
            LocationViewModel model = new LocationViewModel
            {
                Countries = _unitOfWork.Repository<Country>().GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()

                }).OrderBy(x => x.Text).ToList(),
            };
            model.Countries.Add(new SelectListItem{Text = "--Select--",Value = "0"});
            if (model.CityId <= 0)
            {
                model.Cities = new List<SelectListItem>();
            }

            if (id > 0)
            {
                Location location = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
                model.Name = location.Name;
                model.Charge = location.Charge;
                model.Pram = location.Pram;
                model.Longditude = location.Longditude;
                model.Latitude = location.Latitude;
            }

            return PartialView("_AddEditLocation", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditLocation(int id, LocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditLocation", model);
            }

            if (id > 0)
            {
                Location location = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
                if (location != null)
                {
                    location.Name = model.Name;
                    location.ModifiedDate = DateTime.Now;
                    location.Charge = model.Charge;
                    location.Pram = model.Pram;
                    location.Longditude = model.Longditude;
                    location.Latitude = model.Latitude;
                    location.CityId = model.CityId;
                    await _unitOfWork.Repository<Location>().UpdateAsync(location);
                }
            }
            else
            {
                Location location = new Location()
                {
                    Name = model.Name,
                    ModifiedDate = DateTime.Now,
                    AddedDate = DateTime.Now,
                    Charge = model.Charge,
                    Pram = model.Pram,
                    Longditude = model.Longditude,
                    Latitude = model.Latitude,
                    CityId = model.CityId
                 };
                await _unitOfWork.Repository<Location>().InsertAsync(location);
            }
            return RedirectToAction(nameof(LocationIndex));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            Location location = await _unitOfWork.Repository<Location>().GetByIdAsync(id);

            return PartialView("_DeleteLocation", location?.Name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLocation(int id, IFormCollection form)
        {
            Location location = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
            if (location != null)
            {
                await _unitOfWork.Repository<Location>().DeleteAsync(location);
            }
            return RedirectToAction(nameof(CityIndex));
        }


        public List<SelectListItem> GetCityList(int id)
        {
            List<SelectListItem> cityList=new List<SelectListItem>();
            cityList.Add(new SelectListItem{Text = "--Select--",Value = "0"});
            
            if (id>0)
            {
                var cities =  _unitOfWork.Repository<City>().GetAll().Where(x => x.CountryId == id).OrderBy(x => x.Name).ToList();
                foreach (var item in cities)
                {
                    cityList.Add(new SelectListItem{Text = item.Name,Value = item.Id.ToString()});
                }

            }
            ViewBag.CityList = cityList;
            return cityList;
        }

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
        public ActionResult GetLocation(int id)
        {
            var ddlCity = _unitOfWork.Repository<Location>().GetAll().Where(x => x.CityId == id).OrderBy(x => x.Name).ToList();
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
        #endregion
    }
}