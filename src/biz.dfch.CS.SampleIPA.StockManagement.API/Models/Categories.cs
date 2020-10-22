using System.ComponentModel.DataAnnotations.Schema;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Models
{
    public class Categories
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
    }
}
