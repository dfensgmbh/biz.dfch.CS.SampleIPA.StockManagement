using biz.dfch.CS.SampleIPA.StockManagement.API.Models;
using biz.dfch.CS.SampleIPA.StockManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.SampleIPA.StockManagement.Tests
{
    [TestClass]
    public class ProductsControllerTest
    {
        public Products ExpectedProduct = new Products
        {
            Name = "TestProduct",
            MaterialNumber = "TestMaterialNumber",
            Quantity = 0,
            PricePerPiece = 0,
            WeightInKg = 0,
            CategoryId = 6,
            Category = new Categories
            {
                Name = "Diverses"
            }
            
        };

        [TestMethod]
        public void GetProductWithCategoryReturnsExpectedProductWithCategory()
        {
            // Arrange
            var sut = new ProductsController();
            var productId = 17;

            // Act
            var result = sut.GetProductWithCategory(productId);
            Assert.IsTrue(result.Equals(ExpectedProduct));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ExpectedProduct, result);
        }

        [TestMethod]
        public void GetProductByIdReturnsExpectedProduct()
        {
            // Arrange
            var sut = new ProductsController();
            var productId = 17;

            // Act
            var result = sut.GetProductById(productId);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetProductsWithCategoryReturnsAllProductsAndTheirCategory()
        {
            // Arrange
            var sut = new ProductsController();

            // Act
            var result = sut.GetProductsWithCategory();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
