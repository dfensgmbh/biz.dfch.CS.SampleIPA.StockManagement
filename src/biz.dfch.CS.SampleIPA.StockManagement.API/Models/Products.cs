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
