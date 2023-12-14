using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Settings;
public class ConnectionStringOptions
{
    public string DbName { get; set; }
}



public class AppConfig
{
    public bool IsMongoDb { get; set; }
}