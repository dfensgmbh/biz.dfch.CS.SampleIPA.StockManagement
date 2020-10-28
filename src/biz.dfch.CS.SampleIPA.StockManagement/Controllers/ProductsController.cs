/**
 * Copyright 2020 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
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
        private readonly Container container;
        private readonly IEnumerable<Categories> categories;

        public ProductsController()
        {
            container = new Container(new Uri("https://localhost:44386/odata/"));
            categories = container.Categories.ToList();
        }

        public IActionResult Index()
        {
            var products = GetProductsWithCategory();
            if (default == products)
            {
                return View();
            }

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
            var selectedCategory = categories.Single(c => c.Name == selectedCategoryName);
            product.Category = selectedCategory;
            product.CategoryId = selectedCategory.Id;

            container.AddToProducts(product);
            container.SaveChanges();
            
            // Code below needs to be after the first 'container.SaveChanges()', else the Product has no Id (product.Id) --> API can't find Product.
            if (product.Quantity > 0)
            {
                var booking = new Bookings
                {
                    Amount = product.Quantity,
                    DataTime = DateTime.Now,
                    Product = product,
                    ProductId = product.Id
                };
                container.AddToBookings(booking);
                container.SaveChanges();
            }

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
                // ReSharper disable once ReplaceWithSingleCallToSingle --> Reason: Single isn't supported to call on Products
                var product = GetProductById(id);
                var category = categories.Single(c => c.Name == editViewModel.SelectedCategoryName);

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

            var bookingsWithProduct = container.Bookings.AddQueryOption("$expand", "Product");
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
            // ReSharper disable once ReplaceWithSingleCallToSingle --> Reason: Single isn't supported to call on Products
            var product = GetProductById(id);

            container.DeleteObject(product);
            try
            {
                container.SaveChanges();
            }
            catch(Exception)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Book(int id)
        {
            // ReSharper disable once ReplaceWithSingleCallToSingle --> Reason: Single isn't supported to call on Products
            var product = GetProductById(id);

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

            // ReSharper disable once ReplaceWithSingleCallToSingle --> Reason: Single isn't supported to call on Products
            var product = GetProductById(id);

            if (bookViewModel.BookingAction == "Entfernen")
            {
                if(product.Quantity < bookViewModel.Amount)
                {
                    // Amount zu Gross
                    return RedirectToAction(nameof(Index));
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
            var products = GetProductsWithCategory();

            // ReSharper disable once ReplaceWithSingleCallToSingle --> Reason: Single isn't supported to call on Products
            var product = products?.Where(p => p.Id == id).Single();

            return product;
        }

        public List<Products> GetProductsWithCategory()
        {
            var products = container.Products.AddQueryOption("$expand", "Category").ToList();

            return !products.Any() ? default : products;
        }

        public Products GetProductById(int id)
        {
            // ReSharper disable once ReplaceWithSingleCallToSingle --> Reason: Single isn't supported to call on Products
            return container.Products.Where(p => p.Id == id).Single();
        }
    }
}
