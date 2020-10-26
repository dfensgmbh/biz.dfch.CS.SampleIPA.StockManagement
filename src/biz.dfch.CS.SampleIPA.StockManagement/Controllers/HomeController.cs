using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using biz.dfch.CS.SampleIPA.StockManagement.Models;
using Default;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var products = container.Products.AddQueryOption("$expand", "Category");

            var viewModel = new IndexViewModel
            {
                Products = products.ToList()
            };

            return View(viewModel);
        }
        public IActionResult Create()
        {
            var categories = container.Categories;

            var categoryNames = new List<string>();
            foreach(var category in categories)
            {
                categoryNames.Add(category.Name);
            }

            var viewModel = new CreateViewModel
            {
                Categories = categoryNames
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,MaterialNumber,Quantity,PricePerPiece,WeightInKg")] Products product)
        {
            container.AddToProducts(product);
            container.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            return GetProductWithCategory(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,PricePerPiece,WeightInKg")] ProductsDto productDto)
        {
            if (id != productDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = container.Products.Where(p => p.Id == id).Single();

                product.Name = productDto.Name;
                product.PricePerPiece = productDto.PricePerPiece;
                product.WeightInKg = productDto.WeightInKg;

                try
                {
                    container.UpdateObject(product);
                    container.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
   
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        public IActionResult Details(int id)
        {
            return GetProductWithCategory(id);
        }

        public IActionResult Delete(int id)
        {
            return GetProductWithCategory(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = container.Products.Where(p => p.Id == id).Single();

            container.DeleteObject(product);

            try
            {
                container.SaveChanges();
            }
            catch(Exception ex)
            {
                if(ex.HResult == -2146233079)
                {
                    // Product has Booking
                }
            }

            return RedirectToAction(nameof(Index));
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

        public IActionResult GetProductWithCategory(int id)
        {
            var products = container.Products.AddQueryOption("$expand", "Category").ToList();
            if(default == products)
            {
                return NotFound();
            }

            var product = products.Where(p => p.Id == id)?.Single();
            if (default == product)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
