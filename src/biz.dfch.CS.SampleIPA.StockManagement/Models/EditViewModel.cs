using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using System.Collections.Generic;

namespace biz.dfch.CS.SampleIPA.StockManagement.Models
{
    public class EditViewModel
    {
        public Products Product { get; set; }
        public string SelectedCategoryName { get; set; }
        public IEnumerable<string> Categories;
    }
}
