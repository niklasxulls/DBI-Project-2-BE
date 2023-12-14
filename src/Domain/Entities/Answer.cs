using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class Answer : BaseEntityUserTracking
{
    public Answer()
    {
        AnswerVotes = new List<Vote>();
        Comments = new List<Comment>();
        Attachments = new List<Attachment>();
    }
    public int AnswerId { get; set; }
    public Question Question { get; set; } = null!;
    public int QuestionId { get; set; }
    public Question? CorrectAnswerQuestion { get; set; }
    public int? CorrectAnswerQuestionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Vote> AnswerVotes { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Attachment> Attachments { get; set; }

}
