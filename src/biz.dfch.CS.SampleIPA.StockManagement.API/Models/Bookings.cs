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
