using stackblob.Application.Mapping;
using stackblob.Domain.Entities.MongoREL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Models
{
    public class AuthentificatedUser : UserMongoREL, IMapFrom<UserMongoREL>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
