using biz.dfch.CS.SampleIPA.StockManagement.API.Data;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Controllers
{
    [ODataRoutePrefix(nameof(Products))]
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
        public IActionResult CreateProduct([FromBody] Products product)
        {
            if(default == product)
            {
                return BadRequest();
            }

            var category = _context.Categories.Find(product.CategoryId);
            product.Category = category;

            _context.Products.Add(product);
            _context.SaveChanges();

            return Created(product);
        }

        [ODataRoute("({key})")]
        [HttpPatch]
        public IActionResult EditProduct([FromODataUri] int key, [FromBody] Delta<Products> delta)
        {
            var product = _context.Products.Find(key);
            if(default == product)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(product.CategoryId);
            product.Category = category;

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

            if(_context.Bookings.Any(b => b.ProductId == key))
            {
                return BadRequest();
            }

            _context.Products.Remove(product);

            try
            {
                _context.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                return Conflict(ex.Message);
            }

            return NoContent();
        }
    }
}
