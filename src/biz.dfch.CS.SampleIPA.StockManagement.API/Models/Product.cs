namespace biz.dfch.CS.SampleIPA.StockManagement.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaterialNumber { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerPiece { get; set; }
        public decimal WeightInKg { get; set; } 
        public Category Category { get; set; }
    }
}
