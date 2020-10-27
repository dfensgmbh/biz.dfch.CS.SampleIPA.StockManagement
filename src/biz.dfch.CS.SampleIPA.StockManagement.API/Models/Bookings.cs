using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Models
{
    public class Bookings
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime DataTime { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Products Product { get; set; }
    }
}
