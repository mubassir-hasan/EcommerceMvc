using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Ecommerce_MVC_Core.Models.Admin;
using Ecommerce_MVC_Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_MVC_Core.Controllers.Admin
{
    public class LocationController : Controller
    {
        private readonly IRepository<Country> _repoCountry;
        private readonly IRepository<City> _repoCity;
        private readonly IRepository<Location> _repoLocation;

        public LocationController(
            IRepository<Country> repoCountry,
            IRepository<City> repoCity,
            IRepository<Location> repoLocation
            )
        {
            _repoCountry = repoCountry;
            _repoCity = repoCity;
            _repoLocation = repoLocation;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Country

        

        
        public IActionResult CountryIndex(string search="")
        {
            List<CountryListViewModel> model=new List<CountryListViewModel>();

            if (!String.IsNullOrEmpty(search))
            {
                _repoCountry.GetAll().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList().ForEach(b =>
                {
                    CountryListViewModel brand = new CountryListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        TotalCities = _repoCity.GetAll().Where(x => x.CountryId == b.Id).ToList().Count
                };

                    model.Add(brand);
                });
                ViewBag.SearchString = search;
            }
            else
            {
                _repoCountry.GetAll().ToList().ForEach(b =>
                {
                    CountryListViewModel brand = new CountryListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                    };
                    brand.TotalCities = _repoCity.GetAll().Where(x=>x.CountryId==b.Id).ToList().Count;
                    model.Add(brand);
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditCountry(int id)
        {
            CountryViewModel model=new CountryViewModel();
            if (id>0)
            {
                Country country = _repoCountry.GetById(id);
                model.Name = country.Name;
            }

            return PartialView("_AddEditCountry",model);
        }

        [HttpPost]
        public IActionResult AddEditCountry(int id, CountryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditCountry", model);
            }

            if (id>0)
            {
                Country country = _repoCountry.GetById(id);
                if (country!=null)
                {
                    country.Name = model.Name;
                    country.ModifiedDate=DateTime.Now;
                    _repoCountry.Update(country);
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
                _repoCountry.Insert(country);
            }
            return RedirectToAction(nameof(CountryIndex));
        }

        [HttpGet]
        public IActionResult DeleteCountry(int id)
        {
            Country country = _repoCountry.GetById(id);
            
            return PartialView("_DeleteCountry",country?.Name);
        }

        [HttpPost]
        public IActionResult DeleteCountry(int id, IFormCollection form)
        {
            Country country = _repoCountry.GetById(id);
            if (country!=null)
            {
                _repoCountry.Delete(country);
            }
            return RedirectToAction(nameof(CountryIndex));
        }
        #endregion
        ////
        //City Methods
        ////

        #region City

        

        
        public IActionResult CityIndex(string search = "")
        {
            List<CityListViewModel> model = new List<CityListViewModel>();

            if (!String.IsNullOrEmpty(search))
            {
                _repoCity.GetAll().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList().ForEach(b =>
                {
                    CityListViewModel city = new CityListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                       CountryId = b.CountryId,
                       CountryName = _repoCountry.GetAll().First(x=>x.Id==b.CountryId).Name,
                        TotalLocation = b.Locations.Count
                    };

                    model.Add(city);
                });
                ViewBag.SearchString = search;
            }
            else
            {
                _repoCity.GetAll().ToList().ForEach(b =>
                {
                    CityListViewModel city = new CityListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        TotalLocation = b.Locations?.Count ?? 0,
                        CountryId = b.CountryId,
                        CountryName = _repoCountry.GetAll().First(x => x.Id == b.CountryId).Name,

                    };

                    model.Add(city);
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditCity(int id)
        {
            CityViewModel model = new CityViewModel
            {
                Countries = _repoCountry.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
            };
            if (id <= 0) return PartialView("_AddEditCity", model);
            City city = _repoCity.GetById(id);
            model.Name = city.Name;
            model.CountryId = city.CountryId;

            return PartialView("_AddEditCity", model);
        }

        [HttpPost]
        public IActionResult AddEditCity(int id, CityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditCity", model);
            }

            if (id > 0)
            {
                City city = _repoCity.GetById(id);
                if (city != null)
                {
                    city.Name = model.Name;
                    city.ModifiedDate = DateTime.Now;
                    city.CountryId = model.CountryId;
                    _repoCity.Update(city);
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
                _repoCity.Insert(city);
            }
            return RedirectToAction(nameof(CityIndex));
        }

        [HttpGet]
        public IActionResult DeleteCity(int id)
        {
            City city = _repoCity.GetById(id);

            return PartialView("_DeleteCity", city?.Name);
        }

        [HttpPost]
        public IActionResult DeleteCity(int id, IFormCollection form)
        {
            City city = _repoCity.GetById(id);
            if (city != null)
            {
                _repoCity.Delete(city);
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

        

        
        public IActionResult LocationIndex(string search = "")
        {
            List<LocationListViewModel> model = new List<LocationListViewModel>();
            

            if (!String.IsNullOrEmpty(search))
            {
                _repoLocation.GetAll().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList().ForEach(b =>
                {
                    LocationListViewModel location = new LocationListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Latitude = b.Latitude,
                        Longditude = b.Longditude,
                        Pram = b.Pram,
                        Charge = b.Charge,
                        CityId = b.CityId,
                        CityName = _repoCity.GetAll().First(x => x.Id == b.CityId).Name,
                    };
                    
                    model.Add(location);
                });
                ViewBag.SearchString = search;
            }
            else
            {
                _repoLocation.GetAll().ToList().ForEach(b =>
                {
                    LocationListViewModel location = new LocationListViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Latitude = b.Latitude,
                        Longditude = b.Longditude,
                        Pram = b.Pram,
                        Charge = b.Charge,
                        CityId = b.CityId,
                        CityName = _repoCity.GetAll().First(x=>x.Id==b.CityId).Name,
                    };

                    model.Add(location);
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddEditLocation(int id)
        {
            LocationViewModel model = new LocationViewModel
            {
                Countries = _repoCountry.GetAll().Select(c => new SelectListItem
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
                Location location = _repoLocation.GetById(id);
                model.Name = location.Name;
                model.Charge = location.Charge;
                model.Pram = location.Pram;
                model.Longditude = location.Longditude;
                model.Latitude = location.Latitude;
            }

            return PartialView("_AddEditLocation", model);
        }

        [HttpPost]
        public IActionResult AddEditLocation(int id, LocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_AddEditLocation", model);
            }

            if (id > 0)
            {
                Location location = _repoLocation.GetById(id);
                if (location != null)
                {
                    location.Name = model.Name;
                    location.ModifiedDate = DateTime.Now;
                    location.Charge = model.Charge;
                    location.Pram = model.Pram;
                    location.Longditude = model.Longditude;
                    location.Latitude = model.Latitude;
                    location.CityId = model.CityId;
                    _repoLocation.Update(location);
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
                _repoLocation.Insert(location);
            }
            return RedirectToAction(nameof(LocationIndex));
        }

        [HttpGet]
        public IActionResult DeleteLocation(int id)
        {
            Location location = _repoLocation.GetById(id);

            return PartialView("_DeleteLocation", location?.Name);
        }

        [HttpPost]
        public IActionResult DeleteLocation(int id, IFormCollection form)
        {
            Location location = _repoLocation.GetById(id);
            if (location != null)
            {
                _repoLocation.Delete(location);
            }
            return RedirectToAction(nameof(CityIndex));
        }


        public List<SelectListItem> GetCityList(int id)
        {
            List<SelectListItem> cityList=new List<SelectListItem>();
            cityList.Add(new SelectListItem{Text = "--Select--",Value = "0"});
            
            if (id>0)
            {
                var cities = _repoCity.GetAll().Where(x => x.CountryId == id).OrderBy(x => x.Name).ToList();
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
            var ddlCity = _repoCity.GetAll().Where(x => x.CountryId == id).OrderBy(x => x.Name).ToList();
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
            var ddlCity = _repoLocation.GetAll().Where(x => x.CityId == id).OrderBy(x => x.Name).ToList();
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