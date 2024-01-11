using MongoDB.Bson;
using stackblob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        QuestionsCreated = new List<Question>();
        //AnswersCreated = new List<Answer>();
        //MongoQuestionsCreated = new List<MongoQuestion>();
        //MongoAnswersCreated = new List<MongoAnswer>();
    }

    public string UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Name => Firstname + " " + Lastname;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string? StatusText { get; set; }

    public ICollection<Question> QuestionsCreated { get; set; }
    //public ICollection<Answer> AnswersCreated { get; set; }
    //public ICollection<MongoQuestion> MongoQuestionsCreated { get; set; }
    //public ICollection<MongoAnswer> MongoAnswersCreated { get; set; }
}
