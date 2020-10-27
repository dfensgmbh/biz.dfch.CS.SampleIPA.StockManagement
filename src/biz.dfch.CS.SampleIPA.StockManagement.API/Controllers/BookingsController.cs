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
        [EnableQuery]
        [HttpGet]
        public IActionResult GetBookings()
        {
            var bookings = _context.Bookings;

            return Ok(bookings);
        }

        [ODataRoute]
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Bookings booking)
        {
            if(default == booking)
            {
                return BadRequest();
            }

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return Created(booking);
        }
    }
}
