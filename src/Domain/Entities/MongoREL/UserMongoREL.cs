using MongoDB.Bson;
using stackblob.Domain.Entities.MongoREL.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.MongoREL;

public class UserMongoREL : BaseEntityMongoREL
{
    public UserMongoREL()
    {
        QuestionsCreated = new List<QuestionMongoREL>();
        //AnswersCreated = new List<AnswerMongoREL>();
    }

    public string UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Name => Firstname + " " + Lastname;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string? StatusText { get; set; }

    public ICollection<QuestionMongoREL> QuestionsCreated { get; set; }
    //public ICollection<AnswerMongoREL> AnswersCreated { get; set; }
}
