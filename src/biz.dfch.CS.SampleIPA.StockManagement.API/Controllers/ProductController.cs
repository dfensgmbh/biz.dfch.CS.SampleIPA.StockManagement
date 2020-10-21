using biz.dfch.CS.SampleIPA.StockManagement.API.Data;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Controllers
{
    [ODataRoutePrefix(nameof(Product))]
    public class ProductController: ODataController
    {
        private readonly StockManagementContext _context;

        public ProductController(StockManagementContext context)
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
        public CreatedODataResult<Product> CreateProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return Created(product);
        }
    }
}
