using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace stackblob.Infrastructure.Other;
public class IPAddressLocation
{
    public string Ip { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string RegionName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Latitude { get; set; } 
    public decimal Longitude { get; set; } 
    public ICollection<string> Timezones { get; set; } = new List<string>();

}
