using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities.MongoFE;
public class QuestionMongoFE
{
    public ObjectId _id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public QuestionUserMongoFE CreatedBy { get; set; }

    public List<QuestionTagMongoFE> Tags { get; set; } = new List<QuestionTagMongoFE>();
    public List<QuestionAnswerMongoFE> Answers { get; set; } = new List<QuestionAnswerMongoFE>();
}


public class QuestionUserMongoFE
{
    public ObjectId UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; } = string.Empty;
}

public class QuestionTagMongoFE
{
    public ObjectId TagId { get; set; }
    public string Name { get; set; }
}

public class QuestionAnswerMongoFE
{
    public ObjectId AnswerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public QuestionUserMongoFE CreatedBy { get; set; }
}
