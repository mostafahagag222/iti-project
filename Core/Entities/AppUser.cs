using Core.Entities.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class AppUser: IdentityUser
    {
        public string ProfilePicture { get; set; }


        public Gender Gender { get; set; }

        public virtual WishList WishList { get; set; } = new WishList();

        [ForeignKey("WishList")]
        public int WishListId { get; set; }
    }
}