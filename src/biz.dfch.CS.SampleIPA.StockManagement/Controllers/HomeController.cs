using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using biz.dfch.CS.SampleIPA.StockManagement.Models;
using Default;
using Microsoft.Extensions.Configuration;

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
            var products = container.Product;

            var viewModel = new ProductsViewModel
            {
                Products = products.ToList()
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
