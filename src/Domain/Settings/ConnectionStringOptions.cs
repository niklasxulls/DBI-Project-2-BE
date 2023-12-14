using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Settings;
public class ConnectionStringOptions
{
    public bool IsMongoDb { get; set; }
    public string DbName { get; set; }
}
