using biz.dfch.CS.SampleIPA.StockManagement.API.Data;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Controllers
{
    [ODataRoutePrefix(nameof(Bookings))]
    public class BookingsController : ODataController
    {
        private readonly StockManagementContext _context;

        public BookingsController(StockManagementContext context)
        {
            _context = context;

            if (!context.Bookings.Any())
            {
                _context.Database.EnsureCreated();
            }
        }

        [ODataRoute]
        [HttpPost]
        public IActionResult CreateBooking([FromBody] BookingsDto bookingsDto)
        {
            var product = _context.Products.Find(bookingsDto.ProductId);
            if(default == product)
            {
                return NotFound();
            }

            var booking = new Bookings
            {
                Amount = bookingsDto.Amount,
                DataTime = DateTime.Now,
                Product = product
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return Created(booking);
        }
    }
}
