using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.Lookup;
public class SocialTypeType : BaseEntityEnum
{
    public SocialTypeType()
    {
        Users = new List<UserSocialType>();
    }

    public SocialType SocialTypeId { get; set; }
    public ICollection<UserSocialType> Users { get; set; }
}
