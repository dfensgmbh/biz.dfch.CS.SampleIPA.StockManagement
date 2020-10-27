﻿using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using biz.dfch.CS.SampleIPA.StockManagement.Models;
using Default;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public IActionResult Create(Products product, [Bind("SelectedCategoryName")] string selectedCategoryName)
        {
            var selectedCategory = categories.Where(c => c.Name == selectedCategoryName).Single();
            product.Category = selectedCategory;
            product.CategoryId = selectedCategory.Id;
            
            container.AddToProducts(product);
            container.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var categoryNames = new List<string>();
            foreach (var category in categories)
            {
                categoryNames.Add(category.Name);
            }

            var productWithCategory = GetProductWithCategory(id);
            if (default == productWithCategory)
            {
                return NotFound();
            }

            var editViewModel = new EditViewModel
            {
                Product = productWithCategory,
                Categories = categoryNames
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditViewModel editViewModel)
        {
            if (id != editViewModel.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = container.Products.Where(p => p.Id == id).Single();
                var category = categories.Where(c => c.Name == editViewModel.SelectedCategoryName).Single();

                product.Name = editViewModel.Product.Name;
                product.PricePerPiece = editViewModel.Product.PricePerPiece;
                product.WeightInKg = editViewModel.Product.WeightInKg;
                product.Category = category;
                product.CategoryId = category.Id;

                try
                {
                    container.UpdateObject(product);
                    container.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Conflict();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editViewModel);
        }

        public IActionResult Details(int id)
        {
            var productWithCategory = GetProductWithCategory(id);

            var bookingsWithProduct = container.Bookings.AddQueryOption("Expand", "Product");
            var allProductBookings = bookingsWithProduct.Where(b => b.Product.Id == id).ToList();

            var detailsViewModel = new DetailsViewModel
            {
                Product = productWithCategory,
                Bookings = allProductBookings
            };

            return View(detailsViewModel);
        }

        public IActionResult Delete(int id)
        {
            var productWithCategory = GetProductWithCategory(id);

            if(default == productWithCategory)
            {
                return NotFound();
            }

            return View(productWithCategory);
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
                Product = product,
                ProductId = product.Id
            };

            container.AddToBookings(booking);
            container.UpdateObject(product);
            container.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public Products GetProductWithCategory(int id)
        {
            var products = container.Products.AddQueryOption("$expand", "Category").ToList();
            if(default == products)
            {
                return default;
            }

            var product = products.Where(p => p.Id == id)?.Single();
            if (default == product)
            {
                return default;
            }

            return product;
        }
    }
}
