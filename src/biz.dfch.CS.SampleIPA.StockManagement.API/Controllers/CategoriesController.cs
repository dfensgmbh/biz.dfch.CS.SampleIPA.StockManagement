﻿/**
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
using System.Linq;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Controllers
{
    [ODataRoutePrefix(nameof(Categories))]
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
