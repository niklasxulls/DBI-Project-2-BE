using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.Lookup;

public class Country : BaseEntity
{
    public Country()
    {
        LoginLocations = new List<LoginLocation>();
    }

    public int CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public ICollection<LoginLocation> LoginLocations { get; set; }
}

