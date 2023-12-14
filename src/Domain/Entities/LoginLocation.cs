using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.ValueObjects;

namespace stackblob.Domain.Entities;

public class LoginLocation : BaseEntity
{
    public int LoginLocationId { get; set; }
    public int? CountryId { get; set; }
    public Country? Country { get; set; }
    public int? TimeZoneId { get; set; }
    public Entities.Lookup.TimeZone? TimeZone { get; set; }
    public IpAddress IpAddress { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
