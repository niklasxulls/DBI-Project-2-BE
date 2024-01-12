using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using stackblob.Domain.Entities.MongoREL.Defaults;

namespace stackblob.Domain.Entities.MongoREL;

public class QuestionMongoREL //: BaseEntityUserTrackingMongoREL
{
    public QuestionMongoREL()
    {
        //Tags = new List<QuestionTagMongoREL>();
        //Answers = new List<AnswerMongoREL>();
    }
    public string QuestionId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    //public ICollection<QuestionTagMongoREL> Tags { get; set; }
    //public ICollection<AnswerMongoREL> Answers { get; set; }
}
