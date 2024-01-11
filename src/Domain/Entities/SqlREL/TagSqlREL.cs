using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.SqlREL;

public class TagSqlREL
{
    public TagSqlREL()
    {
        Questions = new List<QuestionTagSqlREL>();
    }
    public int TagId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<QuestionTagSqlREL> Questions { get; set; }
}
