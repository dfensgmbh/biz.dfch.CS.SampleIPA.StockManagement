using System.ComponentModel.DataAnnotations.Schema;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Models
{
    public class Products
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string MaterialNumber { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerPiece { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal WeightInKg { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Categories Category { get; set; }

        private bool Equals(Products other)
        {
            return Name.Equals(other.Name) && MaterialNumber.Equals(other.MaterialNumber) && Quantity.Equals(other.Quantity) &&
                   PricePerPiece.Equals(other.PricePerPiece) && WeightInKg.Equals(other.WeightInKg) && CategoryId.Equals(other.CategoryId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Products)obj);
        }

        public override int GetHashCode()
        {
            var hashName = Name == null ? 0 : Name.GetHashCode();
            var hasMaterialNumber = MaterialNumber == null ? 0 : MaterialNumber.GetHashCode();
            var hasQuantity = Quantity.GetHashCode();
            var hasPricePerPiece = PricePerPiece.GetHashCode();
            var hasCategoryId = CategoryId.GetHashCode();

            return hashName ^ hasMaterialNumber ^ hasQuantity ^ hasPricePerPiece ^ hasCategoryId;
        }
    }
}
