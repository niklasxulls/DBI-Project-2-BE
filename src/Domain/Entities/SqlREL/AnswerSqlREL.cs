using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using stackblob.Domain.Entities.SqlREL.Defaults;

namespace stackblob.Domain.Entities.SqlREL;

public class AnswerSqlREL : BaseEntityUserTrackingSqlREL
{
    public int AnswerId { get; set; }
    public QuestionSqlREL Question { get; set; } = null!;
    public int QuestionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
