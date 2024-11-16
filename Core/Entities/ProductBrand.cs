using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

﻿namespace Core.Entities
{
    public class ProductBrand: BaseEntity
    {
        public string Name { get; set; }
        public string Origin { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
