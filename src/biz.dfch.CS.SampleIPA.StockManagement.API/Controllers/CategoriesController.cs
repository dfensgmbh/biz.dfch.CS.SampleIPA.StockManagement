using biz.dfch.CS.SampleIPA.StockManagement.API.Data;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Controllers
{
    [ODataRoutePrefix(nameof(Category))]
    public class CategoriesController : ODataController
    {
        private readonly StockManagementContext _context;

        public CategoriesController(StockManagementContext context)
        {
            _context = context;

            if (!context.Categories.Any())
            {
                _context.Database.EnsureCreated();
            }
        }

        [ODataRoute]
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories;

            return Ok(categories);
        }


        [ODataRoute("({key})")]
        [HttpGet]
        public IActionResult GetCategoryByKey([FromODataUri] int key)
        {
            var category = _context.Categories.Find(key);
            
            if(default == category)
            {
                return NotFound();
            }

            return Ok(category);
        }
    }
}
