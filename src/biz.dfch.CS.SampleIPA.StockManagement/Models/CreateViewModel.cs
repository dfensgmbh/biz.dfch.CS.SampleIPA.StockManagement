using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace biz.dfch.CS.SampleIPA.StockManagement.Models
{
    public class CreateViewModel
    {
        public Products Product;
        public string SelectedItemId { get; set; }
        public IEnumerable<string> Categories;
    }
}
