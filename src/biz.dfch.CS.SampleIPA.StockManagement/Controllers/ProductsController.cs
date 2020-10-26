using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using biz.dfch.CS.SampleIPA.StockManagement.Models;
using Default;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace biz.dfch.CS.SampleIPA.StockManagement.Controllers
{
    public class ProductsController : Controller
    {
        private Container container;
        private IEnumerable<Categories> categories;

        public ProductsController()
        {
            container = new Container(new Uri("https://localhost:44386/odata/"));
            categories = container.Categories.ToList();
        }

        public IActionResult Index()
        {
            var products = container.Products.AddQueryOption("$expand", "Category").ToList();

            var viewModel = new IndexViewModel
            {
                Products = products.ToList()
            };

            return View(viewModel);
        }
        public IActionResult Create()
        {
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
        public IActionResult Create([Bind("Name,MaterialNumber,Quantity,PricePerPiece,WeightInKg")] Products product, [Bind("SelectedItemId")] string selectedItemId)
        {
            var selectedCategory = categories.Where(c => c.Name == selectedItemId).Single();
            product.Category = selectedCategory; 
            
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

        [HttpPost, ActionName(nameof(Delete))]
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

        public IActionResult Book(int id)
        {
            var product = container.Products.Where(p => p.Id == id).Single();

            var bookViewModel = new BookViewModel 
            {
                Id = id,
                Name = product.Name,
                CurrentQuantity = product.Quantity
            };

            return View(bookViewModel);
        }

        [HttpPost, ActionName(nameof(Book))]
        [ValidateAntiForgeryToken]
        public IActionResult Book(int id, BookViewModel bookViewModel)
        {
            if(id != bookViewModel.Id)
            {
                // Wrong
            }

            if(default == bookViewModel.BookingAction) { /* WRONG */ }
           
            var product = container.Products.Where(p => p.Id == id).Single();
           
            if(bookViewModel.BookingAction == "Entfernen")
            {
                if(product.Quantity < bookViewModel.Amount)
                {
                    // Amount zu gross 
                }
                else
                {
                    product.Quantity -= bookViewModel.Amount;
                    bookViewModel.Amount *= -1;
                }
            }
            else
            {
                product.Quantity += bookViewModel.Amount;
            }

            var booking = new Bookings
            {
                Amount = bookViewModel.Amount,
                DataTime = DateTime.Now,
                Product = product
            };

            container.AddToBookings(booking);
            container.UpdateObject(product);
            container.SaveChanges();

            return RedirectToAction(nameof(Index));
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
