using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.Lookup;

public class Tag
{
    public Tag()
    {
        Questions = new List<Question>();
    }
    public int TagId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Question> Questions { get; set; }
}
