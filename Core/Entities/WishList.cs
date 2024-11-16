using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class WishList : BaseEntity
    {

        //public string UserId { get; set; }

        //[ForeignKey(nameof(UserId))]
        //public AppUser User { get; set; }

        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
