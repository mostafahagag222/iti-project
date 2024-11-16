using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class UserWithSearchAndPaging : BaseSpecification<AppUser>
    {
        public UserWithSearchAndPaging(UserSpecs userSpecs) : base(x =>
            (string.IsNullOrEmpty(userSpecs.Search) || x.UserName.ToLower().Contains(userSpecs.Search))
        )
        {
            ApplyPaging(userSpecs.PageSize * (userSpecs.PageIndex - 1), userSpecs.PageSize);
        }
    }
}
