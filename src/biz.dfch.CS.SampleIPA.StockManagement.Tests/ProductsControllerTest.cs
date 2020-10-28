using biz.dfch.CS.SampleIPA.StockManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.SampleIPA.StockManagement.Tests
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public void GetProductWithCategoryReturnsExpectedProductWithCategory()
        {
            // Arrange
            var sut = new ProductsController();
            var productId = 8;

            // Act
            var result = sut.GetProductWithCategory(productId);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
