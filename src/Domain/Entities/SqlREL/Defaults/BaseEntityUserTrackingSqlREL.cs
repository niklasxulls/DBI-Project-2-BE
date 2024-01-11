using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using stackblob.Domain.Entities.SqlREL;

namespace stackblob.Domain.Entities.SqlREL.Defaults;

public class BaseEntityUserTrackingSqlREL : BaseEntitySqlREL
{
    public int? CreatedById { get; set; }
    public UserSqlREL? CreatedBy { get; set; } = null!;
}
