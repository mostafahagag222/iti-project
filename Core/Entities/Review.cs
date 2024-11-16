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
    public class Review : BaseEntity
    {

        // Todo User
        // public string UserId { get; set; }
        // [ForeignKey(UserId)]
        // public AppUser User { get; set; }

        [StringLength(150)]
        public string Body { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Range(1, 5)]
        public int Stars { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product Product { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public AppUser User { get; set; }
    }
}
