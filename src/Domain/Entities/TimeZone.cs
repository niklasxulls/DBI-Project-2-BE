using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.Lookup;

public class TimeZone 
{
    public TimeZone()
    {
        LoginLocations = new List<LoginLocation>();
    }
    public int TimeZoneId { get; set; }
    public string Description { get; set; } = string.Empty;
    public TimeSpan Offset { get; set; }
    public ICollection<LoginLocation> LoginLocations { get; set; }

}
