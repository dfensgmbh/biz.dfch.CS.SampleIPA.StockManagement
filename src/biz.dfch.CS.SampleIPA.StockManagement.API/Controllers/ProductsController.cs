﻿using biz.dfch.CS.SampleIPA.StockManagement.API.Data;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Controllers
{
    [ODataRoutePrefix(nameof(Product))]
    public class ProductsController : ODataController
    {
        private readonly StockManagementContext _context;

        public ProductsController(StockManagementContext context)
        {
            _context = context;

            if (!context.Products.Any())
            {
                _context.Database.EnsureCreated();
            }
        }

        [ODataRoute]
        [EnableQuery]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products;

            return Ok(products);
        }


        [ODataRoute("({key})")]
        [HttpGet]
        public IActionResult GetProductByKey([FromODataUri] int key)
        {
            var product = _context.Products.Find(key);

            if (default == product)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [ODataRoute]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if(default == product)
            {
                return BadRequest();
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return Created(product);
        }

        [ODataRoute("({key})")]
        [HttpPatch]
        public IActionResult EditProduct([FromODataUri] int key, [FromBody] Delta<Product> delta)
        {
            var product = _context.Products.Find(key);
            if(default == product)
            {
                return NotFound();
            }

            delta.Patch(product);
            _context.SaveChanges();

            return Updated(delta);
        }

        [ODataRoute("({key})")]
        [HttpDelete]
        public IActionResult DeleteProduct([FromODataUri] int key)
        {
            var product = _context.Products.Find(key);
            if(default == product)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
