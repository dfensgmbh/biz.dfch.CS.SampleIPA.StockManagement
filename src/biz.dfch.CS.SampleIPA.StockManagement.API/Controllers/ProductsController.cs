using biz.dfch.CS.SampleIPA.StockManagement.API.Data;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

            try
            {
                _context.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                if(ex.InnerException.HResult == -2146232060)
                {
                    return BadRequest();
                }

                return NoContent();
            }

            return NoContent();
        }
        
        //[EnableQuery]
        //[ODataRoute("({key})/Bookings")]
        //[HttpGet]
        //public IActionResult GetProductBookings([FromODataUri] int key)
        //{
        //    var bookings = _context.Bookings.ToList();
        //
        //    return Ok(bookings);
        //}
    }
}
