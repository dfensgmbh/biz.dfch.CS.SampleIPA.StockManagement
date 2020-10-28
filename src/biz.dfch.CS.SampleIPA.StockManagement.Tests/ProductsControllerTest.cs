/**
 * Copyright 2020 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
