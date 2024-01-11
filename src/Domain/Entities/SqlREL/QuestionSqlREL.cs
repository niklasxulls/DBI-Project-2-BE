using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using stackblob.Domain.Entities.SqlREL.Defaults;

namespace stackblob.Domain.Entities.SqlREL;


public class QuestionSqlREL : BaseEntityUserTrackingSqlREL
{
    public QuestionSqlREL()
    {
        Tags = new List<QuestionTagSqlREL>();
        Answers = new List<AnswerSqlREL>();
    }
    public int QuestionId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public ICollection<QuestionTagSqlREL> Tags { get; set; }
    public ICollection<AnswerSqlREL> Answers { get; set; }
}
