using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BasketItem : BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
        public int ProductId { get; set; } 
        
        [JsonIgnore]
        public virtual Basket Basket { get; set; }

        public int BasketId { get; set; }

    }
}
