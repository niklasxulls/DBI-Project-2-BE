using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities;

public class Question
{
    public Question()
    {
        Tags = new List<QuestionTag>();
        //Answers = new List<Answer>();
    }
    public string QuestionId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public ICollection<QuestionTag> Tags { get; set; }
    //public ICollection<Answer> Answers { get; set; }
}
