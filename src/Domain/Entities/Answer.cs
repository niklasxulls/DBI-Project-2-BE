using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities;

public class Answer 
{
    public Answer()
    {
    }
    public ObjectId AnswerId { get; set; }
    //public Question Question { get; set; } = null!;
    //public ObjectId QuestionId { get; set; }
    //public Question? CorrectAnswerQuestion { get; set; }
    //public ObjectId? CorrectAnswerQuestionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}
