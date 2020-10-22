using System;
using System.ComponentModel.DataAnnotations;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Models
{
    public class Bookings
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime DataTime { get; set; }
        public Products Product { get; set; }
    }
}
