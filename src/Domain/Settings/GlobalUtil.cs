using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Settings;
public static class GlobalUtil
{
    public static bool IsMongoDb { get; set; }
    public static string MongoDbName { get; set; }
    public static string ConnectionString { get; set; }
}
