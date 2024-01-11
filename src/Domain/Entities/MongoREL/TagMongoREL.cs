using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities.MongoREL;

public class TagMongoREL
{
    public TagMongoREL()
    {
        Questions = new List<QuestionTagMongoREL>();
    }
    public string TagId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<QuestionTagMongoREL> Questions { get; set; }
}
