using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using stackblob.Domain.Entities.MongoREL.Defaults;

namespace stackblob.Domain.Entities.MongoREL;

public class AnswerMongoREL //: BaseEntityUserTrackingMongoREL
{
    public string AnswerId { get; set; }
    public QuestionMongoREL Question { get; set; } = null!;
    public string QuestionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
