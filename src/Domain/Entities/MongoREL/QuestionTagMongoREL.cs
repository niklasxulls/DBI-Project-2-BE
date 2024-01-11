using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities.MongoREL;

public class QuestionTagMongoREL
{
    public string QuestionId { get; set; }
    public QuestionMongoREL Question { get; set; }
    public string TagId { get; set; }
    public TagMongoREL Tag { get; set; }
}
