using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class Question : BaseEntityUserTracking
{
    public Question()
    {
        Tags = new List<Tag>();
        Answers = new List<Answer>();
    }
    public int QuestionId { get; set; }
    public Guid QuestionIdAccess { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int? CorrectAnswerId { get; set; }
    public Answer? CorrectAnswer { get; set; }

    public ICollection<Tag> Tags { get; set; }
    public ICollection<Answer> Answers { get; set; }
}
