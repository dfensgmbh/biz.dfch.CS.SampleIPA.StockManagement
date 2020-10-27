using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using System.Collections.Generic;

namespace biz.dfch.CS.SampleIPA.StockManagement.Models
{
    public class DetailsViewModel
    {
        public Products Product { get; set; }
        public List<Bookings> Bookings { get; set; }
    }
}
