﻿using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using System.Collections.Generic;

namespace biz.dfch.CS.SampleIPA.StockManagement.Models
{
    public class CreateViewModel
    {
        public Products Product;
        public string SelectedCategoryName { get; set; }
        public IEnumerable<string> Categories;
    }
}
