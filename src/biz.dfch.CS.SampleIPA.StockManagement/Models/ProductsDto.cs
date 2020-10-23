using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace biz.dfch.CS.SampleIPA.StockManagement.Models
{
    public class ProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerPiece { get; set; }
        public decimal WeightInKg { get; set; }

    }
}
