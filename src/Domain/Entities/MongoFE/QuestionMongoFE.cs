using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.MongoFE;
public class QuestionMongoFE
{
    public string QuestionId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    //public DateTime CreatedAt { get; set; }
    //public DateTime? UpdatedAt { get; set; }
    //public QuestionCreatedByMongoFE CreatedBy { get; set; }

    //public List<QuestionTagMongoFE> Tags { get; set; }
    //public List<QuestionAnswerMongoFE> Answers { get; set; }
}


public class QuestionCreatedByMongoFE
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; } = string.Empty;
}

public class QuestionTagMongoFE
{
    public string TagId { get; set; }
    public string Name { get; set; }
}

public class QuestionAnswerMongoFE
{
    public string AnswerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public QuestionAnswerCreatedByMongoFE CreatedBy { get; set; }
}

public class QuestionAnswerCreatedByMongoFE
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; } = string.Empty;
}