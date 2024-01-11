using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities.SqlREL;

public class QuestionTagSqlREL
{
    public int QuestionId { get; set; }
    public QuestionSqlREL Question { get; set; }
    public int TagId { get; set; }
    public TagSqlREL Tag { get; set; }
}
