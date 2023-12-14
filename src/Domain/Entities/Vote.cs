using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class Vote : BaseEntity
{
    public int VoteId { get; set; }
    public bool IsUpVote { get; set; }
    public Question Question { get; set; } = null!;
    public int QuestionId { get; set; }
    public Answer? Answer { get; set; }
    public int? AnswerId { get; set; }
    public User? CreateByInQuestion { get; set; }
    public int? CreateByInQuestionId { get; set; }
    public User? CreateByInAnswer { get; set; }
    public int? CreateByInAnswerId { get; set; }

}
