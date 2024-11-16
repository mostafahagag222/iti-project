using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new();

        [JsonIgnore]
        public virtual AppUser User { get; set; }
        public string UserId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal? ShippingPrice { get; set; }

    }
}
