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

            var product = _context.Products.Find(booking.ProductId);
            booking.Product = product;

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return Created(booking);
        }
    }
}
