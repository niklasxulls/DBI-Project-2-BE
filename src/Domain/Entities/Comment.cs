using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class Comment : BaseEntity
{
    public int CommentId { get; set; }
    public string Description { get; set; } = string.Empty;

    public Question Question { get; set; } = null!;
    public int QuestionId { get; set; }
    public Answer? Answer { get; set; }
    public int? AnswerId { get; set; }
    public User? CreatedByInAnswer { get; set; }
    public int? CreatedByInAnswerId { get; set; }
    public User? CreatedByInQuestion { get; set; }
    public int? CreatedByInQuestionId { get; set; }

}
