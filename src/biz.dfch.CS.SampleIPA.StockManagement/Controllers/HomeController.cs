using biz.dfch.CS.SampleIPA.StockManagement.Models;
using Default;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace biz.dfch.CS.SampleIPA.StockManagement.Controllers
{
    public class HomeController : Controller
    {
        private Container container;

        public HomeController()
        {
            container = new Container(new Uri("https://localhost:44386/odata/"));
        }

        public IActionResult Index()
        {
            var products = container.Products;

            var viewModel = new IndexViewModel
            {
                Products = products.ToList()
            };

            return View(viewModel);
        }
        public IActionResult Create()
        {
            var categories = container.Categories;

            var viewModel = new CreateViewModel
            {
                Categories = categories.ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
