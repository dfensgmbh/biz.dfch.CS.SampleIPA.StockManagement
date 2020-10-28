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

using Microsoft.EntityFrameworkCore;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Data
{
    public class StockManagementContext : DbContext
    {
        public StockManagementContext(DbContextOptions<StockManagementContext> options)
            : base(options) { }

        public DbSet<Products> Products { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>().HasData(
                new Categories { Id = 1, Name = "Elektronik" },
                new Categories { Id = 2, Name = "Haushalt" },
                new Categories { Id = 3, Name = "Möbel" },
                new Categories { Id = 4, Name = "Kleidung" },
                new Categories { Id = 5, Name = "Sport und Freizeit" },
                new Categories { Id = 6, Name = "Diverses" }
                );
        }
    }
}