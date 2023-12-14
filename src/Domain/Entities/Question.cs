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
        QuestionVotes = new List<Vote>();
        Tags = new List<Tag>();
        Comments = new List<Comment>();
        Attachments = new List<Attachment>();
        Answers = new List<Answer>();
    }
    public int QuestionId { get; set; }
    public Guid QuestionIdAccess { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Popularity => VoteUtil.CalculateQuestionPopularity(this);

    public int? CorrectAnswerId { get; set; }
    public Answer? CorrectAnswer { get; set; }

    public ICollection<Vote> QuestionVotes { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
    public ICollection<Answer> Answers { get; set; }
}
