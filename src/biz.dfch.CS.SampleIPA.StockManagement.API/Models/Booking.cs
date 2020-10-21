using System;
using System.ComponentModel.DataAnnotations;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime DataTime { get; set; }
        public Product Product { get; set; }
    }
}
